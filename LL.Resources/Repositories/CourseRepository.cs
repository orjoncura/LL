using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using LL.Resources.Factories;
using LL.Resources.Models;
using Microsoft.EntityFrameworkCore;

namespace LL.Resources.Repositories;

public class CourseRepository(AppDbContext db) : ICourseRepository
{
    public int Insert(string value, int fromId, int toId, int userId)
    {
        Course? course = db.Courses.FirstOrDefault(c => c.Value == value && c.LanguageFromId == fromId && c.LanguageToId == toId && c.IsActive);

        if (course == null)
        {
            course = new Course()
            {
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

    public bool DeleteCourseById(int id, int userId)
    {
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
         List<CourseViewModel> courseViewModels = db.Courses
            .Where(c => c.CreatedById == userId && c.IsActive)
            .Select(DataFactory.Convert).ToList();

         return courseViewModels;
    }
}