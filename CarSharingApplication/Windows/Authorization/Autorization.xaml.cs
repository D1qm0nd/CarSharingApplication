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
using CarSharingApplication.Windows.Authorization;
using CarSharingApplication.Windows.VehicleRent;
using CarSharingApplication.LogLibrary;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization.Metadata;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        private int condition = 0;
        public Autorization()
        {
            App._Logger.options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                //WriteIndented = true
            };
            App._Logger.LogPath = Environment.CurrentDirectory + @"\logs.json";
            InitializeComponent();
            Reg_Button_Click(null, null);
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
                    #region NEW CODE SUPPORT FROM 25.04.2023
                    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                    {
                        string encLogin = PasswordEncryptor.EncryptString(Login.Text);
                        string encPass = PasswordEncryptor.EncryptString(Password.Password);

                        if (App.GetQueryResult<string>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")),
                            $"SELECT UserLogin FROM Rental_Users WHERE UserLogin = '{encLogin}'").Count > 0)
                        {
                            MessageBox.Show("Логин занят");
                            return;
                        }

                        var answ = App.GetScalarResult<int>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")),
                                        $"SELECT [dbo].CheckExistingUser('{encLogin}','{encPass}')");

                        if (answ == -1)
                        {
                            using (var context = new CarSharingDataBaseClassesDataContext())
                            {
                                context.Connection.Open();
                                context.ExecuteCommand( "EXEC REG_USER " +
                                                       $"@UserLogin='{encLogin}', " +
                                                       $"@UserEmail='{Email.Text}', " +
                                                       $"@UserPassword='{encPass}', " +
                                                       $"@UserSurname='{UserSurname.Text}', " +
                                                       $"@UserName='{UserName.Text}', " +
                                                       $"@UserMiddleName='{UserMiddleName.Text}'," +
                                                       $"@UserBirthDayDate='{BDatePicker.Text}'");
                                context.Connection.Close();
                            }
                            registered = true;
                        }
                        else if (answ > 0)
                        {
                            registered = true;
                        }
                        else
                        {
                            MessageBox.Show("Проверьте корректность введённых данных");
                        }
                    }
                    #endregion
                }
                catch (SqlException SqlEx)
                {
                    App._Logger.Log(new LogMessage(null, this.Title, SqlEx.Message, SqlEx.ErrorCode.ToString(), 1));
                }
                finally
                {
                    if (registered)
                        Login_Button_Click(null, null);
                }
            }
        }

        private void MakeLogIn(object sender, RoutedEventArgs e)
        {
            try
            {
                #region NEW CODE SUPPORT FROM 25.04.2023
                if (Login.Text.Length < 5)
                    MessageBox.Show("Слишком мало символов в имени входа");
                else if (Password.Password.Length < 8)
                    MessageBox.Show("Слишком короткий пароль");
                else
                {
                    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                    {
                        string encLogin = PasswordEncryptor.EncryptString(Login.Text);
                        string encPass = PasswordEncryptor.EncryptString(Password.Password);

                        var answ = App.GetScalarResult<int>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")),
                                           $"SELECT [dbo].CheckExistingUser('{encLogin}','{encPass}')");
                        if (answ > 0)
                        {
                            this.Visibility = Visibility.Collapsed;
                            UsersINFO UserInfo = App.GetScalarResult<UsersINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")), $"SELECT * FROM UsersINFO WHERE ID_User = {answ}");
                            if (UserInfo != null)
                            {
                                if (UserInfo.Previlege.TrimEnd() == "админ")
                                {
                                    var ChoiceAuthorizationWindow = new ChoiceLoginWindow(ref UserInfo, this);
                                    ChoiceAuthorizationWindow.Activate();
                                    ChoiceAuthorizationWindow.Show();

                                }
                                else
                                {
                                    if (UserInfo.RentStatus != "в поездке")
                                    {
                                        var CarSelWindow = new CarSelector(ref UserInfo, this, true);
                                        CarSelWindow.Activate();
                                        CarSelWindow.Show();
                                    }
                                    else
                                    {
                                        var TripWND = new TripWindow(UserInfo, this, true);
                                        TripWND.Activate();
                                        TripWND.Show();
                                        this.Visibility = Visibility.Collapsed;
                                    }
                                }
                                ClearFields();
                            }
                        } 
                        else App._Logger.Log(new LogMessage(null,this.Title,"Пользователь не найден","ошибка авторизации",1));
                    }
                }
                #endregion
            }
            catch (SqlException SqlEx)
            {
                App._Logger.Log(new LogMessage(null,this.Title, SqlEx.Message, SqlEx.ErrorCode.ToString(), 1));
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
            App._Logger.Log(new LogMessage(null, this.Title, "Нажатие кнопки переход к регистрации",null,null));
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
            App._Logger.Log(new LogMessage(null, this.Title, "Нажатие кнопки переход к авторизации", null, null));
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
            char[] NotAllowed = {'@', ',', '.', '|', ' ',
                                 '\\', '/', '!', '?', '#', 
                                 '"', '$', '%', '[', ']',
                                 '{', '}', '-', '^', '~', 
                                 '`', '№', '&', '*', '\'', 
                                 '(', ')'};

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
                    App._Logger.Log(new LogMessage(null, this.Title, "Пользователь ввёл недопустимые символы", "Ошибка ввода", 0));
                }
            }
        }
    }
}
