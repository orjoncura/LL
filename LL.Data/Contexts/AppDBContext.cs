using LL.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace LL.Data.Contexts
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public AppDBContext() { }

        public DbSet<Person> People { get; set; }
        public DbSet<PersonLog> PeopleLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserTokenLog> UserTokenLogs { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Statement> Statements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(p => p.FirstName).IsRequired();
                entity.Property(p => p.LastName).IsRequired();
                entity.Property(p => p.IsActive).IsRequired();
            });

            modelBuilder.Entity<PersonLog>(entity =>
            {
                entity.Property(p => p.FirstName).IsRequired();
                entity.Property(p => p.LastName).IsRequired();
                entity.Property(p => p.IsActive).IsRequired();
                entity.Property(p => p.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                entity.Property(u => u.PersonId).IsRequired();

                entity.HasOne(p => p.Person)
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(p => p.Username).IsRequired();
                entity.Property(p => p.PasswordHash).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();
                entity.Property(u => u.PersonId).IsRequired();

                entity.HasOne(p => p.Person)
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.Property(p => p.Username).IsRequired();
                entity.Property(p => p.PasswordHash).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();
                entity.Property(u => u.PersonId).IsRequired();

                entity.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.Property(ut => ut.Token).IsRequired();
                entity.Property(ut => ut.Expiration).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();

                entity.HasOne(ut => ut.User)
                .WithMany()
                .HasForeignKey(ut => ut.UserId);
            });

            modelBuilder.Entity<UserTokenLog>(entity =>
            {
                entity.Property(ut => ut.Token).IsRequired();
                entity.Property(ut => ut.Expiration).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();

                entity.HasOne(ut => ut.UserToken)
                .WithMany()
                .HasForeignKey(ut => ut.UserTokenId);

                entity.HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LoginHistory>(entity => 
            {
                entity.HasOne(ut => ut.User)
                .WithMany()
                .HasForeignKey(ut => ut.UserId);

                entity.Property(p => p.CreatedDate).IsRequired();

                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(ut => ut.Name).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.Property(ut => ut.Name).IsRequired();
                entity.Property(ut => ut.LanguageFromId).IsRequired();
                entity.Property(ut => ut.LanguageToId).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.LanguageFrom)
                .WithMany()
                .HasForeignKey(ut => ut.LanguageFromId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ut => ut.LanguageTo)
                .WithMany()
                .HasForeignKey(ut => ut.LanguageToId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Statement>(entity =>
            {
                entity.Property(ut => ut.OriginalStatement).IsRequired();
                entity.Property(ut => ut.TranslatedStatement).IsRequired();
                entity.Property(ut => ut.LanguageFromId).IsRequired();
                entity.Property(ut => ut.LanguageToId).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.Word)
                .WithMany()
                .HasForeignKey(ut => ut.WordId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ut => ut.LanguageFrom)
                .WithMany()
                .HasForeignKey(ut => ut.LanguageFromId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ut => ut.LanguageTo)
                .WithMany()
                .HasForeignKey(ut => ut.LanguageToId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
