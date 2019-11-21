using System;

namespace PowerVolumeInterface
{
    public interface IPowerTrade
    {
        DateTime Date { get; }
        IPowerPeriod[] Periods { get; }
    }
}
