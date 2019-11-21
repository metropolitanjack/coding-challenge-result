using System;
using System.Collections.Generic;

namespace PowerVolumeInterface
{
    public interface IVolumeAggregate
    {
        DateTime Date { get; set; }
        Dictionary<int, double> Volumes { get; }
    }
}
