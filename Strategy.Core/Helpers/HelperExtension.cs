using System.Runtime.InteropServices;
using System;

namespace Strategy.Core.Helpers
{
    public class HelperExtension
    {
        private TimeZoneInfo GetBrasilTimeZone()
        {
            TimeZoneInfo brasiliaTime = null;
            brasiliaTime = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo")
                : TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            return brasiliaTime;
        }
    }
}
