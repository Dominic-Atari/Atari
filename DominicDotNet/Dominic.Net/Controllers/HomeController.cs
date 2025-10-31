using Microsoft.AspNetCore.Mvc;
using Dominic.Net.Models;
using Dominic.Net.ViewModels;
using System.Diagnostics;

namespace Dominic.Net.Controllers
{
   public class HomeController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        public ViewResult Index()
        {
            var PiesOfWeek = _pieRepository.PiesOfWeek;

            var homeViewModel = new HomeViewModel(PiesOfWeek);

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
