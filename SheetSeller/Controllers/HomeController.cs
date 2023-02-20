using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(string term = "", int currentPage = 1)
        {
            var Sheets = sheetService.GetSheetList(term,true,currentPage);
            return View(Sheets);
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
