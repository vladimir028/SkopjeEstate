using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RealEstateProject.Models;

namespace RealEstateProject.Controllers
{
    public class PropertiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Properties
        [AllowAnonymous]
        public ActionResult Index(string location, string minP, string maxP, string minS, string maxS, string type)
        {
            ViewBag.PropertyList = db.Properties.Select(p => p.Type).Distinct().ToList();
            List<Property> properties = db.Properties.ToList();

            if (!location.IsNullOrWhiteSpace())
            {
                properties = properties.FindAll(p => p.Location.Equals(location));
            }

            if(!minP.IsNullOrWhiteSpace() && Int32.TryParse(minP, out int min))
            {
                properties = properties.FindAll(p => p.Price >= min);
            }
            if (!maxP.IsNullOrWhiteSpace() && Int32.TryParse(maxP, out int max))
            {
                properties = properties.FindAll(p => p.Price <= max);
            }

            if (!minS.IsNullOrWhiteSpace() && Int32.TryParse(minS, out int min_s))
            {
                properties = properties.FindAll(p => p.Size >= min_s);
            }
            if (!maxS.IsNullOrWhiteSpace() && Int32.TryParse(maxS, out int max_s))
            {
                properties = properties.FindAll(p => p.Size <= max_s);
            }

            if (!type.IsNullOrWhiteSpace())
            {
                properties = properties.FindAll(p => p.Type.Equals(type));
            }

            properties.ForEach(property =>
            {
                if (property.ImageURL.Count == 0)
                {
                    property.ImageURL.Add(new Image("https://st4.depositphotos.com/14953852/24787/v/450/depositphotos_247872612-stock-illustration-no-image-available-icon-vector.jpg"));
                    db.Images.Add(property.ImageURL[0]);
                    db.SaveChanges();
                }
            });

            properties.ForEach(property =>
            {
                if (property.ImageURL.Count > 1)
                {
                    property.ImageURL.ForEach(p =>
                    {
                        if (p.ImageURL.Equals("https://st4.depositphotos.com/14953852/24787/v/450/depositphotos_247872612-stock-illustration-no-image-available-icon-vector.jpg"))
                        {
                            db.Images.Remove(p);
                            db.SaveChanges();
                        }
                    });
                    
                }
            });


            return View(properties);
        }

        // GET: Properties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            String username = User.Identity.GetUserName();
            bool sameAgency = false;
            if (username.IsNullOrWhiteSpace())
            {
                sameAgency = true;
                ViewBag.sameAgency = sameAgency;
                return View(property);
            }
            var currentAgent = db.Users.First(i => i.UserName.Equals(username));

          
            if (currentAgent.Ime.Equals("admin"))
            {
                sameAgency = true;
                ViewBag.sameAgency = sameAgency;
                return View(property);
            }

            if(currentAgent.Agency == null || property.Agent.Agency == null)
            {
                ViewBag.sameAgency = sameAgency;
                return View(property);
            }

            var agencyId = property.Agent.Agency.Id;

            var currAgentAgencyId = currentAgent.Agency.Id;

          
            if(agencyId == currAgentAgencyId)
            {
                sameAgency = true;
            }

            ViewBag.sameAgency = sameAgency;
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // GET: Properties/Create
        public ActionResult Create()
        {
            ViewBag.PropertyList = db.Properties.Select(p => p.Type).Distinct().ToList();
            ViewBag.Users = db.Users.Where(i => i.Agency != null).ToList();
            return View();
        }


        [HttpPost]
        public ActionResult Create(String type, String name, String description, int price, int size, String location, String address, String usage, String userId)
        {
            Property property = new Property();
            property.Type = type;
            property.Name = name;
            property.Description = description;
            property.Price = price;
            property.Size = size;
            property.Location = location;
            property.Address = address;
            property.BuyRent = usage;
            property.ImageURL = new List<Image>();
            property.Lattitude = "0";
            property.Lattitude = "0";
            ApplicationUser Agent = db.Users.Find(userId);
            property.Agent = Agent;
            property.Owner = Agent;
            db.Properties.Add(property);
            db.SaveChanges();
            int mostRecentId = property.Id;

            //return RedirectToAction("/Images/Create/" + mostRecentId);
            return RedirectToAction("Create", "Images", new { id = mostRecentId });


        }
        // GET: Properties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.Status = property.BuyRent;
            ViewBag.PropertyList = db.Properties.Select(p => p.Type).Distinct().ToList();
            IdentityRole role = db.Roles.Where(x => x.Name.Equals("Agent")).First();
            ViewBag.Agents = db.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(role.Id))).ToList();
            return View(property);
        }



        [HttpPost]
        public ActionResult Edit(int propertyId, String type, String name, String description, int price, int size, String location, String address, String usage, String agentId)
        {
           
            Property property = db.Properties.Find(propertyId);

            if (property == null)
            {
                return HttpNotFound();
            }


            property.Type = type;
            property.Name = name;
            property.Description = description;
            property.Price = price;
            property.Size = size;
            property.Location = location;
            property.Address = address;
            property.BuyRent = usage;
            property.Agent = db.Users.First(x => x.Id.Equals(agentId));
            property.Owner = db.Users.First(x => x.Id.Equals(agentId));
            

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
            //return RedirectToAction("Index", "Properties");
        }

        // GET: Properties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);

            if (property == null)
            {
                return HttpNotFound();
            }
            //property.ImageURL.ForEach(i => db.Images.Remove(i));
            db.Images.RemoveRange(property.ImageURL);
            db.SaveChanges();
            db.Properties.Remove(property);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
            //return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Property property = db.Properties.Find(id);
            db.Properties.Remove(property);
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
