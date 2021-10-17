using System.Diagnostics;
namespace SchemeCreator.Data.Extensions
{
    public static class DebugExtension
    {
        public static void Log<T>(this T classToLog, string message) => Debug.WriteLine("\t" + message, typeof(T).Name);
    }
}
