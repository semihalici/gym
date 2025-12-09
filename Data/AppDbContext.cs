using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gym_odev.Models; // Models klasörünü görsün diye

namespace Gym_odev.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Egitmen> Egitmenler { get; set; }
    public DbSet<Hizmet> Hizmetler { get; set; }
    public DbSet<Randevu> Randevular { get; set; }
}