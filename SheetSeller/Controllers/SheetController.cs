using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SheetSeller.Models.Domain;
using SheetSeller.Repositories.Abstract;

namespace SheetSeller.Controllers
{
    public class SheetController : Controller
    {
        private readonly ISheetService sheetService;
        private readonly UserManager<ApplicationUser> userManager;
        public SheetController(ISheetService sheetService, UserManager<ApplicationUser> userManager)
        {
            this.sheetService = sheetService;
            this.userManager = userManager;
        }
        [Authorize]
        public IActionResult Create()
        {
            Sheet model = new();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Sheet model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            model.Author = user;
            var res = await sheetService.CreateSheetAsync(model);
            if (res.StatusCode == 0)
            {
                TempData["msg"] = res.Message;
                return View();
            }
            else
            {
                TempData["msg"] = "Successfuly created";
                return View();
            }
        }
    }
}
