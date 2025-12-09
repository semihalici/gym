using System.ComponentModel.DataAnnotations;

namespace Gym_odev.Models;

public class Randevu
{
    public int Id { get; set; }

    public string AppUserId { get; set; } // Randevuyu alan üye
    public AppUser? AppUser { get; set; }

    public int EgitmenId { get; set; } // Seçilen hoca
    public Egitmen? Egitmen { get; set; }

    public int HizmetId { get; set; } // Seçilen ders (Yoga vb.)
    public Hizmet? Hizmet { get; set; }

    public DateTime Tarih { get; set; } // Ne zaman?
}