using DemoLT1.Data;
using DemoLT1.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoLT1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Good> Goods;
        private bool IsAdmin = false;

        public MainWindow(User user)
        {
            InitializeComponent();
            if (user == null) 
            {
                InitGuestWindow();
            }
            else
            {
                InitUserWindow(user);
            }
            Loaded += (_, __) => LoadGoods();
        }

        private void LoadGoods()
        {
            Goods = DbHelper.GetGoods();
            RefreshGoods();
        }
        private void RefreshGoods()
        {
            GoodsPanel.Children.Clear();
            if (Goods == null) { 
                return;
            }
            IEnumerable<Good> goods = Goods;

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string search = SearchBox.Text.ToLower();

                goods = goods.Where(g => (g.Article ?? "").ToLower().Contains(search) ||
                (g.Description ?? "").ToLower().Contains(search) ||
                (g.Fabric ?? "").ToLower().Contains(search) ||
                (g.Supplier ?? "").ToLower().Contains(search) ||
                (g.Category ?? "").ToLower().Contains(search) ||
                (g.GoodName ?? "").ToLower().Contains(search));
            }

            if (FilterBox.SelectedIndex > 0 && FilterBox.SelectedValue != null) { 
                int supplierId = FilterBox.SelectedIndex;
                goods = goods.Where(g => g.IdSupplier == supplierId);
            }

            switch (SortBox.SelectedIndex)
            {
                case 1:
                    goods = goods.OrderBy(g => g.Count); 
                    break;
                case 2:
                    goods = goods.OrderByDescending(g => g.Count);
                    break;
            }

            foreach (Good good in goods) 
            { 
                var item = new GoodPanel(good, IsAdmin);
                item.Editing += LoadGoods;
                GoodsPanel.Children.Add(item);
            }
        }

        private void InitGuestWindow()
        {
            BackButton.Content = "Назад";
            FullNameLabel.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Collapsed;
            SearchBox.Visibility = Visibility.Collapsed;
            FilterBox.Visibility = Visibility.Collapsed;
            SortBox.Visibility = Visibility.Collapsed;
        }

        private void InitUserWindow(User user)
        {
            FullNameLabel.Content = user.FullName;

            this.Title += $"({user.Role})";

            if (user.IdRole == 1)
            {
                InitFilterBox();
                IsAdmin = true;
            }
            else if (user.IdRole == 2) 
            {
                AddButton.Visibility = Visibility.Collapsed;
                InitFilterBox();
            } else if (user.IdRole == 3)
            {
                AddButton.Visibility = Visibility.Collapsed;
                SearchBox.Visibility = Visibility.Collapsed;
                FilterBox.Visibility = Visibility.Collapsed;
                SortBox.Visibility = Visibility.Collapsed;
            }
        }

        private void InitFilterBox()
        {
            var list = DbHelper.GetSuppliers();
            list.Insert(0, new ComboBoxItems { Id = 0, Name = "Без фильтра" });
            FilterBox.ItemsSource = list;
            FilterBox.DisplayMemberPath = "Name";
            FilterBox.SelectedValuePath = "Id";
            FilterBox.SelectedIndex = 0;

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow window = new AddEditWindow(null, false)
            {
                Owner = Application.Current.MainWindow
            };
            window.Closed += (s, args) =>
            {
                LoadGoods();
            };
            window.ShowDialog();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshGoods();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshGoods();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshGoods();
        }
    }
}
