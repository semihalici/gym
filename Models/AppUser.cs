using Microsoft.AspNetCore.Identity;

namespace Gym_odev.Models;

public class AppUser : IdentityUser
{
    public string? AdSoyad { get; set; }
    public DateTime KayitTarihi { get; set; } = DateTime.Now;
}