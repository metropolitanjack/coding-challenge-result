using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PowerVolumeInterface;

namespace PowerVolumeTest.Mock
{
    class MockPowerService : IPowerService
    {
        private readonly int _delayMs;
        private readonly bool _throwException;
        public MockPowerService(int delayMs = 0, bool throwException = false) { _delayMs = delayMs;_throwException = throwException; }
        public async Task<IEnumerable<IPowerTrade>> GetTrades(DateTime date)
        {
            await Task.Delay(_delayMs);
            if (_throwException) throw new Exception("Error: Something unexpected happened");
            return null;
        }
    }
}
