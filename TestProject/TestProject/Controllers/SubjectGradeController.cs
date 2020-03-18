using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class SubjectGradeController : Controller
    {
        DiaryDBContext db;
        public SubjectGradeController(DiaryDBContext context)
        {
            db = context;
        }

        // вывод списка предметов с оценками выбранного ученика
        public async Task<IActionResult> Grade(int? studentId)
        {
            if (studentId != null)
            {
                var stgrade = await db.SubjectGrade.FromSqlInterpolated($"Select * From SubjectGrade Where StudentId={studentId}").ToListAsync();
                return View(stgrade);
            }
            return NotFound();
        }

        // вызов формы для ввода оценки
        public async Task<IActionResult> Edit(string sbname)
        {
            if (sbname != null)
            {
                SubjectGrade sbgrade = await db.SubjectGrade.FirstOrDefaultAsync(sb => sb.SubjectName == sbname);
                if (sbgrade != null)
                    return View(sbgrade);
            }
            return NotFound();
        }

        // сохранение оценки в бд
        [HttpPost]
        public async Task<IActionResult> Edit(SubjectGrade sbgrade)
        {
            switch (sbgrade.Date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    Message();
                    Thread.Sleep(5000);
                    break;
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    db.SubjectGrade.Update(sbgrade);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Grade");
            }
            return RedirectToAction("Grade");
        }

        public string Message()
        {
            return "Запрещено вводить оценку в выходной день!";
        }
    }
}
