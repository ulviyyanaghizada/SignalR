using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ColorController : Controller
    {
        readonly AppDbContext _context;
        public ColorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
                return View(_context.Colors.ToList());
            
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name)
        {
           
                if (!ModelState.IsValid) return View();
                _context.Colors.Add(new Color { Name=Name });
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }

        public IActionResult Delete(int id)
        {
           
                Color color = _context.Colors.Find(id);
                if (color is null)
                {
                    return NotFound();
                }
                _context.Colors.Remove(color);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int? id)
        {
            
                if (id is null) return BadRequest();
                Color color = _context.Colors.Find(id);
                if (color is null) return NotFound();
                return View(color);
            
        }
        [HttpPost]
        public IActionResult Update(int? id, Color color)
        {
            
                if (!ModelState.IsValid) return View();
                if (id is null || id != color.Id) return BadRequest();
                Color existColor = _context.Colors.Find(id);
                if (color is null) return NotFound();
                existColor.Name = color.Name;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }
    }
}
