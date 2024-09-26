using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.Entities;

[PrimaryKey(nameof(ItemId), nameof(CartId))]
public class CartItem
{
    [Column(Order = 0)]
    public Guid ItemId { get; set; }
    [Column(Order = 1)]
    public Guid CartId { get; set; }
    public Item? Item { get; set; }
    public int Quantity { get; set; }
    [Precision(10,2)]
    public decimal SubTotal { get; set; } = 0;
}