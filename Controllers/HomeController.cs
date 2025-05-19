using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectRepository projectRepository;

        public HomeController(ILogger<HomeController> logger, IProjectRepository projectRepository)
        {
            _logger = logger;
            this.projectRepository = projectRepository;
        }

        public IActionResult Index()
        {
            var proyectos = projectRepository.getProyectos().Take(2).ToList();
            var model = new HomeIndexViewModel
            {
                Proyectos = proyectos
            };
            return View(model);

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
