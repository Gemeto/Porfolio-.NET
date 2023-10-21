using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Models.Context;
using WebApplicationTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<PortafolioContext>(
    options => options.UseMySQL(
        builder.Configuration.GetConnectionString("MySql")??""));

builder.Services.AddAuthentication("MyAuthScheme")
    .AddCookie("MyAuthScheme", options => {
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
        options.AccessDeniedPath = "/Error";
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Users/Signin");
    options.Conventions.AllowAnonymousToPage("/Users/Login");
    options.Conventions.AllowAnonymousToPage("/Porfolios/GridView");
    options.Conventions.AddPageRoute("/Porfolios/GridView", "/");
});

builder.Services.AddSingleton<EncryptSHA256>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.Run();
