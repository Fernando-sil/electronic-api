namespace api.Entities;

public class Specification
{
    public int Id { get; set; }
    public string Spec { get; set; } = string.Empty;
    public List<SpecificationItem>? ItemSpecifications { get; set; }
}