using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers
{
    public class ClassSubjectController : Controller
    {
        DiaryDBContext db;
        SubjectService subjectService;

        public ClassSubjectController(DiaryDBContext context, SubjectService service)
        {
            db = context;
            subjectService = service;
        }

        // вывод списка классов, у которых ведётся выбранный предмет
        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            Subjects subject = await subjectService.GetSubject(name);
            if (subject.Name != null)
            {
                var classes =
                    db.ClassSubject.FromSqlInterpolated($"Select * From ClassSubject Where SubjectName={subject.Name}").ToList();
                if (classes != null)
                    return View(classes);
            }
            return NotFound();
        }
    }
}
