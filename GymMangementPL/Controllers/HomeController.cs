using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public IActionResult Index()
        {

            var analtyics = _analyticsService.GetAnalyticsData();
            return View(analtyics);
        }
    }
}
