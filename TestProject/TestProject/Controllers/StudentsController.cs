﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestProject.Controllers
{
    public class StudentsController : Controller
    {
        DiaryDBContext db;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(DiaryDBContext context, ILogger<StudentsController> logger)
        {
            db = context;
            _logger = logger;
        }

        // вывод списка учеников выбранного класса
        [HttpGet]
        public IActionResult Index(string name)
        {
            if (name != null)
            {
                var students = 
                    db.Students.FromSqlInterpolated($"Select * From Students Where ClassName={name}").ToList();
                if(students!=null)
                    return View(students);
            }
            return NotFound();
        }

        // вызов формы для добавления ученика класса
        public IActionResult Add()
        {
            SelectList classes = new SelectList(db.Classes, "Name", "Name");
            ViewBag.Classes = classes;
            return View();
        }

        // добавление нового ученика класса в бд
        [HttpPost]
        public async Task<IActionResult> Add(Students student)
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();
            _logger.LogInformation($"Add student (ID = {student.Id}) to the DiaryDB");
            return RedirectToAction("Index", new { name = student.ClassName });
        }

        // показ удаляемого ученика класса
        [HttpGet]
        public async Task<IActionResult> Delete(string classname, int? id)
        {
            if (classname != null && id != null)
            {
                Students student =
                    await db.Students.FirstOrDefaultAsync(s => s.ClassName == classname && s.Id == id);
                if (student != null)
                    return View(student);
            }
            return NotFound();
        }

        // удаление ученика класса из бд
        [HttpPost]
        public async Task<IActionResult> Delete(Students student)
        {
            if (student != null)
            {
                _logger.LogTrace($"Student (ID = {student.Id}) was defined for remove from DiaryDB");
                db.Students.Remove(student);
                await db.SaveChangesAsync();
                _logger.LogInformation($"Student (ID = {student.Id}) was deleted from DiaryDB");
                return RedirectToAction("Index", new { name = student.ClassName });
            }
            _logger.LogError("Object Students student wasn't defined for remove from DiaryDB");
            return NotFound();
        }
    }
}
