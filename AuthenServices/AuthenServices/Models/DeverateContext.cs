using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuthenServices.Models
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
        public virtual DbSet<AccountInTest> AccountInTest { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Catalogue> Catalogue { get; set; }
        public virtual DbSet<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyCatalogue> CompanyCatalogue { get; set; }
        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<ConfigurationRank> ConfigurationRank { get; set; }
        public virtual DbSet<DetailedStatistic> DetailedStatistic { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionInTest> QuestionInTest { get; set; }
        public virtual DbSet<Rank> Rank { get; set; }
        public virtual DbSet<Role> Role { get; set; }
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

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Account_Company");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<AccountInTest>(entity =>
            {
                entity.HasKey(e => e.Aitid);

                entity.Property(e => e.Aitid)
                    .HasColumnName("AITId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(250);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountInTest)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_AccountInTest_Account");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.AccountInTest)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK_AccountInTest_Test");
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
                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<CatalogueInConfiguration>(entity =>
            {
                entity.HasKey(e => e.Cicid);

                entity.Property(e => e.Cicid)
                    .HasColumnName("CICId")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Catalogue)
                    .WithMany(p => p.CatalogueInConfiguration)
                    .HasForeignKey(d => d.CatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfiguration_Catalogue");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.CatalogueInConfiguration)
                    .HasForeignKey(d => d.ConfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfiguration_Configuration");
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

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Configuration)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_Test");
            });

            modelBuilder.Entity<ConfigurationRank>(entity =>
            {
                entity.Property(e => e.ConfigurationRankId).ValueGeneratedNever();

                entity.HasOne(d => d.ConfigurationRankNavigation)
                    .WithOne(p => p.ConfigurationRank)
                    .HasForeignKey<ConfigurationRank>(d => d.ConfigurationRankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfigurationRank_Configuration");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.ConfigurationRank)
                    .HasForeignKey(d => d.RankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfigurationRank_Rank");
            });

            modelBuilder.Entity<DetailedStatistic>(entity =>
            {
                entity.HasKey(e => e.StatisticId);

                entity.Property(e => e.StatisticId).ValueGeneratedNever();

                entity.Property(e => e.Aitid).HasColumnName("AITId");

                entity.Property(e => e.RankId).HasMaxLength(250);

                entity.HasOne(d => d.Ait)
                    .WithMany(p => p.DetailedStatistic)
                    .HasForeignKey(d => d.Aitid)
                    .HasConstraintName("FK_DetailedStatistic_AccountInTest");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.CreatAt)
                    .HasColumnName("creat_at")
                    .HasColumnType("date");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

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

                entity.Property(e => e.Qitid)
                    .HasColumnName("QITId")
                    .ValueGeneratedNever();

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
                    .HasColumnName("create_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateAt)
                    .HasColumnName("update_at")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(350);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.TestId).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });
        }
    }
}
