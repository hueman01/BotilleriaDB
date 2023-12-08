using BotilleriaDB.Helper;
using BotilleriaDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BotilleriaDB.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ClienteController : Controller
    {
        private BotilleriaDbContext db = new();
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cliente cli)
        {
            ViewBag.ClienteId = new SelectList(db.Clientes, "ClienteId");
            if(ModelState.IsValid)
            {
                var p=db.Clientes.Find(cli.ClienteId);
                if(p != null)
                {
                    ModelState.AddModelError("ClienteId", "ya esta registrado");
                    return View();
                }
                db.Clientes.Add(cli);
                db.SaveChanges();
            }
            return View();
        }
        public async Task <IActionResult> Index(string buscar, string filtro, int? numPag)
        {
            if (buscar == null)
                buscar = filtro;
            else
                numPag = 1;
            ViewData["filtro"] = buscar;
            var cli = from c in db.Clientes select c;
            if (!string.IsNullOrEmpty(buscar))
            {
                cli=cli.Where(x=>x.NombreCliente.ToLower().Contains(buscar.ToLower()));
            }
            int tamPag = 2;
            return View(await PaginatedList<Cliente>.CreateAsync(cli, numPag ?? 1, tamPag));
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {

                var cat = db.Clientes.Find(id);
                if (cat != null)
                {
                    return View(cat);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var cat = db.Clientes.FirstOrDefault(
                    x => x.ClienteId != cliente.ClienteId &&
                    x.NombreCliente == cliente.NombreCliente &&
                    x.Direccion==cliente.Direccion &&
                    x.Telefono==cliente.Telefono);
                if (cat != null)
                {
                    ModelState.AddModelError("NombreCliente", "La categoría esta registrada");
                }
                else
                {
                    db.Clientes.Update(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(cliente);
        }
        public IActionResult Delete(int id)
        {
            var cat = db.Clientes.Find(id);
            if (cat != null)
            {
                
                db.Clientes.Remove(cat);
                db.SaveChanges();

                return Json("ok");
            }
            return RedirectToAction("Index");
        }
    }

}

