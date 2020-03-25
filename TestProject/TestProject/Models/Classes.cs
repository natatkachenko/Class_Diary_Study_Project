using System;
using System.Collections.Generic;

namespace TestProject.Models
{
    public partial class Class
    {
        public string Name { get; set; }
        public ICollection<Students> Students { get; set; } 
        public Class()
        {
            Students = new List<Students>();
        }
    }
}
