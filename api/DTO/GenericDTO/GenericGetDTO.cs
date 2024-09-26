using api.Entities;

namespace api.DTO.GenericDTO;

public record GenericGetDTO(int Id, string Name){

    public static explicit operator GenericGetDTO(Brand brand){
        return new GenericGetDTO(Id:brand.Id,Name:brand.BrandName);
    }

    public static explicit operator GenericGetDTO(Specification specification){
        return new GenericGetDTO(Id:specification.Id, Name: specification.Spec);
    }
    public static explicit operator GenericGetDTO(Category category){
        return new GenericGetDTO(Id:category.Id, Name:category.CategoryName);
    }
};