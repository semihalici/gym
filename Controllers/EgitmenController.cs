using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Gym_odev.Data;
using Gym_odev.Models;
using System.Linq;

namespace Gym_odev.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Adminler girebilir!
    public class EgitmenController : Controller
    {
        private readonly AppDbContext _context;

        public EgitmenController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Listeleme Sayfası
        public IActionResult Index()
        {
            var egitmenler = _context.Egitmenler.ToList();
            return View(egitmenler);
        }

        // 2. Ekleme Sayfası (Formu Gösterir)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. Ekleme İşlemi (Veriyi Kaydeder)
        [HttpPost]
        public IActionResult Create(Egitmen egitmen)
        {
            if (ModelState.IsValid)
            {
                _context.Egitmenler.Add(egitmen);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(egitmen);
        }

        // 4. Silme İşlemi
        public IActionResult Delete(int id)
        {
            var egitmen = _context.Egitmenler.Find(id);
            if (egitmen != null)
            {
                _context.Egitmenler.Remove(egitmen);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}