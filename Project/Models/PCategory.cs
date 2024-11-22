using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class PCategory
    {
        [Required, Key, Display(Name = "ID")]
        public int CatgId { get; set; }
        [Display(Name = "Category")]
        public string CatgName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
