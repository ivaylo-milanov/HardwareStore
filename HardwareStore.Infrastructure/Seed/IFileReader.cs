namespace HardwareStore.Infrastructure.Seed
{
    public interface IFileReader
    {
        string[] GetFilesNames(string jsonFileTemplate);

        IEnumerable<T> LoadJson<T>(string fileName);
    }
}
