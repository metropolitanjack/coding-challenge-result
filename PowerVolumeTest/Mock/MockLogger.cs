using System;
using System.Collections.Generic;
using System.Linq;
using PowerVolumeInterface;

namespace PowerVolumeTest.Mock
{
    internal class MockLogger : ILogger
    {
        List<string> _logEntries;
        public MockLogger()
        {
            _logEntries = new List<string>();
        }

        public void Log(LogEntry entry)
        {
            string output = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " " + entry.Severity.ToString() + ": " + entry.Message + " " + (entry.Exception != null ? entry.Exception.Message : "");
            _logEntries.Add(output);
        }

        public List<string> Entries
        {
            get { return _logEntries.ToList(); }
        }

    }
}
