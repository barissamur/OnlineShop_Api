using EventBus;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineShop_Api.Data;
using OnlineShop_Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// eklendi
builder.WebHost.UseUrls("http://*:5005");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // Ýzin vermek istediðiniz kökenleri buraya ekleyin
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

//event bus ayarlarý 
builder.Services.AddMassTransit(x =>
{
    //x.AddConsumer<TestMessageConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        //cfg.ReceiveEndpoint("test-message-queue", e =>
        //{
        //    e.ConfigureConsumer<TestMessageConsumer>(context);
        //});
    });
});


builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
builder.Services.AddSingleton<IEventBus, AzureServiceBusEventBus>();

//ocelot ayarlarý
builder.Services.ConfigureConsul(builder.Configuration);
 

// log ayarlarý
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


//eklendi
app.UseCors("AllowAllOrigins"); // ConfigureServices'da tanýmladýðýnýz CORS politikasýnýn adýný buraya ekleyin


app.MapIdentityApi<IdentityUser>();

// consul register
// Uygulama ömrünü yönetmek için IHostApplicationLifetime alýn
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

// Consul ile kayýt iþlemi
app.RegisterWithConsul(lifetime, builder.Configuration);

//

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
