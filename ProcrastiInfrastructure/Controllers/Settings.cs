using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProcrastiInfrastructure.Controllers
{
    [Authorize]
    public class Settings : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
