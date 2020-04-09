using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace TestProject.Services
{
    public class StudentService
    {
        private DiaryDBContext db;
        private IMemoryCache cache;

        public StudentService(DiaryDBContext _db, IMemoryCache _cache)
        {
            db = _db;
            cache = _cache;
        }

        public async Task<Students> GetStudent(int id)
        {
            Students student = null;
            if (!cache.TryGetValue(id, out student))
            {
                student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
                if (student != null)
                {
                    cache.Set(student.Id, student,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return student;
        }
    }
}
