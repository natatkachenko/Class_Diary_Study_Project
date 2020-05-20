using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Services
{
    public class HomeService:IHomeService
    {
        private readonly DiaryDBContext _db;

        public HomeService(DiaryDBContext db)
        {
            _db = db;
        }

        public IEnumerable<Classes> AllClasses()
        {
            return _db.Classes.ToList();
        }
    }

    public interface IHomeService
    {
        IEnumerable<Classes> AllClasses();
    }
}
