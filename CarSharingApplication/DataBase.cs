using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApplication
{

    public class DataBase<ContextT> where ContextT : DataContext, new()
    {
        private static DataBase<ContextT> _Instance;

        public static DataBase<ContextT> Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DataBase<ContextT>();
                return _Instance;
            }
        }
#nullable enable
        private ContextT? _Context;
#nullable enable
        public ContextT? Context { get => _Context; }

        public delegate ContextT CtxCreateFunc(string Connection);

        public CtxCreateFunc createfunc;

        public void OpenConnection(string ConnectionString)
        {
            CloseConnection();
            _Context = createfunc(ConnectionString);
        }

        public void CloseConnection()
        {
            if (_Context != null)
                _Context!.Dispose();
        }

#nullable enable
        public List<T>? GetQueryResult<T>(string query_command)
        {
            if (_Context == null)
                throw new NullReferenceException(nameof(DataContext));
            try
            {
                List<T> list;
                Context!.Connection.Open();
                list = Context!.ExecuteQuery<T>(query_command).ToList();
                Context!.Connection.Close();
                return list;
            }
            catch
            {
                return null;
            }
        }
#nullable enable
        public T? GetScalarResult<T>( string query_command)
        {
            if (_Context == null)
                throw new NullReferenceException(nameof(DataContext));
            try
            {
                List<T> list;
                    Context!.Connection.Open();
                    list = Context.ExecuteQuery<T>(query_command).ToList();
                    Context!.Connection.Close();
                return list[0];
            }
            catch 
            {
                return default;
            }
        }

        public bool ExecuteNonQuery( string command)
        {
            if (_Context == null)
                throw new NullReferenceException(nameof(DataContext));
            try
            {
                Context!.Connection.Open();
                Context!.ExecuteCommand(command);
                Context!.Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private DataBase() { }
    }
}
