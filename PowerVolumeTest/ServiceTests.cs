using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerVolumeExtract.Svc;
using PowerVolumeInterface;
using PowerVolumeTest.Mock;
using System.Threading;

namespace PowerVolumeTest
{
    [TestClass]
    public class ServiceTests
    {
        ILogger mockLogger = new MockLogger();
        IPowerService mockPowerService = new MockPowerService();
        IVolumeAggregator mockAggregator = new MockVolumeAggregator();
        IExtractWriter mockExtractWriter = new MockExtractWriter();
        double intervalMins = 60000;
        int waitTest = 5000;


        [TestMethod]
        public void givennullpowerserviceconstructorparam_shouldthrowargumentnullexception()
        {
            try
            {
                mockPowerService = null;
                Service pveSvc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMins);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch(ArgumentNullException)  {   }
        }

        [TestMethod]
        public void givennullvolumeaggregatorconstructorparam_shouldthrowargumentnullexception()
        {
            try
            {
                mockAggregator = null;
                Service pveSvc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMins);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException) { }
        }

        [TestMethod]
        public void givennullextractwriterconstructorparam_shouldthrowargumentnullexception()
        {
            try
            {
                IExtractWriter extractWriter = null;
                Service pveSvc = new Service(mockPowerService, mockAggregator, extractWriter, mockLogger, intervalMins);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException) { }
        }

        [TestMethod]
        public void givennullloggerconstructorparam_shouldthrowargumentnullexception()
        {
            try
            {
                mockLogger = null;
                Service pveSvc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMins);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException) { }
        }

        [TestMethod]
        public void givenextractintervallessthanoneminute_shouldthrowargumenoutofrangeexception()
        {
            try
            {
                double intervalMs = 500;
                Service pveSvc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMs);
                Assert.Fail("Expected ArgumentOutOfRangeException");
            }
            catch (ArgumentOutOfRangeException) { }
        }

        

        [TestMethod]
        public void givenanylonginterval_shouldrunextractonstartup()
        {
            Service svc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMins);
            svc.Start();
            Assert.IsTrue(Utilities.Log(mockLogger).Contains(Utilities.FakeExpectedMessage1));

            svc.Stop();
        }

        [TestMethod]
        public void givenlongrunningpowerservice_shouldlogwarningsonextractschedule()
        {
            double extractIntervalMs = 1000;
            double powerServiceLongRunningMs = extractIntervalMs * 3;
            double testCheckDelayMs = extractIntervalMs * 4.5;

            mockPowerService = new MockPowerService((int)powerServiceLongRunningMs);
            Service.MinIntervalMs = extractIntervalMs;
            Service svc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, extractIntervalMs);
            
            svc.Start();

            //keep service alive
            Thread.Sleep((int)testCheckDelayMs);

            Assert.IsTrue(Utilities.Log(mockLogger).Contains(Utilities.FakeExceptionMessage2));

            svc.Stop();
        }

        [TestMethod]
        public void givenexceptioninpowerservice_shouldhandleandlogerrorinservice()
        {
            mockPowerService = new MockPowerService(1000, true);
            Service svc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMins);
            svc.Start();
           
            //keep service alive
            Thread.Sleep(waitTest);

            Assert.IsTrue(Utilities.Log(mockLogger).Contains(Utilities.FakeExceptionMessage1));

            svc.Stop();
        }

        [TestMethod]
        public void givenexceptioninaggregator_shouldhandleandlogerrorinservice()
        {
            mockAggregator = new MockVolumeAggregator(true);
            Service svc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMins);
            svc.Start();

            //keep service alive
            Thread.Sleep(waitTest);

            Assert.IsTrue(Utilities.Log(mockLogger).Contains(Utilities.FakeExceptionMessage1));

            svc.Stop();
        }

        [TestMethod]
        public void givenexceptioninextractwritter_shouldhandleandlogerrorinservice()
        {
            mockExtractWriter = new MockExtractWriter(true);
            Service svc = new Service(mockPowerService, mockAggregator, mockExtractWriter, mockLogger, intervalMins);
            svc.Start();

            //keep service alive
            Thread.Sleep(waitTest);

            Assert.IsTrue(Utilities.Log(mockLogger).Contains(Utilities.FakeExceptionMessage1));

            svc.Stop();
        }

    }
}
