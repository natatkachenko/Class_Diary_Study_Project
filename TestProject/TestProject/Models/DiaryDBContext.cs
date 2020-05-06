using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestProject.Models
{
    public partial class DiaryDBContext : DbContext
    {
        public DiaryDBContext(DbContextOptions<DiaryDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<ClassSubject> ClassSubject { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<SubjectGrade> SubjectGrade { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassSubject>(entity =>
            {
                entity.HasKey(e => new { e.SubjectName, e.ClassName })
                    .HasName("PK__ClassSubject");

                entity.Property(e => e.SubjectName).HasMaxLength(50);

                entity.Property(e => e.ClassName).HasMaxLength(50);
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Class");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => new { e.ClassName, e.Id })
                    .HasName("PK_Students");

                entity.Property(e => e.Id)
                    .HasColumnName("StudentID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ClassName).HasMaxLength(50);
            });

            modelBuilder.Entity<SubjectGrade>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ClassName, e.StudentId, e.SubjectName })
                    .HasName("PK_SubjectGrade");

                entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ClassName).HasMaxLength(50);

                modelBuilder.Entity<SubjectGrade>()
                .HasOne(s => s.Students)
                .WithMany(st => st.subjectGrades)
                .HasForeignKey(s => new { s.ClassName, s.StudentId })
                .HasConstraintName("FK_SubjectGradeStudents");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Subjects__4C5A7D54DC73CA2F");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_Users");
            });

            //роли пользователей
            string adminRoleName = "admin";
            string studentRoleName = "student";
            string teacherRoleName = "teacher";
            string directorRoleName = "director";
            string parentRoleName = "parent";

            string adminEmail = "TestProjectAdmin@protonmail.com";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Name = adminRoleName };
            Role studentRole = new Role { Name = studentRoleName };
            Role teacherRole = new Role { Name = teacherRoleName };
            Role directorRole = new Role { Name = directorRoleName };
            Role parentRole = new Role { Name = parentRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, roleName = adminRole.Name };

            modelBuilder.Entity<Role>()
                .HasData(new Role[] { adminRole, studentRole, teacherRole, directorRole, parentRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
