using System;
using Microsoft.AspNetCore.Identity;

namespace Auth.Models;

public class User : IdentityUser<Guid>
{
    public bool IsActive { get; set; }
    public string IdentityProvider { get; set; }
}