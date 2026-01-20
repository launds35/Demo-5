using DemoLT1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoLT1.Data
{
    public static class DbHelper
    {
        public static List<Good> GetGoods()
        {
            try
            {
                var list = new List<Good>();
                string sql = @"SELECT g.IdGood, g.Article, g.GoodName, g.UnitOfMeasure, g.Price, 
                    g.IdSupplier, s.Supplier, g.IdFabric, f.Fabric, g.IdCategory, c.Category, 
                    g.Discount, g.Count, g.Description, g.Фото
                    FROM Goods g JOIN Fabrics f ON f.IdFabric = g.IdFabric join Suppliers s on s.IdSupplier = g.IdSupplier 
                    join Categories c on c.IdCategory = g.IdCategory;";

                using (SqlConnection conn = Db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Good
                            {
                                Id = reader.GetInt32(0),
                                Article = reader.GetString(1),
                                GoodName = reader.GetString(2),
                                UnitOfMeasure = reader.GetString(3),
                                Price = reader.GetDouble(4),
                                IdSupplier = reader.GetInt32(5),
                                Supplier = reader.GetString(6),
                                IdFabric = reader.GetInt32(7),
                                Fabric = reader.GetString(8),
                                IdCategory = reader.GetInt32(9),
                                Category = reader.GetString(10),
                                Discount = reader.GetInt32(11),
                                Count = reader.GetInt32(12),
                                Description = reader.GetString(13),
                                Photo = reader.IsDBNull(14) ? null : reader.GetString(14)
                            });
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Ошибка загрузки списка из бд:\n{ex}",
                      "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
                
                return null;
            }
        }

        public static bool AddGood(Good good)
        {
            try
            {
                string sql = @"INSERT INTO Goods (IdGood, Article, GoodName, UnitOfMeasure, Price, 
                    IdSupplier, IdFabric, IdCategory, 
                    Discount, Count, Description, Фото)
                    VALUES (@IdGood, @Article, @GoodName, @UnitOfMeasure, @Price, 
                    @IdSupplier, @IdFabric, @IdCategory, 
                    @Discount, @Count, @Description, @Фото);";

                using (SqlConnection conn = Db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("@IdGood", good.Id);
                    cmd.Parameters.AddWithValue("@Article", good.Article);
                    cmd.Parameters.AddWithValue("@GoodName", good.GoodName);
                    cmd.Parameters.AddWithValue("@UnitOfMeasure", good.UnitOfMeasure);
                    cmd.Parameters.AddWithValue("@Price", good.Price);
                    cmd.Parameters.AddWithValue("@IdSupplier", good.IdSupplier);
                    cmd.Parameters.AddWithValue("@IdFabric", good.IdFabric);
                    cmd.Parameters.AddWithValue("@IdCategory", good.IdCategory);
                    cmd.Parameters.AddWithValue("@Discount", good.Discount);
                    cmd.Parameters.AddWithValue("@Count", good.Count);
                    cmd.Parameters.AddWithValue("@Description", good.Description);
                    cmd.Parameters.AddWithValue("@Фото", good.Photo);

                    var result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ошибка добавления товара (ID: {good.Id}):\n{ex}",
                      "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return false;
        }

        public static bool DeleteGood(int id)
        {
            try
            {
                string sql = @"Delete FROM Goods WHERE IdGood = @IdGood;";

                using (SqlConnection conn = Db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("@IdGood", id);

                    var result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }

                }
            }
            catch
            {

                MessageBox.Show($"Товар находится в заказе!",
                      $"Ошибка удаления товара (ID: {id})", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return false;
        }

        public static int GetMaxId()
        {
            try
            {
                string sql = @"SELECT MAX(IdGood) From Goods;";

                using (SqlConnection conn = Db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            return reader.GetInt32(0);
                        }
                    }

                }
            }
            catch
            {

                MessageBox.Show($"Ошибка получения максимального Id",
                      $"Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return -1;
        }


        public static bool EditGood(Good good)
        {
            try
            {
                string sql = @"
                Update Goods SET Article = @Article, GoodName = @GoodName, 
                    UnitOfMeasure = @UnitOfMeasure, Price = @Price, 
                    IdSupplier = @IdSupplier, IdFabric = @IdFabric, IdCategory = @IdCategory, 
                    Discount = @Discount, Count = @Count, Description = @Description, Фото = @Фото
                WHERE IdGood = @IdGood;";

                using (SqlConnection conn = Db.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    cmd.Parameters.AddWithValue("@IdGood", good.Id);
                    cmd.Parameters.AddWithValue("@Article", good.Article);
                    cmd.Parameters.AddWithValue("@GoodName", good.GoodName);
                    cmd.Parameters.AddWithValue("@UnitOfMeasure", good.UnitOfMeasure);
                    cmd.Parameters.AddWithValue("@Price", good.Price);
                    cmd.Parameters.AddWithValue("@IdSupplier", good.IdSupplier);
                    cmd.Parameters.AddWithValue("@IdFabric", good.IdFabric);
                    cmd.Parameters.AddWithValue("@IdCategory", good.IdCategory);
                    cmd.Parameters.AddWithValue("@Discount", good.Discount);
                    cmd.Parameters.AddWithValue("@Count", good.Count);
                    cmd.Parameters.AddWithValue("@Description", good.Description);
                    cmd.Parameters.AddWithValue("@Фото", good.Photo);

                    var result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ошибка редактирования товара (ID: {good.Id}):\n{ex}",
                      "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            return false;
        }

        public static List<ComboBoxItems> GetSuppliers()
        {
            try
            {
                var list = new List<ComboBoxItems>();
                string sql = "Select IdSupplier, Supplier from Suppliers;";

                using (SqlConnection conn = Db.GetConnection()) {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка из бд {ex}",
                    "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public static List<ComboBoxItems> GetFabrics()
        {
            try
            {
                var list = new List<ComboBoxItems>();
                string sql = "Select IdFabric, Fabric from Fabrics;";

                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                        return list;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки списка из бд",
                    "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public static List<ComboBoxItems> GetCategories()
        {
            try
            {
                var list = new List<ComboBoxItems>();
                string sql = "Select IdCategory, Category from Categories;";

                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                        return list;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки списка из бд",
                    "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public static User Authorize(string login, string password)
        {
            try
            {
                string sql = @"select u.IdRole, r.Role, u.FullName From Users u JOIN
                Roles r ON r.IdRole = u.IdRole WHERE u.Login = @login AND u.Password = @password";
                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read())
                        {
                            return new User
                            {
                                IdRole = reader.GetInt32(0),
                                Role = reader.GetString(1),
                                FullName = reader.GetString(2)
                            };
                        }
                    }
                }
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
