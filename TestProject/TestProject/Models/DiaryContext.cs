using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Models
{
    public class DiaryContext: DbContext
    {
        public DbSet<Classes> Classes { get; set; }

        public DiaryContext(DbContextOptions<DiaryContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
