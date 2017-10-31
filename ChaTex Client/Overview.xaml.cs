using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IO.Swagger.Api;
using IO.Swagger.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window
    {

        private ObservableCollection<GroupDTO> groups;
        int CurrentChannelId;
        DateTime latestMessage;
        MessagesApi messagesApi;

        public Overview()
        {

            InitializeComponent();

            CurrentChannelId = -1;


            UsersApi usersApi = new UsersApi();
            groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());
            messagesApi = new MessagesApi();
            tvGroups.ItemsSource = groups;

        }

        private void PopulateChat()
        {
            Console.WriteLine("Populating chat");
            ClearChat();
            List<GetMessageDTO> messages = messagesApi.GetMessages(CurrentChannelId, 0, 25);

            foreach (GetMessageDTO message in messages)
            {
                AddMessage(message);
            }
        }

        private void ChannelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            Console.WriteLine("Selection change!");

            if (e.NewValue is ChannelDTO channel)
            {
                CurrentChannelId = (int)channel.Id;
                PopulateChat();

                //Start fetching new messages
                new Thread(new ThreadStart(() =>
                {
                    while(true)
                    {
                        FetchNewMessages();
                    }
                })).Start();
            }
        }

        void AddMessage(GetMessageDTO message)
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

        void ClearChat()
        {
            spnlMessages.Children.RemoveRange(0, spnlMessages.Children.Count);
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            MessagesApi messagesApi = new MessagesApi();
            messagesApi.CreateMessage(CurrentChannelId, txtMessage.Text);
            txtMessage.Clear();
        }

        private void FetchNewMessages()
        {
            MessagesApi messagesApi = new MessagesApi();
            IEnumerable<GetMessageDTO> messages = messagesApi.GetMessagesSince(CurrentChannelId, latestMessage);

            //Add to ui when the ui thread is ready
            Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate ()
            {
                AddNewMessages(messages);
            });
        }

        private void AddNewMessages(IEnumerable<GetMessageDTO> messages)
        {
            foreach (GetMessageDTO msg in messages)
            {
                AddMessage(msg);
            }

        }

        private void btnEditChannel_Click(object sender, RoutedEventArgs e)
        {
            var wEditChannel = new EditChannel();
            wEditChannel.ShowDialog();
        }
        
        private void btnNewGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGroup createNewGroup = new CreateNewGroup();
            createNewGroup.ShowDialog();
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSendMessage.IsEnabled = txtMessage.Text.Length > 0;
        }
    }
}
