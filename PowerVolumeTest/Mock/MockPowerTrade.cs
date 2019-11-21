using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerVolumeInterface;

namespace PowerVolumeTest.Mock
{
    internal class MockPowerTrade : IPowerTrade
    {
        public MockPowerTrade(DateTime date, int numPeriods = 24)
        {
            Date = date;
            Periods = new IPowerPeriod[numPeriods];
            for (int k = 0; k < numPeriods; k++)
                Periods[k] = new MockPowerPeriod(k+1, 0);
        }
        public DateTime Date { get; set; }
        public IPowerPeriod[] Periods { get; set; }
       
    }
}
