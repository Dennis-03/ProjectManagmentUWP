using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagmentApp.Model
{
    public class User
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AvatarPath { get; set; }
    }
}
