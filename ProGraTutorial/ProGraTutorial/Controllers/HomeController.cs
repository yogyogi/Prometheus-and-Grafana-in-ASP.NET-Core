using Microsoft.AspNetCore.Mvc;
using ProGraTutorial.Models;
using System.Diagnostics;

namespace ProGraTutorial.Controllers
{
    public class HomeController : Controller
    {
        private AppCustomMeter appCustomMeter;

        public HomeController(AppCustomMeter appCustomMeter)
        {
            this.appCustomMeter = appCustomMeter;
        }

        public IActionResult Index()
        {
            appCustomMeter.ProductSold("Football", 3);

            return View();
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
