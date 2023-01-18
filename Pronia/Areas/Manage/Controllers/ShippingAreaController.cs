using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ShippingAreaController : Controller
    {
        readonly AppDbContext _context;
        public ShippingAreaController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
                return View(_context.ShippingAreas.ToList());
            
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name, string Description, string Image)
        {
            
                if (_context.ShippingAreas.ToList().Count >= 3) return RedirectToAction(nameof(Index));
                if (!ModelState.IsValid) return View();
                _context.ShippingAreas.Add(new ShippingArea { Name = Name, Description = Description, Image = Image });
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }


        public IActionResult Delete(int id)
        {
            
                ShippingArea shippingArea = _context.ShippingAreas.Find(id);
                if (shippingArea is null)
                {
                    return NotFound();
                }
                _context.ShippingAreas.Remove(shippingArea);
                _context.SaveChanges();

            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            
                if (id is null) return BadRequest();
                ShippingArea shippingArea = _context.ShippingAreas.Find(id);
                if (shippingArea is null) return NotFound();
                return View(shippingArea);
            
        }
        [HttpPost]
        public IActionResult Update(int? id, ShippingArea shippingArea)
        {
            
                if (!ModelState.IsValid) return View();
                if (id is null || id != shippingArea.Id) return BadRequest();
                ShippingArea existShippingArea = _context.ShippingAreas.Find(id);
                if (shippingArea is null) return NotFound();
                existShippingArea.Name = shippingArea.Name;
                existShippingArea.Description = shippingArea.Description;
                existShippingArea.Image = shippingArea.Image;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }
    }
}
