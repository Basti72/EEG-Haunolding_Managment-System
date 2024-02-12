using EGG_Haunolding_Management_System.Class;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<MQTTBackroundService>();
//builder.Services.AddHostedService<DBBackroundService>();

string connectionString = builder.Configuration.GetConnectionString("Db");
builder.Services.AddTransient<IDataStore>(ctx => { return new MySQLStore(connectionString); });
builder.Services.AddTransient<IUserStore>(ctx => { return new MySQLStore(connectionString); });
builder.Services.AddTransient<IMQTTCom>(ctx => { return new MySQLStore(connectionString); });
builder.Services.AddTransient<ITopicStore>(ctx => { return new MySQLStore(connectionString); });

builder.Services.AddTransient<AuthenticationLogic>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
