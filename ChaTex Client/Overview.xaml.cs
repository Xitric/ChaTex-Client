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

namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {
        int CurrentChannelId;
        MessagesApi messagesApi;
        UsersApi usersApi;
        public Overview() {
            InitializeComponent();

            CurrentChannelId = -1;

            List<GroupDTO> groups = new List<GroupDTO>();
           
            UsersApi usersApi = new UsersApi();       
            groups = new ObservableCollection<GroupDTO>(usersApi.GetGroupsForUser());
            messagesApi = new MessagesApi(); 
            TViewGroups.ItemsSource = groups;

            AddMessage(new GetMessageDTO
            {
                Content = "Test1",
                Sender = new UserDTO
                {
                    FirstName = "FName1",
                    LastName = "LName1"
                }
            });

            AddMessage(new GetMessageDTO
            {
                Content = "Test2",
                Sender = new UserDTO
                {
                    FirstName = "FName2",
                    LastName = "LName2"
                }
            });

            AddMessage(new GetMessageDTO
            {
                Content = "Test3",
                Sender = new UserDTO
                {
                    FirstName = "FName1",
                    LastName = "LName1"
                }
            });
        }

        private void PopulateChat() {
            Console.WriteLine("Populating chat");
            ClearChat();
            List<GetMessageDTO> messages = new List<GetMessageDTO>();//messagesApi.GetMessages(CurrentChannelId);
            messages.Add(new GetMessageDTO
            {
                Content = "Test3",
                Sender = new UserDTO
                {
                    FirstName = "FName1",
                    LastName = "LName1"
                }
            });
            messages.Add(new GetMessageDTO
            {
                Content = "Test2",
                Sender = new UserDTO
                {
                    FirstName = "FName2",
                    LastName = "LName2"
                }
            });
            foreach (GetMessageDTO message in messages)
            {
                AddMessage(message);
            }
        }

        private void ChannelSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e) {
            Console.WriteLine("Selection change!");
            
            if (e.NewValue is ChannelDTO channel)
            {
                CurrentChannelId = (int)channel.Id;
                PopulateChat();

            }
        }

        void AddMessage(GetMessageDTO message) {
            DockPanel dockPanel = new DockPanel
            {
                Height = Double.NaN,
                LastChildFill = false,
                Width = Double.NaN
            };

            StackPanel stackPanel = new StackPanel();
            dockPanel.Children.Add(stackPanel);

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

            //if(message.Sender.Me)
            //{
            //    DockPanel.SetDock(stackPanel, Dock.Right);
            //    textAuthor.FlowDirection = FlowDirection.RightToLeft;    
            //}

            SP.Children.Add(dockPanel);
        }

        void ClearChat() {
            SP.Children.RemoveRange(0, SP.Children.Count);
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

              CreateNewGroup = new CreateNewGroup();
             CreateNewGroup.Show();
        }

    }
}
