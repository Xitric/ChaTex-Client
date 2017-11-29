using BusinessLayer.Enum;
using ChaTex_Client.UserDialogs;
using ChaTex_Client.ViewModels;
using IO.ChaTex;
using IO.ChaTex.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for ChannelMessageView.xaml
    /// </summary>
    public partial class ChannelMessageView : UserControl
    {
        private readonly ObservableCollection<MessageViewModel> messages;
        private readonly IMessages messagesApi;

        private MessageViewState state;
        private int? currentChannelId;
        private int? currentChatId;

        private DateTime latestMessageTime;
        private CancellationTokenSource messageFetchCancellation;
        private MessageViewModel selectedMessage;
        private Task newMessageTask;
        private Task oldMessageTask;
        private bool keepFetchingMessages;

        public ChannelMessageView(IMessages messagesApi)
        {
            this.messagesApi = messagesApi;

            InitializeComponent();
            messages = new ObservableCollection<MessageViewModel>();
            icMessages.ItemsSource = messages;
        }

        public void SetChannel(int channelId)
        {
            currentChannelId = channelId;

            //Sets our state, so the program knows if we're in a channel or chat
            state = MessageViewState.Channel;

            newMessageTask = beginDisplayingMessages().ContinueWith(t => Console.WriteLine("Message fetcher stopped"));
        }

        public void SetChat(int chatId)
        {
            currentChatId = chatId;
            state = MessageViewState.Chat;

            newMessageTask = beginDisplayingMessages();
        }

        private async Task beginDisplayingMessages()
        {
            //Stop fetching messages in previous channel. We cannot continue until this is truly completed
            await stopFetchingMessages();

            //Repopulate window with new messages
            clearChat();
            await fetchNewMessages();

            //Begin listening for messages in the new channel
            await beginFetchingMessages();
        }

        /// <summary>
        /// Stop listening for new messages from the server.
        /// </summary>
        private async Task stopFetchingMessages()
        {
            messageFetchCancellation?.Cancel();
            keepFetchingMessages = false;

            if (newMessageTask != null)
            {
                await newMessageTask;
            }

            if (oldMessageTask != null)
            {
                await oldMessageTask;
            }

            messageFetchCancellation = new CancellationTokenSource();
            keepFetchingMessages = true;
        }

        /// <summary>
        /// Begin listening for message events from the server.
        /// </summary>
        private async Task beginFetchingMessages()
        {
            while (keepFetchingMessages)
            {
                try
                {
                    IEnumerable<MessageEventDTO> messageEvents = null;

                    if (state == MessageViewState.Channel)
                    {
                        //Get message events
                        //When we call await we pass control back to the caller, so this while loop does not block the UI
                        messageEvents = await messagesApi.GetMessageEventsAsync((int)currentChannelId, latestMessageTime, messageFetchCancellation.Token);
                    }

                    await handleMessageEventsAsync(messageEvents, messageFetchCancellation.Token);
                }
                catch (TaskCanceledException e)
                {
                    Console.WriteLine("The live message fetch task was canceled");
                }
            }
        }

        private async Task handleMessageEventsAsync(IEnumerable<MessageEventDTO> messageEvents, CancellationToken token)
        {
            if (messageEvents == null)
            {
                throw new ApplicationException("Null content received when getting new message events!");
            }

            //Add to ui when ready
            await Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate ()
            {
                foreach (MessageEventDTO msgEvent in messageEvents)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    switch (msgEvent.Type)
                    {
                        case "NewMessage":
                            appendMessage(msgEvent.Message);
                            break;
                        case "DeleteMessage":
                            deleteMessage(msgEvent.Message);
                            break;
                        case "UpdateMessage":
                            updateMessage(msgEvent.Message);
                            break;
                    }
                }
            });
        }

        /// <summary>
        /// Remove all messages from the message view.
        /// </summary>
        private void clearChat()
        {
            messages.Clear();
        }

        private async Task fetchNewMessages()
        {
            IList<GetMessageDTO> newMessages = null;
            try
            {
                if (state == MessageViewState.Channel && currentChannelId != null)
                {
                    newMessages = await messagesApi.GetMessagesAsync((int)currentChannelId, 0, 25, messageFetchCancellation.Token);
                }

                foreach (GetMessageDTO message in newMessages)
                {
                    appendMessage(message);
                }
            }
            catch (HttpOperationException er)
            {
                throw er;
            }
        }

        private async Task fetchOldMessages()
        {
            IList<GetMessageDTO> oldMessages = null;
            try
            {
                if (state == MessageViewState.Channel && currentChannelId != null)
                {
                    oldMessages = await messagesApi.GetMessagesAsync((int)currentChannelId, messages.Count, 10, messageFetchCancellation.Token);
                }

                for (int i = oldMessages.Count - 1; i >= 0; i--)
                {
                    prependMessage(oldMessages[i]);
                }
            }
            catch (HttpOperationException er)
            {
                throw er;
            }
        }

        private void appendMessage(GetMessageDTO message)
        {
            messages.Add(new MessageViewModel(message));
            svMessages.ScrollToBottom();
            updateLatestTime(message);
        }

        private void prependMessage(GetMessageDTO message)
        {
            messages.Insert(0, new MessageViewModel(message));
            updateLatestTime(message);
        }

        private void updateLatestTime(GetMessageDTO message)
        {
            if (message.CreationTime > latestMessageTime)
            {
                latestMessageTime = message.CreationTime;
            }

            if (message.LastEdited != null && message.LastEdited > latestMessageTime)
            {
                latestMessageTime = (DateTime)message.LastEdited;
            }

            if (message.DeletionDate != null && message.DeletionDate > latestMessageTime)
            {
                latestMessageTime = (DateTime)message.DeletionDate;
            }
        }

        private void deleteMessage(GetMessageDTO message)
        {
            int replaceIndex = messages.IndexOf(messages.SingleOrDefault(m => m.Id == message.Id));
            if (replaceIndex != -1)
            {
                messages[replaceIndex] = new MessageViewModel(message);
            }

            latestMessageTime = (DateTime)message.DeletionDate;
        }

        private void updateMessage(GetMessageDTO message)
        {
            int replaceIndex = messages.IndexOf(messages.SingleOrDefault(m => m.Id == message.Id));
            if (replaceIndex != -1)
            {
                messages[replaceIndex] = new MessageViewModel(message);
            }

            latestMessageTime = (DateTime)message.LastEdited;
        }

        private void showUnauthorizedDialog()
        {
            new ErrorDialog("Authentication failed", "You do not have access to this channel.").ShowDialog();
        }

        private void showMissingChannelDialog()
        {
            new ErrorDialog("Not found", "The requested channel does not exist.").ShowDialog();
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSendMessage.IsEnabled = txtMessage.Text.Length > 0;
        }

        private async void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            var messageContentDTO = new MessageContentDTO()
            {
                Message = txtMessage.Text
            };

            try
            {
                await messagesApi.CreateMessageAsync((int)currentChannelId, messageContentDTO);

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private void miEditMessage_Click(object sender, EventArgs e)
        {

        }

        private async void miDeleteMessage_Click(object sender, EventArgs e)
        {
            try
            {
                int id = selectedMessage.Id;
                await messagesApi.DeleteMessageAsync(id);
            }
            catch (HttpOperationException er)
            {
                //TODO: Exception handling
                throw er;
            }
        }

        private void btnManageMessage_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            btn.ContextMenu.Visibility = Visibility.Visible;
            btn.ContextMenu.IsOpen = true;

            selectedMessage = (MessageViewModel)btn.DataContext;
        }

        private async void svMessages_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (currentChannelId == null) return;

            //If we are already fetching old messages, then there is no need to make a new call
            if (oldMessageTask != null && !oldMessageTask.IsCompleted)
            {
                return;
            }

            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.ComputedVerticalScrollBarVisibility != Visibility.Visible) return;

            //If we hit the top of the message view we should load previous messages
            if (scrollViewer.VerticalOffset == 0)
            {
                //Temporarely stop listening to scroll events, as we will be generating some ourselves
                scrollViewer.ScrollChanged -= svMessages_ScrollChanged;

                double oldOffsetFromBottom = scrollViewer.ScrollableHeight;

                try
                {
                    oldMessageTask = fetchOldMessages();
                    await oldMessageTask;
                }
                catch (TaskCanceledException) { }

                scrollViewer.ScrollToVerticalOffset(scrollViewer.ScrollableHeight - oldOffsetFromBottom);
                scrollViewer.ScrollChanged += svMessages_ScrollChanged;
            }
        }
    }
}
