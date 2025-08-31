using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clockin.Services
{
    public interface IStartupDataService
    {
        Task<Guid?> GetLastTabSelectedAsync();
        Task LoadExistingTabs();
    }

    public class StartupDataService : IStartupDataService
    {
        public async Task<Guid?> GetLastTabSelectedAsync()
        {
            const string LastSelectedTabKey = "LastSelectedTabKey";
            var rawTabId = await SecureStorage.Default.GetAsync(LastSelectedTabKey);

            if (!String.IsNullOrEmpty(rawTabId) && Guid.TryParse(rawTabId, out Guid id))
            {
                return id;
            }
            else
            {
                return null;
            }
        }

        public async Task LoadExistingTabs()
        {
            Console.WriteLine("test");
            await SecureStorage.Default.GetAsync("test");
        }
    }
}
