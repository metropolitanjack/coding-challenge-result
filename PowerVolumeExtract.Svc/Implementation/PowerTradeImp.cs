using System;
using System.Linq;
using PowerVolumeInterface;
using Services;

namespace PowerVolumeExtract.Svc.Implementation
{
    //facade : to wrap the given Services.PowerTrade functionality in a IPowerTrade interface
    internal class PowerTradeImp : IPowerTrade
    {
        private PowerTrade _powerTrade;
        public PowerTradeImp(PowerTrade powerTrade)
        {
            _powerTrade = powerTrade;
        }

        public DateTime Date { get { return _powerTrade.Date; } }
        public IPowerPeriod[] Periods
        {
            get
            {
                return _powerTrade.Periods.Any() ? _powerTrade.Periods.Select((x) => new PowerPeriodImp(x)).ToArray() : null;
            }
        }
    }
}
