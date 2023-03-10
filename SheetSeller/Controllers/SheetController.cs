using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using SheetSeller.Models.Domain;
using SheetSeller.Repositories.Abstract;
using SheetSeller.Repositories.Implement;
using SheetSeller.Models.DTO;

namespace SheetSeller.Controllers
{
    [Authorize]
    public class SheetController : Controller
    {
        private readonly ISheetService sheetService;
        private readonly UserManager<ApplicationUser> userManager;
        public SheetController(ISheetService sheetService, UserManager<ApplicationUser> userManager)
        {
            this.sheetService = sheetService;
            this.userManager = userManager;
        }
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
        public IActionResult Edit(int id)
        {
            var sheet = sheetService.GetSheet(id);
            if (sheet == null || sheet.Author.UserName != User.Identity.Name)
            {
                return RedirectToAction("Create");
            }
            return View(sheet);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditSheet model)
        {
            if (!ModelState.IsValid)
            { return Redirect(Request.Headers["Referer"].ToString()); }
            var res = await sheetService.UpdateSheetAsync(model);
            if(res.StatusCode == 0) 
            {
                TempData["msg"]=res.Message;
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                TempData["msg"] = "Changes Saved";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, int id)
        {
            if (file == null)
            {
                TempData["msg"] = "Select a file";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            var result = await sheetService.UploadFileAsync(file,id);
            if (result.StatusCode == 1)
                return Redirect(Request.Headers["Referer"].ToString());
            else
            {
                TempData["msg"] = "Could not set image";
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }
        public IActionResult Sheet(int ID)
        {
            var sheet = sheetService.GetSheet(ID);
            if(sheet == null)
            {
                return RedirectToAction("Create");
            }
            sheet.OwnedBy.Add(sheet.Author);
            if(!sheet.OwnedBy.Any(u => u.UserName==User.Identity.Name))
            {
                return RedirectToAction("Own");
            }
            return View(sheet);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int ID)
        {
            var res = await sheetService.DeleteSheetAsync(sheetService.GetSheet(ID));
            TempData["msg"] = res.Message;
            return RedirectToAction("MyAccount","Account");
        }
    }
}
