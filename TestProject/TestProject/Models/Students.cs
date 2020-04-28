using System;
using System.Collections.Generic;

namespace TestProject.Models
{
    public partial class Students
    {
        public string ClassName { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }

        public List<SubjectGrade> subjectGrades { get; set; }
        public Students()
        {
            subjectGrades = new List<SubjectGrade>();
        }
    }
}
