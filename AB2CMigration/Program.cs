using ConnectionB2C;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

GraphProvider.Graph = await MicrosoftGraphProvider.Build(
             builder.Configuration["GraphB2C:ClientId"],
             builder.Configuration["GraphB2C:TenantId"],
             builder.Configuration["GraphB2C:CLientSecret"]
             );

GraphProvider.Cosmos = new(
             builder.Configuration["Cosmos:ConnectionString"],
             builder.Configuration["Cosmos:DatabaseId"],
             builder.Configuration["Cosmos:ContainerId"]);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
