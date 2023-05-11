using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace CarSharingApplication.Windows.Moderating.EditWindows.Users
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccount.xaml
    /// </summary>
    public partial class PersonalAccount : Window
    {
        private string connectionString = App.GetConnectionString("USERHANDLERConnection");
        private UsersINFO _User;
        public PersonalAccount(ref UsersINFO UserInfo)
        {
            InitializeComponent();
            _User = UserInfo;
            uDriverLicence.Text = _User.ID_DriverLicence;
            uLicenceDatePic.Text = _User.ReceiptDate.ToString();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            var hascategories = App.GetQueryResult<DriversLicencesCategoriesINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("DLHANDLERConnection")),
                $"SELECT * FROM [dbo].GetDriverLicenceCategories('{uDriverLicence.Text}')");
            if (hascategories.Count > 0)
                uLicenceCategories.SetCategories(hascategories);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
        }


        /// <summary>
        /// Нажатие подтверждения изменений информации о водительском удостоверении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommitUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (uDriverLicence.Text == "" && uLicenceDatePic.Text == "") 
                    return;
                #region NEW CODE SUPPORT FROM 25.04.2023
                {
                    var LicenceCategories = App.GetQueryResult<string>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("DLHANDLERConnection")),
                            $"SELECT * FROM [dbo].GetDriverLicenceCategories ('{uDriverLicence.Text}')");
                    if (LicenceCategories.Count == 0) 
                    { 
                        App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(connectionString),
                            $"AddDriverLicenceToUser " +
                            $"@User_ID = {_User.ID_User}, " +
                            $"@DriverLicence = '{uDriverLicence.Text}', " +
                            $"@ReceiptDate = '{uLicenceDatePic.Text}'");
                        _User.ID_DriverLicence = uDriverLicence.Text;
                        _User.ReceiptDate = DateTime.Parse(uLicenceDatePic.Text);
                    }
                    var categories = uLicenceCategories.GetGategories();
                    foreach (DriverLicencesCategories category in categories)
                    {
                        if (category.ReceiptDate == "" && category.EndDate == "")
                        {
                            MessageBox.Show($"Что-то пошло не так, проверьте введённые поля связанных с категорией {category.Name}");
                            return;
                        }
                        if (LicenceCategories.Contains(category.Name.Trim().ToLower()))
                            continue;
                        if (DateTime.Parse(category.ReceiptDate) > DateTime.UtcNow || DateTime.Parse(category.EndDate) < DateTime.UtcNow)
                        {
                            MessageBox.Show($"Что-то пошло не так, проверьте введённые поля связанных с категорией {category.Name}");
                            return;
                        }

                        if (!App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(connectionString),
                            $"EXEC AddCategoryToDriverLicence " +
                            $"@DriverLicence_ID='{uDriverLicence.Text}', " +
                            $"@Category='{category.Name}', " +
                            $"@ReceiptDate = '{category.ReceiptDate}', " +
                            $"@EndDate = '{category.EndDate}'"))
                        {
                            MessageBox.Show("Ошибка добавления категории водительского удостоверения");
                            return;
                        }
                        else 
                            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Добавил категорию {category} к правам {_User.ID_DriverLicence}", null, LogType.UserAction)); 
                    } 
                }
                #endregion
            }
            catch (SqlException sqlex)
            {
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Пользовалетю не удалось добавить категорию", sqlex.Message, LogType.UserMistake | LogType.DataBaseError));
            } 
        }
    }
}
