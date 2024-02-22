using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealEstateProject.Models;

namespace RealEstateProject.Controllers
{
    [Authorize(Roles = "Admin, Agent")]
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.Id = id;
                List<Image> images = db.Images.ToList();
                //images = images.FindAll(x => x.Property.Id == id);
                //images = images.OrderBy(x => x.Id).ToList();
                return View(db.Images.Where(x => x.Property.Id == id).ToList());
            }
            else
            {
                ViewBag.Id = null;
                return View(db.Images.OrderBy(x => x.Id).ToList());
            }
        }

        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            { 
                List<Property> properties = db.Properties.ToList();
                ViewBag.Properties = properties;
                return View();
            }
            else
            {
                ViewBag.Id = id;
                List<Property> properties = db.Properties.ToList();
                properties = properties.FindAll(prop => prop.Id == id);
                ViewBag.Properties = properties;
                return View();
            }
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(int Property_Id, String ImageURL, int? id)
        {
            Image image = new Image();
            image.ImageURL = ImageURL;
            image.Property = db.Properties.First(i => i.Id == Property_Id); 
            db.Images.Add(image);
            db.SaveChanges();
            return RedirectToAction("Index/" + id);
           
        }


        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }


        [HttpPost]
        public ActionResult Edit(String ImageURL, int? id)
        {
            Image image = db.Images.Find(id);
            int propertyId = image.Property.Id;
            image.ImageURL = ImageURL;
            db.SaveChanges();
            return RedirectToAction("Index/" + propertyId);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            int propertyId = image.Property.Id;
            db.Images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Index/" + propertyId);
            //return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
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
