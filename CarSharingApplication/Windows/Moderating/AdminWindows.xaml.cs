using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.PeerToPeer;
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
using CarSharingApplication.SQL.Linq;
using CarSharingApplication.Windows.Moderating.EditWindows.Vehicles;
using CarSharingApplication.Windows.Moderating.EditWindows.Users;
using CarSharingApplication.Windows.Moderating.EditWindows.Rentals;
using CarSharingApplication.LogLibrary;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private UsersINFO _User;
        public AdminWindow(ref UsersINFO user)
        {
            _User = user;
            InitializeComponent();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void DriverLicencesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditDriverLicenses(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void DriverLicencesCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditCategories(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void VehicleClassesButon_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditClasses(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void VehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditVehiclesWindow(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void VehiclesPassportsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditRegistrationCertificates(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void VehicleCoordinatesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditVehiclesCoordinates(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void TrafficAccidentsTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditTrafficAccidentTypes(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void TrafficAccidentsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditTrafficAccidents(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void RentalsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditRentals(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();

        }

        private void ButtonBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
        }
    }
}
