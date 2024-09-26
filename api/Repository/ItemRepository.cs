using api.DataContext;
using api.DTO.BrandDTO;
using api.DTO.GenericDTO;
using api.DTO.ItemDTO;
using api.DTO.SpecificationDTO;
using api.DTO.SpecificationItemDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ItemRepository : IItemRepository
{
    private readonly DatabaseContext _context;
    private const string FailMessage = "not found";

    public ItemRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ResponseRepository<GetItemDTO>> AddItem(AddItemDTO addItemDTO)
    {
       var response = new ResponseRepository<GetItemDTO>();
       var item = addItemDTO.ToItem();
    
       _context.Items.Add(item);
       await _context.SaveChangesAsync();
       response.Data = (GetItemDTO)item;

       return response;
    }
    public async Task<ResponseRepository<GetItemByIdDTO>> GetItemById(Guid id)
    {
        var response = new ResponseRepository<GetItemByIdDTO>();
        var failResponse = new FailResponse();
        // var item = await _context.Items.Include(c => c.Category).Include(b => b.Brand).Include(specs => specs.ItemSpecifications).Include(rating => rating.Ratings!).FirstOrDefaultAsync(item => item.Id == id);
        var item = await _context.Items.Include(c => c.Category).Include(b => b.Brand).Include(specs => specs.ItemSpecifications).Include(rating => rating.Ratings!).ThenInclude(user => user.User).FirstOrDefaultAsync(item => item.Id == id);
        // var item = await _context.Items.Include(c => c.Category).Include(b => b.Brand).Include(specs => specs.ItemSpecifications).Include(rating => rating.Ratings!).ThenInclude(user => user.User).Select(p => new GetItemByIdDTO{
        //     ItemBrand = new GetBrandDTO{Id = p.Brand!.Id, BrandName = p.Brand.BrandName},
        //     ItemCategory = new GetItemCategoryDTO{Id = p.Category!.Id, CategoryName = p.Category.CategoryName},
        //     ItemSpecifications = p.ItemSpecifications!.Select(spec => new GetSpecificationDTO{Spec = string.Empty, Value = spec.Value}).ToList(),
        //     Ratings = p.Ratings!.Select(rating => new GetItemRatingDTO{Rate = rating.Rate, Comment = rating.Comment, UserName = rating.User!.UserName}).ToList(),
        //     AverageRating = (double)p.Score!,
        //     Price = p.Price,
        //     Quantity = p.Quantity,
        //     Description = p.Description,
        //     ItemName = p.ItemName,
        //     Id = p.Id
        // }).FirstOrDefaultAsync(item => item.Id == id);
        if(item is null) {
            failResponse.GenerateFailResponse(response,"Item not found");
            return response;
        }

        var specs = await _context.Specifications.Select(p=>p).ToListAsync();
        if(specs is null) {failResponse.GenerateFailResponse(response,"Specs not found");
            return response;}
   
        var list = item.ItemSpecifications!.Select(p => new GetSpecificationDTO{
            Id = p.SpecificationId,
            Spec = specs!.Find(item => item.Id == p.SpecificationId)!.Spec,
            Value = p.Value
        }).ToList();
        
        var itemById = (GetItemByIdDTO)item!;
      
        itemById.ItemSpecifications = list;
       
        response.Data = itemById;
        return response;
    }

    public async Task<ResponseRepositoryWithCount<List<GetItemsDTO>>> GetAllItems(QueryHelper query)
    {
        var response = new ResponseRepositoryWithCount<List<GetItemsDTO>>();
        var allItems = _context.Items.Include(b => b.Brand).Include(c => c.Category).AsQueryable();
        if(!string.IsNullOrWhiteSpace(query.name)){
            allItems = allItems.Where(item => item.ItemName.Contains(query.name));
        }
        var items = allItems.Select(items => (GetItemsDTO)items);
        var itemResponse = await allItems.ToListAsync();
        response.Count = itemResponse .Count;
        response.Data = [.. items];
        return response;
    }

    public async Task<ResponseRepository<GetItemByIdDTO>> UpdateItem(UpdateItemDTO updateItemDTO, Guid id)
    {
        var response = new ResponseRepository<GetItemByIdDTO>();
        var failResponse = new FailResponse();
        var existingItem = await _context.Items.Include(category => category.Category).Include(brand => brand.Brand).Include(rating => rating.Ratings).FirstOrDefaultAsync(item => item.Id == id);
        if(existingItem is null) {failResponse.GenerateFailResponse(response,"Item not found");
        return response;};
        updateItemDTO.ToItem(existingItem!);
    
        await _context.SaveChangesAsync();
        var item = (GetItemByIdDTO)existingItem!;

        response.Data = item;
        return response;
    }

    public async Task<ResponseRepository<string>> DeleteItem(Guid id)
    {
        var response = new ResponseRepository<string>();
        var failResponse = new FailResponse();
        var deleteItem = await _context.Items.FirstOrDefaultAsync(item => item.Id == id);
        if(deleteItem is null) {
            failResponse.GenerateFailResponse(response, "Item not found");
            return response;
            };

        _context.Items.Remove(deleteItem!);
        await _context.SaveChangesAsync();
        response.Data = "";
        return response;
    }

    public async Task<ResponseRepositoryWithCount<List<GetItemsOnSaleDTO>>> GetItemsOnSale()
    {
        var response = new ResponseRepositoryWithCount<List<GetItemsOnSaleDTO>>();
        var itemsOnSale = await _context.Items.Where(item => item.IsOnSale).Select(p => new GetItemsOnSaleDTO{
            Id = p.Id,
            ItemName = p.ItemName,
            Price = p.Price,
            Score = (double)p.Score!,
            ImageUrl = p.ImageUrl
        }).ToListAsync();
        response.Count = itemsOnSale.Count;
        response.Data = itemsOnSale;
        return response;
    }

    public async Task<ResponseRepository<GetItemDTO>> AddSpecsToItem(List<AddSpecficationsItemDTO> addSpecficationsItemDTO, Guid itemId)
    {
        var response = new ResponseRepository<GetItemDTO>();
        var failResponse = new FailResponse();
        var item = await _context.Items.Include(item => item.ItemSpecifications).FirstOrDefaultAsync(p => p.Id == itemId);
        if(item is null) {
            failResponse.GenerateFailResponse(response, "Item not found");
            return response;
        }

        var inputSpecs = addSpecficationsItemDTO.Select(item => item.SpecificationId);
        var specs = await _context.Specifications.Where(spec => inputSpecs.Contains(spec.Id)).ToListAsync();
        
        if(specs.Count == 0) {
            failResponse.GenerateFailResponse(response, "One or more specs not found");
            return response;
        }
        var validInputSpecs = addSpecficationsItemDTO.IntersectBy(specs.Select(spec => spec.Id),input => input.SpecificationId).ToList();
       
        var specificationItemsToAdd = validInputSpecs.Select(spec => new SpecificationItem{
            Value = spec.Value,
            SpecificationId = spec.SpecificationId,
            ItemId = item.Id
        }).ToList();

        // REVISE
        var existingSpecs = await _context.SpecificationItems.Where(spec => spec.ItemId == itemId).ToListAsync();
        if(existingSpecs is not null) {_context.SpecificationItems.RemoveRange(existingSpecs);}
        _context.SpecificationItems.AddRange(specificationItemsToAdd);
      
        await _context.SaveChangesAsync();
        response.Data = (GetItemDTO)item;
        return response;
        
    }

    public async Task<ResponseRepositoryWithCount<GetItemsByCategoryDTO>> GetItemsByCategory(int categoryId, QueryHelper query)
    {
        var response = new ResponseRepositoryWithCount<GetItemsByCategoryDTO>();
        var failResponse = new FailResponse();
        var existingCategory = await _context.Categories.Include(brand=>brand.Brands).FirstOrDefaultAsync(category => category.Id == categoryId);
        if(existingCategory is null) {
            failResponse.GenerateFailResponse(response, "Category not found");
            return response;
        }
        var existingProducts = _context.Items.Include(category => category.Category).Include(brand => brand.Brand).Where(cat => cat.Category!.Id == categoryId).Select(p => new TempDTO{
            Id = p.Id,
            ItemName = p.ItemName,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            Score = (double)p.Score!,
            BrandName = p.Brand!.BrandName
        }).AsQueryable();

        if(!string.IsNullOrWhiteSpace(query.name)){
            existingProducts = existingProducts.Where(brand => brand.BrandName == query.name);
        }

        var queryProducts = await existingProducts.ToListAsync();
        var categoryBrands = existingCategory.Brands!.Select(brand => new GetBrandDTO{
            Id = brand.Id,
            Name = brand.BrandName,
            ImageUrl = brand.ImageUrl!
        }).ToList();
        var products = new GetItemsByCategoryDTO(existingCategory.CategoryName,categoryBrands,queryProducts);
        response.Data = products;

        response.Count = queryProducts.Count;
        return response;
    }

    public async Task<ResponseRepositoryWithCount<GetItemsByBrandDTO>> GetItemsByBrand(int brandId, QueryHelper query)
    {
        var response = new ResponseRepositoryWithCount<GetItemsByBrandDTO>();
        var failResponse = new FailResponse();
        var existingBrand = await _context.Brands.Include(category => category.Categories).FirstOrDefaultAsync(brand => brand.Id == brandId);
        if(existingBrand is null){
            failResponse.GenerateFailResponse(response, "Brand not found");
            return response;
        }

        var existingProducts = _context.Items.Include(brand => brand.Brand).Include(category => category.Category).Where(br => br.Brand!.Id == brandId).Select(p => new GetItemsOnSaleDTO{
           Id = p.Id,
            ItemName = p.ItemName,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            Score = (double)p.Score!,
            CategoryName = p.Category!.CategoryName
    }).AsQueryable();
    if(!string.IsNullOrWhiteSpace(query.name)){
            existingProducts = existingProducts.Where(category => category.CategoryName == query.name);
        }
    var queryProducts = await existingProducts.ToListAsync();
    var brandCategories = existingBrand.Categories!.Select(category => new GenericGetDTO(
        category.Id,
        category.CategoryName
    )).ToList() ?? new List<GenericGetDTO>();

    var products = new GetItemsByBrandDTO(existingBrand.BrandName, brandCategories, queryProducts);
    response.Count = queryProducts.Count;
    response.Data = products;
    return response;
    }
}
