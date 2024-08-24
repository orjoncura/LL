using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL.Data.Model
{
    public class UserTokenLog
    {
        public long Id { get; set; }

        public int UserTokenId { get; set; }
        public virtual UserToken UserToken { get; set; } = new UserToken();

        public Guid Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public bool IsActive { get; set; }

        public int CreatedById { get; set; }
        public virtual Person CreatedBy { get; set; } = new Person();
        public DateTimeOffset CreatedDate { get; set; }
    }
}
