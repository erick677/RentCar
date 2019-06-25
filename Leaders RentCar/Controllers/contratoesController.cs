using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Leaders_RentCar.Models;

namespace Leaders_RentCar.Controllers
{
    public class contratoesController : Controller
    {
        private leadersrentcarEntities3 db = new leadersrentcarEntities3();
        /*------------Contrato Corporativo---------------*/
        // GET: contratoes
        public ActionResult Index()
        {
            var contrato = db.contrato.Where( x => x.Tipo_Renta == "Corporativo" && x.Estatus == "Abierto").Include(c => c.Vehiculo);
            return View(contrato.ToList());
        }

        public ActionResult CerrarContratoCorporativo()
        {
            var contrato = db.contrato.Where(x => x.Tipo_Renta == "Corporativo" && x.Estatus == "Cerrado").Include(c => c.Vehiculo);
            return View(contrato.ToList());
        }

        public ActionResult CerrarCorporativo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (id.HasValue)
            {
                contrato.Estatus = "Cerrado";
                
                int x = Convert.ToInt32(contrato.FK_Vehiculo);
                var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
                q.Estatus = "Disponible";

                db.SaveChanges();
                return RedirectToAction("CerrarContratoCorporativo");
            }


            return View(contrato);
        }

        // GET: contratoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }

        // GET: contratoes/Create
        public ActionResult Create()
        {

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Estatus == "Disponible"), "VehiculoId", "Marca");
            
            return View();
        }
        public JsonResult GetFk_Vehiculo(int VehiculoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Vehiculo> vehiculos = db.Vehiculo.Where(m => m.VehiculoId == VehiculoId).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(vehiculos, JsonRequestBehavior.AllowGet);
        }
        

        // POST: contratoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(contrato contrato)
        {
            int x = Convert.ToInt32(contrato.FK_Vehiculo);
            var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
            q.Estatus = "Rentado";
            db.SaveChanges();
            

            if (ModelState.IsValid)
            {
                db.contrato.Add(contrato);
                contrato.Tipo_Renta = "Corporativo";
                contrato.Estatus = "Abierto";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }

        // GET: contratoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }

        // POST: contratoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContratoId,Nombre,Apellido,Pais,Ciudad,Direccion,Telefono,Email,Licencia,FK_Vehiculo,Clase,Combustible_Salida,Combustible_Entrada,Kilometraje_Salida,Kilometraje_Entrada,Cantidad_Dias,Dias_Extras,Descuento_Comision,Total,FormaPago,Contrato1,Fecha_Inicio,Fecha_Cierre,Condiciones,Tipo_Renta,Estatus,Referido")] contrato contrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contrato).State = EntityState.Modified;
                contrato.Tipo_Renta = "Corporativo";
                contrato.Estatus = "Abierto";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }

        // GET: contratoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }

        // POST: contratoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contrato contrato = db.contrato.Find(id);
            int x = Convert.ToInt32(contrato.FK_Vehiculo);
            var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
            q.Estatus = "Disponible";

            db.contrato.Remove(contrato);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*------------------------------------------------------Contrato Regular-------------------------------------------------------*/
        // GET: contratoes
        public ActionResult ContratoRegular()
        {
            var contrato = db.contrato.Where(x => x.Tipo_Renta == "Regular" && x.Estatus == "Abierto").Include(c => c.Vehiculo);
            return View(contrato.ToList());
        }

        public ActionResult CerrarContratoRegular()
        {
            var contrato = db.contrato.Where(x => x.Tipo_Renta == "Regular" && x.Estatus == "Cerrado").Include(c => c.Vehiculo);
            return View(contrato.ToList());
        }

        public ActionResult CerrarRegular(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (id.HasValue)
            {
                int x = Convert.ToInt32(contrato.FK_Vehiculo);
                var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
                q.Estatus = "Disponible";

                contrato.Estatus = "Cerrado";
                db.SaveChanges();
                return RedirectToAction("CerrarContratoRegular");
            }


            return View(contrato);
        }

        // GET: contratoes/Details/5
        public ActionResult DetailsRegular(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }

        // GET: contratoes/Create
        public ActionResult CreateRegular()
        {
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return View();
        }

        // POST: contratoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRegular(contrato contrato)
        {
            int x = Convert.ToInt32(contrato.FK_Vehiculo);
            var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
            q.Estatus = "Rentado";
            db.SaveChanges();

            if (ModelState.IsValid)
            {
                db.contrato.Add(contrato);
                contrato.Tipo_Renta = "Regular";
                contrato.Estatus = "Abierto";
                db.SaveChanges();
                return RedirectToAction("ContratoRegular");
            }

            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }

        // GET: contratoes/Edit/5
        public ActionResult EditRegular(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }

        // POST: contratoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRegular([Bind(Include = "ContratoId,Nombre,Apellido,Pais,Ciudad,Direccion,Telefono,Email,Licencia,FK_Vehiculo,Clase,Combustible_Salida,Combustible_Entrada,Kilometraje_Salida,Kilometraje_Entrada,Cantidad_Dias,Dias_Extras,Descuento_Comision,Total,FormaPago,Contrato1,Fecha_Inicio,Fecha_Cierre,Condiciones,Tipo_Renta,Estatus,Referido")] contrato contrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contrato).State = EntityState.Modified;
                contrato.Tipo_Renta = "Regular";
                contrato.Estatus = "Abierto";
                db.SaveChanges();
                return RedirectToAction("ContratoRegular");
            }
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }

        // GET: contratoes/Delete/5
        public ActionResult DeleteRegular(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }

        // POST: contratoes/Delete/5
        [HttpPost, ActionName("DeleteRegular")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedRegular(int id)
        {
            contrato contrato = db.contrato.Find(id);
            int x = Convert.ToInt32(contrato.FK_Vehiculo);
            var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
            q.Estatus = "Disponible";

            db.contrato.Remove(contrato);
            db.SaveChanges();
            return RedirectToAction("ContratoRegular");
        }
        
    }
}
