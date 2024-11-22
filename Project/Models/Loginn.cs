using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Loginn
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string UEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string UPassword { get; set; }

        public bool IsRemember { get; set; }
    }
}
