using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using NLog;
using Microsoft.AspNetCore.Authorization;

namespace TestProject.Controllers
{
    public class SubjectsController : Controller
    {
        DiaryDBContext db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SubjectsController(DiaryDBContext context)
        {
            db = context;
        }

        //вывод списка предметов
        public IActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

        [Authorize(Roles = "admin, director")]
        // вызов формы для добавления предмета
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "admin, director")]
        // добавление нового предмета в бд
        [HttpPost]
        public async Task<IActionResult> Add(Subjects subjects)
        {
            db.Subjects.Add(subjects);
            await db.SaveChangesAsync();
            logger.Info($"Add subject {subjects.Name} to the DiaryDB");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, director")]
        // показ удаляемого предмета
        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            if (name != null)
            {
                Subjects subjects =
                    await db.Subjects.FirstOrDefaultAsync(s => s.Name == name);
                if (subjects != null)
                    return View(subjects);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin, director")]
        // удаление предмета из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Subjects subjects)
        {
            if (subjects != null)
            {
                logger.Trace($"Subject {subjects.Name} was defined for remove from DiaryDB");
                db.Subjects.Remove(subjects);
                await db.SaveChangesAsync();
                logger.Info($"Subject {subjects.Name} was deleted from DiaryDB");
                return RedirectToAction("Index");
            }
            logger.Error("Object Subjects subjects wasn't defined for remove from DiaryDB");
            return NotFound();
        }
    }
}
