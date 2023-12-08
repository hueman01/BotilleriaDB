using BotilleriaDB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BotilleriaDB.Controllers
{
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BotilleriaDbContext db = new();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password) 
        {
            if(email == null || password== null)
            {
                ViewBag.Error = "Debe ingresar Email y Password";
                return View();
            }
            var user=db.Usuarios.FirstOrDefault(u=>u.Email==email && u.Correo== password);
            if(user==null)
            {
                ViewBag.Error = "Email y Contraseña Incorecta";
                return View();
            }
            HttpContext.Session.SetString("nombre", user.Nombre);
            TempData["nombre"] = HttpContext.Session.GetString("nombre");
            return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            return RedirectToAction("Login");
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}