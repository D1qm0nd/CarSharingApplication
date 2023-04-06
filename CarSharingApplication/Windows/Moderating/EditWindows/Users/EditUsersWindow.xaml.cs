using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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


namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для EditUsersWindow.xaml
    /// </summary>
    public partial class EditUsersWindow : Window
    {
        private string ConnectionString = (Application.Current as App).GetConnectionString("DBADMINConnection");
        
        private CarSharingDataBaseClassesDataContext db;
        public EditUsersWindow()
        {
            try 
            { 
                InitializeComponent();
                db = new CarSharingDataBaseClassesDataContext(ConnectionString);
                dt_grid.ItemsSource = db.Rental_Users;
                
                    //from user in
                    //                      ru
                    //                  join admin in ra
                    //                  on user.ID_User equals admin.ID_User select new { user.ID_User, user.UserEMail, user.UserSurname, user.UserName, user.UserMiddleName} ;
            }
            catch (SqlException sqlex) 
            {
                MessageBox.Show(sqlex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Connection.Close();
            this.Owner.Visibility = Visibility.Visible;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                db.SubmitChanges();
            }
            catch (SqlException sqlex) 
            {
                MessageBox.Show(sqlex.Message);
                dt_grid.ItemsSource = db.Rental_Users;
            }
        }
    }
}
