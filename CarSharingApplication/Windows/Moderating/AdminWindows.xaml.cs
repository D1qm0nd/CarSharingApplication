using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using CarSharingApplication.SQL.Linq;
using CarSharingApplication.Windows.Moderating.EditWindows.Vehicles;
using CarSharingApplication.Windows.Moderating.EditWindows.Users;
using CarSharingApplication.Windows.Moderating.EditWindows.Rentals;
using CarSharingApplication.LogLibrary;
using CarSharingApplication.Windows.Moderating.ViewWindows;
using System.Windows.Forms;

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
            TotalPrice.Content = $"{App.GetScalarResult<decimal>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("DBADMINConnection")),"SELECT SUM(TotalPrice) FROM RentalsINFO")} ₽";
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
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

        private void UsersINFO_Click(object sender, RoutedEventArgs e)
        {
            var View = new ViewUsersINFO(_User);
            View.Owner = this;
            this.Visibility = Visibility.Hidden;
            View.Show();
        }

        private void RentalsINFO_Click(object sender, RoutedEventArgs e)
        {
            var View = new ViewRentals_INFO(_User);
            View.Owner = this;
            this.Visibility = Visibility.Hidden;
            View.Show();
        }

        private void VehiclesINFO_Click(object sender, RoutedEventArgs e)
        {
            var View = new ViewVehiclesINFO(_User);
            View.Owner = this;
            this.Visibility = Visibility.Hidden;
            View.Show();
        }
        private void ButtonBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
