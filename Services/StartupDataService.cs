using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clockin.Services
{
    public interface IStartupDataService
    {
        Task<Guid> GetLastTabSelectedAsync();
        Task<int> GetTabCountAsync();
    }

    public class StartupDataService : IStartupDataService
    {
        public async Task<Guid> GetLastTabSelectedAsync()
        {
            const string LastSelectedTabKey = "LastSelectedTabKey";
            var rawTabId = await SecureStorage.Default.GetAsync(LastSelectedTabKey);

            if (!String.IsNullOrEmpty(rawTabId) && Guid.TryParse(rawTabId, out Guid id))
            {
                return id;
            }
            else
            {
                return Guid.CreateVersion7();
            }
        }

        public async Task<int> GetTabCountAsync()
        {
            const string NumberOfTabsKey = "NumberOfTabsKey";
            string? rawTabCount = await SecureStorage.Default.GetAsync(NumberOfTabsKey);
            if (!String.IsNullOrEmpty(rawTabCount) && int.TryParse(rawTabCount, out int count))
            {
                return count;
            }
            else
            {
                return 2; // default number of tabs. Can probably make this less hard-cody
            }
        }
    }
}
