using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class StudentsController : Controller
    {
        DiaryDBContext db;
        public StudentsController(DiaryDBContext context)
        {
            db = context;
        }

        // вывод списка учеников выбранного класса
        public IActionResult Index()
        {
            var students = db.Students.Include(s => s.ClassName);
            return View(students.ToList());
        }

        // вызов формы для добавления ученика класса
        [HttpGet]
        public IActionResult Add(string classname)
        {
            if (classname != null)
            {
                var students = db.Students.FirstOrDefault(s => s.ClassName == classname);
                return View(students);
            }
            return NotFound();
        }

        // добавление нового ученика класса в бд
        [HttpPost]
        public async Task<IActionResult> Add(Students student)
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { classname = student.ClassName });
        }

        // показ удаляемого ученика класса
        [HttpGet]
        public async Task<IActionResult> Delete(string classname, int? studentid)
        {
            if (classname != null && studentid != null)
            {
                Students student =
                    await db.Students.FirstOrDefaultAsync(s => s.ClassName == classname && s.Id == studentid);
                if (student != null)
                    return View(student);
            }
            return NotFound();
        }

        // удаление ученика класса из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Students student)
        {
            if (student != null)
            {
                db.Students.Remove(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { classname = student.ClassName });
            }
            return NotFound();
        }
    }
}
