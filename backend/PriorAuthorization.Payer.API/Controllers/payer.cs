using Microsoft.AspNetCore.Mvc;

namespace PriorAuthorization.Payer.API.Controllers
{
    public class payer : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
