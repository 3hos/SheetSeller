using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SheetSeller.Repositories.Abstract;
using SheetSeller.Models;
using SheetSeller.Models.DTO;
using NuGet.Protocol.Plugins;

namespace SheetSeller.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserService userService;
        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult MyAccount()
        {
            return View();
        }
        public IActionResult EditProfile()
        {
            var user = userService.GetUserByName(User.Identity.Name);
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
    }
}
