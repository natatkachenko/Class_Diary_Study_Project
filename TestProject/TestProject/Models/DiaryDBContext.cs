﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestProject.Models
{
    public partial class DiaryDBContext : DbContext
    {
        public DiaryDBContext()
        {
        }

        public DiaryDBContext(DbContextOptions<DiaryDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClassSubject> ClassSubject { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<SubjectGrade> SubjectGrade { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=BLEDHARDPC;Initial Catalog=DiaryDB;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassSubject>(entity =>
            {
                entity.HasKey(e => new { e.SubjectName, e.ClassName })
                    .HasName("PK__ClassSub__F3D18835013DEB34");

                entity.Property(e => e.SubjectName).HasMaxLength(50);

                entity.Property(e => e.ClassName).HasMaxLength(50);
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.ClassName)
                    .HasName("PK__Classes");

                entity.Property(e => e.ClassName).HasMaxLength(50);
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => new { e.ClassName, e.StudentId })
                    .HasName("PK__Students");

                entity.Property(e => e.ClassName).HasMaxLength(50);

                entity.Property(e => e.StudentId)
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
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasKey(e => e.SubjectName)
                    .HasName("PK__Subjects__4C5A7D54DC73CA2F");

                entity.Property(e => e.SubjectName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}