using System;
using PowerVolumeInterface;

namespace PowerVolumeTest.Mock
{
    internal class MockExtractWriter : IExtractWriter
    {
        private readonly bool _throwException;
        public MockExtractWriter(bool throwException = false) { _throwException = throwException; }
        public void Write(string filename, IVolumeAggregate powerTrade)
        {
            if (_throwException) throw new Exception("Error: Something unexpected happened");
           
        }
    }
}
