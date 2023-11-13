using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PorfolioWeb.Services;
using PorfolioWeb.Services.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PortafolioContextService>(
    options => options.UseMySQL(
        builder.Configuration.GetConnectionString("MySql")??""));

builder.Services.AddAuthentication("MyAuthScheme")
    .AddCookie("MyAuthScheme", options => {
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
        options.AccessDeniedPath = "/Error";
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Users/Signin");
    options.Conventions.AllowAnonymousToPage("/Users/Login");
    options.Conventions.AllowAnonymousToPage("/Porfolios/GridView");
});

builder.Services.AddSingleton<EncryptSHA256Service>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.Run();
