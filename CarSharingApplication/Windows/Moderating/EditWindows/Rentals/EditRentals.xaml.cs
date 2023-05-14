using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace CarSharingApplication.Windows.Moderating.EditWindows.Rentals
{
    /// <summary>
    /// Логика взаимодействия для EditRentals.xaml
    /// </summary>
    public partial class EditRentals : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        private UsersINFO _User;

        public EditRentals(UsersINFO user)
        {
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            try
            {
                InitializeComponent();
                App.AppDataBase.OpenConnection(ConnectionString);
                db = App.AppDataBase.Context;
                dt_grid.ItemsSource = db.Rentals;
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось вснёсти изменения в таблицу {this.Title}", sqlex.Message, LogType.DataBaseError));
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.AppDataBase.CloseConnection();
                db.SubmitChanges();
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                dt_grid.ItemsSource = db.VehicleRegistrCertificates;
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось внести изменения в таблицу {this.Title}", sqlex.Message, LogType.DataBaseError | LogType.UserMistake));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось внести изменения в таблицу {this.Title}", ex.Message, LogType.ProgramError));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Перестал просматривать", null, LogType.UserAction));
        }
    }
}

