using System;
using Auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim,
        UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplyIdentityMapConfiguration(builder);
        }

        private static void ApplyIdentityMapConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users", "auth");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims", "auth");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins", "auth");
            modelBuilder.Entity<UserToken>().ToTable("UserRoles", "auth");
            modelBuilder.Entity<Role>().ToTable("Roles", "auth");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", "auth");
            modelBuilder.Entity<UserRole>().ToTable("UserRole", "auth");
        }
    }
}
