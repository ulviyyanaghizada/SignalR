using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Migrations;
using Pronia.Models;
using Pronia.Models.ViewModels;
namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin,Moderator")]
    public class BannerController:Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public BannerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Banners.ToList());
        }


        public IActionResult Delete(int id)
        {

            Banner banner = _context.Banners.Find(id);
            if (banner is null)
            {
                return NotFound();
            }
            _context.Banners.Remove(banner);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBannerVM bannerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            IFormFile file = bannerVM.Image;
            if (file.ContentType.Contains("image/"))
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
            using (var stream = new FileStream(Path.Combine(_env.WebRootPath, "assets", "images", "banner", fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Banner banner = new Banner { SecondaryTitle = bannerVM.SecondaryTitle, PrimaryTitle = bannerVM.PrimaryTitle, ImageUrl = fileName };
            _context.Banners.Add(banner);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {

            if (id is null) return BadRequest();
            Banner banner = _context.Banners.Find(id);
            if (banner is null) return NotFound();
            return View(banner);

        }
        [HttpPost]
        public IActionResult Update(int? id, Banner banner)
        {

            if (!ModelState.IsValid) return View();
            if (id is null || id != banner.Id) return BadRequest();
            Banner existBanner = _context.Banners.Find(id);
            if (banner is null) return NotFound();
            existBanner.PrimaryTitle = banner.PrimaryTitle;
            existBanner.SecondaryTitle = banner.SecondaryTitle;
            existBanner.ImageUrl = banner.ImageUrl;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
