using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
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



namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for EditChannel.xaml
    /// </summary>
    public partial class EditChannel : Window
    {

     
        public EditChannel()
        {
            InitializeComponent();
        }   
        ChannelsApi chApi = new ChannelsApi();


        //MessageBox for deleting a channel
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            

 //           chApi.DeleteChannel();

            MessageBoxResult deleteChannel = MessageBox.Show("All files will be lost if Channel is deleted. Are you sure you want to delete this Channel? ", "Delete Channel", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(deleteChannel == MessageBoxResult.Yes){
                //er skal insættes funktion til at slette en channel hvis der bliver trykket yes!
                


                //a message box to display that a channel have been deleted 
                MessageBoxResult exitEditChannel = MessageBox.Show("this channel have been succesfully deleted.", "Channel deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    {
                    if(exitEditChannel == MessageBoxResult.OK)
                    {
                        this.Close();  //the edit window will be closed upon sucseful deleting channel
                    }
                }
            }
            else if(deleteChannel == MessageBoxResult.No){
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

  //          chApi.UpdateChannel();


            this.Close();
        }
    }
}
