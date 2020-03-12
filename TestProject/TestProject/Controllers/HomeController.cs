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
using Microsoft.Data.SqlClient;

namespace TestProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DiaryDBContext db;
        public HomeController(DiaryDBContext context)
        {
            db = context;
        }

        // вывод списка классов
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(await db.Classes.ToListAsync());
            }
            return Content("Пользователь не аутентифицирован");
        }

        // вызов формы для добавления класса
        public IActionResult Add()
        {
            return View();
        }

        // добавление нового класса в бд
        [HttpPost]
        public async Task<IActionResult> Add(Classes classes)
        {
            db.Classes.Add(classes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // показ удаляемого класса
        [HttpGet]
        public async Task<IActionResult> Delete(string classname)
        {
            if (classname != null)
            {
                Classes classes =
                    await db.Classes.FirstOrDefaultAsync(c => c.ClassName == classname);
                if (classes != null)
                    return View(classes);
            }
            return NotFound();
        }

        // удаление класса из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Classes classes)
        {
            if (classes != null)
            {
                db.Classes.Remove(classes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // вывод списка студентов выбранного класса
        public async Task<IActionResult> Students(string classname)
        {
            if (classname != null)
            {
                var students = await db.Students.FromSqlInterpolated($"Select * From Students Where ClassName={classname}").ToListAsync();
                return View(students);
            }
            return NotFound();
        }
    }
}
