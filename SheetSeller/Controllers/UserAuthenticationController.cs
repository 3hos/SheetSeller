using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;
using SheetSeller.Repositories.Abstract;
using SheetSeller.Repositories.Implement;
using GoogleAuthentication.Services;
using Newtonsoft.Json;

namespace SheetSeller.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationService authService;
        private IEmailService emailService;
        private readonly UserManager<ApplicationUser> userManager;
        private static readonly string GclientID = "814940515141-b419c2qcp7s9o8n9ltq564o8c3p783ai.apps.googleusercontent.com";
        private static readonly string redirectURL = "https://localhost:7112/UserAuthentication/GoogleLoginCallback";
        public UserAuthenticationController(IUserAuthenticationService authService, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            this.authService = authService;
            this.emailService = emailService;
            this.userManager = userManager;
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Role = "user";
            var result = await this.authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            if (result.StatusCode == 1)
            {
                return RedirectToAction("SendEmailConfirmation", "Account");
            }
            else
            {
                return RedirectToAction(nameof(Registration));
            }
        }

        public IActionResult Login()
        {
            var url = GoogleAuth.GetAuthUrl(GclientID, redirectURL);
            ViewBag.Url = url;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await authService.LoginAsync(model);
            if (result.StatusCode == 1)
                return RedirectToAction("Index", "Home");
            else
            {
                TempData["msg"] = "Could not logged in..";
                return RedirectToAction(nameof(Login));
            }
        }
        public async Task<IActionResult> GoogleLoginCallback(string code)
        {
            try
            {
                string secret = Environment.GetEnvironmentVariable("GoogleSecret",EnvironmentVariableTarget.Machine);
                var token = await GoogleAuth.GetAuthAccessToken(code, GclientID, secret, redirectURL);
                var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
                var googleProfile = JsonConvert.DeserializeObject<GoogleProfile>(userProfile);
                var res = await authService.LoginAsync(googleProfile.Email);
                if (res.StatusCode == 1)
                    return RedirectToAction("Index", "Home");
                else
                {
                    TempData["msg"] = "Could not logged in..";
                    return RedirectToAction(nameof(Login));
                }
            }
            catch
            {
                TempData["msg"] = "Could not logged in..";
                return RedirectToAction(nameof(Login));
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await authService.LogOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            Status result = await authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }
        public IActionResult SendResetPasswordLink()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordLink(ChangePasswordUser model)
        {
            var user = new ApplicationUser();
            if (model.Email != null)
            {
                user = await userManager.FindByEmailAsync(model.Email);
            }
            else if (model.Username != null)
            {
                user = await userManager.FindByNameAsync(model.Username);
            }
            else
            {
                TempData["msg"] = "Specify your email or username";
                return View(model);
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "UserAuthentication", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
            var res = await emailService.SendRestoreLink(user.UserName, user.Email, link);
            if (res.StatusCode == 1)
            {
                TempData["msg"] = "Check your emai and folow link, you've recived";
                return View();
            }
            else
            {
                TempData["msg"] = res.Message;
                return View();
            }
        }
        public IActionResult ResetPassword(string userId, string code)
        {
            var model = new ChangePasswordPass() { userID = userId, token = code };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordPass model)
        {
            if (!ModelState.IsValid) { return View(model); }
            var user = await userManager.FindByIdAsync(model.userID);
            IdentityResult passwordChangeResult = await userManager.ResetPasswordAsync(user, model.token, model.NewPassword);
            if (passwordChangeResult.Succeeded)
            {
                TempData["msg"] = "Password has been changed successfully";
                return View(model);
            }
            else
            {
                TempData["msg"] = "Oops. Error";
                return View(model);
            }
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var res = await authService.ConfirmEmailAsync(userId, code);
            if (res.StatusCode == 0)
            {
                return View("Error");
            }
            else return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ChangeEmail(string userId, string code, string newemail)
        {
            if (userId == null || code == null||newemail==null)
            {
                return View("Error");
            }
            var user = await userManager.FindByIdAsync(userId);
            var res = await userManager.ChangeEmailAsync(user, newemail, code);
            if (res.Succeeded) { return RedirectToAction("Index", "Home"); }
            else return View("Error");
        }
    }
}
