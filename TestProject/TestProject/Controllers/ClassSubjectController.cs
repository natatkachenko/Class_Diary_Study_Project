using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class ClassSubjectController : Controller
    {
        DiaryDBContext db;
        public ClassSubjectController(DiaryDBContext context)
        {
            db = context;
        }

        // вывод списка классов, у которых ведётся выбранный предмет
        [HttpGet]
        public IActionResult Index(string name)
        {
            if (name != null)
            {
                var classes =
                    db.ClassSubject.FromSqlInterpolated($"Select * From ClassSubject Where SubjectName={name}").ToList();
                if (classes != null)
                    return View(classes);
            }
            return NotFound();
        }
    }
}
