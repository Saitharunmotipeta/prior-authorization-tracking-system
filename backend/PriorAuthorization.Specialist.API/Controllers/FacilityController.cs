using Microsoft.AspNetCore.Mvc;

namespace PriorAuthorization.Specialist.API.Controllers
{
    public class FAcilityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
