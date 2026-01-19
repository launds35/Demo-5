using DemoLT1.Models;
using DemoLT1.Data;
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
using System.IO;

namespace DemoLT1
{
    /// <summary>
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private bool IsEditing = false;
        private Good CurrentGood;
        public AddEditWindow(Good good, bool isEditing)
        {
            InitializeComponent();
            CurrentGood = good;
            IsEditing = isEditing;
            InitGoodComboBoxes();
            if (IsEditing)
            {
                InitEditWindow();
            }
            else
            {
                InitAddWindow();
            }
        }

        private void InitGoodComboBoxes()
        {
            var categories = DbHelper.GetCategories();
            var fabrics = DbHelper.GetFabrics();
            var suppliers = DbHelper.GetSuppliers();
            var item = new ComboBoxItems
            {
                Id = 0,
                Name = "Не выбрано"
            };
            categories.Insert(0, item);
            fabrics.Insert(0, item);
            suppliers.Insert(0, item);

            CategoryBox.ItemsSource = categories;
            FabricBox.ItemsSource = fabrics;
            SupplierBox.ItemsSource = suppliers;

            CategoryBox.SelectedValuePath = "Id";
            FabricBox.SelectedValuePath = "Id";
            SupplierBox.SelectedValuePath = "Id";
            CategoryBox.DisplayMemberPath = "Name";
            FabricBox.DisplayMemberPath = "Name";
            SupplierBox.DisplayMemberPath = "Name";

        }
        private void InitEditWindow()
        {
            AddEditButton.Content = "Сохранить изменения";
            Title = "Редактирование товара";
            IdLabel.Content = $"ID товара: {CurrentGood.Id}";
            ArticleBox.Text = CurrentGood.Article;
            DescriptionBox.Text = CurrentGood.Description;
            UnitOfMeasureBox1.Text = CurrentGood.UnitOfMeasure;
            PriceBox.Text = CurrentGood.Price.ToString("F2");
            CategoryBox.SelectedIndex = CurrentGood.IdCategory;
            FabricBox.SelectedIndex = CurrentGood.IdFabric;
            SupplierBox.SelectedIndex = CurrentGood.IdSupplier;
            DiscountBox.Text = CurrentGood.Discount.ToString();
            CountBox.Text = CurrentGood.Count.ToString();
            LabelBox.Text = CurrentGood.GoodName;

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
        private void InitAddWindow()
        {
            Title = "Добавление товара";
            DeleteButton.Visibility = Visibility.Collapsed;
            IdLabel.Visibility = Visibility.Collapsed;
            CategoryBox.SelectedIndex = 0;
            FabricBox.SelectedIndex = 0;
            SupplierBox.SelectedIndex = 0;

        }

        //Функция для кнопки выбора изображения
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            var good = new Good
            {
                Article = ArticleBox.Text,
            };
        }

        private void PriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void DiscountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void CountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
