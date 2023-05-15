using CarSharingApplication.SQL.Linq;
using LoggerLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
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

        public static DataBase<CarSharingDataBaseClassesDataContext> AppDataBase { get => DataBase<CarSharingDataBaseClassesDataContext>.Instance; }

        public static CarSharingDataBaseClassesDataContext ContextCreateFunc(string ConnectionString) => new CarSharingDataBaseClassesDataContext(ConnectionString);
        

        
        public static string path { get
            {
                string _path = App.GetConnectionString("AppPath");
                return _path;
            }
        }

        public static Logger _Logger { get => Logger.Instance(); }
    }
}
