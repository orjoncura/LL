using LL.Core.Interfaces.Extensions;
using LL.Resources.Contexts;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using LL.Resources.Factories;
using LL.Resources.Models;

namespace LL.Resources.Repositories;
public class ExerciseRepository(AppDbContext db, IEncryptionService encryptionService) : IExerciseRepository
{
    public List<ExerciseViewModel> GetByModuleId(string moduleId)
    {
        List<Exercise> exercises = db.Exercises.Where(e => e.ModuleId == encryptionService.Decrypt(moduleId) && e.IsActive).ToList();
        List<ExerciseViewModel> exerciseViewModels = exercises.Any() 
            ? exercises.Select(e => DataFactory.Convert(encryptionService.Encrypt(e.Id), e)).ToList() : [];

        return exerciseViewModels;
    }
    
    public List<int> InsertRange(string moduleId, List<ExerciseViewModel> exerciseViewModels, int userId)
    {
        var exercises = new List<Exercise>();
        
        foreach (ExerciseViewModel exerciseViewModel in exerciseViewModels.Where(s => s.IsValid).ToList())
        {
            var ex = new Exercise
            {
                ModuleId = encryptionService.Decrypt(moduleId) ,
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

