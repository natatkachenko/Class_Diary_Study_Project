using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;

namespace TestProject.ViewModels
{
    public class StudentGradeOnSubjectModel
    {
        public IEnumerable<Students> Students { get; set; }
        public IEnumerable<SubjectGradeModel> SubjectGrades { get; set; }
        public IEnumerable<Subjects> Subjects { get; set; }
    }
}
