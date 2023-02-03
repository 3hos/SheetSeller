using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SheetSeller.Repositories.Abstract;
using SheetSeller.Models;
using SheetSeller.Models.DTO;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Identity;
using SheetSeller.Models.Domain;

namespace SheetSeller.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserService userService;
        private IEmailService emailService;
        private readonly UserManager<ApplicationUser> userManager;
        public AccountController(IUserService userService, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.emailService = emailService;
            this.userManager = userManager;
        }
        public IActionResult MyAccount()
        {
            return View();
        }
        public IActionResult EditProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SetProfileImg(IFormFile file)
        {
            if (file ==null)
            {
                TempData["msg"] = "Select an image";
                return RedirectToAction("MyAccount", "Account");
            }
            var result = await userService.SetImgProfile(file, User.Identity.Name);
            if (result.StatusCode == 1)
                return RedirectToAction("MyAccount", "Account");
            else
            {
                TempData["msg"] = "Could not set image";
                return RedirectToAction("MyAccount", "Account");
            }
        }

        public async Task<IActionResult> SendEmailConfirmation()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "UserAuthentication", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
            _ = emailService.SendConfirmationLink(user.Email, confirmationLink);
            return View();
        }
    }
}
