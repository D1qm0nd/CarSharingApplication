using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using MaterialDesignThemes.Wpf;
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

namespace CarSharingApplication.Windows.Moderating.EditWindows.Users
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccount.xaml
    /// </summary>
    public partial class PersonalAccount : Window
    {
        private string connectionString = App.GetConnectionString("USERHANDLERConnection");
        private UsersINFO User;
        public PersonalAccount(ref UsersINFO UserInfo)
        {
            InitializeComponent();
            User = UserInfo;
            SetHints();
            uDriverLicence.Text = User.ID_DriverLicence;
            uLicenceDatePic.Text = User.ReceiptDate.ToString();
            App._Logger.Log(new LogMessage((ulong)User.ID_User, this.Title, "Пользователь зашёл личный кабинет", null, null));

            // позволить изменять юзера только по (его собственному айди)
        }

        public void SetHints() //Переложить это на WPF
        {
            HintAssist.SetHint(uDriverLicence, "Водительское удостоверение");
            HintAssist.SetHint(uLicenceDatePic, "Дата получения удостоверения");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
        }



        private void CommitUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            App._Logger.Log(new LogMessage((ulong)User.ID_User, this.Title, "Пользователь нажал на кнопку подтвердить", null, null));
            try {
                if (uDriverLicence.Text != "" && uLicenceDatePic.Text != "")
                {
                    #region NEW CODE SUPPORT FROM 25.04.2023
                    {
                        if (uDriverLicence.Text != User.ID_DriverLicence && DateTime.Parse(uLicenceDatePic.Text) != User.ReceiptDate)
                        {
                            App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(connectionString),
                                $"AddDriverLicenceToUser @User_ID = {User.ID_User}, @DriverLicence = '{uDriverLicence.Text}', @ReceiptDate = '{uLicenceDatePic.Text}'");
                            User.ID_DriverLicence = uDriverLicence.Text;
                            User.ReceiptDate = DateTime.Parse(uLicenceDatePic.Text);
                            var categories = uLicenceCategories.GetGategories();
                            foreach (DriverLicencesCategories category in categories)
                            {
                                if (category.ReceiptDate != "" && category.EndDate != "")
                                {
                                    if (!App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(connectionString),
                                        $"EXEC AddCategoryToDriverLicence @DriverLicence_ID='{uDriverLicence.Text}', @Category='{category.Name}', @ReceiptDate = '{category.ReceiptDate}', @EndDate = '{category.EndDate}'"))
                                    {
                                        MessageBox.Show($"Что-то пошло не так, проверьте введённые полей связанных с категорией {category.Name}");
                                        throw new Exception("Ошибка добавления категории водительского удостоверения");
                                    }
                                } else 
                                {
                                    MessageBox.Show($"Что-то пошло не так, проверьте введённые полей связанных с категорией {category.Name}");
                                    throw new Exception("Ошибка добавления категории водительского удостоверения");
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                App._Logger.Log(new LogMessage((ulong)User.ID_User, this.Title, "Пользовалетю не удалось добавить категорию", ex.Message, null));
            } 
        }
    }
}
