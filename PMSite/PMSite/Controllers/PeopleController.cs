using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PMSite.DAL;
using PMSite.Models;
using PMSite.ViewModels;

namespace PMSite.Controllers
{
    public class PeopleController : Controller
    {
        private PMSiteContext db = new PMSiteContext();

        // GET: People
        public ActionResult Index(int? id)
        {
            //create new viewModel for People/Index
            var viewModel = new PeopleIndexData();
            // viewModel.People must include all People
            viewModel.People = db.People;

            //if some persone was select
            if (id != null)
            {
                // take persone what was selected
                Person selectedPeson = viewModel.People.Where(i => i.ID == id).Single();
                ViewBag.ID = id;
                // if Developer return project for this developer
                if ( selectedPeson.PersonType == PersonType.Developer )
                viewModel.Projects = viewModel.People.Where(i => i.ID == id).Single().Projects;
                // if Manager get all project where ManagerID = selected Person ID 
                if (selectedPeson.PersonType == PersonType.Manager)
                    viewModel.Projects = db.Projects.Where(i => i.ManagerID == id);     
            }
            return View(viewModel);
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Firstname,Lastname,PersonType,PhoneNumber,Email")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Firstname,Lastname,PersonType,PhoneNumber,Email")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Include(i => i.Projects).Where(i => i.ID == id).Single();

            // Clear related data Manager ID from project table
            var projects = db.Projects.Where(i => i.ManagerID == id);            
            foreach (var project in projects)
            {
                project.ManagerID = null;
            }
            db.People.Remove(person);
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
