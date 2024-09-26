using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.Entities;

[PrimaryKey(nameof(SpecificationId), nameof(ItemId))]
public class SpecificationItem
{
    public string Value { get; set; } = string.Empty;
    [Column(Order = 0)]
    public int SpecificationId { get; set; }
    [Column(Order = 1)]
    public Guid ItemId { get; set; }
}