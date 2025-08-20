using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clockin.Models
{
    public class ContextModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        // add color scheme option?
    }
}
