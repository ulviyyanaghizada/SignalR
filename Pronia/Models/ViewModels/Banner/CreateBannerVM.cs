﻿namespace Pronia.Models.ViewModels
{
    public class CreateBannerVM
    {
        public string PrimaryTitle { get; set; }
        public string SecondaryTitle { get; set;}
        public IFormFile Image { get; set; }
    }
}
