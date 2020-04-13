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
using TestProject.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLog;

namespace TestProject.Controllers
{
    public class SubjectGradeController : Controller
    {
        DiaryDBContext db;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        StudentService studentService;

        public SubjectGradeController(DiaryDBContext context, StudentService service)
        {
            db = context;
            studentService = service;
        }

        // вывод списка предметов с оценками выбранного ученика
        [HttpGet]
        public async Task<IActionResult> Grade(int id)
        {
            Students student = await studentService.GetStudent(id);
            var stgrade = db.SubjectGrade.FromSqlInterpolated($"Select * From SubjectGrade Where StudentId={student.Id}").ToList();
            if (stgrade != null)
                return View(stgrade);
            return NotFound();
        }

        // вызов формы для добавления новой оценки
        public IActionResult Add()
        {
            SelectList students = new SelectList(db.Students, "Id", "FullName");
            ViewBag.Students = students;
            SelectList subjects = new SelectList(db.Subjects, "Name", "Name");
            ViewBag.Subjects = subjects;
            return View();
        }

        // добавление новой оценки в бд
        [HttpPost]
        public async Task<IActionResult> Add(SubjectGrade sbgrade)
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
                    db.SubjectGrade.Add(sbgrade);
                    await db.SaveChangesAsync();
                    break;
            }
            logger.Info($"Add grade (StudentID = {sbgrade.StudentId}, Subject = {sbgrade.SubjectName}) to the DiaryDB");
            return RedirectToAction("Grade", new { id = sbgrade.StudentId });
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

        // показ удаляемой оценки ученика 
        [HttpGet]
        public async Task<IActionResult> Delete(int? stid, int? id, string sbname)
        {
            if (stid != null && id != null && sbname != null)
            {
                SubjectGrade subjectGrade =
                    await db.SubjectGrade
                    .FirstOrDefaultAsync(s => s.StudentId == stid && s.Id == id && s.SubjectName == sbname);
                if (subjectGrade != null)
                    return View(subjectGrade);
            }
            return NotFound();
        }

        // удаление оценки ученика из бд
        [HttpPost]
        public async Task<IActionResult> Delete(SubjectGrade subjectGrade)
        {
            if (subjectGrade != null)
            {
                logger.Trace($"Grade (StudentID = {subjectGrade.StudentId}, Subject = {subjectGrade.SubjectName}) was defined for remove from DiaryDB");
                db.SubjectGrade.Remove(subjectGrade);
                await db.SaveChangesAsync();
                logger.Info($"Grade (StudentID = {subjectGrade.StudentId}, Subject = {subjectGrade.SubjectName}) was deleted from DiaryDB");
                return RedirectToAction("Grade", new { id = subjectGrade.StudentId });
            }
            logger.Error("Object SubjectGrade subjectGrade wasn't defined for remove from DiaryDB");
            return NotFound();
        }
    }
}
