using api.Controller;
using api.DTO.BrandDTO;
using api.DTO.GenericDTO;
using api.DTO.ItemDTO;
using api.DTO.SpecificationItemDTO;
using api.Helpers;
using api.Interfaces;
using api.Repository;
using apiTest.DataGenerator;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace apiTest.Controllers;

public class ItemControllerTest
{
    private readonly IItemRepository _itemRepository;

    public ItemControllerTest()
    {
        _itemRepository = A.Fake<IItemRepository>();
    }

    [Fact]
    public async Task AddItem_ReturnSuccess()
    {
        var controller = new ItemController(_itemRepository);
        var item = new AddItemDTO{
            ItemName = "item name",
            Description = "item description",
            Quantity = 0,
            Price = 0,
            Score = 0,
            CategoryId = 1,
            BrandId = 1,
            ImageUrl = "image url"
        };
        var result = await controller.AddItem(item);

        var actionResult = result.Result;
        
        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [InlineData("f526918f-0c37-4d6f-91ef-db83dc545d28")]
    [InlineData("1ef189d5-4287-4674-815b-ded8e3db5338")]
    [InlineData("47606be8-1b83-44ef-8c8d-a36adcc67862")]
    public async Task GetItemById_ReturnSuccess(Guid id){
        var controller = new ItemController(_itemRepository);
        var result = await controller.GetitemById(id);

        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [InlineData("f526918f-0c37-4d6f-91ef-db83dc545d28")]
    [InlineData("1ef189d5-4287-4674-815b-ded8e3db5338")]
    [InlineData("47606be8-1b83-44ef-8c8d-a36adcc67862")]
    public async Task GetItemById_ReturnNotFound(Guid id){
        var controller = new ItemController(_itemRepository);
        A.CallTo(() => _itemRepository.GetItemById(id)).Returns(new ResponseRepository<GetItemByIdDTO>{Success = false, Message = "Item not found.", Data = new GetItemByIdDTO()});
        var result = await controller.GetitemById(id);

        var actionResult = result.Result;

        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public async Task GetAllItems_ReturnSuccess(QueryHelper name){
        var controller = new ItemController(_itemRepository);
        var result = await controller.GetAllItems(name);

        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task UpdateItem_ReturnSuccess(){
        var controller = new ItemController(_itemRepository);
        var item = new UpdateItemDTO{
            ItemName = "item name",
            Description = "item description",
            Quantity = 0,
            Price = 0,
            CategoryId = 1,
            BrandId = 1,
            ImageUrl = "image url"
        };
        Guid itemId = new Guid();
        var result = await controller.UpdateItem(item,itemId);

        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task UpdateItem_ReturnNotFound(){
        var controller = new ItemController(_itemRepository);
        var item = new UpdateItemDTO{
            ItemName = "item name",
            Description = "item description",
            Quantity = 0,
            Price = 0,
            CategoryId = 1,
            BrandId = 1,
            ImageUrl = "image url"
        };
        Guid itemId = new Guid();
        A.CallTo(() => _itemRepository.UpdateItem(item,itemId)).Returns(new ResponseRepository<GetItemByIdDTO>{Success = false, Message = "Item not found", Data = new GetItemByIdDTO()});
        var result = await controller.UpdateItem(item,itemId);

        var actionResult = result.Result;

        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    [Theory]
    [InlineData("f526918f-0c37-4d6f-91ef-db83dc545d28")]
    [InlineData("1ef189d5-4287-4674-815b-ded8e3db5338")]
    [InlineData("47606be8-1b83-44ef-8c8d-a36adcc67862")]
    public async Task DeleteItem_ReturnSuccess(Guid id){
        var controller = new ItemController(_itemRepository);
        // A.CallTo(() => _itemRepository.GetItemById(id)).Returns(new ResponseRepository<GetItemByIdDTO>{Success = false, Message = "Item not found.", Data = new GetItemByIdDTO()});
        var result = await controller.DeleteItem(id);

        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [InlineData("f526918f-0c37-4d6f-91ef-db83dc545d28")]
    [InlineData("1ef189d5-4287-4674-815b-ded8e3db5338")]
    [InlineData("47606be8-1b83-44ef-8c8d-a36adcc67862")]
    public async Task DeleteItem_ReturnNotFound(Guid id){
        var controller = new ItemController(_itemRepository);
        A.CallTo(() => _itemRepository.DeleteItem(id)).Returns(new ResponseRepository<string>{Success = false, Message = "Item not found.", Data = ""});
        var result = await controller.DeleteItem(id);

        var actionResult = result.Result;
        // var test = result.Result as NotFoundObjectResult;
        // var tt = test.Value as ResponseRepository<string>;

        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    [Fact]
    public async Task GetItemsOnSale_ReturnSuccess(){
        var controller = new ItemController(_itemRepository);
        var result = await controller.GetItemsOnSale();

        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task AddSpecsToItem_ReturnSuccess(){
        var controller = new ItemController(_itemRepository);
        var addSpecs = new List<AddSpecficationsItemDTO>{
            new AddSpecficationsItemDTO{Value = "value1", SpecificationId = 2},
            new AddSpecficationsItemDTO{Value = "value2", SpecificationId = 3},
            new AddSpecficationsItemDTO{Value = "value3", SpecificationId = 4},
        };
        var itemId = new Guid();
        var result = await controller.AddSpecsToItem(addSpecs, itemId);

        var actionResult = result.Result;
        
        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Fact]
    public async Task AddSpecsToItem_ReturnNotFound(){
        var controller = new ItemController(_itemRepository);
        var addSpecs = new List<AddSpecficationsItemDTO>{
            new AddSpecficationsItemDTO{Value = "value1", SpecificationId = 2},
            new AddSpecficationsItemDTO{Value = "value2", SpecificationId = 3},
            new AddSpecficationsItemDTO{Value = "value3", SpecificationId = 4},
        };
        var itemId = new Guid();
        A.CallTo(() => _itemRepository.AddSpecsToItem(addSpecs, itemId)).Returns(new ResponseRepository<GetItemDTO>{Success = false, Message = "Not found.", Data = new GetItemDTO()});
        var result = await controller.AddSpecsToItem(addSpecs, itemId);

        var actionResult = result.Result;
        // var test = result.Result as NotFoundObjectResult;
        // var tt = test.Value as ResponseRepository<string>;
        // actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public async Task GetItemsByCategory_ReturnSuccess(QueryHelper name){
        var controller = new ItemController(_itemRepository);
        var id = 1;
        var result = await controller.GetItemsByCategory(id, name);

        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public async Task GetItemsByCategory_ReturnNotFound(QueryHelper name){
        var controller = new ItemController(_itemRepository);
        var id = 1;
        A.CallTo(() => _itemRepository.GetItemsByCategory(id,name)).Returns(new ResponseRepositoryWithCount<GetItemsByCategoryDTO>{Success = false, Message = "Category not found", Data = new GetItemsByCategoryDTO(Category:"",Brands:new List<GetBrandDTO>(), Items:new List<TempDTO>())});
        var result = await controller.GetItemsByCategory(id, name);

        var actionResult = result.Result;

        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public async Task GetItemsByBrand_ReturnSuccess(QueryHelper name){
        var controller = new ItemController(_itemRepository);
        var id = 1;
        var result = await controller.GetItemsByBrand(id, name);

        var actionResult = result.Result;

        actionResult.Should().NotBe(null);
        actionResult.Should().BeOfType(typeof(OkObjectResult));
    }
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public async Task GetItemsByBrand_ReturnNotFound(QueryHelper name){
        var controller = new ItemController(_itemRepository);
        var id = 1;
        A.CallTo(() => _itemRepository.GetItemsByBrand(id,name)).Returns(new ResponseRepositoryWithCount<GetItemsByBrandDTO>{Success = false, Message = "Category not found", Data = new GetItemsByBrandDTO(Brand:"",Categories:new List<GenericGetDTO>(), Items:new List<GetItemsOnSaleDTO>())});
        var result = await controller.GetItemsByBrand(id, name);

        var actionResult = result.Result;

        actionResult.Should().BeOfType(typeof(NotFoundObjectResult));
    }
}