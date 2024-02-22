using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using RealEstateProject.Models;

namespace RealEstateProject.Controllers
{
    
    public class AgenciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Agencies
        public ActionResult Index()
        {
            return View(db.Agencies.ToList());
        }

        // GET: Agencies/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = db.Agencies.Find(id);
            if (agency == null)
            {
                return HttpNotFound();
            }

          Dictionary<String, List<Property>> agentPropertiesMap = new Dictionary<String, List<Property>>();
            agency.Users.ForEach(user =>
            {
                agentPropertiesMap[user.Email] = new List<Property>();
                List<Property> properties = db.Properties.ToList();
                List<Property> agentProperties = properties.FindAll(i => i.Owner.Id == user.Id).ToList();
                agentPropertiesMap[user.Email] = agentProperties;
            });
            ViewBag.AgentProperties = agentPropertiesMap;
            return View(agency);
        }

        // GET: Agencies/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(String CompanyName, String Email, String Phone, String Lattitude, String Longtitude, String Address)
        {
            Agency agency = new Agency();
            agency.CompanyName = CompanyName;
            agency.Email = Email;   
            agency.Phone = Phone;
            agency.Lattitude = Lattitude;
            agency.Longtitude = Longtitude;
            agency.Address = Address;
            db.Agencies.Add(agency);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Agencies/Edit/5
        [Authorize(Roles = "Admin, Agent")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = db.Agencies.Find(id);
            if (agency == null)
            {
                return HttpNotFound();
            }
            return View(agency);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Agent")]
        public ActionResult Edit(int? id, String CompanyName, String Email, String Phone, String Lattitude, String Longtitude, String Address)
        {
            Agency agency = db.Agencies.Find(id);
            agency.CompanyName = CompanyName;
            agency.Email = Email;
            agency.Phone = Phone;
            agency.Lattitude = Lattitude;
            agency.Longtitude = Longtitude;
            agency.Address = Address;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Agencies/Delete/5
        [Authorize(Roles = "Admin, Agent")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agency agency = db.Agencies.Find(id);
            
            if (agency == null)
            {
                return HttpNotFound();
            }

            List<ApplicationUser> agents = db.Users.Where(i => i.Agency.Id.Equals(agency.Id)).ToList();
            agents.ForEach(agent =>
            {
                agent.Agency = null;
            });
            db.SaveChanges();
            db.Agencies.Remove(agency);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // POST: Agencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agency agency = db.Agencies.Find(id);
            db.Agencies.Remove(agency);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddAgentToAgency()
        {
            IdentityRole role = db.Roles.Where(x => x.Name.Equals("Agent")).First();   
            List<Agency> allAgencies = db.Agencies.ToList();
            ViewBag.Agents = db.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(role.Id))).ToList();
            ViewBag.Agencies = allAgencies;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddAgentToAgency(int agencyId, String agentId)
        {
            var agent = db.Users.First(i => i.Id == agentId);
            var agency = db.Agencies.Find(agencyId);
            agent.Agency = agency;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
