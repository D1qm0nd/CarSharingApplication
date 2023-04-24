﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

#nullable enable
        /// <summary>
        /// Получить данные по запросу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="query_command"></param>
        /// <returns></returns>
        public static List<T>? GetQueryResult<T>(DataContext context, string query_command)
        {
            try
            {
                List<T> list;
                context.Connection.Open();
                using (context)
                {
                    list = context.ExecuteQuery<T>(query_command).ToList();
                    context.Connection.Close();
                }
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static bool ExecuteNonQuery(DataContext context, string command)
        {
            try
            {
                using (context) 
                {
                    context.ExecuteCommand(command);
                }
                return true;
            }
            catch 
            { 
                return false;
            }
        }


    }
}
