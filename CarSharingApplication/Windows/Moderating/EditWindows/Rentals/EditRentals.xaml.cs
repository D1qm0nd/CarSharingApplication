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

namespace CarSharingApplication.Windows.Moderating.EditWindows.Rentals
{
    /// <summary>
    /// Логика взаимодействия для EditRentals.xaml
    /// </summary>
    public partial class EditRentals : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        public EditRentals()
        {
            try
            {
                InitializeComponent();
                db = new CarSharingDataBaseClassesDataContext(ConnectionString);
                dt_grid.ItemsSource = db.Rentals;
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

