using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Model
{
    public class Word 
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;

        public int LanguageFromId { get; set; }
        public virtual Language LanguageFrom { get; set; } = new Language();

        public int LanguageToId { get; set; }
        public virtual Language LanguageTo { get; set; } = new Language();

        public bool IsActive { get; set; }

        public int CreatedById { get; set; }
        public virtual Person CreatedBy { get; set; } = new Person();
        public DateTimeOffset CreatedDate { get; set; }
    }
}
