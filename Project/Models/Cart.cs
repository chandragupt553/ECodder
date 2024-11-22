using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    
    public class Cart
    {
        
        public virtual int UId { get; set; }
        [ForeignKey("UId")]
        public virtual Customer? customerid { get; set; }
        
        public virtual int PId { get; set; }
        [ForeignKey("PId")]
        public virtual Product? productid { get; set; }
        public int Qty {  get; set; }
    }
}
