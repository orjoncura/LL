using LL.Core.Interfaces.Extensions;
using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using LL.Resources.Factories;
using LL.Resources.Models;

namespace LL.Resources.Repositories;

public class CourseRepository(AppDbContext db, IEncryptionService encryptionService) : ICourseRepository
{
    public int Insert(string title, string value, int fromId, int toId, int userId)
    {
        Course? course = db.Courses.FirstOrDefault(c => c.Value == value && c.LanguageFromId == fromId && c.LanguageToId == toId && c.IsActive);

        if (course == null)
        {
            course = new Course()
            {
                Title = title,
                Value = value,
                LanguageFromId = fromId,
                LanguageToId = toId,
                IsActive = true,
                CreatedById = userId,
                CreatedDate = DateTime.Now,
            };

            db.Add(course);
            db.SaveChanges();
        }
        
        return course.Id;
    }

    public bool UpdateValue(int courseId, string value, int userId)
    {
        Course? course = db.Courses.FirstOrDefault(c => c.Id == courseId && c.IsActive);

        if (course == null)
            return false;

        course.Value = value;
        course.UpdatedById = userId;
        course.UpdatedDate = DateTime.Now;
        db.Courses.Update(course);
        db.SaveChanges();

        return true;
    }

    public bool MarkCourseAsCompleted(int courseId)
    {
        Course? course = db.Courses.FirstOrDefault(c => c.Id == courseId && c.IsActive);

        if (course != null)
        {
            course.IsCompleted = true;
            db.Courses.Update(course);
            db.SaveChanges();
            
            return true;
        }
        
        return false;
    }
    public bool DeleteCourseWord(string moduleId, string wordId, int userId)
    {
        CourseWord? courseWord = db.CourseWords.FirstOrDefault(c => 
            c.ModuleId == encryptionService.Decrypt(moduleId) && c.WordId == encryptionService.Decrypt(wordId) && c.IsActive);
        
        if(courseWord != null)
        {
            courseWord.IsActive = false;
            courseWord.UpdatedById = userId;
            courseWord.UpdatedDate = DateTime.Now;
            
            db.CourseWords.Update(courseWord);
            db.SaveChanges();
            
            return true;
        }
        
        return false;
    }
    public bool DeleteCourseById(string encryptedId, int userId)
    {
        int id = encryptionService.Decrypt(encryptedId);
        Course? course = db.Courses.FirstOrDefault(c => c.Id == id && c.IsActive);
        
        if(course != null)
        {
            course.IsActive = false;
            course.UpdatedById = userId;
            course.UpdatedDate = DateTime.Now;
            
            db.Courses.Update(course);
            db.SaveChanges();
            
            return true;
        }
        
        return false;
    }
    public List<CourseViewModel> GetCoursesByUserId(int userId)
    {
         var courses = db.Courses
            .Where(c => c.CreatedById == userId && c.IsActive)
            .OrderByDescending(c => c.CreatedDate)
            .ToList();

         var courseIds = courses.Select(c => c.Id).ToList();
         var coursesWithModules = db.Modules
            .Where(m => courseIds.Contains(m.CourseId) && m.IsActive)
            .Select(m => m.CourseId)
            .Distinct()
            .ToList()
            .ToHashSet();

         var failureCutoff = DateTime.UtcNow.AddHours(-10);

         return courses.Select(c =>
         {
             var viewModel = DataFactory.Convert(encryptionService.Encrypt(c.Id), c);
             viewModel.HasModules = coursesWithModules.Contains(c.Id);

             if (viewModel.HasModules)
                 viewModel.Status = "Ready";
             else if (ToUtc(c.CreatedDate) <= failureCutoff)
                 viewModel.Status = "Failed";
             else
                 viewModel.Status = "InProgress";

             return viewModel;
         }).ToList();
    }

    private static DateTime ToUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    public CourseViewModel? GetById(string encryptedId)
    {
        var course = db.Courses.SingleOrDefault(c => c.Id == encryptionService.Decrypt(encryptedId) && c.IsActive);
        
        if(course == null) return null;
        
        return DataFactory.Convert(encryptedId, course);
    }
}