using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineShop_Api.Data;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// eklendi

builder.WebHost.UseUrls("http://*:2021");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // İzin vermek istediğiniz kökenleri buraya ekleyin
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
 
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    //options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();
 
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<DataContext>();

//
var app = builder.Build();
  
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


//eklendi
app.UseCors("AllowAllOrigins"); // ConfigureServices'da tanımladığınız CORS politikasının adını buraya ekleyin


app.MapIdentityApi<IdentityUser>();

//
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
