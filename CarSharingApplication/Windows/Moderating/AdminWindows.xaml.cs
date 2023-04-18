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

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow(Rental_Users user)
        {
            InitializeComponent();
            this.Title = $"CarSharing Окно администратора [{user.UserSurname} {user.UserName} {user.UserMiddleName}]";
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void DriverLicencesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditDriverLicenses();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void DriverLicencesCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void VehicleClassesButon_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void VehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditVehiclesWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void VehiclesPassportsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void VehicleCoordinatesButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void TrafficAccidentsTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void TrafficAccidentsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
            EditWindow.Owner = this;
            this.Visibility = Visibility.Hidden;
            EditWindow.Show();
        }

        private void RentalsButton_Click(object sender, RoutedEventArgs e)
        {
            var EditWindow = new EditUsersWindow();
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
        }
    }
}
