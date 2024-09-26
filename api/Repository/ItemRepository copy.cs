// using api.DataContext;
// using api.DTO.ItemDTO;
// using api.DTO.SpecificationDTO;
// using api.DTO.SpecificationItemDTO;
// using api.Entities;
// using api.Interfaces;
// using AutoMapper;
// using Microsoft.EntityFrameworkCore;

// namespace api.Repository;

// public class ItemRepository2 : IItemRepository
// {
//     private readonly IDbContextFactory<DatabaseContext> _context;
//     private readonly IMapper _mapper;
//     private const string FailMessage = "not found";

//     public ItemRepository2(IMapper mapper, IDbContextFactory<DatabaseContext> context)
//     {
//         _mapper = mapper;
//         _context = context;
//     }


//     public async Task<ResponseRepository<GetItemDTO>> AddItem(AddItemDTO addItemDTO)
//     {
//         using(var context = _context.CreateDbContext()){

//        var response = new ResponseRepository<GetItemDTO>();
//        var item = _mapper.Map<Item>(addItemDTO);
//        context.Items.Add(item);
//        await context.SaveChangesAsync();
//        response.Data = _mapper.Map<GetItemDTO>(item);
//        return response;
//         }
//     }
//     public async Task<ResponseRepository<GetItemDTO>> AddSpecToItem(AddSpecificationItemDTO addSpecificationItemDTO)
//     {
//          using(var context = _context.CreateDbContext()){
//         var response = new ResponseRepository<GetItemDTO>();

//         var item = await context.Items.Include(spec => spec.ItemSpecifications).FirstOrDefaultAsync(item => item.Id == addSpecificationItemDTO.ItemId);
//         if(item is null) ResponseResult($"Item {FailMessage}", response);
        
//         var spec = await context.Specifications.FirstOrDefaultAsync(spec => spec.Id == addSpecificationItemDTO.SpecificationId);
//         if(spec is null)ResponseResult($"Specification {FailMessage}", response);

//         var itemSpecifications = new SpecificationItem{
//             ItemId = item!.Id,
//             SpecificationId = spec!.Id,
//             Value = addSpecificationItemDTO.Value
//         };

//         // var itemSpecs = _mapper.Map<GetSpecificationDTO>(itemSpecifications);

//         var itemSpecs = new GetSpecificationDTO{
//             Spec = spec.Spec,
//             Value = addSpecificationItemDTO.Value
//         };

//         item.ItemSpecifications!.Add(itemSpecifications);
//         await context.SaveChangesAsync();
//         var final = _mapper.Map<GetItemDTO>(item);
//         final.ItemSpecification = itemSpecs;
//         response.Data = final;
//         // response.Data = _mapper.Map<GetItemDTO>(item);
//         return response;
//          }
//     }

//     private ResponseRepository<GetItemDTO> ResponseResult(string message, ResponseRepository<GetItemDTO> response){
//         response.Success = false;
//         response.Message = message;
//         return response;
//     }

//     public async Task<ResponseRepository<GetItemByIdDTO>> GetItemById(Guid id)
//     {
//          using(var context = _context.CreateDbContext()){
//         var response = new ResponseRepository<GetItemByIdDTO>();
//         // var list = new List<Task<GetSpecificationDTO>>();
//         var item = await context.Items.Include(specs => specs.ItemSpecifications).FirstOrDefaultAsync(item => item.Id == id);
//         // var itemSpecs = await _context.SpecificationItems.Where(item => item.ItemId == id).ToListAsync();
//         var list = item.ItemSpecifications.Select(async p => new GetSpecificationDTO{
//             Spec = await GetValue(p.SpecificationId),
//             Value = p.Value
//         }).ToList();
//         var itemById = _mapper.Map<GetItemByIdDTO>(item); //make mapping
//         // var test = await GetValue(2);
//         // var test = (await list).ToList();
//         itemById.ItemSpecifications = [.. (await Task.WhenAll(list))];
//         response.Data = itemById;
//         return response;
//          }
//     }
    
//     private async Task<string> GetValue(int id){
//          using(var context = _context.CreateDbContext()){
//         var specs = await context.Specifications.FirstOrDefaultAsync(p=>p.Id == id);
//         return specs!.Spec;}
//     }
//     // private async Task<string> GetValue(int id, Guid itemId){
//     //     var values = await _context.SpecificationItems.FirstOrDefaultAsync(p=>p.ItemId == itemId && p.SpecificationId == id);
//     //     return values!.Value;
//     // }
// }
