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
        public async Task<IActionResult> DeleteProfileImg()
        {
            var result = await userService.DeleteImgProfile(User.Identity.Name);
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
            _ = emailService.SendConfirmationLink(user.UserName ,user.Email, confirmationLink);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(string email)
        {
            if(email == null)
            {
                TempData["email"] = "Specify your email";
                return RedirectToAction("EditProfile", "Account");
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if(email==user.Email)
            {
                TempData["email"] = "It`s your current email";
                return RedirectToAction("EditProfile", "Account");
            }
            var token = await userManager.GenerateChangeEmailTokenAsync(user, email);
            var confirmationLink = Url.Action("ChangeEmail", "UserAuthentication", new { userId = user.Id, code = token, newemail = email }, protocol: HttpContext.Request.Scheme); ;
            var res = await emailService.SendConfirmationLink(user.UserName, email, confirmationLink);
            if (res.StatusCode==1)
            {
                TempData["email"] = "Check your email, and follow confirmation link";
                return RedirectToAction("EditProfile", "Account");
            }
            else
            {
                TempData["email"] = res.Message;
                return RedirectToAction("EditProfile", "Account");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUsername(string username)
        {
            if(username == null)
            {
                TempData["username"] = "Specify new username";
                return RedirectToAction("EditProfile", "Account");
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if(user.UserName==username) 
            {
                TempData["username"] = "It`s your current username";
                return RedirectToAction("EditProfile", "Account");
            }
            var userExist = await userManager.FindByNameAsync(username);
            if (userExist != null) 
            {
                TempData["username"] = "User with this username exists";
                return RedirectToAction("EditProfile", "Account");
            }
            var res = await userManager.SetUserNameAsync(user,username);
            if(res.Succeeded)
            {
                return RedirectToAction("Logout", "UserAuthentication");
            }
            else
            {
                TempData["username"] = "Oops... Some error";
                return RedirectToAction("EditProfile", "Account");
            }
        }
    }
}
