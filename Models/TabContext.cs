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
        ObservableCollection<TimeEntry>? TimeEntries { get; set; }
        public string? Name { get; set; }     
        public string? Icon { get; set; }
        public string? ColorScheme { get; set; }
    }
}
