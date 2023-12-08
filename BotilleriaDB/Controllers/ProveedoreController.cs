using BotilleriaDB.Helper;
using BotilleriaDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BotilleriaDB.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProveedoreController : Controller
    {
        private BotilleriaDbContext db = new();
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Proveedore prov)
        {
            ViewBag.ProveedorId = new SelectList(db.Proveedores, "ProveedorId");
            if (ModelState.IsValid)
            {
                var p = db.Proveedores.Find(prov.ProveedorId);
                if (p != null)
                {
                    ModelState.AddModelError("ProveedorId", "ya esta registrado");
                    return View();
                }
                db.Proveedores.Add(prov);
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
            var pro = from c in db.Proveedores select c;
            if (!string.IsNullOrEmpty(buscar))
            {
                pro = pro.Where(x => x.NombreProveedor.ToLower().Contains(buscar.ToLower()));
            }
            int tamPag = 2;
            return View(await PaginatedList<Proveedore>.CreateAsync(pro, numPag ?? 1, tamPag));
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {

                var cat = db.Proveedores.Find(id);
                if (cat != null)
                {
                    return View(cat);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Proveedore proveedore)
        {
            if (ModelState.IsValid)
            {
                var cat = db.Proveedores.FirstOrDefault(
                    x => x.ProveedorId != proveedore.ProveedorId &&
                    x.NombreProveedor == proveedore.NombreProveedor);
                    //x.Direccion==proveedore.Direccion &&
                    //x.Telefono==proveedore.Telefono);
                if (cat != null)
                {
                    ModelState.AddModelError("NombreProveedor", "El Proveedor esta registrado");
                }
                else
                {
                    db.Proveedores.Update(proveedore);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(proveedore);
        }
        public IActionResult Delete(int id)
        {
            var cat = db.Proveedores.Find(id);
            if (cat != null)
            {
                var prod = db.Proveedores.FirstOrDefault(x => x.ProveedorId == id);
                if (prod != null)
                {
                    return Json("No se puede eliminar porque tiene datos asosiados");
                }
                db.Proveedores.Remove(cat);
                db.SaveChanges();

                return Json("ok");
            }
            return RedirectToAction("Index");
        }

    }
}
