using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace LoggerLib
{


    public class Logger
    {

        public JsonSerializerOptions options { get; set; } = JsonSerializerOptions.Default;

#nullable enable
    private static Logger? _Instance = null;
    public string? LogPath { get; set; } = null;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Logger Instance()
        {
            if (_Instance == null)
            {
                return _Instance = new Logger();
            }
            else return _Instance;
        }

        public virtual bool Log<T>(T logObj)
        {
            if (!File.Exists(LogPath))
            {
                File.Create(LogPath);
            }
            using (StreamWriter streamWriter = new StreamWriter(LogPath, true))
            {
                streamWriter.WriteLine(JsonSerializer.Serialize(logObj, options));
                streamWriter.Close();
            }
            return true;
        }
        private Logger()
        {

        }
    }
}
