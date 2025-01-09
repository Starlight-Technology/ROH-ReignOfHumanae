using System;

namespace ROH.StandardModels.Log
{
    public class LogModel
    {
        public Severity Severity { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; }
    }
}