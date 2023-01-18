using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class CategoryController : Controller
    {
        readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.Categories.ToList());

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name)
        {

            if (!ModelState.IsValid) return View();
            _context.Categories.Add(new Category { Name = Name });
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {

            Category category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
        
            return RedirectToAction(nameof(Index));
    }

        public IActionResult Update(int? id)
        {
            
                if (id is null) return BadRequest();
                Category category = _context.Categories.Find(id);
                if (category is null) return NotFound();
                return View(category);
            
        }
        [HttpPost]
        public IActionResult Update(int? id, Category category)
        {
            
                if (!ModelState.IsValid) return View();
                if (id is null || id != category.Id) return BadRequest();
                Category existCategory = _context.Categories.Find(id);
                if (category is null) return NotFound();
                existCategory.Name = category.Name;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }
    }
}
