using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ResourceServices.Models
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
        public virtual DbSet<Catalogue> Catalogue { get; set; }
        public virtual DbSet<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyCatalogue> CompanyCatalogue { get; set; }
        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<ConfigurationRank> ConfigurationRank { get; set; }
        public virtual DbSet<DetailStatistic> DetailStatistic { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionInTest> QuestionInTest { get; set; }
        public virtual DbSet<Rank> Rank { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Statistic> Statistic { get; set; }
        public virtual DbSet<Test> Test { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=deverate.cxr5rxvkq6ui.us-east-2.rds.amazonaws.com;Database=Deverate;User ID=sa;Password=pass4deverate;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Fullname).HasMaxLength(250);

                entity.Property(e => e.JoinDate).HasColumnType("date");

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(250);

                entity.Property(e => e.Username).HasMaxLength(250);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Answer1)
                    .HasColumnName("Answer")
                    .HasMaxLength(250);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Catalogue>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<CatalogueInConfiguration>(entity =>
            {
                entity.HasKey(e => e.Cicid);

                entity.Property(e => e.Cicid).HasColumnName("CICId");

                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.CatalogueInConfiguration)
                    .HasForeignKey(d => d.CatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfiguration_Catalogue1");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.CatalogueInConfiguration)
                    .HasForeignKey(d => d.ConfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfiguration_Configuration1");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreateAt)
                    .HasColumnName("Create_At")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fax)
                    .HasColumnName("FAX")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<CompanyCatalogue>(entity =>
            {
                entity.HasKey(e => new { e.CompanyId, e.CatalogueId });

                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.CompanyCatalogue)
                    .HasForeignKey(d => d.CatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyCatalogue_Catalogue");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyCatalogue)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyCatalogue_Company");
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasKey(e => e.ConfigId);

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.TestOwner)
                    .WithMany(p => p.Configuration)
                    .HasForeignKey(d => d.TestOwnerId)
                    .HasConstraintName("FK_Configuration_Account");
            });

            modelBuilder.Entity<ConfigurationRank>(entity =>
            {
                entity.HasOne(d => d.Config)
                    .WithMany(p => p.ConfigurationRank)
                    .HasForeignKey(d => d.ConfigId)
                    .HasConstraintName("FK_ConfigurationRank_Configuration");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.ConfigurationRank)
                    .HasForeignKey(d => d.RankId)
                    .HasConstraintName("FK_ConfigurationRank_Rank");
            });

            modelBuilder.Entity<DetailStatistic>(entity =>
            {
                entity.HasKey(e => e.DetailId);

                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.DetailStatistic)
                    .HasForeignKey(d => d.CatalogueId)
                    .HasConstraintName("FK_DetailStatistic_Catalogue");

                entity.HasOne(d => d.Statistic)
                    .WithMany(p => p.DetailStatistic)
                    .HasForeignKey(d => d.StatisticId)
                    .HasConstraintName("FK_DetailStatistic_Statistic");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.CreateAt)
                    .HasColumnName("Create_at")
                    .HasColumnType("date");

                entity.Property(e => e.CreateBy).HasColumnName("Create_by");

                entity.Property(e => e.Question1)
                    .HasColumnName("Question")
                    .HasMaxLength(350);

                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CatalogueId)
                    .HasConstraintName("FK_Question_Catalogue");
            });

            modelBuilder.Entity<QuestionInTest>(entity =>
            {
                entity.HasKey(e => e.Qitid);

                entity.Property(e => e.Qitid).HasColumnName("QITId");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.QuestionInTest)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK_QuestionInTest_Answer");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionInTest)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_QuestionInTest_Question");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.QuestionInTest)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK_QuestionInTest_Test");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.Property(e => e.CreateAt)
                    .HasColumnName("Create_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.UpdateAt)
                    .HasColumnName("Update_at")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(350);
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Statistic)
                    .HasForeignKey(d => d.RankId)
                    .HasConstraintName("FK_Statistic_Rank");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Statistic)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK_DetailedStatistic_Test");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Test_Account");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.ConfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_Configuration");
            });
        }
    }
}
