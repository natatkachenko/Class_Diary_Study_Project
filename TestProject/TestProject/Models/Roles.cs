using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;

namespace TestProject.Models
{
    public class Roles
    {
        public string Name { get; set; }
        public List<Users> Users { get; set; }
        public Roles()
        {
            Users = new List<Users>();
        }
    }
}
