using BusinessLayer.Enum;
using ChaTex_Client.UserDialogs;
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
        private readonly ObservableCollection<GetMessageDTO> messages;
        private readonly IMessages messagesApi;

        private MessageViewState state;
        private int? currentChannelId;
        private int? currentChatId;

        private DateTime latestMessageTime;
        private CancellationTokenSource messageFetchCancellation;
        private GetMessageDTO selectedMessage;
        private Task currentMessageTask;
        private bool keepFetchingMessages;

        public ChannelMessageView(IMessages messagesApi)
        {
            this.messagesApi = messagesApi;

            InitializeComponent();
            messages = new ObservableCollection<GetMessageDTO>();
            icMessages.ItemsSource = messages;
        }

        public void SetChannel(int channelId)
        {
            currentChannelId = channelId;

            //Sets our state, so the program knows if we're in a channel or chat
            state = MessageViewState.Channel;

            currentMessageTask = beginDisplayingMessages();
        }

        public void SetChat(int chatId)
        {
            currentChatId = chatId;
            state = MessageViewState.Chat;

            currentMessageTask = beginDisplayingMessages().ContinueWith(t => Console.WriteLine("Message fetcher stopped"));
        }

        private async Task beginDisplayingMessages()
        {
            //Stop fetching messages in previous channel. We cannot continue until this is truly completed
            await stopFetchingMessages();

            //Repopulate window with new messages
            clearChat();
            await populateChat();

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

            if (currentMessageTask != null)
            {
                await currentMessageTask;
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
                        //messageEvents = messagesApi.GetMessageEvents((int)currentChannelId, latestMessageTime);
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
                            addMessage(msgEvent.Message);
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

        private async Task populateChat()
        {
            IList<GetMessageDTO> messages = null;
            try
            {
                if (state == MessageViewState.Channel && currentChannelId != null)
                {
                    messages = await messagesApi.GetMessagesAsync((int)currentChannelId, 0, 25, messageFetchCancellation.Token);
                    //messages = messagesApi.GetMessages((int)currentChannelId, 0, 25);
                }
                
                foreach (GetMessageDTO message in messages)
                {
                    addMessage(message);
                }
            }
            catch (HttpOperationException er)
            {
                throw er;
            }
        }

        /// <summary>
        /// Adds a new message to the message view.
        /// </summary>
        /// <param name="message">The message to add</param>
        private void addMessage(GetMessageDTO message)
        {
            messages.Add(message);
            latestMessageTime = message.CreationTime;
            svMessages.ScrollToBottom();
        }

        private void deleteMessage(GetMessageDTO message)
        {
            int replaceIndex = messages.IndexOf(messages.SingleOrDefault(m => m.Id == message.Id));
            if (replaceIndex != -1)
            {
                message.Content = ("This message has been deleted on the " + "\n" + ((DateTime)message.DeletionDate).ToLocalTime());
                messages[replaceIndex] = message;
            }

            latestMessageTime = (DateTime)message.DeletionDate;
        }

        private void updateMessage(GetMessageDTO message)
        {
            int replaceIndex = messages.IndexOf(messages.SingleOrDefault(m => m.Id == message.Id));
            if (replaceIndex != -1)
            {
                messages[replaceIndex] = message;
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

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            var messageContentDTO = new MessageContentDTO()
            {
                Message = txtMessage.Text
            };

            try
            {
                messagesApi.CreateMessage((int)currentChannelId, messageContentDTO);

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (HttpOperationException er)
            {
                //TODO: Exception handling
                throw er;
            }
        }

        private void miEditMessage_Click(object sender, EventArgs e)
        {

        }

        private void miDeleteMessage_Click(object sender, EventArgs e)
        {
            try
            {
                int id = selectedMessage.Id;
                messagesApi.DeleteMessage(id);
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

            selectedMessage = (GetMessageDTO)btn.DataContext;
        }
    }
}
