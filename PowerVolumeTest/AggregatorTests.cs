using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerVolumeExtract.Svc.Implementation;
using PowerVolumeInterface;
using PowerVolumeTest.Mock;
using System.Collections.Generic;

namespace PowerVolumeTest
{
    [TestClass]
    public class AggregatorTests
    {
        IVolumeAggregator volumeAggregator = new VolumeAggregator();

        [TestMethod]
        public void givennulloutputfrompowerservice_shouldlogerrorfromaggregator()
        {
            try
            {
                volumeAggregator.Aggregate(DateTime.Now.Date, null);
                Assert.Fail("Expected Argument Exception");
            }
            catch(Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("VolumeAggregator::Aggregate:trade collection provided for aggregation is null"));
            }
        }

        [TestMethod]
        public void givensomenullelementsintradecollection_shouldlogerrorinaggregator()
        {
            DateTime date = DateTime.Now.Date;
            var trades = new List<IPowerTrade>() { new MockPowerTrade(date), new MockPowerTrade(date), null, new MockPowerTrade(date) };
            try
            {
                volumeAggregator.Aggregate(date, trades);
                Assert.Fail("Expected Argument Exception");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("VolumeAggregator::Aggregate:Trade element is null in trades collection provided"));
            }
        }

        [TestMethod]
        public void giveninconsistentnumberofperiodsincollection_shouldlogerrorinaggregator()
        {
            DateTime date = DateTime.Now.Date;
            var trades = new List<IPowerTrade>() { new MockPowerTrade(date), new MockPowerTrade(date), new MockPowerTrade(date), new MockPowerTrade(date, 5) };
            try
            {
                volumeAggregator.Aggregate(date, trades);
                Assert.Fail("Expected Argument Exception");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("VolumeAggregator::Aggregate:Inconsistent number of trade periods provided"));
            }
        }

        [TestMethod]
        public void giveninconsistentdatesincollection_shouldlogerrorinaggregator()
        { 
            DateTime date = DateTime.Now.Date;
            var trades = new List<IPowerTrade>() { new MockPowerTrade(date), new MockPowerTrade(date), new MockPowerTrade(date.AddDays(2)), new MockPowerTrade(date) };
            try
            {
                volumeAggregator.Aggregate(date, trades);
                Assert.Fail("Expected Argument Exception");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("VolumeAggregator::Aggregate:Inconsistent dates in trade collection provided"));
            }
        }

        [TestMethod]
        public void givenspecificvalues_shouldreturncorrectaggregate()
        {
            DateTime date = DateTime.Now.Date;
            var trades = new List<IPowerTrade>() { new MockPowerTrade(date), new MockPowerTrade(date), new MockPowerTrade(date) };

            //create some dummy value periods
            int numPeriods = 24;
            foreach(var pt in trades)
            {
                List<IPowerPeriod> periods = new List<IPowerPeriod>();
                for (int k = 0; k < numPeriods; k++) periods.Add(new MockPowerPeriod() { Period = k + 1, Volume = k });
                ((MockPowerTrade)pt).Periods = periods.ToArray();
            }

            var aggr = volumeAggregator.Aggregate(date, trades);

            //check we returnd the right number of values
            Assert.IsTrue(aggr.Volumes.Count == numPeriods);

            //check the values are as expected
            for(int k = 0; k < aggr.Volumes.Count; k++)
            {
                Assert.IsTrue(aggr.Volumes[k] == trades.Count * k);
            }
            
        }
        
    }
}
