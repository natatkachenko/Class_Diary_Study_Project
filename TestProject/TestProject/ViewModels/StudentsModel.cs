using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestProject.ViewModels
{
    public class StudentsModel
    {
        public SubjectGrade SubjectGrade { get; set; }
        public IEnumerable<SelectListItem> Students { get; set; }
    }
}
