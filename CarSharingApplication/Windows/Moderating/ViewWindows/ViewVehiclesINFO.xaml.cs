﻿using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Windows;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для EditUsersWindow.xaml
    /// </summary>
    public partial class ViewVehiclesINFO : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        private UsersINFO _User;
        public ViewVehiclesINFO(UsersINFO user)
        {
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            try
            {
                InitializeComponent();
                App.AppDataBase.OpenConnection(ConnectionString);
                db = App.AppDataBase.Context;
                dt_grid.ItemsSource = db.VehiclesINFO;
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
    }
}
