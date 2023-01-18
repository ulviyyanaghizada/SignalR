using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SizeController : Controller
    {
       
        readonly AppDbContext _context;
        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Sizes.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name)
        {

            if (!ModelState.IsValid) return View();
            _context.Sizes.Add(new Size { Name = Name });
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {

            Size size = _context.Sizes.Find(id);
            if (size is null)
            {
                return NotFound();
            }
            _context.Sizes.Remove(size);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int? id)
        {

            if (id is null) return BadRequest();
            Size size = _context.Sizes.Find(id);
            if (size is null) return NotFound();
            return View(size);

        }
        [HttpPost]
        public IActionResult Update(int? id, Size size)
        {

            if (!ModelState.IsValid) return View();
            if (id is null || id != size.Id) return BadRequest();
            Size existSize = _context.Sizes.Find(id);
            if (size is null) return NotFound();
            existSize.Name = size.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
