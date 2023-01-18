namespace Pronia.Models.ViewModels
{
    public class UpdateProductImageVM
    {
        public IFormFile? CoverImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public IEnumerable<IFormFile>? OtherImages { get; set; }
        public IEnumerable<int> ImageIds { get; set; }
        public IEnumerable<ProductImages> ProductImages { get; set; }
    }
}
