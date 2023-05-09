using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;

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

        //private string _Text;
        //public string text 
        //{
        //    get { return _Text; }
        //    set { 
        //        _Text = value;
        //        OnPropertyChanged(nameof(text));
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string name = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}

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

        private void ConfirmAccident(object sender, RoutedEventArgs e)
        {
            if (AccidentTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип аварии");
                return;
            }
            if (DescriptionText.Text == "")
            {
                MessageBox.Show("добавьте описание аварии");
                return;
            }

            char[] NotAllowed = {'|', '\\', '/',
                                 '-', '`', '\''};

            foreach (char c in NotAllowed)
            {
                if (DescriptionText.Text.Contains(c))
                {
                    MessageBox.Show($"Недопустимый символ {c}");
                    return;
                }
            }

            App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("CARHANDLERConnection")),
                "INSERT TrafficAccidents VALUES" +
                $"({_Rental.ID_Vehicle}, " +
                $"'{_Rental.ID_DriverLicence}', " +
                $"{App.GetScalarResult<int>(new CarSharingDataBaseClassesDataContext (App.GetConnectionString("CARHANDLERConnection")),
                    $"SELECT ID_TrafficAccidentType FROM TrafficAccidentTypes WHERE TrafficAccidentTypeName = '{AccidentTypes.SelectedValue}'")}, " +
                $"0, " +
                $"'{DescriptionText.Text}')");
            MessageBox.Show("Данные отправлены.");
            this.Close();
        }
    }
}
