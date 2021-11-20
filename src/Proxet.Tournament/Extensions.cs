using System.IO;

namespace Proxet.Tournament
{
    public static class Extensions
    {
        public static void SkipHeaders(this StreamReader sr) => sr.ReadLine();
    }
}
