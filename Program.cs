using CustomerServicesSystem.Data;
using CustomerServicesSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

// Fix ExcelDataReader on non-Windows
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// ── EF Core + Identity ──────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.Password.RequireDigit           = false;
    o.Password.RequiredLength         = 6;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireUppercase       = false;
    o.SignIn.RequireConfirmedAccount  = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath        = "/Account/Login";
    o.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ── Seed admin user ─────────────────────────
using (var scope = app.Services.CreateScope())
{
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var db      = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    if (await userMgr.FindByEmailAsync("admin@css.com") == null)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin@css.com",
            Email    = "admin@css.com",
            FullName = "System Administrator",
            IsActive = true
        };
        await userMgr.CreateAsync(admin, "Admin@123");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
