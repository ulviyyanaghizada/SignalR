using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Controllers
{
    public class ShopController : Controller
    {
        readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ICollection<Category> categories = new List<Category>();
            List<Color> colors = new List<Color>();
           
            
                categories = _context.Categories.ToList();
                colors = _context.Colors.ToList();
                
            
            ViewBag.Colors = colors;
            
            return View(categories);
        }
    }
}
