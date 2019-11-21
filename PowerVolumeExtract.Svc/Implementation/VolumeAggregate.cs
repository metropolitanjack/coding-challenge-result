using System;
using System.Collections.Generic;
using PowerVolumeInterface;

namespace PowerVolumeExtract.Svc.Implementation
{
    public class VolumeAggregate : IVolumeAggregate
    {
        public DateTime Date { get; set; }
        public Dictionary<int, double> Volumes { get;}

        public VolumeAggregate(DateTime date)
        {
            Date = date;
            Volumes = new Dictionary<int, double>();
            for (int k = 0; k < 24; k++) Volumes.Add(k, 0.0);
        }
    }
}
