using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerVolumeExtract.Svc.Implementation;
using PowerVolumeInterface;
using System.IO;

namespace PowerVolumeTest
{
    /*some of the tests in this class are verging on integration tests as they interact with the file system, 
    they may need to be disabled when auto running in a CI context
    */
    [TestClass]
    public class ExtractWriterTests
    {
        IExtractWriter extractWriter;
        string fileextracttestfolder = @"c:\temp\powerextract\unittest";

        [TestMethod]
        public void giveninvalidextractpath_shouldthrowargumentoutofrangeexception()
        {
            try
            {
                extractWriter = new ExtractWriter("xx:\\&362");
                Assert.Fail("Expected ArgumentOutOfRangeException");
            }
            catch (ArgumentOutOfRangeException) { }
        }

        [TestMethod]
        public void givennullinput_shouldthrowargumentnullexception()
        {
            DirectoryInfo df = new DirectoryInfo(fileextracttestfolder);
            if (!df.Exists) df.Create();

            string filename = Guid.NewGuid().ToString() + ".txt";
            try
            {
                extractWriter = new ExtractWriter(fileextracttestfolder);
                extractWriter.Write(filename, null);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException) { }
        }

        [TestMethod]
        public void givenexistingfilename_shouldthrowargumentoutofrangeexception()
        {
            DirectoryInfo df = new DirectoryInfo(fileextracttestfolder);
            if (!df.Exists) df.Create();

            string filename = Guid.NewGuid().ToString() + ".txt";
            try
            {
                //create the extract file, then initialize new extract writer, which will have the effect of trying to write to a locked file
                //so we can check that this is handled by getting an expected outofrange exception (which will be handled gracefully by the wrapping service)
                using (FileStream fs = new FileStream(fileextracttestfolder + @"\" + filename, FileMode.OpenOrCreate))
                {
                    extractWriter = new ExtractWriter(fileextracttestfolder);
                    extractWriter.Write(filename, null);
                    Assert.Fail("Expected ArgumentOutOfRangeException");
                }                
            }
            catch (ArgumentOutOfRangeException) { }
            finally
            {
                try
                {
                    File.Delete(fileextracttestfolder + @"\" + filename);
                    Directory.Delete(fileextracttestfolder, true);
                }
                catch { };
            }
        }

       

    }
}
