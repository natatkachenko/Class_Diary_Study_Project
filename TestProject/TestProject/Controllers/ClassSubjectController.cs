﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            if (className != null)
            {
                List<Subjects> subjects = db.Subjects.ToList();
                List<Classes> classes = db.Classes.ToList();
                IQueryable<StudentGradeModel> students =(IQueryable<StudentGradeModel>) db.Students.FromSqlInterpolated
                    ($"Select Students.ClassName, Students.StudentID, Students.FullName, SubjectGrade.SubjectName, SubjectGrade.Date, SubjectGrade.Grade From Students Left Join SubjectGrade On Students.StudentID=SubjectGrade.StudentID Order By Students.StudentID")
                    .ToList();

                StudentGradeOnSubjectViewModel studentGradeOnSubject =
                    new StudentGradeOnSubjectViewModel
                    {
                        Subjects = new SelectList(subjects, "Name", "Name"),
                        Classes = new SelectList(classes, "Name", "Name"),
                        Students = students
                    };
                if (subjectName != null)
                    studentGradeOnSubject.Students = students.Where(s => s.ClassName == className && s.SubjectName == subjectName);
                return View(studentGradeOnSubject);
            }
            return NotFound();
        }
    }
}
