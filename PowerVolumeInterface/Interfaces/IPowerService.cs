using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerVolumeInterface
{
    public interface IPowerService
    {
        Task<IEnumerable<IPowerTrade>> GetTrades(DateTime date);
    }
}
