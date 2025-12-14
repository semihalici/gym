using Gym_odev.Data;
using Gym_odev.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity (Üyelik) Ayarları - Şifre kuralları gevşek (sau şifresi için)
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3; 
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- BAŞLANGIÇ: Otomatik Veri Ekleme (Seed Data) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // A) Roller yoksa oluştur
        if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("Member")) await roleManager.CreateAsync(new IdentityRole("Member"));

        // B) Admin kullanıcısı yoksa oluştur
        var adminEmail = "g211210001@sakarya.edu.tr"; // Numaran
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new AppUser 
            { 
                UserName = adminEmail, 
                Email = adminEmail, 
                AdSoyad = "Sistem Yöneticisi",
                EmailConfirmed = true 
            };
            await userManager.CreateAsync(admin, "sau");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // C) Hizmetler (Dersler) yoksa oluştur (YENİ EKLENEN KISIM)
        if (!context.Hizmetler.Any())
        {
            context.Hizmetler.AddRange(
                new Hizmet { Ad = "Fitness", Ucret = 200 },
                new Hizmet { Ad = "Pilates", Ucret = 250 },
                new Hizmet { Ad = "Yoga", Ucret = 300 },
                new Hizmet { Ad = "Yüzme", Ucret = 150 },
                new Hizmet { Ad = "Boks", Ucret = 400 },
                new Hizmet { Ad = "Zumba", Ucret = 180 }
            );
            context.SaveChanges(); // Veritabanına kaydet
        }

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veri ekleme sırasında bir hata oluştu.!!!");
    }
}
// --- BİTİŞ ---

// Hata Ayıklama Modları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Giriş yapma kontrolü
app.UseAuthorization();  // Yetki kontrolü

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();