using System.Text;
using api.DataContext;
using api.Dependencies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme{
        Description = """Standard Authorization header using Bearer scheme. Example: "bearer {token}" """,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddControllers();
builder.Services.AddDbContextFactory<DatabaseContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<DatabaseContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddServices();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!))
    };
    // options.Events = new JwtBearerEvents{
    //     OnMessageReceived = ctx => {
    //         ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
    //         if(!string.IsNullOrEmpty(accessToken)) ctx.Token = accessToken;
    //         return Task.CompletedTask;
    //     }
    // };
});

builder.Services.AddCors(options => options.AddPolicy("corsPolicy",build => build.WithOrigins().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

