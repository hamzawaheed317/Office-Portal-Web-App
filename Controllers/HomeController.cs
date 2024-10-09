using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using OfficePortal.Models;
using OfficePortal.Services;
using System.Diagnostics;

namespace OfficePortal.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private LanguageService _localization;
        public HomeController(ILogger<HomeController> logger, LanguageService localization)
        {
            _logger = logger;
            _localization = localization;
        }

        public IActionResult Index()
        {
            ViewBag.WelcomeMessage = _localization.Getkey("str_welcome_message").Value;
            //get culture information
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            return View();
        }
        #region Localization
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        #endregion
        public IActionResult Login()
        {
            // Logic for the login page
            return View();
        }
        

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Calender()
        {
            return View("Calender");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
