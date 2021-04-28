using ProjectManagmentApp.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Model
{   
    public class ZTask : INotifyPropertyChanged
    {
        public long Id = DateTime.Now.Ticks;
        public string TaskName { get; set; }
        public string Description { get; set; }

        public PriorityEnum Priority;
        public long AssignedTo { get; set; }
        public long AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }

        private bool completed;
        public bool Completed
        {
            get { return completed; }
            set
            {
                completed = value;
                OnPropertyChange("Completed");
            }
        }

        private List<Comment> _Comment = new List<Comment>();
        public List<Comment> Comment { get { return _Comment; } set { _Comment = value; } }

        private List<Reaction> _Reaction = new List<Reaction>();
        public List<Reaction> Reaction { get { return _Reaction; } set { _Reaction = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
