using BotilleriaDB.Helper;
using BotilleriaDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BotilleriaDB.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProductoController : Controller
    {
        private BotilleriaDbContext db = new();
        public async Task<IActionResult> Index(string buscar, string filtro, int? numPag)
        {
            var p=db.Productos.Include(p=>p.Categoria).ToList();
            
            if (buscar == null)
                buscar = filtro;
            else
                numPag = 1;
            //lleva el texto buscado a la página
            ViewData["filtro"] = buscar;
            var categorias = from c in db.Productos select c;
            //verifica si no está vacio 
            if (!string.IsNullOrEmpty(buscar))
            {
                categorias = categorias.Where(
                    x => x.NombreProducto.ToLower().Contains(buscar.ToLower()));
            }
            int tamPag = 2; //poner 20 o 25
            return View(await PaginatedList<Producto>
                .CreateAsync(categorias, numPag ?? 1, tamPag));

        }
       
        public IActionResult Create()
        {
            
            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
            return View();
        }
        

        [HttpPost]
        public IActionResult Create(Producto producto, IFormFile Imagen)
        {
            if (Imagen != null)
            {
                //añadimos la ruta donde se guardará la imagen
                string folder = "imagen/";
                //se le agrega un id random al nombre de la imagen
                folder += Guid.NewGuid().ToString() + Imagen.FileName;
                //obtener la ruta del servidor
                IWebHostEnvironment ruta = HttpContext.RequestServices.GetService<IWebHostEnvironment>();
                string serverFolder = Path.Combine(ruta.WebRootPath, folder);
                //copia el archivo a la carpeta en el servidor
                Imagen.CopyTo(new FileStream(serverFolder, FileMode.Create));
                //se guardar la ruta del archivo en la db
                producto.Imagen = folder;
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
            if (ModelState.IsValid)
            {
                var p = db.Productos.Find(producto.ProductoId);
                if (p != null)
                {
                    ModelState.AddModelError("ProductoId", "El código ya está ingresado");
                    return View();
                }
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
       
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {

                var cat = db.Productos.Find(id);
                if (cat != null)
                {
                    return View(cat);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var cat = db.Productos.FirstOrDefault(
                    x => x.ProductoId != producto.ProductoId &&
                    x.NombreProducto == producto.NombreProducto &&
                     
                    x.NombreProveedor == producto.NombreProveedor &&
                    x.Precio == producto.Precio &&
                    x.Stock==producto.Stock &&
                    x.Imagen==producto.Imagen);
                if (cat != null)
                {
                    ModelState.AddModelError("NombreProducto", "El Producto esta registrada");
                }
                else
                {
                    db.Productos.Update(producto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(producto);
        }
        public IActionResult Delete(int id)
        {
            var cat = db.Productos.Find(id);
            if (cat != null)
            {
                var prod = db.Productos.FirstOrDefault(x => x.ProductoId == id);
                if (prod != null)
                {
                    return Json("No se puede eliminar porque tiene datos asosiados");
                }
                db.Productos.Remove(cat);
                db.SaveChanges();

                return Json("ok");
            }
            return RedirectToAction("Index");
        }
    }
    
}