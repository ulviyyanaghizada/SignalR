using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Models.ViewModels;


namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class BrandController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;


        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            
                return View(_context.Brands.ToList());
            
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBrandVM brandVM)
        {
            if (!ModelState.IsValid) return View();
            
            IFormFile file = brandVM.Image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz shekil file deyil");
                return View();
            }
            if (file.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Sheklin olchusu 2mb-dan chox ola bilmez");
                return View();
            }
            string fileName = Guid.NewGuid() + (file.FileName.Length > 64 ? file.FileName.Substring(0, 64) : file.FileName);
            using (var stream = new FileStream(Path.Combine(_env.WebRootPath,"assets","images","brand", fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Brand brand = new Brand { ImageUrl = fileName };
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            
                Brand brand = _context.Brands.Find(id);
                if (brand is null)
                {
                    return NotFound();
                }
                _context.Brands.Remove(brand);
                _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int? id)
        {
            
                if (id is null) return BadRequest();
                Brand brand = _context.Brands.Find(id);
                if (brand is null) return NotFound();
                return View(brand);
            
        }
        [HttpPost]
        public IActionResult Update(int? id, Brand brand)
        {
            
                if (!ModelState.IsValid) return View();
                if (id is null || id != brand.Id) return BadRequest();
                Brand existBrand = _context.Brands.Find(id);
                if (brand is null) return NotFound();
                existBrand.ImageUrl = brand.ImageUrl;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }
    }
}
