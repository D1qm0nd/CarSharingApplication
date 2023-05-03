using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApplication.LogLibrary
{

#nullable enable
    [Serializable]
    public class LogMessage
    {
        public DateTime Date = DateTime.UtcNow;
        public ulong? ID_User { get; set; }
        public string? WindowName { get; set; }
        public string? Description { get; set; }
        public string? error { get; set; }
        public int? Level { get; set; }

        public LogMessage(ulong? id_user, string? windowName, string? description, string? err, int? level ) 
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
