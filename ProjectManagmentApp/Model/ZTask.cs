using ProjectManagmentApp.Constants;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Model
{   
    public class ZTask 
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        [TextBlob("Priority")]
        public PriorityEnum Priority { get; set; }
        public long AssignedTo { get; set; }
        public long AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }

        private List<Comment> _Comment = new List<Comment>();
        [Ignore]
        public List<Comment> Comment { get { return _Comment; } set { _Comment = value; } }

        private List<Reaction> _Reaction = new List<Reaction>();
        [Ignore]
        public List<Reaction> Reaction { get { return _Reaction; } set { _Reaction = value; } }
    }
}
