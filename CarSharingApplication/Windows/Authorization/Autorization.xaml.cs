using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using CarSharingApplication.Windows.Authorization;
using CarSharingApplication.Windows.VehicleRent;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Input;

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
            else if ((Email.Text.Length < 10) || (Email.Text.Count((c) => c == '@') != 1) || (Email.Text.Count((c) => c == '.') != 1) || (Email.Text.Contains("@.")) || (Email.Text.Contains(".@")))
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
            else if (DateTime.Parse(BDatePicker.Text) >= DateTime.UtcNow.AddYears(-18) || DateTime.Parse(BDatePicker.Text) < DateTime.UtcNow.AddYears(-100) || DateTime.Parse(BDatePicker.Text) >= DateTime.UtcNow) 
                MessageBox.Show("Некоректная дата рождения");
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

                            if (App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")),
                                "EXEC REG_USER " +
                                $"@UserLogin='{encLogin}', " +
                                $"@UserEmail='{Email.Text}', " +
                                $"@UserPassword='{encPass}', " +
                                $"@UserSurname='{UserSurname.Text}', " +
                                $"@UserName='{UserName.Text}', " +
                                $"@UserMiddleName='{UserMiddleName.Text}'," +
                                $"@UserBirthDayDate='{BDatePicker.Text}'"))
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
                    App._Logger.Log(new LogMessage(null, this.Title, SqlEx.Message, SqlEx.ErrorCode.ToString(), LogType.DataBaseError));
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
                                App._Logger.Log(new LogMessage((ulong)UserInfo.ID_User, this.Title, "Вошёл в систему", null, null));
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
                                        var TripWND = new TripWindow(ref UserInfo, this, true);
                                        TripWND.Activate();
                                        TripWND.Show();
                                        this.Visibility = Visibility.Collapsed;
                                    }
                                }
                                ClearFields();
                            }
                        } 
                        else App._Logger.Log(new LogMessage(null,this.Title,"Пользователь не найден","ошибка авторизации",LogType.UserMistake));
                    }
                }
                #endregion
            }
            catch (SqlException SqlEx)
            {
                App._Logger.Log(new LogMessage(null,this.Title, SqlEx.Message, SqlEx.ErrorCode.ToString(), LogType.DataBaseError));
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

        }
    }
}
