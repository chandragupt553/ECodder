using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Product
    {
        [Key, Required, Display(Name = "ProductId")]
        public int PId { get; set; }
        [Required, Display(Name = "Product-Name")]
        public string PName { get; set; }
        [Required]
        public int Price { get; set; }
        public virtual int CatgId {  get; set; }
        [ForeignKey("CatgId")]
        public virtual PCategory? Category { get; set; }
        public string img_name { get; set; }

        public string Color { get; set; }
        public string Description { get; set; }
    }
}