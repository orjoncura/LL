using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Model
{
    public class PersonLog 
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public virtual Person? CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
