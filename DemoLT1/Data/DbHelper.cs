using DemoLT1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoLT1.Data
{
    public static class DbHelper
    {
        public static User Authorize(string login, string password)
        {
            try
            {
                
            }
            catch
            {
                MessageBox.Show("Введен неверный логин или пароль!", 
                    "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return null;
        }
    }
}
