using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SheetSeller.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public IActionResult MyAccount()
        {
            return View();
        }
    }
}
