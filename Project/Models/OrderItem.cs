using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; } // Assuming ProductId is unique across products
        public string ProductName { get; set; }
        public string img_name {  get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public virtual int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual OrderTable? Order { get; set; } // Navigation property to the Order entity
    }
}
