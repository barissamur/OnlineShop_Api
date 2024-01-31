using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ocelot  

// Ocelot yapýlandýrmasýný ekleyin
builder.Configuration.AddJsonFile("Configurations/ocelot.json");
builder.Services.AddOcelot(builder.Configuration).AddConsul();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

builder.Logging.AddConsole();

//ocelot
await app.UseOcelot();

app.MapControllers();

app.Run();
