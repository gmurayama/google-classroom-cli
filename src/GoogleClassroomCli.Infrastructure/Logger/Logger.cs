using GoogleClassroomCli.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoogleClassroomCli.Infrastructure
{
    public class Logger
    {
        private IList<Log> _logList;
        private string _logPath;

        public string LogPath
        {
            get { return _logPath; }
            private set { _logPath = !value.EndsWith('/') ? value + "/" : value; }
        }

        public Logger()
        {
            _logList = new List<Log>();
        }

        public Logger(string logPath) : this()
        {
            LogPath = logPath;
        }

        public void AddLog(Log log)
        {
            _logList.Add(log);
        }

        public void AddLog(string message)
        {
            _logList.Add(new Log
            {
                Message = message,
                Level = LogLevel.Error
            });
        }

        public void AddLog(string message, LogLevel logLevel)
        {
            _logList.Add(new Log
            {
                Message = message,
                Level = logLevel
            });
        }

        public void AddLog(string message, LogLevel logLevel, string errorDetails)
        {
            _logList.Add(new Log
            {
                Message = message,
                Level = logLevel,
                ErrorDetails = errorDetails
            });
        }

        public void AddLog(string message, LogLevel logLevel, string errorDetails, DateTime date)
        {
            _logList.Add(new Log
            {
                Message = message,
                Level = logLevel,
                ErrorDetails = errorDetails,
                Date = date
            });
        }

        public void WriteLog()
        {
            if (_logList.Count > 0)
            {
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }

                var fileName = string.Format("{0}log-{1:yyyyMMdd}.txt", LogPath, DateTime.Now.Date);

                using (var writer = new StreamWriter(fileName, true, Encoding.UTF8))
                {
                    foreach (var log in _logList)
                    {
                        writer.WriteLine("{0} - ( {1} ) {2} ; {3}", log.Date.ToString("HH:mm:ss"), log.Level.ToString(), log.Message, log.ErrorDetails);
                    }
                }
            }
        }
    }
}
