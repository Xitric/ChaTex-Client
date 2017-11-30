using Microsoft.Rest;
using System.Windows;

namespace ChaTex_Client.UserDialogs
{
    /// <summary>
    /// Interaction logic for ExceptionDialog.xaml
    /// </summary>
    public partial class ExceptionDialog : Window
    {
        public string ErrorCode { get; set; }
        public string StackTrace { get; set; }

        public ExceptionDialog(HttpOperationException e)
        {
            ErrorCode = e.Response.StatusCode.ToString();
            StackTrace = e.StackTrace;
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
