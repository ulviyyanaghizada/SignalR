﻿namespace Pronia.Models.ViewModels
{
    public class BasketVM
    {
        public ICollection<FlowerBasketItemVM> Flowers { get; set; }    
        public double TotalPrice { get; set; }
    }
}
