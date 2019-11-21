using PowerVolumeInterface;

namespace PowerVolumeTest.Mock
{
    internal class MockPowerPeriod :IPowerPeriod
    {
        public MockPowerPeriod(int period = 0, double volume = 0) { Period = period; Volume = volume; }
        public int Period { get; set; }
        public double Volume { get; set; }
    }
}
