using IO.Swagger.Api;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
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

        public ChannelMessageView()
        {
            messagesApi = new MessagesApi();
            InitializeComponent();
        }

        public void SetChannel(int channelId)
        {
            CurrentChannelId = channelId;
            ClearChat();
            PopulateChat();

            //TODO: Interrupt

            messageFetcherThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    FetchNewMessages();
                }
            }));

            messageFetcherThread.Start();
        }

        /// <summary>
        /// Remove all messages from the message view.
        /// </summary>
        private void ClearChat()
        {
            spnlMessages.Children.RemoveRange(0, spnlMessages.Children.Count);
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
            if (message.CreationTime != null)
            {
                latestMessage = (DateTime)message.CreationTime;
            }

            DockPanel dpnlMessage = new DockPanel
            {
                Height = Double.NaN,
                LastChildFill = false,
                Width = Double.NaN
            };

            Border bMessageBorder = new Border()
            {
                Padding = new Thickness(10),
                Background = Brushes.WhiteSmoke
            };

            StackPanel spnlMessageText = new StackPanel();
            bMessageBorder.Child = spnlMessageText;
            dpnlMessage.Children.Add(bMessageBorder);

            TextBlock txtbMessageAuthor = new TextBlock
            {
                Text = message.Sender.FirstName + " " + (message.Sender.MiddleInitial == null ? "" : message.Sender.MiddleInitial + ". ") + message.Sender.LastName,
                FontSize = 10,
                Foreground = Brushes.DimGray
            };
            spnlMessageText.Children.Add(txtbMessageAuthor);

            TextBlock txtbMessageContent = new TextBlock
            {
                Text = message.Content
            };
            spnlMessageText.Children.Add(txtbMessageContent);

            if ((bool)message.Sender.Me)
            {
                DockPanel.SetDock(bMessageBorder, Dock.Right);
                txtbMessageAuthor.FlowDirection = FlowDirection.RightToLeft;
            }

            spnlMessages.Children.Add(dpnlMessage);
            svMessages.ScrollToBottom();
        }

        /// <summary>
        /// Get and display new messages from the web service. This operation will block until it receives a result, and should therefore be called from a separate thread.
        /// </summary>
        private void FetchNewMessages()
        {
            MessagesApi messagesApi = new MessagesApi();
            IEnumerable<GetMessageDTO> messages = messagesApi.GetMessagesSince(CurrentChannelId, latestMessage);

            //Add to ui when the ui thread is ready
            Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate ()
            {
                foreach (GetMessageDTO msg in messages)
                {
                    AddMessage(msg);
                }
            });
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSendMessage.IsEnabled = txtMessage.Text.Length > 0;
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            MessagesApi messagesApi = new MessagesApi();
            messagesApi.CreateMessage(CurrentChannelId, new MessageContentDTO() { Message = txtMessage.Text });
            txtMessage.Clear();
        }
    }
}
