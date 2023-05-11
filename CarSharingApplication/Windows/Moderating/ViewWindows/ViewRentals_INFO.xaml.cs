using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using System.Data.SqlClient;
using System.Windows;

namespace CarSharingApplication.Windows.Moderating.ViewWindows
{
    /// <summary>
    /// Логика взаимодействия для EditRentals.xaml
    /// </summary>
    public partial class ViewRentals_INFO : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        private UsersINFO _User;

        public ViewRentals_INFO(UsersINFO user)
        {
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            try
            {
                InitializeComponent();
                db = new CarSharingDataBaseClassesDataContext(ConnectionString);
                dt_grid.ItemsSource = db.RentalsINFO;
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось вснёсти изменения в таблицу {this.Title}", sqlex.Message, LogType.DataBaseError));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Перестал просматривать", null, LogType.UserAction));
        }
    }
}

