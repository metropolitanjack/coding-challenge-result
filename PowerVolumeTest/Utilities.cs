using PowerVolumeInterface;
using PowerVolumeTest.Mock;

namespace PowerVolumeTest
{
    internal class Utilities
    {
        public static string Log(ILogger ml)
        {
            return string.Join("|", ((MockLogger)ml).Entries.ToArray());
        }

        public static string FakeExpectedMessage1 = "Beginning scheduled extract run";
        public static string FakeExceptionMessage1 = "Error: Something unexpected happened";
        public static string FakeExceptionMessage2 = "Warning: Previous scheduled extract is still running";
    }
}
