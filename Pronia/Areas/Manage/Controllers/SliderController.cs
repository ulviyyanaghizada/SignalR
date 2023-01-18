using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Models.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SliderController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            
                return View(_context.Sliders.ToList());
            
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateSliderVM sliderVM )
        {
            if (!ModelState.IsValid) return View();
            //CheckOrder:
            //if (_context.Sliders.Any(s => s.Order == slider.Order))
            //{
            //    slider.Order++;
            //    goto CheckOrder;
            //}
            IFormFile file = sliderVM.Image;
            if (!file.ContentType.Contains("Image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz shekil file deyil");
                return View();
            }
            if (file.Length > 2*1024*1024)
            {
                ModelState.AddModelError("Image", "Sheklin olchusu 2mb-dan chox ola bilmez");
                return View();
            }
            string fileName=Guid.NewGuid() + (file.FileName.Length > 64 ? file.FileName.Substring(0,64) : file.FileName);

            using (var stream = new FileStream(Path.Combine(_env.WebRootPath, "assets", "images", "slider", fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Slider slider = new Slider { Description=sliderVM.Description,Order=sliderVM.Order,PrimaryTitle=sliderVM.PrimaryTitle,SecondaryTitle=sliderVM.SecondaryTitle,ImageUrl=fileName};
            if (_context.Sliders.Any(s => s.Order == slider.Order))
            {
                ModelState.AddModelError("Order", $"{slider.Order} sirasinda artiq slider var");
                return View();
            }
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
                       
                Slider slider = _context.Sliders.Find(id);
                if (slider is null)
                {
                    return NotFound();
                }
                _context.Sliders.Remove(slider);
                _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            
                if (id is null) return BadRequest();
                Slider slider = _context.Sliders.Find(id);
                if (slider is null) return NotFound();
                return View(slider);
            
        }
        [HttpPost]
        public IActionResult Update(int? id, Slider slider)
        {
            
            if (id is null || id != slider.Id || id==0 || slider is null) return BadRequest();
            if (!ModelState.IsValid) return View();
            Slider anotherSlider = _context.Sliders.FirstOrDefault(s=>s.Order == slider.Order);
            if (anotherSlider != null)
            {
                anotherSlider.Order = _context.Sliders.Find(id).Order;
            }
            
            Slider existSlider = _context.Sliders.Find(slider.Id);

            existSlider.PrimaryTitle = slider.PrimaryTitle;
            existSlider.SecondaryTitle = slider.SecondaryTitle;
            existSlider.Description = slider.Description;
            existSlider.ImageUrl = slider.ImageUrl;
            existSlider.Order = slider.Order;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
