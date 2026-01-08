using Microsoft.AspNetCore.Identity;
using DevSpot.Constants;
namespace DevSpot.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var usermanager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateUserwithRole(usermanager, "admin@devspot.com", "Admin123!", Roles.Admin);
            await CreateUserwithRole(usermanager, "jobseeker@devspot.com", "Jobseeker123!", Roles.Jobseeker);
            await CreateUserwithRole(usermanager, "employer@devspot.com", "Employer123!", Roles.Employer);
        }


        public static async Task CreateUserwithRole(
            UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email
                };
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
                else
                {
                    throw new Exception($"Failed Creating User. Errors :{string.Join(",", result.Errors)}");
                }
            }

        }
    }
}
