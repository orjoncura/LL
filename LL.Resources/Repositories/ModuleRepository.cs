using System.Transactions;
using LL.Core.Interfaces.Extensions;
using LL.Core.Interfaces.Repositories;
using LL.Core.Models.ViewModels;
using LL.Resources.Contexts;
using LL.Resources.Factories;
using LL.Resources.Models;

namespace LL.Resources.Repositories;

public class ModuleRepository(AppDbContext db, IEncryptionService encryptionService)  : IModuleRepository
{
    public List<ModuleViewModel> GetByCourseId(string courseId) 
        => db.Modules.Where(m => m.CourseId == encryptionService.Decrypt(courseId) && m.IsActive)
            .OrderBy(m => m.Sequence)
            .Select(c => DataFactory.Convert(encryptionService.Encrypt(c.Id), c)).ToList();

    public bool ModuleCreated(int courseId, int typeId) 
        => db.Modules.Any(m => m.CourseId == courseId && m.TypeId == typeId && m.IsActive);
    
    public bool MarkModuleAsComplete(string encryptedId)
    {
        int moduleId = encryptionService.Decrypt(encryptedId);
        var module = db.Modules.SingleOrDefault(m => m.Id == moduleId);
        if (module == null) return false;
        
        module.Completed = true;
        db.Modules.Update(module);
        
        var nextModule = db.Modules
            .SingleOrDefault(m => 
                m.Sequence == module.Sequence + 1
                && m.CourseId == module.CourseId
                && m.Unlocked == false 
                && m.IsActive);
        
        if (nextModule != null)
        {
            nextModule.Unlocked = true;
            db.Modules.Update(nextModule);
            
            var moduleLog = new ModuleLog()
            {
                ModuleId = nextModule.Id,
                CourseId = nextModule.CourseId,
                Title = nextModule.Title,
                TypeId = nextModule.TypeId,
                Unlocked = nextModule.Unlocked,
                Completed = nextModule.Completed,
                IsActive = nextModule.IsActive,
                CreatedById = nextModule.CreatedById,
                CreatedDate = DateTime.Now
            };

            db.Add(moduleLog);
            db.SaveChanges();
        }
        
        db.Modules.Update(module);
        db.SaveChanges();   
        
        return true;
    }
    public int Insert(int courseId, string title, int typeId, int sequence, bool unlocked, int loginId)
    {
        using (var scope = new TransactionScope())
        {
            var module = new Module()
            {
                CourseId = courseId,
                Title = title,
                TypeId = typeId,
                Unlocked = unlocked,
                Completed = false,
                Sequence = sequence,
                IsActive = true,
                CreatedById = loginId,
                CreatedDate = DateTime.Now
            };

            db.Add(module);
            db.SaveChanges();

            var moduleLog = new ModuleLog()
            {
                ModuleId = module.Id,
                CourseId = courseId,
                Title = title,
                TypeId = typeId,
                Unlocked = unlocked,
                Completed = false,
                IsActive = true,
                CreatedById = loginId,
                CreatedDate = DateTime.Now
            };

            db.Add(moduleLog);
            db.SaveChanges();

            scope.Complete();
            
            return module.Id;
        }
    }
}