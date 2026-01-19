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
using System.Windows.Shapes;

namespace DemoLT1
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PwdInputBox.Password;

            //User user = DbHelper.Authorize(login, password);

            User testUser = new User
            {
                IdRole = 1,
                Role = "Администратор",
                FullName = "Иванов Иван Ивановиччччч"
            };

            MainWindow window = new MainWindow(testUser);
            window.Closed += (s, args) => this.Show();
            this.Hide();
            window.Show();
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow(null);
            window.Closed += (s, args) => this.Show();
            this.Hide();
            window.Show();
        }
    }
}
