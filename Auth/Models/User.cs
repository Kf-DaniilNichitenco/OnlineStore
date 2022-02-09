using System;
using Microsoft.AspNetCore.Identity;

namespace Auth.Models
{
    public class User : IdentityUser<Guid>
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ushort Age { get; set; }
        public string IdentityProvider { get; set; }
    }
}