
namespace PowerVolumeInterface
{
    public interface IExtractWriter
    {
        void Write(string filename, IVolumeAggregate trade);
        void Write(string filename, IVolumeAggregate trade, string comment);
        void Write(string filename, IVolumeAggregate trade, string comment1, string comment2);
        void Write(string filename, IVolumeAggregate trade, string comment1, string comment2, string comment3);
    }
}
