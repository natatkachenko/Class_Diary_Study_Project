using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Message { get; set; }
    }
}
