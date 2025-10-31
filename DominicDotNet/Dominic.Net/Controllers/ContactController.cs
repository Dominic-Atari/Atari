using Microsoft.AspNetCore.Mvc;

namespace Dominic.Net.Controllers
{
    public class ContactController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
