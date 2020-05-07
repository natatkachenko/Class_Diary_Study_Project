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
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "admin, director")]
        // вызов формы для добавления класса, изучающего предмет
        public IActionResult Add()
        {
            SelectList subjects = new SelectList(db.Subjects, "Name", "Name");
            ViewBag.Subjects = subjects;
            SelectList classes = new SelectList(db.Classes, "Name", "Name");
            ViewBag.Classes = classes;
            return View();
        }

        [Authorize(Roles = "admin, director")]
        // добавление нового класса, изучающего предмет в бд
        [HttpPost]
        public async Task<IActionResult> Add(ClassSubject classSubject)
        {
            db.ClassSubject.Add(classSubject);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { name = classSubject.SubjectName });
        }

        [Authorize(Roles = "admin, director")]
        // показ удаляемого класса, изучающего предмет
        [HttpGet]
        public async Task<IActionResult> Delete(string subjectName, string className)
        {
            if (subjectName != null && className != null)
            {
                ClassSubject classSubject =
                    await db.ClassSubject.FirstOrDefaultAsync(c => c.SubjectName == subjectName && c.ClassName == className);
                if (classSubject != null)
                    return View(classSubject);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin, director")]
        // удаление класса, изучающего предмет из бд
        [HttpPost]
        public async Task<IActionResult> Delete(ClassSubject classSubject)
        {
            if (classSubject != null)
            {
                db.ClassSubject.Remove(classSubject);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { name = classSubject.SubjectName });
            }
            return NotFound();
        }
    }
}
