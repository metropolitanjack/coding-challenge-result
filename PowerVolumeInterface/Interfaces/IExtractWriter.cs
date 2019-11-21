
namespace PowerVolumeInterface
{
    public interface IExtractWriter
    {
        void Write(string filename, IVolumeAggregate trade);
    }
}
