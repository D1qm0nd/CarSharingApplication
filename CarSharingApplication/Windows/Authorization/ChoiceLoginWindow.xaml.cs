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

namespace CarSharingApplication.Windows.Authorization
{
    /// <summary>
    /// Логика взаимодействия для ChoiceLoginWindow.xaml
    /// </summary>
    public partial class ChoiceLoginWindow : Window
    {
        private UsersINFO User;
        public ChoiceLoginWindow(ref UsersINFO user)
        {
            User = user;
            InitializeComponent();
        }

        private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var AdmWindow = new AdminWindow(ref User);
            AdmWindow.Owner = this;
            AdmWindow.Activate();
            AdmWindow.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void UserLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var CarSelWindow = new CarSelector(ref User);
            CarSelWindow.Owner = this;
            CarSelWindow.Activate();
            CarSelWindow.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Activate();
            this.Owner.Visibility = Visibility.Visible;
            GC.Collect();
        }
    }
}
