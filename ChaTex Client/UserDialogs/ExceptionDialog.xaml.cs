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

        public ExceptionDialog(/*TODO: ApiException e*/)
        {
            //TODO: ErrorCode = e.ErrorCode.ToString();
            //TODO: StackTrace = e.ToString();
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
