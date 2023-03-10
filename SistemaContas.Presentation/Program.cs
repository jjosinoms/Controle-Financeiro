using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Politica de autenticação do projeto

//Permissao para gravação de cookies
builder.Services.Configure<CookiePolicyOptions>
    (options => { options.MinimumSameSitePolicy = SameSiteMode.None; });

//tipo de identificação de usuarios (autenticacao)
builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    //pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
