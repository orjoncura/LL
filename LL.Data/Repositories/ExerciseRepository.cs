using LL.Data.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.Short;
using LL.Core.Models.ViewModels;
using LL.Data.Factories;
using LL.Data.Model;

namespace LL.Data.Repositories;
public class ExerciseRepository(AppDBContext db) : IExerciseRepository
{
    public List<ExerciseViewModel> GetByCourseWordId(int courseWordId)
    {
        List<Statement> exercises = db.Exercises.Where(e => e.SeminarWordId == courseWordId && e.IsActive).ToList();
        List<ExerciseViewModel> exerciseViewModels = exercises.Any() ? exercises.Select(DataFactory.Convert).ToList() : [];

        return exerciseViewModels;
    }
    
    public List<int> InsertRange(int seminarWordId, List<ExerciseViewModel> exerciseViewModels, int userId)
    {
        var exercises = new List<Statement>();
        
        foreach (ExerciseViewModel exerciseViewModel in exerciseViewModels.Where(s => s.IsValid).ToList())
        {
            var ex = new Statement
            {
                SeminarWordId = seminarWordId,
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

