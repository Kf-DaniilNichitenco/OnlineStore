using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Auth.IdentityServices.AspNetIdentity
{
    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        public async Task<IdentityResult> CreateIfNotExistAsync(string roleName)
        {
            var role = await FindByNameAsync(roleName);

            if (role != null)
            {
                return IdentityResult.Success;
            }

            role = new Role
            {
                Name = roleName
            };

            var result = await CreateAsync(role);

            return result;
        }
    }
}
