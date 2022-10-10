using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.IO;
using System.Text;

namespace IntAlk1_Assignment.Services
{
    public class Logger
    {
        private readonly string _logTypeInfo = "[INFO]";
        private readonly string _logTypeWarning = "[WARNING]";
        private readonly string _logTypeError = "[ERROR]";

        public Logger()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "log");
            Directory.CreateDirectory(path);
        }

        public void Info(string message)
        {
            string logMessage = _logTypeInfo + " " + message;
            Write(logMessage);
        }

        public void Error(string message)
        {
            string logMessage = _logTypeError + " " + message;
            Write(logMessage);
        }

        public void Warning(string message)
        {
            string logMessage = _logTypeWarning + " " + message;
            Write(logMessage);
        }

        private void Write(string message)
        {
            string logFile = DateTime.Now.ToString("yy-MM-dd") + ".log";
            string path = Path.Combine(Environment.CurrentDirectory, "log", logFile);

            using (StreamWriter streamWriter = new (path, true))
            {
                streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + message);
            }
        }
    }
}
