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
    /// Логика взаимодействия для VehicleRent.xaml
    /// </summary>
    public partial class VehicleRent : Window
    {
        public VehicleRent(VehiclesINFO rent_vehicle)
        {
            InitializeComponent();
            this.Title = $"Аренда {rent_vehicle.Brand} {rent_vehicle.Mark} {rent_vehicle.Class.TrimEnd()} {rent_vehicle.Color} ₽ {rent_vehicle.PricePerHour}";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Activate();
        }
    }
}
