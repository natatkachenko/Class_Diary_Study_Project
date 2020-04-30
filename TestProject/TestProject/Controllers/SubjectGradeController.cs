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
using TestProject.Descriptor;
using TestProject.ViewModels;

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
            var studentGrade = db.SubjectGrade.FromSqlInterpolated($"Select * From SubjectGrade Where StudentId={student.Id}").ToList();
            if (studentGrade != null)
                return View(studentGrade);
            return NotFound();
        }

        // вызов формы для добавления новой оценки
        public IActionResult Add()
        {
            SelectList classes = new SelectList(db.Classes, "Name", "Name");
            ViewBag.Classes = classes;
            SelectList students = new SelectList(db.Students, "Id", "FullName");
            ViewBag.Students = students;
            SelectList subjects = new SelectList(db.Subjects, "Name", "Name");
            ViewBag.Subjects = subjects;
            return View();
        }

        // добавление новой оценки в бд
        [HttpPost]
        public async Task<IActionResult> Add(SubjectGrade subjectGrade)
        {
            switch (subjectGrade.Date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return new ContentResult()
                    {
                        Content = Messages.CantAssessOnWeekends
                    };
            }
            switch (subjectGrade.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    db.SubjectGrade.Add(subjectGrade);
                    await db.SaveChangesAsync();
                    break;
            }
            logger.Info($"Add grade (StudentID = {subjectGrade.StudentId}, Subject = {subjectGrade.SubjectName}) to the DiaryDB");
            return RedirectToAction("Grade", new { id = subjectGrade.StudentId });
        }

        // вызов формы для ввода оценки
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string className, int? studentId, string subjectName)
        {
            if (id != null && className!=null && studentId != null && subjectName != null)
            {
                SubjectGrade subjectGrade =
                    await db.SubjectGrade
                    .FirstOrDefaultAsync(sb => sb.Id == id && sb.ClassName==className && sb.StudentId == studentId && sb.SubjectName == subjectName);
                if (subjectGrade != null)
                    return View(subjectGrade);
            }
            return NotFound();
        }

        // сохранение оценки в бд
        [HttpPost]
        public async Task<IActionResult> Edit(SubjectGrade subjectGrade)
        {
            switch (subjectGrade.Date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return new ContentResult()
                    {
                        Content = Messages.CantAssessOnWeekends
                    };
            }
            switch (subjectGrade.Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    db.SubjectGrade.Update(subjectGrade);
                    await db.SaveChangesAsync();
                    break;
            }
            logger.Info($"Edit grade (ID = {subjectGrade.Id}, StudentID = {subjectGrade.StudentId}, Subject = {subjectGrade.SubjectName}) in the DiaryDB");
            return RedirectToAction("Grade", new { id = subjectGrade.StudentId });
        }

        // показ удаляемой оценки ученика 
        [HttpGet]
        public async Task<IActionResult> Delete(int? studentId, string className, int? id, string subjectName)
        {
            if (studentId != null && className != null && id != null && subjectName != null)
            {
                SubjectGrade subjectGrade =
                    await db.SubjectGrade
                    .FirstOrDefaultAsync(s => s.StudentId == studentId && s.ClassName==className && s.Id == id && s.SubjectName == subjectName);
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
                logger.Trace($"Grade (ID = {subjectGrade.Id}, StudentID = {subjectGrade.StudentId}, Subject = {subjectGrade.SubjectName}) was defined for remove from DiaryDB");
                db.SubjectGrade.Remove(subjectGrade);
                await db.SaveChangesAsync();
                logger.Info($"Grade (ID = {subjectGrade.Id}, StudentID = {subjectGrade.StudentId}, Subject = {subjectGrade.SubjectName}) was deleted from DiaryDB");
                return RedirectToAction("Grade", new { id = subjectGrade.StudentId });
            }
            logger.Error("Object SubjectGrade subjectGrade wasn't defined for remove from DiaryDB");
            return NotFound();
        }

        // вывод оценок студентов определенного класса с фильтрацией по предметам
        public IActionResult StudentGradeOnSubject(string className, int? studentId, string subjectName)
        {
            IQueryable<SubjectGrade> subjectGrades = db.SubjectGrade.Include(s => s.Students);
            if (className != null)
            {
                List<Subjects> subjects = db.Subjects.ToList();
                List<Classes> classes = db.Classes.ToList();
                subjectGrades = subjectGrades.Where(s => s.ClassName == className);

                StudentGradeOnSubjectViewModel studentGradeOnSubject =
                    new StudentGradeOnSubjectViewModel
                    {
                        Subjects = new SelectList(subjects, "Name", "Name"),
                        Classes = new SelectList(classes, "Name", "Name"),
                        SubjectGrades = subjectGrades.ToList()
                    };
                if (subjectName != null)
                    studentGradeOnSubject.SubjectGrades = subjectGrades.Where(s => s.ClassName == className && s.SubjectName == subjectName);
                return View(studentGradeOnSubject);
            }
            return NotFound();
        }
    }
}
