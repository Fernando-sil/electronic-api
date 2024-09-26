using api.Entities;

namespace api.DTO.SpecificationDTO;

public class GetSpecificationDTO
{
    public int Id { get; set; }
    public string Spec { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public static explicit operator GetSpecificationDTO(SpecificationItem specificationitem){
        return new GetSpecificationDTO{
            Spec = string.Empty,
            Value = specificationitem.Value
        };
    }
    
    public static explicit operator GetSpecificationDTO(Specification specification){
        return new GetSpecificationDTO{
            Id = specification.Id,
            Spec = specification.Spec,
        };
    }

    public Specification ToSpecification(){
        return new Specification{
            Id = Id,
            Spec = Spec
        };
    }
}