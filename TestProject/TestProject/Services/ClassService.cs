using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace TestProject.Services
{
    public class ClassService
    {
        private DiaryDBContext db;
        private IMemoryCache cache;

        public ClassService(DiaryDBContext _db, IMemoryCache _cache)
        {
            db = _db;
            cache = _cache;
        }

        public async Task<Classes> GetClass(string name)
        {
            Classes stclass = null;
            if(!cache.TryGetValue(name, out stclass))
            {
                stclass = await db.Classes.FirstOrDefaultAsync(c => c.Name == name);
                if (stclass != null)
                {
                    cache.Set(stclass.Name, stclass,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return stclass;
        }
    }
}
