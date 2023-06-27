using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SheetSeller.Repositories.Abstract;

namespace SheetSeller.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISheetService sheetService;

        public HomeController(ISheetService sheetService)
        {
            this.sheetService = sheetService;
        }
        public IActionResult Index(string term = "",string tag="", int currentPage = 1, string sorting="Popularity")
        {
            var Sheets = sheetService.GetSheetList(term,tag,true,currentPage,sorting);
            return View(Sheets);
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
