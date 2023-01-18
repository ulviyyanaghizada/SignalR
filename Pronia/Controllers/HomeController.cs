using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Abstractions.Services;
using Pronia.Areas.Manage.Controllers;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Models.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        public HomeController(AppDbContext context,IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            IQueryable<Product> products = _context.Products.Where(p => !p.IsDeleted).Include(p => p.ProductImages).Take(4);
            HomeVM home = new HomeVM { Sliders = _context.Sliders.OrderBy(s=> s.Order) ,Brands=_context.Brands,
                FeaturedProducts = products , LastestProducts = products.OrderByDescending(p=>p.Id)};

            List<Client> clients = new List<Client>();
            
            List<ShippingArea> shippingAreas = new List<ShippingArea>();
            
            
                clients = _context.Clients.ToList();
               
                shippingAreas = _context.ShippingAreas.ToList();
              
            
            ViewBag.ShippingAreas = shippingAreas;
          
            ViewBag.banner = _context.Banners.ToList();
            ViewBag.Clients = clients; 
            return View(home);
        }
        [HttpPost]
        public IActionResult LoadProducts(int skip = 4, int take = 4)
        {
            HomeVM home = new HomeVM
            {
                FeaturedProducts = _context.Products.Where(p => !p.IsDeleted).Include(p => p.ProductImages).Skip(skip).Take(take)
            };
            return PartialView("_ProductPartial", home);
        }
         public IActionResult SetSession(string key,string value)
        {
            HttpContext.Session.SetString(key,value);
            return Content("Ok");
        }
        public IActionResult GetSession(string key)
        {
            string value = HttpContext.Session.GetString(key);
            return Content(value);
        }
        public IActionResult SetCookies(string key,string value)
        {
            HttpContext.Response.Cookies.Append(key, value,new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(1)
            });
            return Content("Ok");
        }
        public IActionResult GetCookies(string key)
        {
            return Content(HttpContext.Request.Cookies[key]);
        }

        public IActionResult SendMail()
        {
            _emailService.Send("Ccefermustafayev@gmail.com", "Your Account is not Safe...", "We offer you");

            return View();
        }
    }
}
