using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;

namespace TestProject.ViewModels
{
    public class StudentGradeModel
    {
        public string ClassName { get; set; }
        public string StudentID { get; set; }
        public string FullName { get; set; }
        public string SubjectName { get; set; }
        public DateTime Date { get; set; }
        public int Grade { get; set; }
    }
}
