using api.DataContext;
using api.DTO.GenericDTO;
using api.DTO.SpecificationDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace api.Repository;

public class SpecificationRepository : ISpecification
{
    private readonly DatabaseContext _context;

    public SpecificationRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ResponseRepository<Specification>> AddSpec(AddSpecificationDTO addSpecificationDTO)
    {
        var response = new ResponseRepository<Specification>();
        var spec = addSpecificationDTO.FromAddSpecificationDTO();
        _context.Specifications.Add(spec);
        await _context.SaveChangesAsync();
        response.Data = spec;
        return response;
    }

    public async Task<ResponseRepository<List<GenericGetDTO>>> AddSpecs(List<GenericUpdateDTO> genericUpdateDTOs)
    {
        var response = new ResponseRepository<List<GenericGetDTO>>();
        var specsToAdd = genericUpdateDTOs.Select(spec => new Specification{Spec = spec.Name}).ToList();
        _context.Specifications.AddRange(specsToAdd);
        await _context.SaveChangesAsync();
        var specs = specsToAdd.Select(spec => (GenericGetDTO)spec).ToList();
        response.Data= specs;
        return response;
    }

    public async Task<ResponseRepository<string>> DeleteSpec(int id)
    {
        var response = new ResponseRepository<string>();
        var failResponse = new FailResponse();
        var spec = await _context.Specifications.FirstOrDefaultAsync(s => s.Id == id);
        if(spec is null){
            failResponse.GenerateFailResponse(response, "Specification not found");
            return response;
        }
        _context.Specifications.Remove(spec);
        await _context.SaveChangesAsync();
        response.Message = $"Specification {spec.Spec} has been deleted";
        return response;
    }

    public async Task<ResponseRepositoryWithCount<List<GenericGetDTO>>> GetAllSpecs()
    {
        var response = new ResponseRepositoryWithCount<List<GenericGetDTO>>();
        var dbSpecs = await _context.Specifications.Select(p => 
        new GenericGetDTO(p.Id, p.Spec)).ToListAsync();
        
        response.Data = dbSpecs;
        response.Count = dbSpecs.Count;
        return response;
    }

    public async Task<ResponseRepository<GenericGetDTO>> GetSpec(int id)
    {
        var response = new ResponseRepository<GenericGetDTO>();
        var failResponse = new FailResponse();
        var dbSpec = await _context.Specifications.FirstOrDefaultAsync(spec => spec.Id == id);
        if(dbSpec is null){
            failResponse.GenerateFailResponse(response, "Specification not found");
            return response;
        }
        response.Data = (GenericGetDTO)dbSpec;
        return response;
    }

    public async Task<ResponseRepository<GenericGetDTO>> UpdateSpec(GenericUpdateDTO genericUpdateDTO, int id)
    {
        var response = new ResponseRepository<GenericGetDTO>();
        var failResponse = new FailResponse();
        var dbSpec = await _context.Specifications.FirstOrDefaultAsync(spec => spec.Id ==id);
        if(dbSpec is null){
            failResponse.GenerateFailResponse(response, "Item not found");
            return response;
        }
        dbSpec.Spec = genericUpdateDTO.Name;
        await _context.SaveChangesAsync();

        response.Data = (GenericGetDTO)dbSpec;
        return response;
    }
}