using DemoLT1.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DemoLT1
{
    /// <summary>
    /// Interaction logic for GoodPanel.xaml
    /// </summary>
    public partial class GoodPanel : UserControl
    {
        private Good CurrentGood;
        public event Action Editing;
        public GoodPanel(Good good, bool isAdmin)
        {
            InitializeComponent();
            if(isAdmin)
            {
                MouseDoubleClick += UserControl_MouseDoubleClick;
            }
            CurrentGood = good;
            LoadGood();
        }

        private void LoadGood()
        {
            CategoryName.Content = $"{CurrentGood.Category} | {CurrentGood.GoodName}";
            Description.Content = "Описание товара: " + CurrentGood.Description;
            Supplier.Content = "Поставщик: " + CurrentGood.Supplier;
            Fabric.Content = "Производитель: " + CurrentGood.Fabric;
            Count.Content = $"Количество на складе: {CurrentGood.Count}";
            if (CurrentGood.Count == 0) {
                Count.Background = Brushes.LightBlue;
            }
            DiscountBox.Content = CurrentGood.Discount.ToString();
            if(CurrentGood.Discount > 15)
            {
                DiscountBorder.Background = (Brush)Application.Current.Resources["Discount"];
            }
            OldPrice.Text = CurrentGood.Price.ToString("F2");

            if (CurrentGood.Discount > 0) 
            {
                NewPrice.Text = (CurrentGood.Price - CurrentGood.Price * ((double)CurrentGood.Discount / 100)).ToString("F2");
                OldPrice.Foreground = Brushes.Red;
                OldPrice.TextDecorations = TextDecorations.Strikethrough;
            } else
            {
                NewPrice.Visibility = Visibility.Collapsed;
            }
            UnitOfMeasure.Content = "Единица измерения: " + CurrentGood.UnitOfMeasure;
            if (CurrentGood.Photo != null) 
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures", CurrentGood.Photo);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(path);
                image.EndInit();

                ImageBox.Source = image;
            }
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddEditWindow window = new AddEditWindow(CurrentGood, true)
            {
                Owner = Application.Current.MainWindow
            };
            window.Closed += (s, args) =>
            {
                Editing?.Invoke();
            };
            window.ShowDialog();
        }
    }
}
