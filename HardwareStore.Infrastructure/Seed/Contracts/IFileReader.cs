namespace HardwareStore.Infrastructure.Seed.Contracts
{
    public interface IFileReader
    {
        string[] GetFilesNames(string jsonFileTemplate);

        IEnumerable<T> LoadJson<T>(string fileName);
    }
}
