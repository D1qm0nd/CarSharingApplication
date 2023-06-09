﻿using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using System;
using System.Data.SqlClient;
using System.Windows;


namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для EditUsersWindow.xaml
    /// </summary>
    public partial class EditUsersWindow : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        private UsersINFO _User;

        public EditUsersWindow(UsersINFO user)
        {
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            try
            { 
                InitializeComponent();
                App.AppDataBase.OpenConnection(ConnectionString);
                db = App.AppDataBase.Context;
                dt_grid.ItemsSource = db.Rental_Users;
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
