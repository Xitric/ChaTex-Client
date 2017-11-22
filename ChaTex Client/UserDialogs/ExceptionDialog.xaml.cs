using IO.Swagger.Client;
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

namespace ChaTex_Client.UserDialogs
{
    /// <summary>
    /// Interaction logic for ExceptionDialog.xaml
    /// </summary>
    public partial class ExceptionDialog : Window
    {
        public string ErrorCode { get; set; }
        public string StackTrace { get; set; }

        public ExceptionDialog(ApiException e)
        {
            ErrorCode = e.ErrorCode.ToString();
            StackTrace = e.ToString();
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(StackTrace);
        }
    }
}
