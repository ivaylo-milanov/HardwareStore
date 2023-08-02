namespace HardwareStore.Infrastructure.Data
{
    using Newtonsoft.Json;

    public static class JsonFileReader
    {
        public static IEnumerable<T> LoadJson<T>(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json)!;
            }
        }
    }
}
