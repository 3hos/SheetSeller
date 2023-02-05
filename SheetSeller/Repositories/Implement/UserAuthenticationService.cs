using Microsoft.AspNetCore.Identity;
using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;
using System.Security.Claims;
using System.Security.Policy;

namespace SheetSeller.Repositories.Implement
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private IEmailService emailService;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
        }
        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var username = await userManager.FindByNameAsync(model.Username);
            var usermail = await userManager.FindByEmailAsync(model.Email);
            if (username != null || usermail != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                EmailConfirmed = false
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));


            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "You have registered successfully";
            return status;
        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in successfully";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User is locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on logging in";
            }

            return status;
        }

        public async Task LogOutAsync()
        {
            await signInManager.SignOutAsync();

        }
            public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        {
            var status = new Status();

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;
        }
        public async Task<Status> ChangePasswordAsync(ChangePasswordPass model, string token, string userId)
        {
            var status = new Status();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;
        }
        public async Task<Status> ConfirmEmailAsync(string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Status() { Message = "User don't exist", StatusCode = 0 };
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return new Status() { StatusCode = 1 };
            else
                return new Status() { Message = "Sorry. We can't confirm your email", StatusCode = 0 };
        }
    }
}
