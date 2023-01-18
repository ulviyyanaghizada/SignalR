using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Models.ViewModels;
using Pronia.Utilies.Extensions;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.MaxPageCount = Math.Ceiling((decimal)_context.Products.Count() / 5);
            if (page > ViewBag.MaxPageCount || page < 1)
            {
                return BadRequest();
            }
            ViewBag.CurrentPage = page;
            IEnumerable<Product> products = _context.Products.Skip((page - 1) * 5).Take(5).Include(p => p.ProductColors).ThenInclude(pc => pc.Color).Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).Include(p => p.ProductImages);
            return View(products);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Product existed = _context.Products.Include(p=>p.ProductImages).Include(p=>p.ProductColors).Include(p=>p.ProductSizes).FirstOrDefault(p=>p.Id==id);
            if (existed == null) return NotFound();
            foreach (ProductImages image in existed.ProductImages)
            {
                image.ImageUrl.DeleteFile(_env.WebRootPath,"assets/images/product");
                //_context.ProductImages.Remove(image);
            }
            _context.ProductSizes.RemoveRange(existed.ProductSizes);
            _context.ProductColors.RemoveRange(existed.ProductColors);
            _context.ProductImages.RemoveRange(existed.ProductImages);
            existed.IsDeleted = true;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductVM createProduct)
        {
            var coverImg = createProduct.CoverImage;
            var hoverImg = createProduct.HoverImage;
            var otherImg = createProduct.OtherImages ?? new List<IFormFile>();

            string result = coverImg?.CheckValidate("image/", 600);

            if (result?.Length > 0)
            {
                ModelState.AddModelError("CoverImage", result);
            }
            result = hoverImg?.CheckValidate("image/", 600);
            if (result?.Length > 0)
            {
                ModelState.AddModelError("HoverImage", result);
            }

            foreach (IFormFile image in otherImg)
            {
                result = image.CheckValidate("image/", 600);
                if (result?.Length > 0)
                {
                    ModelState.AddModelError("OtherImages", result);
                }
            }

            foreach  (int colorId in createProduct.ColorIds)
            {
                if (!_context.Colors.Any(c=> c.Id==colorId))
                {
                    ModelState.AddModelError("ColorIds", "Bu Id'li Color yoxdur!");
                    break;
                }
            }

            foreach (int sizeId in createProduct.SizeIds)
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "Bu Id'li Size yoxdur!");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                return View();
            }
            var sizes = _context.Sizes.Where(s => createProduct.SizeIds.Contains(s.Id));
            var colors = _context.Colors.Where(c => createProduct.ColorIds.Contains(c.Id));

            Product newProduct = new Product 
            {
                Name = createProduct.Name,
                CostPrice = createProduct.CostPrice,
                Description = createProduct.Description,
                SellPrice = createProduct.SellPrice,
                Discount = createProduct.Discount,
                IsDeleted = false,
                SKU = "1"
                
            };
            List<ProductImages> images = new List<ProductImages>();
            images.Add(new ProductImages { ImageUrl = coverImg?.SaveFile(Path.Combine(_env.WebRootPath, "assets",
                "images", "product")), IsCover = true, Product = newProduct });
            if (hoverImg != null)
            {
                images.Add(new ProductImages
                {
                    ImageUrl = hoverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets",
               "images", "product")),
                    IsCover = false,
                    Product = newProduct
                });
            }

            
            foreach (var item in otherImg)
            {
                
                images.Add(new ProductImages { ImageUrl = item?.SaveFile(Path.Combine(_env.WebRootPath, "assets",
                    "images", "product")), IsCover = null, Product = newProduct });
            }
            newProduct.ProductImages = images;
            _context.Products.Add(newProduct);
            foreach (var item in colors)
            {
                _context.ProductColors.Add(new ProductColor { Product = newProduct, ColorId = item.Id });
            }
            foreach (var item in sizes)
            {
                _context.ProductSizes.Add(new ProductSize { Product = newProduct, SizeId = item.Id });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UpdateProduct(int? id)
        {
            if (id is null) return BadRequest();

            Product product = _context.Products.Include(p=>p.ProductColors).Include(p=>p.ProductSizes).FirstOrDefault(p=> p.Id==id);
            if (product is null) return NotFound();

            UpdateProductVM updateProduct = new UpdateProductVM {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Discount = product.Discount,
                SellPrice = product.SellPrice,
                CostPrice = product.CostPrice,
                ColorIds = new List<int>(),
                SizeIds = new List<int>()
            
            };
            foreach (var color in product.ProductColors)
            {
                updateProduct.ColorIds.Add(color.ColorId);
            }
            foreach (var size in product.ProductSizes)
            {
                updateProduct.SizeIds.Add(size.SizeId);
            }
            ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            return View(updateProduct);
        }
        [HttpPost]
        public IActionResult UpdateProduct(int? id,UpdateProductVM updateProduct)
        {
            if(id == null) return BadRequest();
            foreach (int colorId in (updateProduct.ColorIds ?? new List<int>()))
            {
                if (!_context.Colors.Any(c=>c.Id==colorId))
                {
                    ModelState.AddModelError("ColorIds", "Bu Id'li Color yoxdur!");
                    break;
                }
            }

            foreach (int sizeId in updateProduct.SizeIds)
            {
                if (!_context.Sizes.Any(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "Bu Id'li Size yoxdur!");
                    break;
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_context.Colors, "Id", "Name");
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                return View();
            }
            var prod = _context.Products.Include(p => p.ProductColors).Include(p => p.ProductSizes).FirstOrDefault(p => p.Id == id);
            if (prod == null) return NotFound();
            
            foreach (var item in prod.ProductColors)
            {
                if (updateProduct.ColorIds.Contains(item.ColorId))
                {
                    updateProduct.ColorIds.Remove(item.ColorId);
                }
                else
                {
                    _context.ProductColors.Remove(item);
                }
            }
           
            foreach (var colorId in updateProduct.ColorIds)
            {
                _context.ProductColors.AddRange(new ProductColor { Product = prod,ColorId = colorId});
            }
           
            foreach (var item in prod.ProductSizes)
            {
                if (updateProduct.SizeIds.Contains(item.SizeId))
                {
                    updateProduct.SizeIds.Remove(item.SizeId);
                }
                else
                {
                    _context.ProductSizes.Remove(item);
                }
            }

            foreach (var sizeId in updateProduct.SizeIds)
            {
                _context.ProductSizes.AddRange(new ProductSize { Product = prod, SizeId = sizeId });
            }

            prod.Name = updateProduct.Name; 
            prod.Description=updateProduct.Description;
            prod.Discount= updateProduct.Discount;
            prod.SellPrice= updateProduct.SellPrice;
            prod.CostPrice= updateProduct.CostPrice;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult UpdateProductImage(int? id)
        {
            if (id == null) return BadRequest();
            var prod = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == id);
            if (prod == null) return NotFound();
            UpdateProductImageVM updateProductImage = new UpdateProductImageVM
            {
                ProductImages = prod.ProductImages
            };
            return View(updateProductImage);
        }

        public IActionResult DeleteProductImage(int? id)
        {
            if (id == null) return BadRequest();
            var productImage = _context.ProductImages.Find(id);
            if (productImage == null) return NotFound();
            _context.ProductImages.Remove(productImage);
            _context.SaveChanges();
            return Ok();
        }
    }
}
