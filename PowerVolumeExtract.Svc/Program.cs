using System.Configuration;
using Topshelf;
using PowerVolumeInterface;
using PowerVolumeExtract.Svc.Implementation;

namespace PowerVolumeExtract.Svc
{
    class Program
    {
        static void Main(string[] args)
        {
            AppSettingsReader rdr = new AppSettingsReader();
            string logfilename = (string)rdr.GetValue("logfilename", typeof(string));
            string extractFolder = (string)rdr.GetValue("extractlocation", typeof(string)); 
            double intervalMins = (double)rdr.GetValue("extractintervalmins", typeof(double));

            //initialise container here. but for the puposes of this test we are not using container, so initialise the services directly
            //and pass via constructor interface
            ILogger logger = new Logger(logfilename, true);
            IPowerService powerService = new PowerServiceImp(new Services.PowerService());
            IVolumeAggregator volumeAggregator = new VolumeAggregator();
            IExtractWriter extractWriter = new ExtractWriter(extractFolder);
            

            HostFactory.Run(serviceConfig =>
                {
                    serviceConfig.Service<Service>(
                        serviceInstance=>
                            {
                                serviceInstance.ConstructUsing(() => new Service(powerService, volumeAggregator, extractWriter, logger, intervalMins * 60000 ));
                                serviceInstance.WhenStarted(execute => execute.Start());
                                serviceInstance.WhenStopped(execute => execute.Stop());
                                serviceInstance.WhenPaused(execute => execute.Pause());
                                serviceInstance.WhenContinued(execute => execute.Continue());

                            });

                    serviceConfig.EnableServiceRecovery(recoveryOption => recoveryOption.RestartService(1));
                    serviceConfig.EnablePauseAndContinue();
                    serviceConfig.SetServiceName("PowerVolumeExtract.Svc.Service");
                    serviceConfig.SetDescription("PowerVolumeExtract.Svc Service");
                    serviceConfig.SetDisplayName("PowerVolumeExtract.Svc Service");

                    serviceConfig.StartAutomatically();
                });
        }
    }
}
