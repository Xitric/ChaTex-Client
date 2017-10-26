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

namespace ChaTex_Client {
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {

        public Overview() {
            InitializeComponent();

            List<GroupDTO> groups = new List<GroupDTO>();
            // Dummy data
            //Group g = new Group();
            //g.Name = "ABC";
            //g.Channels = new List<Channel>();
            //Channel c1 = new Channel();
            //c1.Name = "TC1";

            //g.Channels.Add(c1);
            //groups.Add(g);
            // End of dummy data
            UsersApi usersApi = new UsersApi();       
            groups = usersApi.GetGroupsForUser();
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

        //TODO: TView change listener to clear SP

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

            //if(message.FromMe)
            //{
            //    DockPanel.SetDock(stackPanel, Dock.Right);
            //    textAuthor.FlowDirection = FlowDirection.RightToLeft;    
            //}

            SP.Children.Add(dockPanel);
        }

        void ClearChat() {
            SP.Children.RemoveRange(0, SP.Children.Count);
        }
    }
}
