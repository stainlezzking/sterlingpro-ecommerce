using System.ComponentModel.DataAnnotations.Schema;

namespace sterlingpro.assessment.Common.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
