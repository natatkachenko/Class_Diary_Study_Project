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
        public async Task<IActionResult> Index(string classname)
        {
            if (classname != null)
            {
                var students = await db.Students.FromSqlInterpolated($"Select * From Students Where ClassName={classname}").ToListAsync();
                return View(students);
            }
            return NotFound();
        }

        // вызов формы для добавления ученика класса
        public IActionResult Add()
        {
            return View();
        }

        // добавление нового ученика класса в бд
        [HttpPost]
        public async Task<IActionResult> Add(Students student)
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // показ удаляемого ученика класса
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Students student =
                    await db.Students.FirstOrDefaultAsync(s => s.StudentId == id);
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
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        int a = 1;
        [HttpPost]
        public async Task<IActionResult> Delete2(Students student)
        {
            if (student != null)
            {
                db.Students.Remove(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
