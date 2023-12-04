using EGG_Haunolding_Magement_System.Class;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IDataStore>(ctx =>
{
    return new StaticDataStore(new List<DataItem>
    {
        new DataItem("Oberndorfer", new DateTime(2023, 12, 12, 12, 0, 0), 50, 52),
        new DataItem("Bell", new DateTime(2023, 12, 12, 12, 0, 0), -12, -8),
        new DataItem("Roider", new DateTime(2023, 12, 12, 12, 0, 0), 0, 3),
        new DataItem("Oberndorfer", new DateTime(2023, 12, 12, 12, 0, 30), 49, 52),
        new DataItem("Bell", new DateTime(2023, 12, 12, 12, 0, 30), -7, -8),
        new DataItem("Roider", new DateTime(2023, 12, 12, 12, 0, 30), 2, 3),
        new DataItem("Oberndorfer", new DateTime(2023, 12, 12, 12, 1, 0), 54, 52),
        new DataItem("Bell", new DateTime(2023, 12, 12, 12, 1, 0), -8, -8),
        new DataItem("Roider", new DateTime(2023, 12, 12, 12, 1, 0), 5, 3)
    });
});


var app = builder.Build();

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
