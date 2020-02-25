using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestProject.Models;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("api/classes")]
    public class ClassesController : Controller
    {
        Context db;
        public ClassesController(Context context)
        {
            db = context;
        }

        [HttpGet]
        public IEnumerable<Classes> Get()
        {
            return db.Classes.ToList();
        }

        [HttpGet("{name}")]
        public Classes Get(string name)
        {
            Classes classes = db.Classes.FirstOrDefault(x => x.Name == name);
            return classes;
        }

        [HttpPost]
        public IActionResult Post(Classes classes)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Add(classes);
                db.SaveChanges();
                return Ok(classes);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            Classes classes = db.Classes.FirstOrDefault(x => x.Name == name);
            if (classes != null)
            {
                db.Classes.Remove(classes);
                db.SaveChanges();
            }
            return Ok(classes);
        }
    }
}