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
    public class SubjectGradeController : Controller
    {
        DiaryDBContext db;
        public SubjectGradeController(DiaryDBContext context)
        {
            db = context;
        }

        // вывод списка предметов с оценками выбранного ученика
        [HttpGet]
        public IActionResult Grade(int? id)
        {
            if (id != null)
            {
                var stgrade = db.SubjectGrade.FromSqlInterpolated($"Select * From SubjectGrade Where StudentId={id}").ToList();
                if (stgrade != null)
                    return View(stgrade);
            }
            return NotFound();
        }

        // вызов формы для ввода оценки
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, int? studentid, string subjectname)
        {
            if (id!=null && studentid!=null && subjectname != null)
            {
                SubjectGrade sbgrade = 
                    await db.SubjectGrade
                    .FirstOrDefaultAsync(sb => sb.Id==id && sb.StudentId==studentid && sb.SubjectName == subjectname);
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
                    return new ContentResult()
                    {
                        Content = "Запрещено вводить оценку в выходной день!"
                    };
            }
            switch (sbgrade.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    db.SubjectGrade.Update(sbgrade);
                    await db.SaveChangesAsync();
                    break;
            }
            return RedirectToAction("Grade", new { id = sbgrade.StudentId });
        }
    }
}
