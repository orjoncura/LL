using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Model
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CreatedById { get; set; }
        public virtual Person? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
