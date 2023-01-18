﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        [Range (0.0 , Double.MaxValue)]
        public double CostPrice { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double SellPrice { get; set; }
        [Range(0,100)]
        public int Discount { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ProductImages>? ProductImages { get; set; }
        public ICollection<ProductColor>? ProductColors { get; set;}
        public ICollection<ProductSize>? ProductSizes { get; set; }
        public ProductInformation? ProductInformations { get; set; }
       
    }
}
