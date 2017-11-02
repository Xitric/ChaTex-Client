using IO.Swagger.Api;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for ChannelMessageView.xaml
    /// </summary>
    public partial class ChannelMessageView : UserControl
    {
        private int? CurrentChannelId;
        private DateTime latestMessage;
        private MessagesApi messagesApi;
        private Thread messageFetcherThread;
        private CancellationTokenSource cancellation;
        ObservableCollection<GetMessageDTO> messages = new ObservableCollection<GetMessageDTO>();

        public ChannelMessageView()
        {
            messagesApi = new MessagesApi();
            InitializeComponent();
            icMessages.ItemsSource = messages;
        }

        public void SetChannel(int channelId)
        {
            //Stop fetching messages in previous channel
            StopFetchingMessages();

            //Repopulate window with new messages
            CurrentChannelId = channelId;
            ClearChat();
            PopulateChat();

            //Begin listening for messages in the new channel
            BeginFetchingMessages();
        }

        /// <summary>
        /// Stop listening for new messages from the server.
        /// </summary>
        private void StopFetchingMessages()
        {
            cancellation?.Cancel();
        }

        /// <summary>
        /// Begin listening for new messages from the server.
        /// </summary>
        private void BeginFetchingMessages()
        {
            cancellation = new CancellationTokenSource();
            messageFetcherThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    while (true)
                    {
                        FetchNewMessages();
                    }
                }
                catch (TaskCanceledException) { }
            }));

            messageFetcherThread.Start();
        }

        /// <summary>
        /// Get and display new messages from the web service. This operation will block until it receives a result, and should therefore be called from a separate thread.
        /// </summary>
        private void FetchNewMessages()
        {
            IEnumerable<GetMessageDTO> messages = messagesApi.GetMessagesSince(CurrentChannelId, latestMessage, cancellation.Token);

            //Add to ui when ready
            Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate ()
            {
                foreach (GetMessageDTO msg in messages)
                {
                    AddMessage(msg);
                }
            });
        }

        /// <summary>
        /// Remove all messages from the message view.
        /// </summary>
        private void ClearChat()
        {
            messages.Clear();
        }

        private void PopulateChat()
        {
            List<GetMessageDTO> messages = messagesApi.GetMessages(CurrentChannelId, 0, 25); //TODO: Rely on default

            foreach (GetMessageDTO message in messages)
            {
                AddMessage(message);
            }
        }

        /// <summary>
        /// Adds a new message to the message view.
        /// </summary>
        /// <param name="message">The message to add</param>
        private void AddMessage(GetMessageDTO message)
        {
            messages.Add(message);
            if (message.CreationTime != null)
            {
                latestMessage = (DateTime)message.CreationTime;
            }

            svMessages.ScrollToBottom();
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSendMessage.IsEnabled = txtMessage.Text.Length > 0;
        }

        ///
        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            MessagesApi messagesApi = new MessagesApi();
            var messageContentDTO = new MessageContentDTO()
            {
                Message = txtMessage.Text
            };
            messagesApi.CreateMessage(CurrentChannelId, messageContentDTO);

            txtMessage.Clear();
            txtMessage.Focus();
        }
    }
}
