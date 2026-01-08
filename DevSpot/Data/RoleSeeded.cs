using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Data
{
    public class RoleSeeded
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (! await rolemanager.RoleExistsAsync(Roles.Admin))
            {
                await rolemanager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            if (!await rolemanager.RoleExistsAsync(Roles.Jobseeker))
            {
                await rolemanager.CreateAsync(new IdentityRole(Roles.Jobseeker));
            }

             if (! await rolemanager.RoleExistsAsync(Roles.Employer))
            {
                await rolemanager.CreateAsync(new IdentityRole(Roles.Employer));
            }

        }
    }
}
