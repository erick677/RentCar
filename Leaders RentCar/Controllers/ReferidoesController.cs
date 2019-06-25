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
    public class ReferidoesController : Controller
    {
        private leadersrentcarEntities3 db = new leadersrentcarEntities3();

        // GET: Referidoes
        public ActionResult Index()
        {
            var referido = db.Referido.Include(r => r.contrato).Include(r => r.contrato1);
            return View(referido.ToList());
        }

        // GET: Referidoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referido referido = db.Referido.Find(id);
            if (referido == null)
            {
                return HttpNotFound();
            }
            return View(referido);
        }

        // GET: Referidoes/Create
        public ActionResult Create()
        {
            contrato contrato = new contrato();
            
            ViewBag.FK_Referido = new SelectList(db.contrato.Where( m => m.Referido != null && m.Estatus == "Abierto"), "ContratoId", "Referido");
            ViewBag.FK_NumeroContrato = new SelectList(db.contrato.Where(m => m.Estatus == "Abierto"), "ContratoId", "ContratoId");
            return View();
        }

        // POST: Referidoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReferidoId,FK_Referido,FK_NumeroContrato,Comision,Estatus")] Referido referido)
        {

            if (ModelState.IsValid)
            {
                db.Referido.Add(referido);
                referido.Estatus = "Pendiente";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Referido = new SelectList(db.contrato, "ContratoId", "Nombre", referido.FK_Referido );
            ViewBag.FK_NumeroContrato = new SelectList(db.contrato, "ContratoId", "Nombre", referido.FK_NumeroContrato);
            return View(referido);
        }

        // GET: Referidoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referido referido = db.Referido.Find(id);
            if (referido == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_Referido = new SelectList(db.contrato, "ContratoId", "Nombre", referido.FK_Referido);
            ViewBag.FK_NumeroContrato = new SelectList(db.contrato, "ContratoId", "Nombre", referido.FK_NumeroContrato);
            return View(referido);
        }

        // POST: Referidoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReferidoId,FK_Referido,FK_NumeroContrato,Comision,Estatus")] Referido referido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referido).State = EntityState.Modified;
                referido.Estatus = "Pendiente";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_Referido = new SelectList(db.contrato, "ContratoId", "Nombre", referido.FK_Referido);
            ViewBag.FK_NumeroContrato = new SelectList(db.contrato, "ContratoId", "Nombre", referido.FK_NumeroContrato);
            return View(referido);
        }

        // GET: Referidoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referido referido = db.Referido.Find(id);
            if (referido == null)
            {
                return HttpNotFound();
            }
            return View(referido);
        }

        // POST: Referidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Referido referido = db.Referido.Find(id);
            db.Referido.Remove(referido);
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
    }
}
