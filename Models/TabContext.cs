using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clockin.Models
{
    public class TabContext
    {
        public Guid Id { get; set; }
        public ObservableCollection<TimeEntry>? TimeEntries { get; set; }
        public bool IsCheckedIn { get; set; }
        public string? Name { get; set; }     
        public string? Icon { get; set; }
        public string? ColorScheme { get; set; }

        public static TabContext CreateNewTabData()
        {
            return new TabContext()
            {
                Id = Guid.CreateVersion7(),
                TimeEntries = [],
                IsCheckedIn = false,
                Name = "Tab1",
            };
        }
    }
}
