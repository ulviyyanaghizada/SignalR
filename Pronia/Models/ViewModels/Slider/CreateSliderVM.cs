using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models.ViewModels
{
    public class CreateSliderVM
    {
       
        public string PrimaryTitle { get; set; }
        public string SecondaryTitle { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public IFormFile Image { get; set; }
    }
}
