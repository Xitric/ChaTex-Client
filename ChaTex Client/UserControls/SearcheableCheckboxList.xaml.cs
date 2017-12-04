using ChaTex_Client.ViewModels;
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
    /// Interaction logic for SearcheableCheckboxList.xaml
    /// </summary>
    public partial class SearcheableCheckboxList : UserControl
    {
        //Define Header and HasBonus properties on control
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(SearcheableCheckboxList));
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private readonly ObservableCollection<SearcheableCheckboxViewModel> visibleRows;
        private List<SearcheableCheckboxViewModel> allRows;

        public SearcheableCheckboxList()
        {
            InitializeComponent();

            allRows = new List<SearcheableCheckboxViewModel>();
            visibleRows = new ObservableCollection<SearcheableCheckboxViewModel>();
            icCheckboxes.ItemsSource = visibleRows;
        }

        public void Clear()
        {
            allRows.Clear();
            visibleRows.Clear();
            txtSearch.Clear();
        }

        public void SetData<T>(List<T> data) where T : SearcheableCheckboxViewModel
        {
            allRows = new List<SearcheableCheckboxViewModel>(data);
            allRows.Sort((first, second) =>
            {
                return string.Compare(first.Content, second.Content);
            });

            foreach (SearcheableCheckboxViewModel row in allRows)
            {
                visibleRows.Add(row);
            }
        }

        public void SelectWhere(Func<int, bool> filter)
        {
            foreach (SearcheableCheckboxViewModel row in allRows)
            {
                row.IsSelected = filter(row.Value);
            }
        }

        public List<int?> GetSelectedValues()
        {
            return allRows.Where(r => r.IsSelected).Select(r => (int?)r.Value).ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            visibleRows.Clear();

            foreach (SearcheableCheckboxViewModel row in allRows)
            {
                bool match = txtSearch.Text.Length == 0 ||
                    row.Content.IndexOf(txtSearch.Text, StringComparison.InvariantCultureIgnoreCase) >= 0;

                if (match)
                {
                    visibleRows.Add(row);
                }
            }
        }

        private void bRow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Border row = sender as Border;
            SearcheableCheckboxViewModel rowViewModel = row.DataContext as SearcheableCheckboxViewModel;

            rowViewModel.IsSelected = !rowViewModel.IsSelected;
        }
    }
}
