﻿using System;
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
    public class VehiculoesController : Controller
    {
        private leadersrentcarEntities3 db = new leadersrentcarEntities3();

        // GET: Vehiculoes
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            return View(db.Vehiculo.ToList());
        }

        // GET: Vehiculoes
        public ActionResult VehiculoDisponible()
        {
            var Vehiculo  = (from a in db.Vehiculo where a.Estatus == "Disponible" select a);

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            return View(Vehiculo.ToList());
        }

        public ActionResult Disponible(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (id.HasValue)
            {
                vehiculo.Estatus = "Disponible";
                db.SaveChanges();
                return RedirectToAction("VehiculoDisponible");
            }


            return View(vehiculo);
        }

        // GET: Vehiculoes
        public ActionResult VehiculoRentado()
        {
            var Vehiculo = (from a in db.Vehiculo where a.Estatus == "Disponible" select a);

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            return View(Vehiculo.ToList());
        }

        // GET: Vehiculoes Mantenimiento
        public ActionResult VehiculoMantenimiento()
        {
            var Vehiculo = (from a in db.Vehiculo where a.Estatus == "Mantenimiento" select a);

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            return View(Vehiculo.ToList());
        }
        
        public ActionResult Mantenimiento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (id.HasValue)
            {
                vehiculo.Estatus = "Mantenimiento";
                db.SaveChanges();
                return RedirectToAction("VehiculoMantenimiento");
            }
            
            
            return View(vehiculo);
        }

        // GET: Vehiculoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // GET: Vehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehiculoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VehiculoId,Marca,Modelo,Placa,Chasis,Año,Clase,Combustible,Kilometraje,Tarifa,Estatus")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Vehiculo.Add(vehiculo);
                vehiculo.Estatus = "Disponible";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehiculo);
        }

        // GET: Vehiculoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: Vehiculoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehiculoId,Marca,Modelo,Placa,Chasis,Año,Clase,Combustible,Kilometraje,Tarifa,Estatus")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehiculo).State = EntityState.Modified;
                vehiculo.Estatus = "Disponible";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehiculo);
        }

        // GET: Vehiculoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: Vehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehiculo vehiculo = db.Vehiculo.Find(id);
            db.Vehiculo.Remove(vehiculo);
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
