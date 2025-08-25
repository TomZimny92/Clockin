using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clockin.Models
{
    public class AppContext
    {
        public int NumberOfTabs { get; set; }
        public Guid LastUsedTab { get; set; }

    }
}
