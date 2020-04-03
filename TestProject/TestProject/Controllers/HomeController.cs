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
    [Authorize]
    public class HomeController : Controller
    {
        DiaryDBContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(DiaryDBContext context, ILogger<HomeController> logger)
        {
            db = context;
            _logger = logger;
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
            _logger.LogInformation($"Add class {classes.Name} to the DiaryDB");
            return RedirectToAction("Index");
        }

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

        // удаление класса из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Classes classes)
        {
            if (classes != null)
            {
                _logger.LogTrace($"Class {classes.Name} was defined for remove from DiaryDB");
                db.Classes.Remove(classes);
                await db.SaveChangesAsync();
                _logger.LogInformation($"Class {classes.Name} was deleted from DiaryDB");
                return RedirectToAction("Index");
            }
            _logger.LogError("Object Classes classes wasn't defined for remove from DiaryDB");
            return NotFound();
        }
    }
}
