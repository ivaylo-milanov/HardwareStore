namespace HardwareStore.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public static class SessionExtension
    {
        public static T? Get<T>(this ISession session, string name)
        {
            var value = session.GetString(name);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void Set<T>(this ISession session, string name, T value)
        {
            session.SetString(name, JsonConvert.SerializeObject(value));
        }

        public static void Remove(this ISession session, string name)
        {
            session.Remove(name);
        }
    }
}
