using Strategy.Core.Domain.Interfaces.Services;
using Strategy.Core.Domain.Models;
using System.Runtime.InteropServices;

namespace Strategy.Core.Services
{
    public class PhysicalAccountService : IProducts
    {
        public void Save(AccountBase request)
        {
            //rule for product B
        }

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