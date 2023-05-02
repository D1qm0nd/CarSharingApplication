using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для VehicleRent.xaml
    /// </summary>
    public partial class VehicleRent : Window
    {
        private UsersINFO _User;
        private VehiclesINFO _Vehicle;
        private bool _ShowOwner;
        public VehicleRent(UsersINFO User, VehiclesINFO Vehicle, bool showOwner)
        {
            _ShowOwner = showOwner;
            InitializeComponent();
            _User = User;
            _Vehicle = Vehicle;
            this.Title = $"Аренда {Vehicle.Brand} {Vehicle.Mark} {Vehicle.Class.TrimEnd()} {Vehicle.Color} ₽ {Vehicle.PricePerHour}";
            Card.SetVehicleInfo(Vehicle,"");
            Picker.PricePerHour = (double)Vehicle.PricePerHour;
            rentbtn.btn.Click += PayAndStart_Click;
        }

        private void PayAndStart_Click(object sender, RoutedEventArgs e)
        {
            if (rentbtn.cbox.IsChecked != true || CreditCard.isEmpty())
                return;
            App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")),
                $"EXEC Rent @DriverLicence = '{_User.ID_DriverLicence}', @ID_Vehicle = {_Vehicle.ID_Vehicle}, @RentalTime = '{DateTime.Now.ToString("HH:mm")}', @CountOfHours = {Picker.HourPicker.Value}");
            var TripWND = new TripWindow(_User,false);
            TripWND.Owner = this.Owner.Owner;
            TripWND.Activate();
            TripWND.Show();
            this.Owner.Close();//??
            _ShowOwner=false;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ShowOwner == true)
            {
                this.Owner.Visibility = Visibility.Visible;
                this.Activate();
            }
            else
            { 
                this.Owner.Close();
            }
        }
    }
}
