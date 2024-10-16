namespace HardwareStore.Infrastructure.Seed
{
    using HardwareStore.Infrastructure.Seed.Contracts;
    using Newtonsoft.Json;

    public class FileReader : IFileReader
    {
        public IEnumerable<T> LoadJson<T>(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json)!;
            }
        }

        public string[] GetFilesNames(string jsonFileTemplate)
        {
            var importsPath = Path.Combine(Environment.CurrentDirectory, "..", "HardwareStore.Infrastructure", "Imports");
            var fileNames = Directory.GetFiles(importsPath, jsonFileTemplate);

            return fileNames.ToArray();
        }
    }
}
