using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext using SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Index";
});

// Configure Identity
builder.Services.AddDefaultIdentity<CustomerApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    // Configure other options as needed
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Optional: Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings, lockout settings, etc.
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios.
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication is used
app.UseAuthorization();

app.MapRazorPages();


// Ensure database is created and migrations are applied:
async Task InitializeApplicationAsync(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync(); // Await the migration

        // Optionally seed the database (ensure SeedData class is appropriately defined)
        SeedData.Initialize(services);
    }
}

// Call the initialize method and run the application
await InitializeApplicationAsync(app);

app.Run();

