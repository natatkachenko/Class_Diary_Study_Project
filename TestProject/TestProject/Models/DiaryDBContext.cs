﻿using System;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassSubject>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.ClassName })
                    .HasName("PK__ClassSubject");

                entity.Property(e => e.Name).HasMaxLength(50);

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

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SubjectGrade>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.StudentId, e.SubjectName })
                    .HasName("PK_SubjectGrade");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__Subjects__4C5A7D54DC73CA2F");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
