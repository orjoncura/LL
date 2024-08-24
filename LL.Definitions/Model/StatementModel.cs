using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.SharedDefinitions.Model
{
    public class StatementModel
    {
        public string OriginalStatement { get; set; } = string.Empty;
        public string TranslatedStatement { get; set; } = string.Empty;
        public int LanguageFrom { get; set; }
        public int LanguageTo { get; set; }
    }
}
