using System;
using System.Collections.Generic;

namespace PowerVolumeInterface
{
    public interface IVolumeAggregator
    {
        IVolumeAggregate Aggregate(DateTime date, IEnumerable<IPowerTrade> trades);
    }
}
