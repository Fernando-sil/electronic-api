using api.DTO.GenericDTO;
using api.DTO.SpecificationDTO;
using api.Entities;
using api.Repository;

namespace api.Interfaces;

public interface ISpecification
{
    Task<ResponseRepository<Specification>> AddSpec(AddSpecificationDTO addSpecificationDTO);
    Task<ResponseRepository<List<GenericGetDTO>>> AddSpecs(List<GenericUpdateDTO> addSpecificationDTO);
    Task<ResponseRepository<string>> DeleteSpec(int id);
    Task<ResponseRepositoryWithCount<List<GenericGetDTO>>> GetAllSpecs();
    Task<ResponseRepository<GenericGetDTO>> GetSpec(int id);
    Task<ResponseRepository<GenericGetDTO>> UpdateSpec(GenericUpdateDTO genericUpdateDTO, int id);
}