using System;
using System.Linq;
using System.Collections.Generic;
using PowerVolumeInterface;

namespace PowerVolumeExtract.Svc.Implementation
{
    public class VolumeAggregator : IVolumeAggregator
    {
        public const int NumPeriods = 24;
        public IVolumeAggregate Aggregate(DateTime date, IEnumerable<IPowerTrade> trades)
        {
            if (trades == null) throw new ArgumentNullException("trades", "VolumeAggregator::Aggregate:trade collection provided for aggregation is null");

            VolumeAggregate aggregate = new VolumeAggregate(date);

            foreach (var trade in trades)
            {
                if (trade == null) throw new ArgumentOutOfRangeException("trades", "VolumeAggregator::Aggregate:Trade element is null in trades collection provided");


                if ((trade.Periods == null) || (trade.Periods.Length < NumPeriods))
                    throw new ArgumentOutOfRangeException("trades", "VolumeAggregator::Aggregate:Inconsistent number of trade periods provided");


                if(trade.Date != date.Date)
                    throw new ArgumentOutOfRangeException("trades", "VolumeAggregator::Aggregate:Inconsistent dates in trade collection provided");


                for (int k = 0; k < NumPeriods; k++)
                    aggregate.Volumes[k] += trade.Periods.Where(x => x.Period == k + 1).Single().Volume;
               
               /*if we could be sure that the periods where stored in the right order when coming from the service, when we wouldnt need the 'Where' clause
               so the following might be more efficient: 
               for (int k = 0; k < NumPeriods; k++)
                    aggregate.Volumes[k] += trade.Periods[k].Volume;
               */
            }
            return aggregate;
        }
    }
}
