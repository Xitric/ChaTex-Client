using System.Windows;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Window {

        public Overview() {

            InitializeComponent();
        }
        
        private void btnNewGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGroup createNewGroup = new CreateNewGroup();
            createNewGroup.ShowDialog();
        }

        private void ucGroupViewView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
