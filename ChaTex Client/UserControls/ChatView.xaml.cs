using IO.ChaTex;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ChaTex_Client.UserControls
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        private readonly IUsers usersApi;

        //TODO: ObservableCollection<ChatDTO> chats;
        //ChatAPI _chatApi = null;
        public static ChatView m_Instance = null;

        public ChatView(IUsers usersApi)
        {
            this.usersApi = usersApi;

            InitializeComponent();
            //_chatApi = new ChatApi();
        }

        public void Update()
        {
            //chats = new ObservableCollection<ChatDTO>(_chatApi.GetAllChats());
            //lstBoxChats.ItemsSource = chats;
        }

        private void txtSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchChats.Text.Length > 0)
            {
                //lstBoxChats.ItemsSource = chats.Where(x => x.Name.ToLower().Contains(txtSearchChats.Text.ToLower()));
            }
            else
            {
                //lstBoxChats.ItemsSource = chats;
            }
        }

        private void lstBoxChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO: 
            /*
            var chat = (ChatDTO)lstBoxChats.SelectedItem;
            if (chat != null)
            {
                //_chatApi.GetMessagesBetweenUser((int)user.Id);
                ucChannelMessageView.SetChat((int)chat.Id);
            }*/
        }

        private void btnCreateChat_Click(object sender, RoutedEventArgs e)
        {
            CreateChat createChat = new CreateChat(usersApi);
            createChat.ShowDialog();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
    }
}
