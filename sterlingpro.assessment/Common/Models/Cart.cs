namespace sterlingpro.assessment.Common.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
