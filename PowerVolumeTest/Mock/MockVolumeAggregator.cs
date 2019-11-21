using System;
using System.Collections.Generic;
using PowerVolumeInterface;

namespace PowerVolumeTest.Mock
{
    internal class MockVolumeAggregator : IVolumeAggregator
    {
        private readonly bool _throwException;
        public MockVolumeAggregator(bool throwException = false) { _throwException = throwException; }
        public IVolumeAggregate Aggregate(DateTime date, IEnumerable<IPowerTrade> trades)
        {
            if (_throwException) throw new Exception("Error: Something unexpected happened");
            return null;
        }
    }
}
