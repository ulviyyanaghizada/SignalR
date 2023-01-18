using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
namespace Pronia.ViewComponents
{
    public class QuickViewComponent:ViewComponent
    {
        readonly AppDbContext _context;
        public QuickViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            var product = _context.Products.Include(pi => pi.ProductImages).Include(pc => pc.ProductColors).
                ThenInclude(pc => pc.Color).Include(ps => ps.ProductSizes).ThenInclude(ps => ps.Size).FirstOrDefault(p=>p.Id == id);

            return View(product);
        }
    }
}
