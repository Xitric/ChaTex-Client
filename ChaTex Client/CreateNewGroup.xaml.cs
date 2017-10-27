using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
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

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for CreateNewGroup.xaml
    /// </summary>
    public partial class CreateNewGroup : Window
    {
       
        public CreateNewGroup()
        {
            InitializeComponent();
            //  List<GroupDTO> Groups = new List<GroupDTO>();
            // UsersApi usersApi = new UsersApi();
            // Groups = usersApi.GetGroupsForUser();
            //LBroles.ItemsSource = Groups;
            //LBmembers.ItemsSource = Groups;
            List<UserDTO> DummyList = new List<UserDTO>();
            int ulenght = 20;
            UserDTO[] users = new UserDTO[ulenght];

            for (int i = 0; i < ulenght; i++)
            {
                users[i] = new UserDTO();
                users[i].Id = i;
                users[i].FirstName = "User " + i;
                DummyList.Add(users[i]);
            }

            LBroles.ItemsSource = DummyList;
            LBusers.ItemsSource = DummyList;

        }

        List<UserDTO> usersToAdd = new List<UserDTO>();
       
            private void Button_Click(object sender, RoutedEventArgs e)
              {
            IGroupsApi groupsApi = new GroupsApi();

           
            //IChannelsApi channelApi = new ChannelsApi();
             try
             {

               // groupsApi.CreateGroup(new CreateGroupDTO()
                //{
                  //  AllowEmployeeAcknowledgeable = false, AllowEmployeeBookmark = false, AllowEmployeeSticky = false, GroupName = TBgroupName.Text
                //});

                /*
                 foreach (UserDTO item in LBusers.Items)
                 { 
                     usersToAdd.Add(item);
                     Console.WriteLine(item.FirstName);
                   //  Console.WriteLine(item);
                 }
                 */
                



      

                
                // Console.WriteLine(LBusers.SelectedItems.Count);
                //Console.WriteLine(LBroles.SelectedItems.Count);

                /*  foreach(UserDTO user in LBusers.SelectedItems)
                   {
                       if (LBusers.IsEnabled)
                       {
                           Console.WriteLine("uwporeje");
                       }
                   }*/
               // Console.WriteLine(LBusers.SelectedItems.Count());
               
               /* foreach (UserDTO user in LBusers.SelectedItems)
                {
                    usersToAdd.Add(user);
                    Console.WriteLine(usersToAdd.Count());
                    Console.WriteLine(user.FirstName);
                }*/
                /*
                foreach(UserDTO user in usersToAdd)
                {
                    Console.WriteLine(user.FirstName);
                }*/

                //if some boxes are checked for roles
                //make a new list roles for roleIDs
                //groupApi.AddRolesToGroup(thenewgroup.Id, newListRoles)

                //checkboxes should be members not in checked roles
                //if some boxes are checked for members
                //make a new list users for userIDs
                //groupApi.AddUsersToGroup (thenewgroup.ID, newListUsers)
             //   usersToAdd.Clear();
              //  LBusers.SelectedIndex = -1;
            }
            catch (ApiException er)
            {
                    throw er;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
