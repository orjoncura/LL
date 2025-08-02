using LL.Core.Enums;
using LL.Resources.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LL.Resources.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public AppDbContext() { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserLog> UserLogs { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<NewUserRequest> NewUserRequests { get; set; }
    public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageLog> MessageLogs { get; set; }
    public DbSet<MessageStatus> MessageStatuses { get; set; }
    public DbSet<MessageContent> MessageContents { get; set; }
    public DbSet<LoginHistory> LoginHistory { get; set; }
    public DbSet<ServerError> ServerErrors { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseWord> CourseWords { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<ModuleLog> ModuleLogs { get; set; }
    public DbSet<ModuleType> ModuleTypes { get; set; }
    public DbSet<ImportanceRating> ImportanceRatings { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<WordType> WordTypes { get; set; }
    public DbSet<WordMeaning> WordMeanings { get; set; }
    public DbSet<WordDefinition> WordDefinitions { get; set; }
    public DbSet<WordLink> WordLinks { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    
    public class DateTimeOffsetToUtcConverter : ValueConverter<DateTimeOffset, DateTime>
    {
        public DateTimeOffsetToUtcConverter()
            : base(
                v => DateTime.SpecifyKind(v.UtcDateTime, DateTimeKind.Unspecified),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) 
        { }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetToUtcConverter>()
            .HaveColumnType("timestamp"); // PostgreSQL: stores as UTC
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
         
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(p => p.Email).IsRequired();
            entity.Property(p => p.PasswordHash).IsRequired();
            entity.Property(p => p.Salt).IsRequired();
            entity.Property(u => u.IsActive).IsRequired();
        });
        
        modelBuilder.Entity<User>().HasData(
            Enum.GetValues(typeof(UserEnum))
                .Cast<UserEnum>()
                .Select(e => new User()
                {
                    Id = (int)e,
                    Email = string.Empty,
                    PasswordHash = string.Empty,
                    Salt = string.Empty,
                    IsActive = false
                })
        );

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

            entity.HasOne(ut => ut.User)
                .WithMany()
                .HasForeignKey(ut => ut.UserId);
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
            entity.Property(p => p.CreatedDate).IsRequired();
        });
        
        modelBuilder.Entity<MessageStatus>().HasData(
            Enum.GetValues(typeof(MessageStatusEnum))
                .Cast<MessageStatusEnum>()
                .Select(e => new MessageStatus
                {
                    Id = (int)e,
                    Value = e.ToString(),
                    CreatedDate = DateTimeOffset.Now
                })
        );
        
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
        
        modelBuilder.Entity<ServerError>(entity => 
        {
            entity.Property(p => p.InnerException).IsRequired();
            entity.Property(p => p.StackTrace).IsRequired();
            entity.Property(p => p.InternetProtocol).IsRequired();
            entity.Property(p => p.Data).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();
        });
        
        modelBuilder.Entity<Language>(entity =>
        {
            entity.Property(ut => ut.Value).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();
        });
        
        modelBuilder.Entity<Language>().HasData(
            Enum.GetValues(typeof(LanguageEnum))
                .Cast<LanguageEnum>()
                .Select(e => new Language
                {
                    Id = (int)e,
                    Value = e.ToString(),
                    CreatedDate = DateTimeOffset.Now
                })
        );
        
        modelBuilder.Entity<ImportanceRating>(entity =>
        {
            entity.Property(ut => ut.Value).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();
        });
        
        modelBuilder.Entity<ImportanceRating>().HasData(
            Enum.GetValues(typeof(ImportanceRatingEnum))
                .Cast<ImportanceRatingEnum>()
                .Select(e => new ImportanceRating
                {
                    Id = (int)e,
                    Value = e.ToString(),
                    CreatedDate = DateTimeOffset.Now
                })
        );
        
        modelBuilder.Entity<Word>(entity =>
        {
            entity.Property(ut => ut.Name).IsRequired();
            entity.Property(ut => ut.AudioPath).IsRequired();
            entity.Property(ut => ut.ImportanceRatingId).IsRequired()
                .HasDefaultValue(ImportanceRatingEnum.Low);
            
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();
            
            entity.Property(ut => ut.CreatedById).IsRequired();

            entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasMany(t => t.WordMeanings)
                .WithOne(d => d.Word)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<WordMeaning>(entity =>
        {
            entity.Property(ut => ut.WordId).IsRequired();
            entity.Property(ut => ut.TypeId).IsRequired();
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();

            entity.HasOne(ut => ut.Word)
                .WithMany(ut => ut.WordMeanings)
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
            
            entity.HasMany(ut => ut.WordDefinitions)
                .WithOne(ut => ut.WordMeaning)
                .HasForeignKey(ut => ut.WordMeaningId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WordDefinition>(entity =>
        {
            entity.Property(ut => ut.Value).IsRequired();
            entity.Property(ut => ut.WordMeaningId).IsRequired();
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();

            entity.HasOne(ut => ut.WordMeaning)
                .WithMany(ut => ut.WordDefinitions)
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
            entity.Property(p => p.CreatedDate).IsRequired();
        });
        
        modelBuilder.Entity<WordType>().HasData(
            Enum.GetValues(typeof(WordTypeEnum))
                .Cast<WordTypeEnum>()
                .Select(e => new WordType
                {
                    Id = (int)e,
                    Value = e.ToString(),
                    CreatedDate = DateTimeOffset.Now
                })
        );
        
        modelBuilder.Entity<WordLink>(entity =>
        {
            entity.Property(ut => ut.WordId).IsRequired();
            
            entity.HasOne(ut => ut.Word)
                .WithMany()
                .HasForeignKey(ut => ut.WordId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(ut => ut.LanguageId).IsRequired();
            entity.Property(ut => ut.Value).IsRequired();
            entity.Property(ut => ut.CreatedById).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();

            entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(ut => ut.Value).IsRequired();
            entity.Property(ut => ut.LanguageFromId).IsRequired();
            entity.Property(ut => ut.LanguageToId).IsRequired();
            
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(ut => ut.CreatedById).IsRequired();
            entity.Property(ut => ut.CreatedDate).IsRequired();
            entity.Property(ut => ut.UpdatedById).IsRequired(false);
            entity.Property(ut => ut.UpdatedDate).IsRequired(false);
            
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
            
            entity.HasOne(ut => ut.UpdatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<ModuleType>(entity =>
        {
            entity.Property(ut => ut.Value).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();
        });
        
        modelBuilder.Entity<ModuleType>().HasData(
            Enum.GetValues(typeof(ModuleTypeEnum))
                .Cast<ModuleTypeEnum>()
                .Select(e => new ModuleType()
                {
                    Id = (int)e,
                    Value = e.ToString(),
                    CreatedDate = DateTimeOffset.Now
                })
        );
        
        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(ut => ut.CourseId).IsRequired();
            entity.Property(ut => ut.Title).IsRequired();   
            entity.Property(ut => ut.TypeId).IsRequired()
                .HasDefaultValue(ModuleTypeEnum.Flashcards);
            entity.Property(ut => ut.Unlocked).IsRequired();
            entity.Property(ut => ut.Completed).IsRequired();
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(ut => ut.CreatedById).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();     
            
            entity.HasOne(ut => ut.Course)
                .WithMany()
                .HasForeignKey(ut => ut.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.Type)
                .WithMany()
                .HasForeignKey(ut => ut.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);            
        });     
        
        modelBuilder.Entity<ModuleLog>(entity =>
        {
            entity.Property(ut => ut.ModuleId).IsRequired();
            entity.Property(ut => ut.CourseId).IsRequired();
            entity.Property(ut => ut.Title).IsRequired();       
            entity.Property(ut => ut.TypeId).IsRequired()
                .HasDefaultValue(ModuleTypeEnum.Flashcards);
            entity.Property(ut => ut.Unlocked).IsRequired();
            entity.Property(ut => ut.Completed).IsRequired();
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(ut => ut.CreatedById).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();     
                        
            entity.HasOne(ut => ut.Module)
                .WithMany()
                .HasForeignKey(ut => ut.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.Course)
                .WithMany()
                .HasForeignKey(ut => ut.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.Type)
                .WithMany()
                .HasForeignKey(ut => ut.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);            
        });
        
        modelBuilder.Entity<CourseWord>(entity =>
        {
            entity.Property(ut => ut.WordId).IsRequired();
            entity.Property(ut => ut.ModuleId).IsRequired();
            entity.Property(ut => ut.ImportanceRatingId).IsRequired();
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(ut => ut.CreatedById).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();            
            entity.Property(ut => ut.UpdatedById).IsRequired(false);
            entity.Property(ut => ut.UpdatedDate).IsRequired(false);
            
            entity.HasOne(ut => ut.Word)
                .WithMany()
                .HasForeignKey(ut => ut.WordId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ut => ut.Module)
                .WithMany()
                .HasForeignKey(ut => ut.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.ImportanceRating)
                .WithMany()
                .HasForeignKey(ut => ut.ImportanceRatingId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);            
            
            entity.HasOne(ut => ut.UpdatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.Property(ut => ut.Original).IsRequired();
            entity.Property(ut => ut.Translated).IsRequired();
            entity.Property(ut => ut.ModuleId).IsRequired();
            entity.Property(ut => ut.IsActive).IsRequired();
            entity.Property(ut => ut.CreatedById).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();
            entity.Property(ut => ut.UpdatedById).IsRequired(false);
            entity.Property(ut => ut.UpdatedDate).IsRequired(false);
            
            entity.HasOne(ut => ut.Module)
                .WithMany()
                .HasForeignKey(ut => ut.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(ut => ut.CreatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
                        
            entity.HasOne(ut => ut.UpdatedBy)
                .WithMany()
                .HasForeignKey(ut => ut.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

