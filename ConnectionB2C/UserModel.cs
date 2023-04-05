using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionB2C
{
    public class UserModel
    {
        public string? ID { get; set; }
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? LastSignedIn { get; set; }
    }
}
