
using EventBus;
using MassTransit;
using Shop.Web.Messages;
using Shop.Web.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

// SignalR'� ekleyin
builder.Services.AddSignalR();

//event bus ayarlar� 
builder.Services.AddMassTransit(x =>
{
    // consumerlar� ekle
    x.AddConsumer<StockMessageConsumer>();

    // publish s�n�f�ndan kal�t�m almayan bir consumer olup olmad���n� kontrol et
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        // hangi kuyru�a hangi s�n�f consumer olacak
        cfg.ReceiveEndpoint("result_stock", e =>
        {
            e.UseRawJsonSerializer(); // d��ardan gelen json format�n� serialize et
            e.ConfigureConsumer<StockMessageConsumer>(context);
        });
    });
});

builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
builder.Services.AddSingleton<IEventBus, AzureServiceBusEventBus>();

// log ayarlar�
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR hub'�n� yap�land�r�n
app.MapHub<StockHub>("/stockHub"); 

app.Run();
