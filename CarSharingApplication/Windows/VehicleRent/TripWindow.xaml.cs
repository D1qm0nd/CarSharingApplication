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
        public TripWindow(UsersINFO user, Window owner, bool showOwner)
        {
            this.Owner = owner;
            _ShowOwner = showOwner;
            InitializeComponent();
            _User = user;
            _Rental = App.GetScalarResult<RentalsINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                    $"SELECT TOP(1) * FROM RentalsINFO WHERE ID_DriverLicence = {_User.ID_DriverLicence} AND EndTime > GETDATE() AND RentalStatus='стандартная'");
            if (_Rental != null)
            {
                rt.SetTime(_Rental.EndTime);
                _Vehicle = App.GetScalarResult<VehiclesINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                    $"SELECT * FROM VehiclesINFO WHERE ID_Vehicle = {_Rental.ID_Vehicle}");
                Card.SetVehicleInfo(_Vehicle, "Ошибка загрузки данных");
            }
            else this.Close();
            
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxButton button = MessageBoxButton.YesNo;

                var a = MessageBox.Show("", "Вы уверены что хотите завершить поездку раньше срока?", button);
                if (a == MessageBoxResult.Yes)
                {
                    App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                        $"UPDATE Rentals SET RentalStatus='досрочная' WHERE ID_Rental = {_Rental.ID_Rental}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                    this.Close();
            }
        }
    }
}
