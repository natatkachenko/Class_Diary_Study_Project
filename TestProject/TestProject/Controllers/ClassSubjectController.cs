using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using TestProject.Services;
using TestProject.ViewModels;

namespace TestProject.Controllers
{
    public class ClassSubjectController : Controller
    {
        DiaryDBContext db;
        SubjectService subjectService;

        public ClassSubjectController(DiaryDBContext context, SubjectService service)
        {
            db = context;
            subjectService = service;
        }

        // вывод списка классов, у которых ведётся выбранный предмет
        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            Subjects subject = await subjectService.GetSubject(name);
            if (subject.Name != null)
            {
                var classes =
                    db.ClassSubject.FromSqlInterpolated($"Select * From ClassSubject Where SubjectName={subject.Name}").ToList();
                if (classes != null)
                    return View(classes);
            }
            return NotFound();
        }

        // вывод оценок студентов опредеоенного класса с фильтрацией по предметам
        public IActionResult StudentGradeOnSubject(string className, string subjectName)
        {
            if (className != null && subjectName != null)
            {
                List<Subjects> subjects = 
                    db.Subjects.Select(s => new Subjects { Name = s.Name }).ToList();
                List<Students> students =
                    db.Students.Select(s => new Students { ClassName = className, Id = s.Id, FullName = s.FullName }).ToList();
                List<SubjectGradeModel> subjectGrades =
                    db.SubjectGrade.Select(s => new SubjectGradeModel { Date = s.Date, Grade = s.Grade, SubjectName = subjectName }).ToList();
                StudentGradeOnSubjectModel studentGradeOnSubject =
                    new StudentGradeOnSubjectModel
                    {
                        Subjects = subjects,
                        Students = students,
                        SubjectGrades = subjectGrades
                    };
                return View(studentGradeOnSubject);
            }
            return NotFound();
        }
    }
}
