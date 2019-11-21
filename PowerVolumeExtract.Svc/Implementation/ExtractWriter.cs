using System;
using System.IO;
using PowerVolumeInterface;

namespace PowerVolumeExtract.Svc.Implementation
{
    public class ExtractWriter : IExtractWriter
    {
        private readonly string _extractFolder;
        public ExtractWriter(string extractFolder)
        {
            //check that the folder exists, if not try to create it
            try
            {
                DirectoryInfo df = new DirectoryInfo(extractFolder);
                if (!df.Exists) df.Create();
            }
            catch { throw new ArgumentOutOfRangeException(extractFolder); }

            _extractFolder = extractFolder;

        }
        public void Write(string filename, IVolumeAggregate aggregate)
        {
            string fullPath = _extractFolder + @"\" + filename;
            FileInfo fi = new FileInfo(fullPath);

            if (fi.Exists)
                throw new ArgumentOutOfRangeException("aggregate", "ExtractWriter::Write: filename provided for write operation already exists");

            if (aggregate == null)
                throw new ArgumentNullException("aggregate", "ExtractWriter::Write: aggregate collection provided for write operation is null");

            using (StreamWriter sr = new StreamWriter(fullPath))
            {
                //write header
                sr.WriteLine(FormatLine(new[] { "LocalTime", "Volume" }));
                TimeSpan start = new TimeSpan(23, 0, 0);
                for(int k = 0; k < aggregate.Volumes.Count; k++)
                {
                    var currTimeSpan = start + new TimeSpan(k, 0, 0);
                    sr.WriteLine(FormatLine(new[] { currTimeSpan.Hours.ToString("00") + ":00", aggregate.Volumes[k].ToString() }));
                }
            }
        }

        private string FormatLine(string[] items)
        {
            return string.Join(",", items);
        }
    }
}
