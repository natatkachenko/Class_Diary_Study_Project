using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using TestProject.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLog;
using Microsoft.AspNetCore.Authorization;

namespace TestProject.Controllers
{
    public class StudentsController : Controller
    {
        DiaryDBContext db;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        ClassService classService;

        public StudentsController(DiaryDBContext context, ClassService service)
        {
            db = context;
            classService = service;
        }

        // вывод списка учеников выбранного класса
        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            Classes stclass = await classService.GetClass(name);
            if (stclass.Name != null)
            {
                var students = 
                    db.Students.FromSqlInterpolated($"Select * From Students Where ClassName={stclass.Name}").ToList();
                if (students != null)
                    return View(students);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin, director")]
        // вызов формы для добавления ученика класса
        public IActionResult Add()
        {
            SelectList classes = new SelectList(db.Classes, "Name", "Name");
            ViewBag.Classes = classes;
            return View();
        }

        [Authorize(Roles = "admin, director")]
        // добавление нового ученика класса в бд
        [HttpPost]
        public async Task<IActionResult> Add(Students student)
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();
            logger.Info($"Add student (ID = {student.Id}) to the DiaryDB");
            return RedirectToAction("Index", new { name = student.ClassName });
        }

        [Authorize(Roles = "admin, director")]
        // показ удаляемого ученика класса
        [HttpGet]
        public async Task<IActionResult> Delete(string classname, int? id)
        {
            if (classname != null && id != null)
            {
                Students student =
                    await db.Students.FirstOrDefaultAsync(s => s.ClassName == classname && s.Id == id);
                if (student != null)
                    return View(student);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin, director")]
        // удаление ученика класса из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Students student)
        {
            if (student != null)
            {
                logger.Trace($"Student (ID = {student.Id}) was defined for remove from DiaryDB");
                db.Students.Remove(student);
                await db.SaveChangesAsync();
                logger.Info($"Student (ID = {student.Id}) was deleted from DiaryDB");
                return RedirectToAction("Index", new { name = student.ClassName });
            }
            logger.Error("Object Students student wasn't defined for remove from DiaryDB");
            return NotFound();
        }
    }
}
