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
    public class SubjectsController : Controller
    {
        DiaryDBContext db;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(DiaryDBContext context, ILogger<SubjectsController> logger)
        {
            db = context;
            _logger = logger;
        }

        //вывод списка предметов
        public IActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

        // вызов формы для добавления предмета
        public IActionResult Add()
        {
            return View();
        }

        // добавление нового предмета в бд
        [HttpPost]
        public async Task<IActionResult> Add(Subjects subjects)
        {
            db.Subjects.Add(subjects);
            await db.SaveChangesAsync();
            _logger.LogInformation($"Add subject {subjects.Name} to the DiaryDB");
            return RedirectToAction("Index");
        }

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

        // удаление предмета из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Subjects subjects)
        {
            if (subjects != null)
            {
                _logger.LogTrace($"Subject {subjects.Name} was defined for remove from DiaryDB");
                db.Subjects.Remove(subjects);
                await db.SaveChangesAsync();
                _logger.LogInformation($"Subject {subjects.Name} was deleted from DiaryDB");
                return RedirectToAction("Index");
            }
            _logger.LogError("Object Subjects subjects wasn't defined for remove from DiaryDB");
            return NotFound();
        }
    }
}
