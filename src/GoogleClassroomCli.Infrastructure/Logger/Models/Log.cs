using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleClassroom.Infrastructure.Models
{
    public class Log
    {
        public Log()
        {
            Date = DateTime.Now;
        }

        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string ErrorDetails { get; set; }
    }
}
