using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Model
{
    public class Reaction
    {
        [PrimaryKey]
        public long Id { get; set; }
        public long ReactedToId { get; set; }
        public long ReactedById { get; set; }
    }
}
