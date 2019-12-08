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
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Applicant> Applicant { get; set; }
        public virtual DbSet<CatalogueInConfigRank> CatalogueInConfigRank { get; set; }
        public virtual DbSet<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyCatalogue> CompanyCatalogue { get; set; }
        public virtual DbSet<CompanyRank> CompanyRank { get; set; }
        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<DefaultAnswer> DefaultAnswer { get; set; }
        public virtual DbSet<DefaultCatalogue> DefaultCatalogue { get; set; }
        public virtual DbSet<DefaultQuestion> DefaultQuestion { get; set; }
        public virtual DbSet<DefaultRank> DefaultRank { get; set; }
        public virtual DbSet<DetailResult> DetailResult { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionInTest> QuestionInTest { get; set; }
        public virtual DbSet<RankInConfiguration> RankInConfiguration { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Test> Test { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=deverate.cjd2ogwhqpge.ap-southeast-1.rds.amazonaws.com;Database=Deverate;User ID=deverate;Password=pass4deverate;Trusted_Connection=False;");
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

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Answer1)
                    .IsRequired()
                    .HasColumnName("Answer")
                    .HasMaxLength(250);

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

            modelBuilder.Entity<CatalogueInConfigRank>(entity =>
            {
                entity.HasKey(e => new { e.ConfigId, e.CompanyRankId, e.CompanyCatalogueId })
                    .HasName("PK_CatalogueInRank_1");

                entity.HasOne(d => d.CompanyCatalogue)
                    .WithMany(p => p.CatalogueInConfigRank)
                    .HasForeignKey(d => d.CompanyCatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInRank_CompanyCatalogue");

                entity.HasOne(d => d.Co)
                    .WithMany(p => p.CatalogueInConfigRank)
                    .HasForeignKey(d => new { d.ConfigId, d.CompanyRankId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfigRank_RankInConfiguration");
            });

            modelBuilder.Entity<CatalogueInConfiguration>(entity =>
            {
                entity.HasKey(e => new { e.ConfigId, e.CompanyCatalogueId })
                    .HasName("PK_CatalogueInConfiguration_1");

                entity.HasOne(d => d.CompanyCatalogue)
                    .WithMany(p => p.CatalogueInConfiguration)
                    .HasForeignKey(d => d.CompanyCatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CatalogueInConfiguration_CompanyCatalogue");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.CatalogueInConfiguration)
                    .HasForeignKey(d => d.ConfigId)
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

            modelBuilder.Entity<CompanyCatalogue>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyCatalogue)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyCatalogue_Company");
            });

            modelBuilder.Entity<CompanyRank>(entity =>
            {
                entity.Property(e => e.CompanyRankId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyRank)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyRank_Company");
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasKey(e => e.ConfigId);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Configuration)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_Account");
            });

            modelBuilder.Entity<DefaultAnswer>(entity =>
            {
                entity.Property(e => e.DefaultAnswerId).ValueGeneratedNever();

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.DefaultQuestion)
                    .WithMany(p => p.DefaultAnswer)
                    .HasForeignKey(d => d.DefaultQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefaultAnswer_DefaultQuestion");
            });

            modelBuilder.Entity<DefaultCatalogue>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<DefaultQuestion>(entity =>
            {
                entity.Property(e => e.DefaultQuestionId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.DefaultCatalogue)
                    .WithMany(p => p.DefaultQuestion)
                    .HasForeignKey(d => d.DefaultCatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefaultQuestion_DefaultCatalogue");
            });

            modelBuilder.Entity<DefaultRank>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<DetailResult>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.CompanyCatalogueId });

                entity.HasOne(d => d.CompanyCatalogue)
                    .WithMany(p => p.DetailResult)
                    .HasForeignKey(d => d.CompanyCatalogueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailResult_CompanyCatalogue");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.DetailResult)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailResult_Test");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.CreateAt)
                    .HasColumnName("Create_at")
                    .HasColumnType("date");

                entity.Property(e => e.Question1)
                    .IsRequired()
                    .HasColumnName("Question")
                    .HasMaxLength(250);

                entity.HasOne(d => d.CompanyCatalogue)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CompanyCatalogueId)
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

            modelBuilder.Entity<RankInConfiguration>(entity =>
            {
                entity.HasKey(e => new { e.ConfigId, e.CompanyRankId });

                entity.HasOne(d => d.CompanyRank)
                    .WithMany(p => p.RankInConfiguration)
                    .HasForeignKey(d => d.CompanyRankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfigurationRank_CompanyRank");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.RankInConfiguration)
                    .HasForeignKey(d => d.ConfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConfigurationRank_Configuration");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FinishTime).HasColumnType("datetime");

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

                entity.HasOne(d => d.CompanyRank)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.CompanyRankId)
                    .HasConstraintName("FK_Test_CompanyRank");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.ConfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Test_Configuration");
            });
        }
    }
}
