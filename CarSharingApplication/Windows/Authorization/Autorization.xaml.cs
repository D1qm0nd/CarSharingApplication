using GMap.NET.MapProviders;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.Configuration;
using System.Data.Entity.Infrastructure.Design;
using System.Data.Entity;
using System.Xml.Serialization;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using CarSharingApplication.SQL.Linq;
using CarSharingApplication;

namespace CourseWorkProject
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        private int condition = 0;

        //private Thickness nPadding = new Thickness(0, 20, 0, 0);
        //private Thickness nMargin = new Thickness(20, 0, 20, 0);

        public Autorization()
        {
            InitializeComponent();
            SetHints();
            Reg_Button_Click(null, null);
        }

        private void SetHints()
        {
            HintAssist.SetHint(Login,"Имя для входа");
            HintAssist.SetHint(Email, "Почта");
            HintAssist.SetHint(Password, "Пароль");
            HintAssist.SetHint(RepeatPassword, "Повторите пароль");
            HintAssist.SetHint(UserSurname, "Фамилия");
            HintAssist.SetHint(UserName, "Имя");
            HintAssist.SetHint(UserMiddleName, "Отчество");
            HintAssist.SetHint(BDatePicker, "Дата рождения");

        }
        private void MakeRegister(object sender, RoutedEventArgs e)
        {
            bool registered = false;
            if (Login.Text.Length < 5)
                MessageBox.Show("Слишком мало символов в имени входа");
            else if (Email.Text.Length < 10 || !Email.Text.Contains("@") || !Email.Text.Contains("."))
                MessageBox.Show("Почта не является корректной");
            else if (Password.Password.Length < 8)
                MessageBox.Show("Слишком короткий пароль");
            else if (Password.Password != RepeatPassword.Password)
                MessageBox.Show("Пароли не совпадают");
            else if (UserSurname.Text.Length == 0)
                MessageBox.Show("Вы не ввели фамилию");
            else if (UserName.Text.Length == 0)
                MessageBox.Show("Вы не ввели имя");
            else if (UserMiddleName.Text.Length == 0)
                MessageBox.Show("Вы не ввели отчество");
            else if (BDatePicker.Text == "")
                MessageBox.Show("Вы не ввели дату рождения");
            else
            {
                try
                {
                    using (var db = new CarSharingDataBaseClassesDataContext(ConfigurationManager.ConnectionStrings["USERHANDLERConnection"].ConnectionString))
                    {
                        db.Connection.Open();
                        
                       
                        using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                        {
                            string encLogin = PasswordEncryptor.EncryptString(Login.Text);
                            string encPass = PasswordEncryptor.EncryptString(Password.Password);
                            //dec = PasswordEncryptor.DecryptString(enc);
                        

                            var request = $"SELECT COUNT(*) FROM Rental_Users WHERE UserLogin='{encLogin}' AND UserPassword='{encPass}'";

                            var answ = db.ExecuteQuery<int>(request).ToArray();

                            if (answ[0] == 0)
                            {
                                db.ExecuteCommand($"EXEC REG_USER " +
                                                  $"@UserLogin='{encLogin}', " +
                                                  $"@UserEmail='{Email.Text}', " +
                                                  $"@UserPassword='{encPass}', " +
                                                  $"@UserSurname='{UserSurname.Text}', " +
                                                  $"@UserName='{UserName.Text}', " +
                                                  $"@UserMiddleName='{UserMiddleName.Text}'," +
                                                  $"@UserBirthDayDate='{BDatePicker.Text}'");
                            }
                            else
                            {
                                ClearFields();
                                throw new Exception("Пользователь с таким именем для входа или почтой уже зарегестрирован");
                            }
                        }
                        db.Connection.Close();
                        registered = true;
                    }
                }
                catch (SqlException SqlEx)
                {
                    MessageBox.Show($"SQL ERROR {SqlEx.ErrorCode.ToString()}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (registered)
                        Login_Button_Click(null, null);
                    ClearFields();
                }
            }
        }

        private void MakeLogIn(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new CarSharingDataBaseClassesDataContext(ConfigurationManager.ConnectionStrings["USERHANDLERConnection"].ConnectionString))
                {
                    db.Connection.Open();

                    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                    {
                        string encLogin = PasswordEncryptor.EncryptString(Login.Text);
                        string encPass = PasswordEncryptor.EncryptString(Password.Password);


                        string command = $"SELECT COUNT(*) FROM Rental_Users WHERE UserLogin='{encLogin}' AND UserPassword='{encPass}'";

                        var a = db.ExecuteQuery<int>(command).ToList();
                        if (a[0] == 1)
                        {
                            command = $"SELECT * FROM Rental_Users WHERE UserLogin='{encLogin}' AND UserPassword='{encPass}'";
                            var r_user = db.ExecuteQuery<Rental_Users>(command).ToList();
                            var user = r_user[0];
                            if (PasswordEncryptor.DecryptString(user.UserPassword) == Password.Password)
                            {
                                this.Visibility = Visibility.Hidden;
                                if (db.ExecuteQuery<int>($"SELECT COUNT(*) FROM Rental_Admins WHERE ID_User = {user.ID_User}").ToArray()[0] == 1)
                                {
                                    var AdmWindow = new AdminWindow(user);
                                    AdmWindow.Owner = this;
                                    AdmWindow.Show();
                                }
                                else
                                {
                                    var CarSelWindow = new CarSelector(ref user);
                                    CarSelWindow.Owner = this;
                                    CarSelWindow.Show();
                                }
                            }
                            else
                            {
                                throw new Exception("Данные введены неверно");
                            }
                        } else 
                        {
                            throw new Exception("Пользователь не найден");
                        }
                    }
                    db.Connection.Close();
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                ClearFields();
            }
        }

        private void HideFields() 
        {
            EmailGrid.Visibility = Visibility.Collapsed;
            RepeatPasswordGrid.Visibility = Visibility.Collapsed;
            UserSurnameGrid.Visibility = Visibility.Collapsed;
            UserNameGrid.Visibility = Visibility.Collapsed;
            UserMiddleNameGrid.Visibility = Visibility.Collapsed;
            BDatePickerGrid.Visibility = Visibility.Collapsed;
        }
        private void ShowFields() 
        {
            EmailGrid.Visibility = Visibility.Visible;
            RepeatPasswordGrid.Visibility = Visibility.Visible;
            UserSurnameGrid.Visibility = Visibility.Visible;
            UserNameGrid.Visibility = Visibility.Visible;
            UserMiddleNameGrid.Visibility = Visibility.Visible;
            BDatePickerGrid.Visibility = Visibility.Visible;
        }

        private void Reg_Button_Click(object sender, RoutedEventArgs e)
        {
            if (condition != 2) 
            {
                ShowFields();
                lbl.Content = "Аутентификация";
                inputButton.Click -= MakeLogIn;
                inputButton.Click += MakeRegister;
                inputButton.Content = "Зарегистрироваться";
            }
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if (condition != 1)
            {
                HideFields();
                lbl.Content = "Авторизация";
                inputButton.Click -= MakeRegister;
                inputButton.Click += MakeLogIn;
                inputButton.Content = "Войти";
            }
        }

        private void ClearFields()
        {
            Login.Clear();
            Email.Clear();
            Password.Clear();
            RepeatPassword.Clear();
            UserSurname.Clear();
            UserName.Clear();
            UserMiddleName.Clear();
            BDatePicker.Text = "";
        }

        private void Login_TextChanged(object sender, KeyEventArgs e)
        {
            char[] NotAllowed = {'@', ',', '.', 
                                 '|', ' ', '\\', 
                                 '/', '!', '?', 
                                 '#', '"', '$', 
                                 '%', '[', ']',
                                 '{', '}', '-',
                                 '^', '~', '`', 
                                 '№', '&', '*', 
                                 '\'', '(', ')'};

            foreach (char c in NotAllowed) {
                if (Login.Text.Contains(c))
                {
                    string notallowedinstr = "";
                    foreach (char c2 in NotAllowed)
                    {
                        if (c2 != NotAllowed[NotAllowed.Length - 1]) notallowedinstr += $"'{c2}', ";
                        else notallowedinstr += $"'{c2}'";
                    }
                    MessageBox.Show($"Вы ввели символ '{c}' из перечня недопустимых:\n{notallowedinstr}", "Ввод недопусимого значения");
                    Login.Clear();
                }
            }
            
        }
    }
}
