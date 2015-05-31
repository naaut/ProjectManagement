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
    public class ProjectsController : Controller
    {
        private PMSiteContext db = new PMSiteContext();

        // GET: Projects
        public ActionResult Index(string sortOrder, string searchString, string currentSearch, int? id, int? SelectedManager, int? CurrentFilter)
        {
            var viewModel = new PeopleIndexData();            
            //
            if (searchString == null)
                searchString = currentSearch;
            ViewBag.CurrentSearch = searchString;
            //
            if (SelectedManager == null)
                SelectedManager = CurrentFilter;
            ViewBag.CurrentFilter = SelectedManager;

            // Creating DropDown List Content for filtering by Manager
            var managers = db.People.Where(i => i.PersonType == PersonType.Manager).OrderBy(q => q.Firstname).ToList();
            ViewBag.SelectedManager = new SelectList(managers, "ID", "FullName", SelectedManager);
            int managerID = SelectedManager.GetValueOrDefault();

            //get all project filtered by manager
            var projects = db.Projects.Where(c => !SelectedManager.HasValue || c.ManagerID == managerID)
                .Include(p => p.ActionCompany).Include(p => p.CustomerCompany).Include(p => p.Manager);

            // try find content in Name or Decription fields in all projects allready filtered by manager
            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                    || s.Description.Contains(searchString)).Where(c => !SelectedManager.HasValue || c.ManagerID == managerID);
            }


            // Get sortOrder            
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartTime" ? "StartTime_desc" : "StartTime";
            ViewBag.EndDateSortParm = sortOrder == "EndTime" ? "EndTime_desc" : "EndTime";
            ViewBag.PrioritySortParm = sortOrder == "Priority" ? "Priority_desc" : "Priority";
            // Sorting projects 
            switch (sortOrder)
            {
                case "Name":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "Priority":
                    projects = projects.OrderBy(s => s.Priority);
                    break;
                case "StartTime":
                    projects = projects.OrderBy(s => s.StartTime);
                    break;
                case "EndTime":
                    projects = projects.OrderBy(s => s.EndTime);
                    break;
                case "Priority_desc":
                    projects = projects.OrderByDescending(s => s.Priority);
                    break;
                case "StartTime_desc":
                    projects = projects.OrderByDescending(s => s.StartTime);
                    break;
                case "EndTime_desc":
                    projects = projects.OrderByDescending(s => s.EndTime);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            // add filtered and sorted projects to viewModel
            viewModel.Projects = projects;

            // if some project was selected
            if (id != null)
            {
                ViewBag.ProjectID = id.Value;                               
                // get all developer for selected project and add to viewModel
                viewModel.People = viewModel.Projects.Where(i => i.ProjectID == id.Value).Single().Developers;               
            }

            return View(viewModel);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            var project = new Project();           
            PopulateAssignedDevelopersData(project);
            PopulateActionCompanyDropDownList();
            PopulateCustomerCompanyDropDownList();
            PopulatePMDropDownList();
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,Name,Description,Priority,StartTime,EndTime,CustomerCompanyID,ActionCompanyID,ManagerID")] Project project, string[] selectedDevelopers)
        {
            if (selectedDevelopers != null)
            {
                foreach (var developer in selectedDevelopers)
                {
                    var developerToAdd = db.People.Find(int.Parse(developer));
                    project.Developers.Add(developerToAdd);
                }
            }


            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }                    
            
            PopulateAssignedDevelopersData(project);
            PopulateActionCompanyDropDownList();
            PopulateCustomerCompanyDropDownList();
            PopulatePMDropDownList();
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Include(i => i.Developers).Where(i => i.ProjectID == id).Single();

            if (project == null)
            {
                return HttpNotFound();
            }

            PopulateAssignedDevelopersData(project);
            PopulateActionCompanyDropDownList(project.ActionCompanyID);
            PopulateCustomerCompanyDropDownList(project.CustomerCompanyID);
            PopulatePMDropDownList(project.ManagerID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string[] selectedDevelopers)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(project).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.ActionCompanyID = new SelectList(db.Companies, "ID", "Name", project.ActionCompanyID);
            //ViewBag.CustomerCompanyID = new SelectList(db.Companies, "ID", "Name", project.CustomerCompanyID);
            //ViewBag.ManagerID = new SelectList(db.People, "ID", "Firstname", project.ManagerID);
            //return View(project);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var projectToUpdate = db.Projects.Include(i => i.Developers).Where(i => i.ProjectID == id).Single();


            if (TryUpdateModel(projectToUpdate, "", new string[] { "Name", "Description", "Priority", "StartTime", "EndTime", "ManagerID", "ActionCompanyID", "CustomerCompanyID" }))
            {
                try
                {
                    UpdateProjectsDeveloper(selectedDevelopers, projectToUpdate);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");

                }

            }

            PopulateAssignedDevelopersData(projectToUpdate);
            PopulateActionCompanyDropDownList(projectToUpdate.ActionCompanyID);
            PopulateCustomerCompanyDropDownList(projectToUpdate.CustomerCompanyID);
            PopulatePMDropDownList(projectToUpdate.ManagerID);
            return View(projectToUpdate);

        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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

        // DropDown List for Manager Select 
        private void PopulatePMDropDownList(object selectedManager = null)
        {
            var managersQuery = from d in db.People
                                where d.PersonType == PersonType.Manager
                                orderby d.Firstname
                                select d;
            ViewBag.ManagerID = new SelectList(managersQuery, "ID", "FullName", selectedManager);
        }
        // DropDown List for Action Company Select 
        private void PopulateActionCompanyDropDownList(object selectedActionCompany = null)
        {
            var companyQuery = from c in db.Companies
                               where c.CompanyType == CompanyType.Action
                               orderby c.Name
                               select c;

            ViewBag.ActionCompanyID = new SelectList(companyQuery, "ID", "Name", selectedActionCompany);
        }
        // DropDown List for Customer Company Select 
        private void PopulateCustomerCompanyDropDownList(object selectedCustomerCompany = null)
        {
            var companyQuery = from c in db.Companies
                               where c.CompanyType == CompanyType.Customer
                               orderby c.Name
                               select c;

            ViewBag.CustomerCompanyID = new SelectList(companyQuery, "ID", "Name", selectedCustomerCompany);
        }
        // List og checkbox for assign developers
        private void PopulateAssignedDevelopersData(Project project)
        {

            var allDevelopers = db.People.Where(c => c.PersonType == PersonType.Developer);
            
            var developerProjects = new HashSet<int>(project.Developers.Select(c => c.ID));
            var viewModel = new List<AssignedDeveloperData>();
            foreach (var developer in allDevelopers)
            {
                viewModel.Add(new AssignedDeveloperData
                {
                    DeveloperID = developer.ID,
                    FullName = developer.FullName,
                    Assigned = developerProjects.Contains(developer.ID)
                });
            }
            ViewBag.Developers = viewModel;
        }
        
        // update assigned developers-project
        private void UpdateProjectsDeveloper(string[] selectedDevelopers, Project developerToUpdate)
        {
            if (selectedDevelopers == null)
            {
                developerToUpdate.Developers = new List<Person>();
                return;
            }

            var selectedDevelopersHS = new HashSet<string>(selectedDevelopers);
            var projectDevelopers = new HashSet<int>(developerToUpdate.Developers.Select(c => c.ID));

            foreach (var developer in db.People)
            {
                if (selectedDevelopersHS.Contains(developer.ID.ToString()))
                {
                    if (!projectDevelopers.Contains(developer.ID))
                    {
                        developerToUpdate.Developers.Add(developer);
                    }
                }
                else
                {
                    if (projectDevelopers.Contains(developer.ID))
                    {
                        developerToUpdate.Developers.Remove(developer);
                    }
                }
            }
        }

    }
}
