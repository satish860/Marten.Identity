using System;
using System.Collections.Generic;
using System.Text;

namespace Marten.Identity
{
    public class IdentityUser
    {
        public IdentityUser()
        {
            this.Claims = new List<IdentityUserClaim>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public List<IdentityUserClaim> Claims { get; set; }
    }
}
