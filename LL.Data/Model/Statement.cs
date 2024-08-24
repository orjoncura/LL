using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Model
{
    public class Statement
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public virtual Word Word { get; set; } = new Word();

        public string OriginalStatement { get; set; } = string.Empty;
        public string TranslatedStatement { get; set; } = string.Empty;
        public int LanguageFrom { get; set; }
        public int LanguageTo { get; set; }
        public bool IsActive { get; set; }

        public int CreatedById { get; set; }
        public virtual Person CreatedBy { get; set; } = new Person();
        public DateTimeOffset CreatedDate { get; set; }
    }
}
