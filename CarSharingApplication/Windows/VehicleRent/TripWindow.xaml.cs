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
        private UsersINFO _User;
        private VehiclesINFO _Vehicle;
        public TripWindow(UsersINFO user)
        {
            InitializeComponent();
            _User = user;
            _Vehicle = App.GetScalarResult<VehiclesINFO>(new CarSharingDataBaseClassesDataContext("CARHANDLERConnection"),
                           $"SELECT TOP(1) * FROM RentalsINFO WHERE ID_DriverLicence = {_User.ID_DriverLicence} AND EndTime > GETDATE()");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            throw new NotImplementedException("Осуществить, VehicleRent - проверку на открытие");
        }
    }
}
