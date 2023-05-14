using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using System;
using System.Linq;
using System.Windows;

namespace CarSharingApplication.Windows.Moderating.EditWindows.Accidents
{
    /// <summary>
    /// Логика взаимодействия для AddAccident.xaml
    /// </summary>
    public partial class AddAccident : Window
    {
        private string connectionString { get; set; } = App.GetConnectionString("CARHANDLERConnection");
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
            App.AppDataBase.OpenConnection(connectionString);
            var _types = App.AppDataBase.GetQueryResult<string>("SELECT TrafficAccidentTypeName FROM TrafficAccidentTypes");
            App.AppDataBase.CloseConnection();

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
                this.Owner?.Close();
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
            App.AppDataBase.OpenConnection(connectionString);
            foreach (var item in AccidentTypes.SelectedItems)
                App.AppDataBase.ExecuteNonQuery(
                    "BEGIN TRANSACTION " +
                    "INSERT TrafficAccidents VALUES" +
                    $"({_Rental.ID_Vehicle}, " +
                    $"'{_Rental.ID_DriverLicence}', " +
                    $"{App.AppDataBase.GetScalarResult<int>(
                        "SELECT ID_TrafficAccidentType " +
                        "FROM TrafficAccidentTypes " +
                        $"WHERE TrafficAccidentTypeName = '{item}'")}, " +
                    $"0, " +
                    $"'{DescriptionText.Text}')" +
                    "COMMIT TRANSACTION");
            App.AppDataBase.CloseConnection();
            MessageBox.Show("Данные отправлены.");
            this.Close();
        }
    }
}
