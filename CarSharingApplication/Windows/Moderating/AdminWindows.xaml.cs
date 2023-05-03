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
            this.Title = $"Окно администратора";
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Зашёл в окно администрирования", null, null));
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Пользователи", null, null));
        }

        private void DriverLicencesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditDriverLicenses(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Водительские удостоверения", null, null));

        }

        private void DriverLicencesCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditCategories(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Категории водительких удостоверений", null, null));

        }

        private void VehicleClassesButon_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditClasses(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Классы ТС", null, null));

        }

        private void VehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditVehiclesWindow(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Транспортные средства", null, null));

        }

        private void VehiclesPassportsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditRegistrationCertificates(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Свидетельства регистрации ТС", null, null));

        }

        private void VehicleCoordinatesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditVehiclesCoordinates(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Координаты ТС", null, null));

        }

        private void TrafficAccidentsTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditTrafficAccidentTypes(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Типы Аварийных случаев", null, null));

        }

        private void TrafficAccidentsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditTrafficAccidents(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Типы Аварийныу случаи", null, null));

        }

        private void RentalsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditRentals(_User);
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Нажал кнопку Аренды ТС", null, null));

        }

        private void ButtonBACK_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Покинул окно администрирования", null, null));

        }
    }
}
