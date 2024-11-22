using Project.Data;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{

    public class UniqueMobileNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (ProjectContext)validationContext.GetService(typeof(ProjectContext));

            var customer = dbContext.Customer.FirstOrDefault(c => c.MobileNumber == (long)value);

            if (customer != null)
            {
                return new ValidationResult("Mobile number is already in use.");
            }

            return ValidationResult.Success;
        }
    }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (ProjectContext)validationContext.GetService(typeof(ProjectContext));

            var customer = dbContext.Customer.FirstOrDefault(c => c.UEmail == (string)value);

            if (customer != null)
            {
                return new ValidationResult("Email is already in use.");
            }

            return ValidationResult.Success;
        }
    }
    public class Customer
    {
        [Required, Key]
        public int UId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string UFName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string ULName { get; set; }

        [Required(ErrorMessage = "Mobile Number is required"), RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10 digit number!!")]
        [UniqueMobileNumber(ErrorMessage = "Mobile number is already in use.")]
        public long MobileNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email-Id format")]
        [UniqueEmail(ErrorMessage = "Email is already in use.")]
        public string UEmail { get; set; }

        [Required(ErrorMessage = "Password is required"), RegularExpression(@"[a-zA-Z0-9@]+$", ErrorMessage = "Invalid Password! (It should contain letters, digits and special character '@')")]
        [DataType(DataType.Password)]
        public string UPassword { get; set; }
    }
}
