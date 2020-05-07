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
using NLog;

namespace TestProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DiaryDBContext db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

        [Authorize(Roles = "admin, director")]
        // вызов формы для добавления класса
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "admin, director")]
        // добавление нового класса в бд
        [HttpPost]
        public async Task<IActionResult> Add(Classes classes)
        {
            db.Classes.Add(classes);
            await db.SaveChangesAsync();
            logger.Info($"Add class {classes.Name} to the DiaryDB");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, director")]
        // показ удаляемого класса
        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            if (name != null)
            {
                Classes classes =
                    await db.Classes.FirstOrDefaultAsync(c => c.Name == name);
                if (classes != null)
                    return View(classes);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin, director")]
        // удаление класса из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Classes classes)
        {
            if (classes != null)
            {
                logger.Trace($"Class {classes.Name} was defined for remove from DiaryDB");
                db.Classes.Remove(classes);
                await db.SaveChangesAsync();
                logger.Info($"Class {classes.Name} was deleted from DiaryDB");
                return RedirectToAction("Index");
            }
            logger.Error("Object Classes classes wasn't defined for remove from DiaryDB");
            return NotFound();
        }
    }
}
