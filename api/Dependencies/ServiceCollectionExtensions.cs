using api.Helpers;
using api.Interfaces;
using api.Repository;

namespace api.Dependencies;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services){
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISpecification, SpecificationRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<IEmailRepository, EmailRespository>();
        services.AddScoped<IPasswordInterface, Password>();
        services.AddScoped<ITokenInterface, Token>();
        services.AddScoped<ICurrentUserInterface, CurrentUser>();
        return services;
    }
}