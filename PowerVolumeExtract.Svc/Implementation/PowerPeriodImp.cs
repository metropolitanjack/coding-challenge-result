using PowerVolumeInterface;
using Services;

namespace PowerVolumeExtract.Svc.Implementation
{
    //facade : to wrap the given Services.PowerPeriod functionality in a IPowerPeriod interface
    public class PowerPeriodImp : IPowerPeriod
    {
        PowerPeriod _powerPeriod;
        public PowerPeriodImp(PowerPeriod powerPeriod)
        {
            _powerPeriod = powerPeriod;
        }

        public int Period
        {
            get { return _powerPeriod.Period;   }
            set { _powerPeriod.Period = value;  }
        }

        public double Volume
        {
            get { return _powerPeriod.Volume; }
            set { _powerPeriod.Volume = value; }
        }
    }
}
