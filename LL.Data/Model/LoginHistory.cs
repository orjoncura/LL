using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Model
{
    public class LoginHistory 
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public int CreatedById { get; set; }
        public virtual Person? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
