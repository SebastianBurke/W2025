using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using HIAAA.DAL.Interfaces;
using HIAAA.DAL.Repositories;
using Microsoft.Extensions.FileProviders;
using HIAAA.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HIAAA.Areas.Identity.Data;
using HIAAA.DAL;
using HIAAA.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HIA3Connection") ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<HIAAAUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

// Add services to the container
builder.Services.AddScoped<IRole, RoleRepository>();
builder.Services.AddScoped<IApp, AppRepository>();
builder.Services.AddScoped<IAppAdminRepository, AppAdminRepository>();

builder.Services.AddControllersWithViews();

builder.Services.AddNotyf(config=> { config.DurationInSeconds = 10;config.IsDismissable = true;config.Position = NotyfPosition.BottomRight; });
builder.Services.AddHttpClient<IRole, RoleRepository>(options => options.BaseAddress = new Uri("http://localhost:5264"));
builder.Services.AddHttpClient<IApp, AppRepository>(options => options.BaseAddress = new Uri("http://localhost:5264"));
builder.Services.AddHttpClient<IAppAdminRepository, AppAdminRepository>(options => options.BaseAddress = new Uri("http://localhost:5264"));
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    
    // Skip authentication for static files
    options.Events.OnRedirectToLogin = context =>
    {
        // Check if the request is for static files
        var path = context.Request.Path;
        if (path.StartsWithSegments("/lib") || 
            path.StartsWithSegments("/js") || 
            path.StartsWithSegments("/css") || 
            path.StartsWithSegments("/node_modules") ||
            path.StartsWithSegments("/_content"))
        {
            context.Response.StatusCode = 200;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
// builder.Services.AddHttpClient();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
    RequestPath = "/node_modules"
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<HIAAAUser>>();

    // Ensure ADMIN role exists
    if (!await roleManager.RoleExistsAsync("ADMIN"))
    {
        await roleManager.CreateAsync(new IdentityRole("ADMIN"));
    }
    // Ensure APPADMIN role exists
    if (!await roleManager.RoleExistsAsync("APPADMIN"))
    {
        await roleManager.CreateAsync(new IdentityRole("APPADMIN"));
    }
    // Ensure the user 'Richard Chan' with ADMIN role exists
    var adminEmail = "rchan@cegep-heritage.qc.ca";
    var adminPassword = "Chan123!";
    var existingUser = await userManager.FindByEmailAsync(adminEmail);

    if (existingUser == null)
    {
        var adminUser = new HIAAAUser
        {
            UserName = adminEmail,
            Email = adminEmail
        };

        var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createUserResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "ADMIN");
        }
        else
        {
            throw new Exception($"Failed to create user: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
        }
    }
    else
    {
        // Ensure the user is assigned to the ADMIN role
        if (!await userManager.IsInRoleAsync(existingUser, "ADMIN"))
        {
            await userManager.AddToRoleAsync(existingUser, "ADMIN");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapRazorPages();

app.MapControllers();

app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();