using BusinessLayer.Enum;
using ChaTex_Client.UserDialogs;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
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
        private readonly MessagesApi messagesApi;

        public ChannelMessageView()
        {
            messagesApi = new MessagesApi();
            InitializeComponent();
            messages = new ObservableCollection<GetMessageDTO>();
            icMessages.ItemsSource = messages;
        }

        public void SetChannel(int channelId)
        {
            currentChannelId = channelId;

            //Sets our state, so the program knows if we're in a channel or chat
            state = MessageViewState.Channel;

            //Stop fetching messages in previous channel
            stopFetchingMessages();

            //Repopulate window with new messages
            clearChat();
            populateChat();

            //Begin listening for messages in the new channel
            beginFetchingMessages();
        }

        public void SetChat(int chatId)
        {
            state = MessageViewState.Chat;
            currentChatId = chatId;

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
            messageFetcherThread.Start();
        }

        /// <summary>
        /// Get message events from the web service and update the message view. This operation will block until it receives a result, and should therefore be called from a separate thread.
        /// </summary>
        private void fetchNewMessages()
        {
            try
            {
                IEnumerable<MessageEventDTO>  messageEvents = messagesApi.GetMessageEvents(currentChannelId, latestMessage, cancellation.Token);

                if (state == MessageViewState.Channel)
                {
                    messagesApi.GetMessageEvents(currentChannelId, latestMessage, cancellation.Token);
                }

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
                new ExceptionDialog(er).ShowDialog();
            }
        }


        /// <summary>
        /// Remove all messages from the message view.
        /// </summary>
        private void clearChat()
        {
            messages.Clear();
        }

        private void populateChat()
        {
            List<GetMessageDTO> messages = null;
            try
            {
                if (state == MessageViewState.Channel)
                {
                    messages = messagesApi.GetMessages(currentChannelId, 0, 25); //TODO: Rely on default
                }

                foreach (GetMessageDTO message in messages)
                {
                    addMessage(message);
                }
            }
            catch (ApiException er)
            {
                new ExceptionDialog(er).ShowDialog();
            }
        }

        /// <summary>
        /// Adds a new message to the message view.
        /// </summary>
        /// <param name="message">The message to add</param>
        private void addMessage(GetMessageDTO message)
        {
            messages.Add(message);
            latestMessage = (DateTime)message.CreationTime;
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
                messagesApi.CreateMessage(currentChannelId, messageContentDTO);

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (ApiException er)
            {
                new ExceptionDialog(er).ShowDialog();
            }
        }

        private void miEditMessage_Click(object sender, EventArgs e)
        {

        }

        private void miDeleteMessage_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)selectedMessage.Id;
                messagesApi.DeleteMessage(id);
            }
            catch (ApiException er)
            {
                new ExceptionDialog(er).ShowDialog();
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
