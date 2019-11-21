using System;
using System.Linq;
using System.Threading.Tasks;
using PowerVolumeInterface;

namespace PowerVolumeExtract.Svc
{
    public class Service
    {
        private readonly IPowerService _powerService;
        private readonly IVolumeAggregator _volumeAggregator;
        private readonly IExtractWriter _extractWriter;
        private readonly ILogger _logger;
        private readonly double _intervalMs;
        private System.Timers.Timer _extractTimer;
        private bool _isRunning;
        private object _syncLock = true;
        private string _filenamePrefix = "PowerPosition_";
        public static double MinIntervalMs = 60000;
        

        public Service(IPowerService powerService, IVolumeAggregator volumeAggegator, IExtractWriter extractWriter, ILogger logger, double intervalMs)
        {
            if (powerService == null) throw new ArgumentNullException("powerService");
            if (volumeAggegator == null) throw new ArgumentNullException("volumeAggregator");
            if (extractWriter == null) throw new ArgumentNullException("extractWriter");
            if (logger == null) throw new ArgumentNullException("logger");
            if (intervalMs < MinIntervalMs) throw new ArgumentOutOfRangeException("intervalMins");

            _powerService = powerService;
            _volumeAggregator = volumeAggegator;
            _extractWriter = extractWriter;
            _logger = logger;
            _intervalMs = intervalMs;
            
        }

        public bool Start()
        {
            _extractTimer = new System.Timers.Timer(_intervalMs);
            _extractTimer.Elapsed += _extractTimer_Elapsed;
            _extractTimer.Start();
            LogInfo("Service started...");

            //run initial extract
            RunExtract();
            
            return true;
        }

        

        public bool Stop()
        {
            _extractTimer.Stop();
            _extractTimer.Dispose();
            LogInfo("Service stopped...");
            return true;
        }

        public bool Pause()
        {
            _extractTimer.Stop();
            LogInfo("Service paused...");
            return true;
        }

        public bool Continue()
        {
            _extractTimer.Start();
            LogInfo("Service continued...");
            return true;
        }

        private void _extractTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RunExtract();
        }

        private void RunExtract()
        {
            if (IsRunning)
            {
                LogWarning("Previous scheduled extract is still running");
                return;
            }

            IsRunning = true;
            LogInfo("Beginning scheduled extract run");

            DateTime date = DateTime.Now;
            DateTime businessDate = date.Date;
            string filename = GetExtractFileName(date);

            //do: retrieval
            LogInfo("Beginning Power volume retrieval");
            var powerTradesTask = _powerService.GetTrades(businessDate);
            powerTradesTask.ContinueWith(t => { IsRunning = false; LogError(t); }, TaskContinuationOptions.OnlyOnFaulted);

            /*do: aggregation*******************************************/
            var aggregateTask = powerTradesTask.ContinueWith(t => 
            {
                LogInfo("Power volumes retrieved. Beginning aggregator");
                return _volumeAggregator.Aggregate(businessDate, powerTradesTask.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            aggregateTask.ContinueWith(t => { IsRunning = false; LogError(t); }, TaskContinuationOptions.OnlyOnFaulted);
            /************************************************************/

            /*do: write to extract file**********************************/
            var writeExtractTask = aggregateTask.ContinueWith(t => 
            {
                LogInfo("Aggregation complete. Beginning report writer");
                _extractWriter.Write(filename, aggregateTask.Result);
            },TaskContinuationOptions.OnlyOnRanToCompletion);
            writeExtractTask.ContinueWith(t => { IsRunning = false; LogError(t);}, TaskContinuationOptions.OnlyOnFaulted);
            /************************************************************/
            
            /*do: log complete*******************************************/
            writeExtractTask.ContinueWith(t => 
            {
                IsRunning = false;
                LogInfo("Scheduled extract run completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

        }

        private string GetExtractFileName(DateTime date)
        {
            return _filenamePrefix + date.ToString("yyyyMMdd_HHmm") + ".csv";
        }

        private bool IsRunning
        {
            get { lock(_syncLock) { return _isRunning; } }
            set { lock(_syncLock) { _isRunning = value; } }
        }

        private void LogInfo(string info)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, info));
        }

        private void LogWarning(string warning)
        {
            _logger.Log(new LogEntry(LoggingEventType.Warning, warning));
        }

        private void LogError(string err)
        {
            _logger.Log(new LogEntry(LoggingEventType.Error, err));
        }
        private void LogError(Task t)
        {
            var err = GetError(t);
            LogError(err);
        }

        private string GetError(Task t)
        {
            var res = "";
            if (t == null) return res;
            if (t.Exception == null) return res;
            res += t.Exception.Message;
            if (t.Exception.InnerExceptions != null)
                res = t.Exception.InnerExceptions.Aggregate(res, (current, e) => current + (" : " + e.Message));

            return res;
        }

    }
}
