using BotilleriaDB.Helper;
using BotilleriaDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BotilleriaDB.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class DetalleController : Controller
    {
        private BotilleriaDbContext db = new();
        
        public IActionResult Create()
        {
            

            return View();
        }
        
        [HttpPost]
        public IActionResult Create(DetalleVentum deta)
        {

            if (ModelState.IsValid)
            {

                var cat = db.DetalleVenta.FirstOrDefault(
                    c => c.DetalleVentaId == deta.DetalleVentaId);
                if (cat != null)
                {
                    ModelState.AddModelError("DetalleVentaId", "La categoría ya está registrada");
                    return View();
                }

                db.DetalleVenta.Add(deta);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Index(string buscar, string filtro, int? numPag)
        {
            var p = db.DetalleVenta.Include(p => p.VentaNavigation).ToList();
            if (buscar == null)
                buscar = filtro;
            else
                numPag = 1;
            //lleva el texto buscado a la página
            ViewData["filtro"] = buscar;
            var categoria = from c in db.DetalleVenta select c;
            //verifica si no está vacio 
            if (!string.IsNullOrEmpty(buscar))
            {
                categoria = categoria.Where(
                    x => x.DetalleVentaId.ToString().Contains(buscar.ToLower()));
            }
            int tamPag = 2; //poner 20 o 25
            return View(await PaginatedList<DetalleVentum>
                .CreateAsync(categoria, numPag ?? 1, tamPag));
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {

                var cat = db.DetalleVenta.Find(id);
                if (cat != null)
                {
                    return View(cat);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(DetalleVentum detalleVentum)
        {
            if (ModelState.IsValid)
            {
                var cat = db.DetalleVenta.FirstOrDefault(
                    x => x.DetalleVentaId != detalleVentum.DetalleVentaId &&
                    x.Cantidad == detalleVentum.Cantidad);

                if (cat != null)
                {
                    ModelState.AddModelError("NombreCliente", "El Cliente esta registrado");
                }
                else
                {
                    db.DetalleVenta.Update(detalleVentum);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(detalleVentum);
        }
        public IActionResult Delete(int id)
        {
            var cat = db.DetalleVenta.Find(id);
            if (cat != null)
            {
                var prod = db.DetalleVenta.FirstOrDefault(x => x.DetalleVentaId == id);
                if (prod != null)
                {
                    return Json("No se puede eliminar porque tiene datos asosiados");
                }
                db.DetalleVenta.Remove(cat);
                db.SaveChanges();

                return Json("ok");
            }
            return RedirectToAction("Index");
        }
    }
}

