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
        public string Transaltion { get; set; } = string.Empty;
        public int LanguageFrom { get; set; }
        public int LanguageTo { get; set; }
        public bool IsActive { get; set; }

        public int CreatedById { get; set; }
        public virtual Person CreatedBy { get; set; } = new Person();
        public DateTimeOffset CreatedDate { get; set; }
    }
}
