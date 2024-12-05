using Microsoft.AspNetCore.Mvc;
using SIMS_WEB_APDP.Models;
using System.Diagnostics;

namespace SIMS_WEB_APDP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Hello(int id, string name)
        {
            // return Ok("Hello you");
            // return Ok($"id : {id}, name : {name}");
            ViewData["id"] = id; // day bien ra ngoai view
            ViewData["name"] = name; // truyen bien ra ngoai view
            return View(); // goi ra 1 view
        }

        public IActionResult Index()
        {
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
