using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using CarSharingApplication.Windows.Moderating.EditWindows.Accidents;
using System;
using System.Windows;

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
        public TripWindow(ref UsersINFO user, Window owner, bool showOwner)
        {
            this.Owner = owner;
            _ShowOwner = showOwner;
            _User = user;
            InitializeComponent();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            _Rental = App.GetScalarResult<RentalsINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                    $"SELECT TOP(1) * FROM RentalsINFO WHERE ID_DriverLicence = {_User.ID_DriverLicence} AND EndTime > GETDATE() AND RentalStatus='стандартная'");
            if (_Rental != null)
            {
                rt.SetTime(_Rental.EndTime);
                _Vehicle = App.GetScalarResult<VehiclesINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                    $"SELECT * FROM VehiclesINFO WHERE ID_Vehicle = {_Rental.ID_Vehicle}");
                Card.SetVehicleInfo(_Vehicle, "Ошибка загрузки данных");
            }
            else
            {
                this.Close();
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ShowOwner == true)
            {
                this.Owner.Show();
                this.Owner.Visibility = Visibility.Visible;
                this.Owner.Activate();
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
            }
            else 
            {
                this.Owner.Close();
            }
        }

        private void FinishTripClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = MessageBox.Show("Вы уверены что хотите завершить поездку раньше срока?", "Закончить поездку", MessageBoxButton.YesNo);
                if (a == MessageBoxResult.Yes)
                {
                    if(App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                        $"UPDATE Rentals SET RentalStatus='досрочная' WHERE ID_Rental = {_Rental.ID_Rental}"))
                        App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Досрочно окончил аренду ТС {_Rental.ID_Vehicle}", null, LogType.UserAction));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не смог отменить поездку", ex.Message, LogType.ProgramError));
            }
        }

        private void AccidentClick(object sender, RoutedEventArgs e)
        {
            var accidentWND = new AddAccident(ref _User, ref _Rental, this, true);
            accidentWND.Activate();
            accidentWND.Show();
            this.Visibility = Visibility.Collapsed;
        }
    }
}
