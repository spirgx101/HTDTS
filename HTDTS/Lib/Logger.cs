using System;
using System.IO;
using System.Text;

namespace HTDTS.Lib
{
    public enum LoggerLevel : int
    {
        DEBUG = 0,
        INFO = 1,
        WARNING = 2,
        ERROR = 3,
        REMOTE = 4
    }

    public class Logger
    {
        private System.Object lockThis = new System.Object();

        public string FilePath { get; set; }
        public string MacPath { get; set; }
        public LoggerLevel Level { get; set; }

        public void Write(string format, params object[] arg)
        {
            Write(string.Format(format, arg));
        }

        public void Write(LoggerLevel level, string message)
        {
            if (level < Level) return;

            if (string.IsNullOrEmpty(FilePath))
            {
                //FilePath = Directory.GetCurrentDirectory();
                //FilePath = Application.StartupPath;
                //FilePath = Environment.CurrentDirectory;
                FilePath = System.AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine(FilePath);
            }

            if (MacPath == string.Empty)
            {
                MacPath = "NONE";
            }

            string filename = FilePath +
                    string.Format("\\Log\\{0}_{1:yyyy-MM}.txt", MacPath, DateTime.Now.Date);
            FileInfo finfo = new FileInfo(filename);

            if (finfo.Directory.Exists == false)
            {
                finfo.Directory.Create();
            }
            string writeString = string.Format("{0:yyyy/MM/dd HH:mm:ss} {1,-8} {2}",
                    DateTime.Now, level, message) + Environment.NewLine;
            lock (lockThis)
            {
                File.AppendAllText(filename, writeString, Encoding.Unicode);
            }
        }
    }
}
