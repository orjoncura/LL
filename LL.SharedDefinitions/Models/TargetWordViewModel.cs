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
        public int Type { get; set; }

        public TargetWordViewModel() { }

        public TargetWordViewModel(Word word)
        {
            Name = word.Name;
            Definition = word.Definition;
        }
    }
}
