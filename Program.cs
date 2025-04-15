// File: Program.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PhanVuBaoMinh.Data;
using PhanVuBaoMinh.Repositories;
using PhanVuBaoMinh.Services;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole(); // Ghi log ra console
    logging.AddDebug();   // Ghi log ra debug output
});

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout 30 phút
    options.Cookie.HttpOnly = true; // Cookie chỉ truy cập qua HTTP
    options.Cookie.IsEssential = true; // Cookie là cần thiết
});



// Kiểm tra cấu hình (tùy chọn, để debug)
var emailSettings = builder.Configuration.GetSection("EmailSettings");
if (!emailSettings.Exists())
{
    throw new InvalidOperationException("Phần EmailSettings không tồn tại trong appsettings.json.");
}
else
{
    Console.WriteLine("EmailSettings đã được tìm thấy trong appsettings.json.");
}


// Lấy thông tin xác thực từ appsettings.json cho Google và Facebook
var googleAuth = builder.Configuration.GetSection("Authentication:Google");
var fbAuth = builder.Configuration.GetSection("Authentication:Facebook");

// Kết nối cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Identity với ApplicationUser, không yêu cầu xác nhận email
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Thêm hỗ trợ Razor Pages và MVC Controllers
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();




// Thêm vào phần cấu hình dịch vụ (sau builder.Services.AddControllersWithViews())
builder.Services.AddTransient<IEmailSender, EmailSender>();




// Đăng ký các Repository để sử dụng Dependency Injection
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();



// Cấu hình xác thực ngoài với Google và Facebook
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "GOOGLE_CLIENT_ID"; // Thay bằng Client ID thật của Google
        options.ClientSecret = "GOOGLE_CLIENT_SECRET"; // Thay bằng Client Secret thật của Google
    })
    .AddFacebook(options =>
    {
        options.AppId = "FACEBOOK_APP_ID"; // Thay bằng App ID thật của Facebook
        options.AppSecret = "FACEBOOK_APP_SECRET"; // Thay bằng App Secret thật của Facebook
    });

var app = builder.Build();


// Tạo vai trò "Admin" nếu chưa tồn tại
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Gán vai trò "Admin" cho tài khoản minhphankik@gmail.com
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var user = await userManager.FindByEmailAsync("minhphankik@gmail.com");
    if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
}


// Cấu hình middleware cho ứng dụng
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Thêm middleware Session trước Authentication
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Cấu hình routing cho Admin Area và route mặc định
app.MapControllerRoute(
    name: "admin_area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();