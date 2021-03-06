﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestManagementServices.Models
{
    public partial class DeverateContext : DbContext
    {
        public DeverateContext()
        {
        }

        public DeverateContext(DbContextOptions<DeverateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Applicant> Applicant { get; set; }
        public virtual DbSet<Catalogue> Catalogue { get; set; }
        public virtual DbSet<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual DbSet<CatalogueInSemester> CatalogueInSemester { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<DetailResult> DetailResult { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionInTest> QuestionInTest { get; set; }
        public virtual DbSet<Rank> Rank { get; set; }
        public virtual DbSet<RankInSemester> RankInSemester { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Semester> Semester { get; set; }
        public virtual DbSet<Test> Test { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=deverate.ckp53lvgge9h.ap-southeast-1.rds.amazonaws.com;Database=Deverate;User ID=deverate;Password=pass4deverate;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.JoinDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(250);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Account_Company");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RankId)
                    .HasConstraintName("FK_Account_CompanyRank");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerText)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Applicant>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Catalogue>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Catalogue)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_CompanyCatalogue_Company");
            });

            modelBuilder.Entity<CatalogueInRank>(entity =>
            {
                entity.HasKey(e => new { e.CatalogueId, e.RankId });

                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.CatalogueInRank)
                    .HasForeignKey(d => d.CatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInRank_Catalogue");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.CatalogueInRank)
                    .HasForeignKey(d => d.RankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInRank_Rank");
            });

            modelBuilder.Entity<CatalogueInSemester>(entity =>
            {
                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.CatalogueInSemester)
                    .HasForeignKey(d => d.CatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfiguration_CompanyCatalogue");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.CatalogueInSemester)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfiguration_Configuration");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(250);
            });

            modelBuilder.Entity<DetailResult>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.CatalogueInSemesterId });

                entity.HasOne(d => d.CatalogueInSemester)
                    .WithMany(p => p.DetailResult)
                    .HasForeignKey(d => d.CatalogueInSemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailResult_CatalogueInConfiguration1");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.DetailResult)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailResult_Test");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_CompanyCatalogue");
            });

            modelBuilder.Entity<QuestionInTest>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.QuestionId })
                    .HasName("PK_QuestionInTest_1");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.QuestionInTest)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK_QuestionInTest_Answer");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionInTest)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionInTest_Question");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.QuestionInTest)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionInTest_Test");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Rank)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_CompanyRank_Company");
            });

            modelBuilder.Entity<RankInSemester>(entity =>
            {
                entity.HasKey(e => new { e.RankId, e.SemesterId })
                    .HasName("PK_RankInConfig_1");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.RankInSemester)
                    .HasForeignKey(d => d.RankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RankInConfig_Rank");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.RankInSemester)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RankInConfig_Configuration");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Semester)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_Company");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FinishTime).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Test_Account");

                entity.HasOne(d => d.Applicant)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.ApplicantId)
                    .HasConstraintName("FK_Test_Applicant");

                entity.HasOne(d => d.PotentialRank)
                    .WithMany(p => p.TestPotentialRank)
                    .HasForeignKey(d => d.PotentialRankId)
                    .HasConstraintName("FK_Test_CompanyRank1");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.TestRank)
                    .HasForeignKey(d => d.RankId)
                    .HasConstraintName("FK_Test_CompanyRank");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_Configuration");
            });
        }
    }
}
