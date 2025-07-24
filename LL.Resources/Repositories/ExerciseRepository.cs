using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using LL.Resources.Factories;
using LL.Resources.Models;

namespace LL.Resources.Repositories;
public class ExerciseRepository(AppDbContext db) : IExerciseRepository
{
    public List<ExerciseViewModel> GetByCourseId(int courseId)
    {
        List<Exercise> exercises = db.Exercises.Where(e => e.CourseId == courseId && e.IsActive).ToList();
        List<ExerciseViewModel> exerciseViewModels = exercises.Any() ? exercises.Select(DataFactory.Convert).ToList() : [];

        return exerciseViewModels;
    }
    
    public List<int> InsertRange(int courseId, List<ExerciseViewModel> exerciseViewModels, int userId)
    {
        var exercises = new List<Exercise>();
        
        foreach (ExerciseViewModel exerciseViewModel in exerciseViewModels.Where(s => s.IsValid).ToList())
        {
            var ex = new Exercise
            {
                CourseId = courseId,
                Original = exerciseViewModel.Original,
                Translated = exerciseViewModel.Translated,
                Extra = exerciseViewModel.Extra,
                IsActive = true,
                CreatedById = userId
            };
            
            exercises.Add(ex);
        }

        if (exercises.Any())
        {
            db.AddRange(exercises);
            db.SaveChanges();
        }

        return exercises.Select(s => s.Id).ToList();
    }
}

