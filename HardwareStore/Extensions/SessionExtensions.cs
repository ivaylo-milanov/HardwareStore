namespace HardwareStore.Extensions
{
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string name)
        {
            return session.Get<T>(name);
        }

        public static void Set<T>(this ISession session, string name, T value)
        {
            session.Set<T>(name, value);
        }

        public static void Remove(this ISession session, string name)
        {
            session.Remove(name);
        }
    }
}
