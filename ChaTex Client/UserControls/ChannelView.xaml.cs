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
    public partial class ChannelView : UserControl, EditableElementView
    {
        private readonly ObservableCollection<MessageViewModel> messages;
        private readonly IMessages messagesApi;
        private readonly IChannels channelsApi;

        private MessageViewState state;
        private int? currentChannelId;
        private int? currentChatId;

        private DateTime latestEventTime;
        private CancellationTokenSource messageFetchCancellation;
        private MessageViewModel selectedMessage;
        private Task newMessageTask;
        private Task oldMessageTask;
        private bool keepFetchingMessages;

        public event Action<ChannelDTO> ChannelDeleted;
        public event Action<ChannelDTO> ChannelRenamed;

        public ChannelView(IMessages messagesApi, IChannels channelsApi)
        {
            this.messagesApi = messagesApi;
            this.channelsApi = channelsApi;

            InitializeComponent();
            messages = new ObservableCollection<MessageViewModel>();
            icMessages.ItemsSource = messages;
        }

        public void SetChannel(int? channelId)
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
            if (currentChannelId == null) return;
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
                    IEnumerable<ChannelEventDTO> channelEvents = null;

                    if (state == MessageViewState.Channel)
                    {
                        //Get channel events
                        //When we call await we pass control back to the caller, so this while loop does not block the UI
                        channelEvents = await channelsApi.GetChannelEventsAsync((int)currentChannelId, latestEventTime, messageFetchCancellation.Token);
                    }

                    await handleChannelEventsAsync(channelEvents, messageFetchCancellation.Token);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("The live message fetch task was canceled");
                }
                catch (HttpOperationException er)
                {
                    new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
                }
            }
        }

        private async Task handleChannelEventsAsync(IEnumerable<ChannelEventDTO> channelEvents, CancellationToken token)
        {
            if (channelEvents == null)
            {
                throw new ApplicationException("Null content received when getting new channel events!");
            }

            //Add to ui when ready
            await Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate ()
            {
                foreach (ChannelEventDTO channelEvent in channelEvents)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    switch (channelEvent.Type)
                    {
                        case "newMessage":
                            appendMessage(channelEvent.Message);
                            break;
                        case "updateMessage":
                            updateMessage(channelEvent.Message);
                            break;
                        case "deleteMessage":
                            deleteMessage(channelEvent.Message);
                            break;
                        case "renameChannel":
                            ChannelRenamed?.Invoke(channelEvent.Channel);
                            break;
                        case "deleteChannel":
                            ChannelDeleted?.Invoke(channelEvent.Channel);
                            break;
                    }

                    updateLatestEventTime(channelEvent.TimeOfOccurrence);
                }
            });
        }

        /// <summary>
        /// Remove all messages from the message view.
        /// </summary>
        private void clearChat()
        {
            messages.Clear();
            latestEventTime = DateTime.UtcNow;
        }

        private async Task fetchNewMessages()
        {
            IList<GetMessageDTO> newMessages = null;
            try
            {
                if (state == MessageViewState.Channel && currentChannelId != null)
                {
                    newMessages = await messagesApi.GetMessagesAsync((int)currentChannelId, cancellationToken: messageFetchCancellation.Token);
                }

                foreach (GetMessageDTO message in newMessages)
                {
                    appendMessage(message);
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private async Task fetchOldMessages()
        {
            IList<GetMessageDTO> oldMessages = null;
            try
            {
                if (state == MessageViewState.Channel && currentChannelId != null)
                {
                    DateTime earliestMessageTime = messages.Count() == 0 ?
                        DateTime.MaxValue : messages[0].CreationDate;

                    oldMessages = await messagesApi.GetMessagesAsync((int)currentChannelId, earliestMessageTime, 10, messageFetchCancellation.Token);
                }

                for (int i = oldMessages.Count - 1; i >= 0; i--)
                {
                    prependMessage(oldMessages[i]);
                }
            }
            catch (HttpOperationException er)
            {
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private void appendMessage(GetMessageDTO message)
        {
            MessageViewModel messageViewModel = new MessageViewModel(message);

            if (messages.LastOrDefault()?.AuthorId == messageViewModel.AuthorId)
            {
                messageViewModel.FirstInSequence = false;
            }

            messages.Add(messageViewModel);
            svMessages.ScrollToBottom();
            updateLatestEventTime(message.CreationTime, message.DeletionDate, message.LastEdited);
        }

        private void prependMessage(GetMessageDTO message)
        {
            MessageViewModel messageViewModel = new MessageViewModel(message);

            if (messages.FirstOrDefault()?.AuthorId == messageViewModel.AuthorId)
            {
                messages.First().FirstInSequence = false;
            }

            messages.Insert(0, messageViewModel);
        }

        private void updateLatestEventTime(params DateTime?[] times)
        {
            foreach (DateTime? time in times)
            {
                if (time == null) continue;
                if (time > latestEventTime)
                {
                    latestEventTime = (DateTime)time;
                }
            }
        }

        private void deleteMessage(GetMessageDTO message)
        {
            int replaceIndex = messages.IndexOf(messages.SingleOrDefault(m => m.Id == message.Id));
            if (replaceIndex != -1)
            {
                messages[replaceIndex] = new MessageViewModel(message);
            }
        }

        private void updateMessage(GetMessageDTO message)
        {
            int replaceIndex = messages.IndexOf(messages.SingleOrDefault(m => m.Id == message.Id));
            if (replaceIndex != -1)
            {
                messages[replaceIndex] = new MessageViewModel(message);
            }
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSendMessage.IsEnabled = txtMessage.Text.Length > 0;
        }

        private async void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await messagesApi.CreateMessageAsync((int)currentChannelId, txtMessage.Text);

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
                new ErrorDialog(er.Response.ReasonPhrase, er.Response.Content).ShowDialog();
            }
        }

        private void DockPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseOverChangedInternal(sender, true);
        }

        private void DockPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseOverChangedInternal(sender, false);
        }

        private void isMouseOverChangedInternal(object sender, bool isMouseOver)
        {
            DockPanel dpnlOuter = (DockPanel)sender;
            DockPanel dpnlInner = (DockPanel)dpnlOuter.Children[0];

            MessageViewModel messageViewModel = (MessageViewModel)dpnlOuter.DataContext;
            if (!messageViewModel.Me) return;

            foreach (UIElement child in dpnlInner.Children)
            {
                if (child is Button)
                {
                    ((Button)child).Visibility = isMouseOver ? Visibility.Visible : Visibility.Hidden;
                }
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

        public bool Edit()
        {
            throw new NotImplementedException();
        }
    }
}
