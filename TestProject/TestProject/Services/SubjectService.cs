using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace TestProject.Services
{
    public class SubjectService
    {
        private DiaryDBContext db;
        private IMemoryCache cache;

        public SubjectService(DiaryDBContext _db, IMemoryCache _cache)
        {
            db = _db;
            cache = _cache;
        }

        public async Task<Subjects> GetSubject(string name)
        {
            Subjects subject = null;
            if (!cache.TryGetValue(name, out subject))
            {
                subject = await db.Subjects.FirstOrDefaultAsync(s => s.Name == name);
                if (subject != null)
                {
                    cache.Set(subject.Name, subject,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return subject;
        }
    }
}
