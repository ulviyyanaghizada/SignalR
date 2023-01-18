using System.ComponentModel.DataAnnotations;

namespace Pronia.Models.ViewModels
{
    public class UserLoginVM
    {
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistance { get; set; }
    }
}
