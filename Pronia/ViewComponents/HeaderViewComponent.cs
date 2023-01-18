﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Configuration;
using Pronia.DAL;
using Pronia.Models.ViewModels;
using System.Collections.Generic;

namespace Pronia.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        readonly AppDbContext _context;
        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM header = new HeaderVM()
            {
                Settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value),
                Basket=GetBasket()
            };
            return View(header);
        }
        BasketVM GetBasket()
        {
            BasketVM basket = new BasketVM();
            List<BasketItemVM> items = new List<BasketItemVM>();
            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["basket"]))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
            }

            if (items!=null)
            {
                basket.Flowers = new List<FlowerBasketItemVM>();
                foreach (var item in items)
                {
                    FlowerBasketItemVM flower = new FlowerBasketItemVM();
                    flower.Product = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == item.Id);
                    flower.Count = item.Count;
                    basket.Flowers.Add(flower);
                    basket.TotalPrice += flower.Product.SellPrice * flower.Count;
                }
            }
            
            return basket;
        }                        
    }
}
