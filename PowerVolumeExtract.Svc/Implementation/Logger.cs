using System;
using System.IO;
using PowerVolumeInterface;

namespace PowerVolumeExtract.Svc.Implementation
{
    //using facade pattern that allows me to swap the loggers by changing the implementation of this class only.
    //normally i would NOT write my own logger, instead I would use a logging library like log4net or microsoft enterprise library logging
    //however for this test to avoid lots of dependencies or possible version compatibility issues causing build to break when shipped, im just going to roll a simple one here

    public class Logger : ILogger
    {
        private string _logFileName;
        private bool _echoToConsole;
        private StreamWriter _sw;

        public Logger(string logFileName, bool echoToConsole)
        {
            _logFileName = logFileName;
            _echoToConsole = echoToConsole;
            if (!string.IsNullOrEmpty(logFileName))
            {
                FileInfo f = new FileInfo(logFileName);
                DirectoryInfo df = f.Directory;
                if (!df.Exists) df.Create();
                //_sw = new StreamWriter(_logFileName, true);
            }
        }

        public async void Log(LogEntry entry)
        {
            string output = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " " + entry.Severity.ToString() + ": " + entry.Message + " " + (entry.Exception != null ? entry.Exception.Message : "");
            if (_echoToConsole) Console.WriteLine(output);
            if (_logFileName != null)
            {
                _sw = new StreamWriter(_logFileName, true);
                await _sw.WriteLineAsync(output);
                _sw.Flush();
                _sw.Close();
            }
        }
    }
}
