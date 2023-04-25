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
        private List<DriversLicences> Licence;
        private Rental_Users User;
        public PersonalAccount(ref Rental_Users user)
        {
            InitializeComponent();
            User = user;
            SetHints();
            Licence = App.GetQueryResult<DriversLicences>(new CarSharingDataBaseClassesDataContext(connectionString), $"EXEC GetDriverLicenceByUserID @User_ID = {user.ID_User}");
            if (Licence.Count > 0)
            { 
                uDriverLicence.Text = Licence[0].ID_DriverLicence;
                uLicenceDatePic.Text = Licence[0].ReceiptDate.ToString();
            }
            //a.Clear();
            GC.Collect();
            // позволить изменять юзера только по (его собственному айди)
        }

        public void SetHints()
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
            if (uDriverLicence.Text != "" && uLicenceDatePic.Text != "")
            {
                if (Licence != null)
                {
                    if (Licence.Count > 0)
                    {
                        if (Licence[0].ReceiptDate.ToString() != uLicenceDatePic.Text && Licence[0].ID_DriverLicence != uDriverLicence.Text)
                        {
                            App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(connectionString),
                                $"AddDriverLicenceToUser @User_ID = {User.ID_User} @DriverLicence = {uDriverLicence.Text} @ReceiptDate = {uLicenceDatePic.Text}");
                        }
                        try
                        {
                            var newLicence = App.GetQueryResult<DriversLicences>(new CarSharingDataBaseClassesDataContext(connectionString),
                                $"EXEC GetDriverLicenceByUserID @User_ID = {User.ID_User}")[0];
                            var categories = uLicenceCategories.GetGategories();
                            foreach (DriverLicencesCategories category in categories)
                            {
                                if (!App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(connectionString),
                                    $"EXEC AddCategoryToDriverLicence @DriverLicence_ID='{uDriverLicence.Text}', @Category='{category.Name}', @ReceiptDate = '{category.ReceiptDate}', @EndDate = '{category.EndDate}'"))
                                {
                                     throw new Exception($"Что-то пошло не так, проверьте введённые полей связанных с категорией {category.Name}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
