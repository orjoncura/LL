using LL.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace LL.Data.Contexts
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public AppDBContext() { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserTokenLog> UserTokenLogs { get; set; }
        public DbSet<NewUserRequest> NewUserRequests { get; set; }
        public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageLog> MessageLogs { get; set; }
        public DbSet<MessageStatus> MessageStatuses { get; set; }
        public DbSet<MessageContent> MessageContents { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordType> WordTypes { get; set; }
        public DbSet<WordMeaning> WordMeanings { get; set; }
        public DbSet<WordDefinition> WordDefinitions { get; set; }
        public DbSet<WordLink> WordLinks { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<SeminarWord> SeminarWords { get; set; }
        public DbSet<SeminarWordRank> SeminarWordRank { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(p => p.Email).IsRequired();
                entity.Property(p => p.PasswordHash).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.Property(p => p.Email).IsRequired();
                entity.Property(p => p.PasswordHash).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();

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
            
            modelBuilder.Entity<NewUserRequest>(entity =>
            {
                entity.Property(ut => ut.Email).IsRequired();
                entity.Property(ut => ut.IP).IsRequired();
                entity.Property(ut => ut.Token).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(ut => ut.CreatedDate).IsRequired();
            });

            modelBuilder.Entity<ResetPasswordRequest>(entity =>
            {
                entity.Property(ut => ut.UserId).IsRequired();
                entity.Property(ut => ut.IP).IsRequired();
                entity.Property(ut => ut.Token).IsRequired();
                entity.Property(ut => ut.CreatedDate).IsRequired();
                
                entity.HasOne(ut => ut.User)
                    .WithMany()
                    .HasForeignKey(ut => ut.UserId)
                    .OnDelete(DeleteBehavior.Restrict);;
            });
            
            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(ut => ut.RecipientAddress).IsRequired();
                entity.Property(ut => ut.Subject).IsRequired();
                entity.Property(ut => ut.StatusId).IsRequired();
                entity.Property(ut => ut.MessageContentId).IsRequired();

                entity.HasOne(p => p.Recipient)
                    .WithMany()
                    .HasForeignKey(p => p.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.Status)
                    .WithMany()
                    .HasForeignKey(p => p.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.MessageContent)
                    .WithMany()
                    .HasForeignKey(p => p.MessageContentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<MessageLog>(entity =>
            {
                entity.Property(ut => ut.MessageId).IsRequired();
                entity.Property(ut => ut.RecipientAddress).IsRequired();
                entity.Property(ut => ut.Subject).IsRequired();
                entity.Property(ut => ut.StatusId).IsRequired();
                entity.Property(ut => ut.MessageContentId).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(ut => ut.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.Message)
                    .WithMany()
                    .HasForeignKey(ut => ut.MessageId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.Recipient)
                    .WithMany()
                    .HasForeignKey(p => p.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.Status)
                    .WithMany()
                    .HasForeignKey(p => p.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.MessageContent)
                    .WithMany()
                    .HasForeignKey(p => p.MessageContentId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.CreatedBy)
                    .WithMany()
                    .HasForeignKey(p => p.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<MessageStatus>(entity =>
            {
                entity.Property(ut => ut.Value).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<MessageContent>(entity =>
            {
                entity.Property(ut => ut.Value).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
            });
            
            modelBuilder.Entity<LoginHistory>(entity => 
            {
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(ut => ut.Value).IsRequired();
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
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                
                entity.Property(ut => ut.CreatedById).IsRequired();

                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<WordMeaning>(entity =>
            {
                entity.Property(ut => ut.WordId).IsRequired();
                entity.Property(ut => ut.TypeId).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.Word)
                    .WithMany()
                    .HasForeignKey(ut => ut.WordId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(ut => ut.Type)
                    .WithMany()
                    .HasForeignKey(ut => ut.TypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ut => ut.CreatedById).IsRequired();

                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WordDefinition>(entity =>
            {
                entity.Property(ut => ut.Value).IsRequired();
                entity.Property(ut => ut.WordMeaningId).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.WordMeaning)
                    .WithMany()
                    .HasForeignKey(ut => ut.WordMeaningId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ut => ut.CreatedById).IsRequired();

                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<WordType>(entity =>
            {
                entity.Property(ut => ut.Value).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<WordLink>(entity =>
            {
                entity.Property(ut => ut.SourceId).IsRequired();
                entity.HasOne(ut => ut.Source)
                    .WithMany()
                    .HasForeignKey(ut => ut.SourceId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(ut => ut.TargetId).IsRequired();
                entity.HasOne(ut => ut.Target)
                    .WithMany()
                    .HasForeignKey(ut => ut.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();

                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Statement>(entity =>
            {
                entity.Property(ut => ut.Original).IsRequired();
                entity.Property(ut => ut.Translated).IsRequired();
                entity.Property(ut => ut.SeminarWordId).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                
                entity.HasOne(ut => ut.SeminarWord)
                    .WithMany()
                    .HasForeignKey(ut => ut.SeminarWordId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Seminar>(entity =>
            {
                entity.Property(ut => ut.Value).IsRequired();
                entity.Property(ut => ut.LanguageFromId).IsRequired();
                entity.Property(ut => ut.LanguageToId).IsRequired();
                
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                
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
            
            modelBuilder.Entity<SeminarWordRank>(entity =>
            {
                entity.Property(ut => ut.Value).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                
                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<SeminarWord>(entity =>
            {
                entity.Property(ut => ut.WordId).IsRequired();
                entity.Property(ut => ut.SeminarId).IsRequired();
                entity.Property(ut => ut.SeminarWordRankId).IsRequired();
                entity.Property(ut => ut.IsActive).IsRequired();
                entity.Property(ut => ut.CreatedById).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                
                entity.HasOne(ut => ut.Word)
                    .WithMany()
                    .HasForeignKey(ut => ut.WordId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ut => ut.Seminar)
                    .WithMany()
                    .HasForeignKey(ut => ut.SeminarId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(ut => ut.SeminarWordRank)
                    .WithMany()
                    .HasForeignKey(ut => ut.SeminarWordRankId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(ut => ut.CreatedBy)
                    .WithMany()
                    .HasForeignKey(ut => ut.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
