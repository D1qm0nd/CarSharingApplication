using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using CarSharingApplication.Windows.Moderating.EditWindows.Accidents;
using System;
using System.Configuration;
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
        private string connectionString { get; set; } = App.GetConnectionString("CARHANDLERConnection");
        public TripWindow(ref UsersINFO user, Window owner, bool showOwner)
        {
            this.Owner = owner;
            this.Owner.Visibility = Visibility.Collapsed;
            _ShowOwner = showOwner;
            _User = user;
            InitializeComponent();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            App.AppDataBase.OpenConnection(connectionString);
            _Rental = App.AppDataBase.GetScalarResult<RentalsINFO>(
                    "SELECT TOP(1) * FROM RentalsINFO " +
                    $"WHERE ID_DriverLicence = {_User.ID_DriverLicence} " +
                    "AND EndTime > GETDATE() AND RentalStatus='стандартная'");
            if (_Rental != null)
            {
                rt.SetTime(_Rental.EndTime);
                _Vehicle = App.AppDataBase.GetScalarResult<VehiclesINFO>(
                    $"SELECT * FROM VehiclesINFO WHERE ID_Vehicle = {_Rental.ID_Vehicle}");
                Card.SetVehicleInfo(_Vehicle, "Ошибка загрузки данных");
            }
            else
            {
                this.Close();
            }
            App.AppDataBase.CloseConnection();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
            if (_ShowOwner == true)
            {
                this.Owner.Show();
                this.Owner.Visibility = Visibility.Visible;
                this.Owner.Activate();
            }
            else 
            {
                this.Owner?.Close();
            }
        }

        private void FinishTripClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = MessageBox.Show("Вы уверены что хотите завершить поездку раньше срока?", "Закончить поездку", MessageBoxButton.YesNo);
                if (a == MessageBoxResult.Yes)
                {
                    App.AppDataBase.OpenConnection(connectionString);
                    if(App.AppDataBase.ExecuteNonQuery(
                        $"UPDATE Rentals SET RentalStatus='досрочная' WHERE ID_Rental = {_Rental.ID_Rental}"))
                        App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Досрочно окончил аренду ТС {_Rental.ID_Vehicle}", null, LogType.UserAction));
                    App.AppDataBase.CloseConnection();
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
