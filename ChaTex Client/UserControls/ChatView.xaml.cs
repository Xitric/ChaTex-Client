using IO.Swagger.Api;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        ObservableCollection<ChatDTO> chats;
        //ChatAPI _chatApi = null;

        public static ChatView m_Instance = null;
       

        public static ChatView GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new ChatView();
            }
            m_Instance.populateUI();
            return m_Instance;
        }
        private ChatView()
        {
            InitializeComponent();

            //_chatApi = new ChatApi();
        }

        private void populateUI()
        {
            //chats = new ObservableCollection<ChatDTO>(_chatApi.GetAllChats());
            lstBoxChats.ItemsSource = chats;
        }

        private void txtSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchChats.Text.Length > 0)
            {
                lstBoxChats.ItemsSource = chats.Where(x => x.Name.ToLower().Contains(txtSearchChats.Text.ToLower()));
            }
            else
            {
                lstBoxChats.ItemsSource = chats;
            }
        }

        private void lstBoxChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var chat = (ChatDTO)lstBoxChats.SelectedItem;
            if (chat != null)
            {
                //_chatApi.GetMessagesBetweenUser((int)user.Id);
                ucChannelMessageView.SetChat((int)chat.Id);
            }
        }

        private void btnCreateChat_Click(object sender, RoutedEventArgs e)
        {
            CreateChat createChat = new CreateChat();
            createChat.ShowDialog();
        }
    }
}
