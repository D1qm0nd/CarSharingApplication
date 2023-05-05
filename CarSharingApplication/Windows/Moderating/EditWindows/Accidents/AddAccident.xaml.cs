using CarSharingApplication.LogLibrary;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarSharingApplication.Windows.Moderating.EditWindows.Accidents
{
    /// <summary>
    /// Логика взаимодействия для AddAccident.xaml
    /// </summary>
    public partial class AddAccident : Window
    {
        private bool _ShowOwner = true;
        private RentalsINFO _Rental;
        private UsersINFO _User;
        public AddAccident(ref UsersINFO user, ref RentalsINFO rental, Window owner, bool showOwner)
        {
            _User = user;
            _Rental = rental;
            _ShowOwner = showOwner;
            this.Owner = owner;
            InitializeComponent();
            var _types = App.GetQueryResult<string>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")), "SELECT TrafficAccidentTypeName FROM TrafficAccidentTypes");
            AccidentTypes.ItemsSource = _types; 
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ShowOwner == true)
            {
                this.Owner.Show();
                this.Owner.Visibility = Visibility.Visible;
                this.Owner.Activate();
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
            }
            else
            {
                this.Owner.Close();
            }
        }
    }
}
