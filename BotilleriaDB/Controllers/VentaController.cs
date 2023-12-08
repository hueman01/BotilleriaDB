using BotilleriaDB.Helper;
using BotilleriaDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BotilleriaDB.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class VentaController : Controller
    {
        private BotilleriaDbContext db = new();
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Venta ve)
        {
            ViewBag.VentaId = new SelectList(db.Ventas, "VentaId");
            if (ModelState.IsValid)
            {
                var p = db.Ventas.Find(ve.VentaId);
                if (p != null)
                {
                    ModelState.AddModelError("VentaId", "ya esta registrado");
                    return View();
                }
                db.Ventas.Add(ve);
                db.SaveChanges();
            }
            return View();
        }
        public async Task<IActionResult> Index(string buscar, string filtro, int? numPag)
        {
            if (buscar == null)
                buscar = filtro;
            else
                numPag = 1;
            ViewData["filtro"] = buscar;
            var v = from c in db.Ventas select c;
            if (!string.IsNullOrEmpty(buscar))
            {
                v = v.Where(x => x.NombreCliente.ToLower().Contains(buscar.ToLower()));
            }
            int tamPag = 2;
            return View(await PaginatedList<Venta>.CreateAsync(v, numPag ?? 1, tamPag));
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {

                var cat = db.Ventas.Find(id);
                if (cat != null)
                {
                    return View(cat);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Venta venta)
        {
            if (ModelState.IsValid)
            {
                var cat = db.Ventas.FirstOrDefault(
                    x => x.VentaId != venta.VentaId &&
                    x.NombreCliente == venta.NombreCliente);
                   
                if (cat != null)
                {
                    ModelState.AddModelError("NombreCliente", "El Cliente esta registrado");
                }
                else
                {
                    db.Ventas.Update(venta);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(venta);
        }
        public IActionResult Delete(int id)
        {
            var cat = db.Ventas.Find(id);
            if (cat != null)
            {
                var prod = db.Ventas.FirstOrDefault(x => x.VentaId == id);
                if (prod != null)
                {
                    return Json("No se puede eliminar porque tiene datos asosiados");
                }
                db.Ventas.Remove(cat);
                db.SaveChanges();

                return Json("ok");
            }
            return RedirectToAction("Index");
        }
    }
}
