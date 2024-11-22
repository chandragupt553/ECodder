using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class OrderTable
    {
        [Key,Required]
        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public List<OrderItem> OrderItems { get; set; }
       
    }
}