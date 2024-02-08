
using EventBus;
using MassTransit;
using Shop.Web.Messages;
using Shop.Web.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

// SignalR'ý ekleyin
builder.Services.AddSignalR();

//event bus ayarlarý 
builder.Services.AddMassTransit(x =>
{
    // consumerlarý ekle
    x.AddConsumer<StockMessageConsumer>();

    // publish sýnýfýndan kalýtým almayan bir consumer olup olmadýðýný kontrol et
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        // hangi kuyruða hangi sýnýf consumer olacak
        cfg.ReceiveEndpoint("result_stock", e =>
        {
            e.UseRawJsonSerializer(); // dýþardan gelen json formatýný serialize et
            e.ConfigureConsumer<StockMessageConsumer>(context);
        });
    });
});

builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
builder.Services.AddSingleton<IEventBus, AzureServiceBusEventBus>();

// log ayarlarý
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

// SignalR hub'ýný yapýlandýrýn
app.MapHub<StockHub>("/stockHub"); 

app.Run();
