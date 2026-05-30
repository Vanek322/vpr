using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace vpr.Models;

public partial class VprDbContext : DbContext
{
    public VprDbContext()
    {
    }

    public VprDbContext(DbContextOptions<VprDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassLevel> ClassLevels { get; set; }

    public virtual DbSet<Protocol> Protocols { get; set; }

    public virtual DbSet<ScoreRatio> ScoreRatios { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherAssignment> TeacherAssignments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=vpr_db;Username=postgres; Password=1111");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("classes_pkey1");

            entity.ToTable("classes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('classes_id_seq1'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.IdClassLevel).HasColumnName("id_class_level");
            entity.Property(e => e.SymbolOfClass).HasColumnName("symbol_of_class");

            entity.HasOne(d => d.ClassLevel).WithMany(p => p.Classes)
                .HasForeignKey(d => d.IdClassLevel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("classes_id_class_level_fkey");
        });

        modelBuilder.Entity<ClassLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("classes_pkey");

            entity.ToTable("class_levels");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('classes_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<Protocol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("protocols_pkey");

            entity.ToTable("protocols");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdStudent).HasColumnName("id_student");
            entity.Property(e => e.IdTeacherAssignment).HasColumnName("id_teacher_assignment");
            entity.Property(e => e.PreviousGrade).HasColumnName("previous_grade");
            entity.Property(e => e.TotalScore).HasColumnName("total_score");
            entity.Property(e => e.Variant).HasColumnName("variant");

            entity.HasOne(d => d.Student).WithMany(p => p.Protocols)
                .HasForeignKey(d => d.IdStudent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("protocols_id_student_fkey");

            entity.HasOne(d => d.TeacherAssignment).WithMany(p => p.Protocols)
                .HasForeignKey(d => d.IdTeacherAssignment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("protocols_id_teacher_assignment_fkey");
        });

        modelBuilder.Entity<ScoreRatio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("score_ratios_pkey");

            entity.ToTable("score_ratios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.IdClassLevel).HasColumnName("id_class_level");
            entity.Property(e => e.IdSubject).HasColumnName("id_subject");
            entity.Property(e => e.MaxScore).HasColumnName("max_score");
            entity.Property(e => e.MinScore).HasColumnName("min_score");

            entity.HasOne(d => d.IdClassLevelNavigation).WithMany(p => p.ScoreRatios)
                .HasForeignKey(d => d.IdClassLevel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("score_ratios_id_class_level_fkey");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.ScoreRatios)
                .HasForeignKey(d => d.IdSubject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("score_ratios_id_subject_fkey");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("students_pkey");

            entity.ToTable("students");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.IdClass).HasColumnName("id_class");
            entity.Property(e => e.ParticipantCode).HasColumnName("participant_code");

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.IdClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("students_id_class_fkey");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subjects_pkey");

            entity.ToTable("subjects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("teachers_pkey");

            entity.ToTable("teachers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
        });

        modelBuilder.Entity<TeacherAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("teacher_assignments_pkey");

            entity.ToTable("teacher_assignments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdClass).HasColumnName("id_class");
            entity.Property(e => e.IdSubject).HasColumnName("id_subject");
            entity.Property(e => e.IdTeacher).HasColumnName("id_teacher");

            entity.HasOne(d => d.Class).WithMany(p => p.TeacherAssignments)
                .HasForeignKey(d => d.IdClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_teacher_assignments_to_classes");

            entity.HasOne(d => d.Subject).WithMany(p => p.TeacherAssignments)
                .HasForeignKey(d => d.IdSubject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_teacher_assignments_to_subjects");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherAssignments)
                .HasForeignKey(d => d.IdTeacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_teacher_assignments_to_teachers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
