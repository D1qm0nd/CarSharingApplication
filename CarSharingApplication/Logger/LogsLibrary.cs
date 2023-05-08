using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApplication.LogLibrary
{
    [Flags]
    public enum LogType
    {
        UserAction = 0,
        UserMistake = 1,
        ProgramError = 2,
        DataBaseError = 3,
    }

#nullable enable
    [Serializable]
    public class LogMessage
    {
        public string Date { get { return String.Format("{0:dd.MM.yyyy-HH:mm:ss}",DateTime.UtcNow); } }
        public string MachineName { get; } = Environment.MachineName;
        public ulong? ID_User { get; set; }
        public string? WindowName { get; set; }
        public string? Description { get; set; }
        public string? error { get; set; }
        public LogType? Level { get; set; }

        public LogMessage(ulong? id_user, string? windowName, string? description, string? err, LogType? level ) 
        {
            ID_User = id_user;
            WindowName = windowName;
            Description = description;
            error = err;
            Level = level;
        }

        ~LogMessage() { }
    }
}
