using LL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.SharedDefinitions.Models
{
    public class TargetWordViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public TargetWordViewModel() { }

        public TargetWordViewModel(Word word)
        {
            Name = word.Name;
            Translation = word.Translation;
            Definition = word.Definition;
            Type = word.Type;
        }
    }
}
