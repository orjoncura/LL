using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.SharedDefinitions.Model
{
    public class SeminarViewModel
    {
        public StatementModel TargetWord { get; set; } = new StatementModel();

        public List<StatementModel> Sentence { get; set; } = new List<StatementModel>();
    }
}
