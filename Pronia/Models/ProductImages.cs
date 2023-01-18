﻿namespace Pronia.Models
{
    public class ProductImages
    {
        public int Id { get; set; }
        public bool? IsCover { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
