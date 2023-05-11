using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using CarSharingApplication.Windows.VehicleRent;
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
        public ChoiceLoginWindow(ref UsersINFO user, Window owner)
        {
            this.Owner = owner;
            User = user;
            InitializeComponent();
            App._Logger.Log(new LogMessage((ulong)User.ID_User, this.Title, "Выбирал под какой ролью зайти в систему", null, null));
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
            User = App.GetScalarResult<UsersINFO>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")), $"SELECT * FROM UsersINFO WHERE ID_User = {User.ID_User}");
            if (User.RentStatus != "в поездке")
            {
                var CarSelWindow = new CarSelector(ref User, this, true);
                CarSelWindow.Activate();
                CarSelWindow.Show();
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                try
                {
                    this.Visibility = Visibility.Collapsed;
                    var TripWND = new TripWindow(ref User, this, true);
                    TripWND.Activate();
                    TripWND.Show();
                }
                catch 
                {
                    this.Visibility = Visibility.Visible;
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App._Logger.Log(new LogMessage((ulong)User.ID_User, this.Title, "Вышел из окна", null, LogType.UserAction));
            this.Owner.Activate();
            this.Owner.Visibility = Visibility.Visible;
            GC.Collect();
        }
    }
}
