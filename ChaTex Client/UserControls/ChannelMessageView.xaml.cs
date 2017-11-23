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
        private MessageViewState state;
        private int? currentChannelId;
        private int? currentChatId;
        private DateTime latestMessage;
        private Thread messageFetcherThread;
        private CancellationTokenSource cancellation;
        private GetMessageDTO selectedMessage;
        private readonly ObservableCollection<GetMessageDTO> messages;
        private readonly IMessages messagesApi;

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

            SourceChanged();
        }

        public void SetChat(int chatId)
        {
            currentChatId = chatId;
            state = MessageViewState.Chat;

            SourceChanged();
        }

        private void SourceChanged()
        {
            //Stop fetching messages in previous channel
            stopFetchingMessages();

            //Repopulate window with new messages
            clearChat();
            populateChat();

            //Begin listening for messages in the new channel
            beginFetchingMessages();
        }

        /// <summary>
        /// Stop listening for new messages from the server.
        /// </summary>
        private void stopFetchingMessages()
        {
            cancellation?.Cancel();
        }

        /// <summary>
        /// Begin listening for message events from the server.
        /// </summary>
        private void beginFetchingMessages()
        {
            /*
            cancellation = new CancellationTokenSource();
            messageFetcherThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    while (true)
                    {
                        fetchNewMessages();
                    }
                }
                catch (TaskCanceledException) { }
            }));
            messageFetcherThread.Start();*/
        }

        /// <summary>
        /// Get message events from the web service and update the message view. This operation will block until it receives a result, and should therefore be called from a separate thread.
        /// </summary>
        private void fetchNewMessages()
        {
            //TODO: 
            /*
            try
            {
                IEnumerable<MessageEventDTO>  messageEvents = messagesApi.GetMessageEvents(currentChannelId, latestMessage, cancellation.Token);

                //Add to ui when ready
                Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate ()
                {
                    foreach (MessageEventDTO msgEvent in messageEvents)
                    {
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
            catch (ApiException er)
            {
                switch (er.ErrorCode)
                {
                    case 401:
                        showUnauthorizedDialog();
                        break;
                    case 404:
                        showMissingChannelDialog();
                        break;
                    default:
                        new ExceptionDialog(er).ShowDialog();
                        break;
                }
            }
            */
        }

        /// <summary>
        /// Remove all messages from the message view.
        /// </summary>
        private void clearChat()
        {
            messages.Clear();
        }

        private async void populateChat()
        {
            IList<GetMessageDTO> messages = null;
            try
            {
                if (state == MessageViewState.Channel && currentChannelId != null)
                {
                    messages = await messagesApi.GetMessagesAsync((int)currentChannelId, 0, 25); //TODO: Rely on default
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
            latestMessage = message.CreationTime;
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

            latestMessage = (DateTime)message.DeletionDate;
        }

        private void updateMessage(GetMessageDTO message)
        {
            int replaceIndex = messages.IndexOf(messages.SingleOrDefault(m => m.Id == message.Id));
            if (replaceIndex != -1)
            {
                messages[replaceIndex] = message;
            }

            latestMessage = (DateTime)message.LastEdited;
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
