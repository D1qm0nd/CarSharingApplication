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

namespace CarSharingApplication.Windows.Moderating.EditWindows.Vehicles
{
    /// <summary>
    /// Логика взаимодействия для EditRegistrationCertificates.xaml
    /// </summary>
    public partial class EditRegistrationCertificates : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        public EditRegistrationCertificates()
        {
            try
            {
                InitializeComponent();
                db = new CarSharingDataBaseClassesDataContext(ConnectionString);
                dt_grid.ItemsSource = db.VehicleRegistrCertificates;
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
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
                dt_grid.ItemsSource = db.VehicleRegistrCertificates;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
        }
    }
}
