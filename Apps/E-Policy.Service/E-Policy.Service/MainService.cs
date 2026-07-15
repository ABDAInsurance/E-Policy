using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

using E_Policy.Service.Jobs;

namespace E_Policy.Service
{
    public partial class MainService : ServiceBase
    {
        Timer schedulerTimer = new Timer();

        public MainService()
        {
            InitializeComponent();
        }

        public void OnTesting()
        {
            MessageLog.Info("Service On Testing ...");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            new GeneratePolicyDocumentJob().Run();

            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            MessageLog.Debug(string.Format("Time Taken : {0}", timeSpan.ToString(@"m\:ss\.fff")));

            NLog.LogManager.Shutdown();
        }

        protected override void OnStart(string[] args)
        {
            MessageLog.Info("Service Starting ...");

            schedulerTimer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            schedulerTimer.Interval = ApplicationConfiguration.IntervalTime;
            schedulerTimer.AutoReset = true;
            schedulerTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            schedulerTimer.Stop();
            schedulerTimer.Dispose();

            MessageLog.Info("Service Stopping ...");

            NLog.LogManager.Shutdown();
        }

        private void OnTimerEvent(object source, ElapsedEventArgs e)
        {
            schedulerTimer.Stop();

            new GeneratePolicyDocumentJob().Run();

            schedulerTimer.AutoReset = true;
            schedulerTimer.Start();
        }
    }
}
