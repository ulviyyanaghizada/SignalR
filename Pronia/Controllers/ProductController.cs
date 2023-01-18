using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pronia.Models.ViewModels;
using System.Linq;

namespace Pronia.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddBasket(int? id) 
        {

            List<BasketItemVM> items = new List<BasketItemVM>();
            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["basket"]))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
            }
            BasketItemVM item= items.FirstOrDefault(i=>i.Id== id);
            if (item==null)
            {
                item = new BasketItemVM()
                {
                    Id = (int)id,
                    Count = 1
                };
                items.Add(item);
            }
            else
            {
                item.Count++;
            }
            string basket = JsonConvert.SerializeObject(items);
            HttpContext.Response.Cookies.Append("basket", basket, new CookieOptions
            {
                MaxAge=TimeSpan.FromDays(5)
            });
            return RedirectToAction(nameof(Index),"Home");
        }
    }
}
