﻿using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace CarSharingApplication.Windows.Moderating.EditWindows.Vehicles
{
    /// <summary>
    /// Логика взаимодействия для EditVehiclesCoordinates.xaml
    /// </summary>
    public partial class EditVehiclesCoordinates : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        private UsersINFO _User;

        public EditVehiclesCoordinates(UsersINFO user)
        {
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            try
            {
                InitializeComponent();
                App.AppDataBase.OpenConnection(ConnectionString);
                db = App.AppDataBase.Context;
                dt_grid.ItemsSource = db.VehicleCoordinates;
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось загрузить данные таблицы таблицу {this.Title}", sqlex.Message, LogType.DataBaseError));
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.AppDataBase.CloseConnection();
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.SubmitChanges();
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Внёс изменения в таблицу {this.Title}", null, LogType.UserAction));
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                dt_grid.ItemsSource = db.Rental_Users;
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

    }
}
