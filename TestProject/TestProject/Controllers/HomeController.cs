using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Controllers
{
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
            return View(await db.Classes.ToListAsync());
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
    }
}
