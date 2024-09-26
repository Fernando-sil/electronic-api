using Microsoft.EntityFrameworkCore;

namespace api.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public bool IsPurchasePaid { get; set; } = false;
    [Precision(10,2)]
    public decimal? Total { get; set; }
    public DateTime? DatePurchasePaid { get; set; }
    public User? User { get; set; }
    public List<CartItem>? CartItems { get; set; }
}