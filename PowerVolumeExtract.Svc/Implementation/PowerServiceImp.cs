using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PowerVolumeInterface;

namespace PowerVolumeExtract.Svc.Implementation
{
    //facade : to wrap the given Services.PowerService functionality in a PowerVolumeInterface.IPowerService interface
    public class PowerServiceImp : PowerVolumeInterface.IPowerService
    {
        private readonly Services.IPowerService _powerService;
        public PowerServiceImp(Services.IPowerService powerService)
        {
            if (powerService == null) throw new ArgumentNullException("powerService");
            _powerService = powerService;
        }

        public async Task<IEnumerable<IPowerTrade>> GetTrades(DateTime date)
        {
            var t = await _powerService.GetTradesAsync(date);
            return t.Select((x) => new PowerTradeImp(x));
        }
    }
}
