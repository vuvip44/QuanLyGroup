using Microsoft.EntityFrameworkCore;
using QuanLy.Data;
using QuanLy.Infrastructure.Repository;
using QuanLy.Infrastructure.Repository.IRepository;
using QuanLy.Infrastructure.Service;
using QuanLy.Infrastructure.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Razor Pages (nếu cần, ví dụ để hỗ trợ các trang tĩnh hoặc Identity)
builder.Services.AddRazorPages();

// Add connection to database
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add dependency injection for repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IUserGroupRepository, UserGroupRepository>();

// Add dependency injection for services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IUserGroupService, UserGroupService>();

// Configure logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventSourceLogger();
    // Có thể thêm logging vào file nếu cần
    // logging.AddFile("Logs/app-{Date}.log");
});

// Add support for static files (CSS, JS, images, etc.)


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable serving static files (CSS, JS, images, etc.)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages (nếu có)
app.MapRazorPages();

// Remove .WithStaticAssets() and MapStaticAssets() as they are not standard
// If you are using a custom middleware for static assets, you can reimplement it here

app.Run();