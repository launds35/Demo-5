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
using Microsoft.Win32;

namespace DemoLT1
{
    /// <summary>
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private bool IsEditing = false;
        private bool IsPhotoEditing = false;

        private Good CurrentGood;
        private int CurrentId;
        private string TargetPath;
        private string FileName;

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
            TargetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures", $"{CurrentId}.jpg");
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
            CurrentId = CurrentGood.Id;
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
            CurrentId = DbHelper.GetMaxId() + 1;
        }

        //Функция для кнопки выбора изображения
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png";
            openFileDialog.Title = "Выбор изображения для товара";

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            FileName = openFileDialog.FileName;


            var origImage = new BitmapImage();
            origImage.BeginInit();
            origImage.UriSource = new Uri(FileName);
            origImage.CacheOption = BitmapCacheOption.OnLoad;
            origImage.EndInit();

            if (origImage.PixelWidth > 300 || origImage.PixelHeight > 200)
            {
                MessageBox.Show("Изображение не может быть больше чем 300х200 пикселей!", "Ошибка загрузки изображения",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                IsPhotoEditing = true;
            }
            ImageBox.Source = origImage;

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Подтвердить удаление товара?", "Удаление товара",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (DbHelper.DeleteGood(CurrentGood.Id))
                {
                    var targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures", $"{CurrentId}.jpg");

                    if (File.Exists(targetPath))
                    {
                        File.Delete(targetPath);
                    }
                    MessageBox.Show("Успешное удаление товара!", "Удаление товара",
                MessageBoxButton.OK, MessageBoxImage.Information);
                    IdLabel.Content = string.Empty;
                    ArticleBox.Text = string.Empty;
                    DescriptionBox.Text = string.Empty;
                    UnitOfMeasureBox1.Text = string.Empty;
                    PriceBox.Text = string.Empty;
                    CategoryBox.SelectedIndex = 0;
                    FabricBox.SelectedIndex = 0;
                    SupplierBox.SelectedIndex = 0;
                    DiscountBox.Text = string.Empty;
                    CountBox.Text = string.Empty;
                    LabelBox.Text = string.Empty;
                }
            }
        }

        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryBox.SelectedIndex == 0 ||
               FabricBox.SelectedIndex == 0 ||
               SupplierBox.SelectedIndex == 0 ||
               DiscountBox.Text == string.Empty ||
               CountBox.Text == string.Empty ||
               PriceBox.Text == string.Empty)
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            string photo = null;

            if (IsPhotoEditing)
                photo = $"{CurrentId}.jpg";

            var good = new Good
            {
                Id = CurrentId,
                Article = ArticleBox.Text,
                Description = DescriptionBox.Text,
                GoodName = LabelBox.Text,
                UnitOfMeasure = UnitOfMeasureBox1.Text,
                IdSupplier = SupplierBox.SelectedIndex,
                IdFabric = FabricBox.SelectedIndex,
                IdCategory = CategoryBox.SelectedIndex,
                Discount = Convert.ToInt32(DiscountBox.Text),
                Count = Convert.ToInt32(CountBox.Text),
                Price = Convert.ToDouble(PriceBox.Text),
                Photo = photo
            };

            var result = false;
            if (IsEditing)
            {
                result = DbHelper.EditGood(good);

            }
            else
            {
                result = DbHelper.AddGood(good);
                
            }
            if (result)
            {
                MessageBox.Show($"{Title}: Успех!", "Успех!",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (IsPhotoEditing)
                File.Copy(FileName, TargetPath, true);
        }

        private void PriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!char.IsDigit(e.Text, 0) && e.Text != ".")
            {
                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if(textBox.Text.Contains(".") && e.Text == ".")
            {
                e.Handled = true;
            }
        }

        private void DiscountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void CountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void ArticleBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 6)
            {
                e.Handled = true;
            }
        }
    }
}
