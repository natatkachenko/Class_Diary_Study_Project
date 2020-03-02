using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public IActionResult Index()
        {
            return View(db.Classes.ToList());
        }
    }
}
