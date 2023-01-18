using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Pronia.DAL;
using Pronia.Models;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ClientController : Controller
    {
        readonly AppDbContext _context;
        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
                return View(_context.Clients.ToList());
            

        }



        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string FullName, string Description, string Role, string Image)
        {
            
                if (!ModelState.IsValid) return View();
                _context.Clients.Add(new Client { FullName = FullName, Description = Description, Role = Role, Image = Image });
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            

        }


        public IActionResult Delete(int id)
        {
            
                Client client = _context.Clients.Find(id);
                if (client is null)
                {
                    return NotFound();
                }
                _context.Clients.Remove(client);
                _context.SaveChanges();

            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            
                if (id is null) return BadRequest();
                Client client = _context.Clients.Find(id);
                if (client is null) return NotFound();
                return View(client);
            
        }
        [HttpPost]
        public IActionResult Update(int? id, Client client)
        { 
            
                if (!ModelState.IsValid) return View();
                if (id is null || id!=client.Id) return BadRequest();
                Client existClient = _context.Clients.Find(id);
                if(client is null ) return NotFound();
                existClient.FullName = client.FullName;
                existClient.Description= client.Description;
                existClient.Role = client.Role;
                existClient.Image= client.Image;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }
    }
}
