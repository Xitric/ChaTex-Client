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
        UsersApi usersApi;
        MainWindow parent;

        public Overview(MainWindow parent)
        {

            InitializeComponent();
            this.parent = parent;

            CurrentChannelId = -1;


            UsersApi usersApi = new UsersApi();
            groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());
            messagesApi = new MessagesApi();
            TViewGroups.ItemsSource = groups;

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

                //Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate ()
                //{
                    
                //});
            }
        }

        void AddMessage(GetMessageDTO message)
        {
            if (message.CreationTime != null)
            {
                latestMessage = (DateTime)message.CreationTime;
            }

            DockPanel dockPanel = new DockPanel
            {
                Height = Double.NaN,
                LastChildFill = false,
                Width = Double.NaN
            };

            Border border = new Border()
            {
                Padding = new Thickness(10),
                Background = Brushes.WhiteSmoke
            };

            StackPanel stackPanel = new StackPanel();
            border.Child = stackPanel;
            dockPanel.Children.Add(border);

            TextBlock textAuthor = new TextBlock
            {
                Text = message.Sender.FirstName + " " + message.Sender.LastName,
                FontSize = 10,
                Foreground = Brushes.DimGray
            };
            stackPanel.Children.Add(textAuthor);

            TextBlock textMessage = new TextBlock
            {
                Text = message.Content
            };
            stackPanel.Children.Add(textMessage);

            if ((bool)message.Sender.Me)
            {
                Console.WriteLine("AAA");
                DockPanel.SetDock(border, Dock.Right);
                textAuthor.FlowDirection = FlowDirection.RightToLeft;
            }

            SP.Children.Add(dockPanel);
        }

        void ClearChat()
        {
            SP.Children.RemoveRange(0, SP.Children.Count);
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            MessagesApi messagesApi = new MessagesApi();
            messagesApi.CreateMessage(CurrentChannelId, MessageField.Text);
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

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            parent.beginEditchannel();
        }

        private void NewGroupBtn_Click(object sender, RoutedEventArgs e)
        {

            //GroupDTO newGroup = new GroupDTO();
            //newGroup.Id = 112312321;
            //newGroup.Name = "TestGroup123";
            //newGroup.Channels = new List<ChannelDTO>();
            //ChannelDTO c1 = new ChannelDTO();
            //c1.Name = "Channel 123";
            //newGroup.Channels.Add(c1);
            //groups.Add(newGroup);

            CreateNewGroup createNewGroup = new CreateNewGroup();
            createNewGroup.Show();
        }

    }
}
