using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Model
{
    public class Reaction
    {
        long Id = DateTime.Now.Ticks;
        public long ReactedToId { get; set; }
        public long ReactedById { get; set; }
    }
}
