using CarSharingApplication.SQL.Linq;
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

namespace CarSharingApplication.Windows.VehicleRent
{
    /// <summary>
    /// Логика взаимодействия для TripWindow.xaml
    /// </summary>
    public partial class TripWindow : Window
    {
        private bool _ShowOwner;
        private UsersINFO _User;
        private VehiclesINFO _Vehicle;
        private RentalsINFO _Rental;
        public TripWindow(UsersINFO user, bool showOwner)
        {
            _ShowOwner = showOwner;
            InitializeComponent();
            _User = user;
            _Rental = App.GetScalarResult<RentalsINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                           $"SELECT TOP(1) * FROM RentalsINFO WHERE ID_DriverLicence = {_User.ID_DriverLicence} AND EndTime > GETDATE() AND RentalStatus='стандартная'");
            _Vehicle = App.GetScalarResult<VehiclesINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                    $"SELECT * FROM VehiclesINFO WHERE ID_Vehicle = {_Rental.ID_Vehicle}");
            Card.SetVehicleInfo(_Vehicle,"");
            _ShowOwner = showOwner;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ShowOwner == true)
            {
                this.Owner.Show();
                this.Owner.Visibility = Visibility.Visible;
                this.Owner.Activate();
            }
            else 
            {
                this.Owner.Close();
            }
            //throw new NotImplementedException("Осуществить, VehicleRent - проверку на открытие");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                while
                (
                    !App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                                        $"UPDATE Rentals SET RentalStatus='досрочная' WHERE ID_Rental = {_Rental.ID_Rental}")
                ) ;
            }
            catch { }
            finally 
            {
                this.Close();
            }
        }
    }
}
