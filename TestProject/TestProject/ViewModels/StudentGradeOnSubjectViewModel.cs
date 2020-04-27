using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestProject.ViewModels
{
    public class StudentGradeOnSubjectViewModel
    {
        public IEnumerable<StudentGradeModel> Students { get; set; }
        public SelectList Subjects { get; set; }
        public SelectList Classes { get; set; }
    }
}
