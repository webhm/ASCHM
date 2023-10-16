using ASCHM.Context;
using ASCHM.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.AddSession(options =>
{
    // Establecer el tiempo de expiración de la sesión después de detectar inactividad (10 minutos en este caso)
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    // Establecer el tiempo máximo de expiración de la sesión (45 minutos en este caso)
    options.Cookie.MaxAge = TimeSpan.FromMinutes(45);
});

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IHRMDBContext, HRMDBContext>();
builder.Services.AddScoped<IOperation, Operation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) 
{   
    app.UseExceptionHandler("/Home/Error");  
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.Run();
