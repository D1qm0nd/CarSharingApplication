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
        bool a = true;
        public VehicleRent(UsersINFO User, VehiclesINFO Vehicle)
        {
            InitializeComponent();
            this.Title = $"Аренда {Vehicle.Brand} {Vehicle.Mark} {Vehicle.Class.TrimEnd()} {Vehicle.Color} ₽ {Vehicle.PricePerHour}";
            Card.SetVehicleInfo(Vehicle,"");
            Picker.PricePerHour = (double)Vehicle.PricePerHour;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Activate();
            a = !a;
        }
    }
}
