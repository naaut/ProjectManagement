namespace PMSite.Migrations
{
    using PMSite.DAL;
using PMSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PMSite.DAL.PMSiteContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PMSite.Models.PMSiteContext";
        }

        protected override void Seed(PMSite.DAL.PMSiteContext context)
        {
            var developers = new List<Person>
            {
                new Person {Firstname ="Ivan",Lastname="Ivanov",Email="ivan@company.com",PhoneNumber="+7 555-555-4444",PersonType = PersonType.Developer},
                new Person {Firstname ="Alexander",Lastname="Carson",Email="alexander@company.com",PhoneNumber="+7 913-555-4444",PersonType = PersonType.Developer},
                new Person {Firstname ="Sonay",Lastname="Blade",Email="sonya@company.com",PhoneNumber="+7 009-555-4444",PersonType = PersonType.Developer},
                new Person {Firstname ="Petr",Lastname="Petrov",Email="petrov@company.com",PhoneNumber="+7 334-555-6666",PersonType = PersonType.Developer},
                new Person {Firstname ="Vitaliy",Lastname="Volkov",Email="volkov@company.com",PhoneNumber="+7 322-555-2451",PersonType = PersonType.Developer},
                new Person {Firstname ="Vasiliy",Lastname="Vasilev",Email="vasilev@company.com",PhoneNumber="+7 444-555-6663",PersonType = PersonType.Developer},
                new Person {Firstname ="Nick",Lastname="Derst",Email="derst@company.com",PhoneNumber="+7 555-555-1478",PersonType = PersonType.Developer},
                new Person {Firstname ="Aarone",Lastname="Spectre",Email="spectre@company.com",PhoneNumber="+7 555-566-4444",PersonType = PersonType.Developer}
            };

            developers.ForEach(s => context.People.AddOrUpdate(p => p.Lastname, s));
            context.SaveChanges();


            var customerCompanies = new List<Company>
            {
                new Company {Name="Company 1", Email="company1@company1.com",CompanyType = CompanyType.Customer},
                new Company {Name="Company 2", Email="company2@company2.com",CompanyType = CompanyType.Customer},
                new Company {Name="Company 3", Email="company3@company3.com",CompanyType = CompanyType.Customer},
                new Company {Name="Company 4", Email="company4@company4.com",CompanyType = CompanyType.Customer}
            };

            customerCompanies.ForEach(s => context.Companies.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var actionCompanies = new List<Company>
            {
                
                new Company {Name="Action Company 1", Email="actioncompany1@company1.com",CompanyType = CompanyType.Action},
                new Company {Name="Action Company 2", Email="actioncompany2@company2.com",CompanyType = CompanyType.Action},
                new Company {Name="Action Company 3", Email="actioncompany3@company3.com",CompanyType = CompanyType.Action}
            };

            actionCompanies.ForEach(s => context.Companies.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();




            var managers = new List<Person>
            {
                new Person {Firstname ="Lime",Lastname="Orange",Email="lime@company.com",PhoneNumber="+7 555-555-4444",PersonType = PersonType.Manager},
                new Person {Firstname ="Pine",Lastname="Apple",Email="pine@company.com",PhoneNumber="+7 913-555-4444",PersonType = PersonType.Manager},
                new Person {Firstname ="Venera",Lastname="Pluton",Email="venera@company.com",PhoneNumber="+7 009-555-4444",PersonType = PersonType.Manager}
            };

            managers.ForEach(s => context.People.AddOrUpdate(p => p.Lastname, s));
            context.SaveChanges();


            var projects = new List<Project>
            {
                new Project {Name="DB Server Migration",Priority=100,Description="MSSQL 2012 to MSSQL 2014",StartTime=DateTime.Parse("2015-01-10"),EndTime=DateTime.Parse("2015-09-01"),ManagerID = managers.FirstOrDefault().ID, ActionCompanyID = actionCompanies.First().ID, CustomerCompanyID=customerCompanies.First().ID},
                new Project {Name="AD Migration",Priority=50,Description="Description for Project",StartTime=DateTime.Parse("2015-03-15"),EndTime=DateTime.Parse("2015-10-01"),ManagerID = managers.FirstOrDefault().ID,ActionCompanyID = actionCompanies.First().ID, CustomerCompanyID=customerCompanies.Last().ID},
                new Project {Name="PM Site Developing",Priority=10,Description="Create site for Project Management",StartTime=DateTime.Parse("2013-01-20"),EndTime=DateTime.Parse("2015-05-01"),ManagerID = managers.Last().ID,ActionCompanyID = actionCompanies.Last().ID, CustomerCompanyID=customerCompanies.First().ID},
                new Project {Name="WP 8.1 Project",Priority=1,Description="Time Tracker for jira tasks",StartTime=DateTime.Parse("2014-01-10"),EndTime=DateTime.Parse("2015-06-25"),ManagerID = managers.FirstOrDefault().ID,ActionCompanyID = actionCompanies.First().ID, CustomerCompanyID=customerCompanies.Last().ID},
                new Project {Name="ASP.NET Site Project",Priority=32,Description="Test task",StartTime=DateTime.Parse("2015-01-10"),EndTime=DateTime.Parse("2016-12-01"),ManagerID = managers.Last().ID,ActionCompanyID = actionCompanies.Last().ID, CustomerCompanyID=customerCompanies.First().ID}

            };
            projects.ForEach(s => context.Projects.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            AddOrUpdateDevelopers(context, "DB Server Migration", "Ivanov");
            AddOrUpdateDevelopers(context, "DB Server Migration", "Petrov");
            AddOrUpdateDevelopers(context, "DB Server Migration", "Volkov");
            AddOrUpdateDevelopers(context, "DB Server Migration", "Spectre");
            AddOrUpdateDevelopers(context, "AD Migration", "Derst");
            AddOrUpdateDevelopers(context, "AD Migration", "Blade");
            AddOrUpdateDevelopers(context, "WP 8.1 Project", "Vasilev");
            AddOrUpdateDevelopers(context, "WP 8.1 Project", "Carson");
            AddOrUpdateDevelopers(context, "PM Site Developing", "Vasilev");
            AddOrUpdateDevelopers(context, "PM Site Developing", "Carson");
            AddOrUpdateDevelopers(context, "PM Site Developing", "Petrov");
            AddOrUpdateDevelopers(context, "PM Site Developing", "Derst");
            AddOrUpdateDevelopers(context, "ASP.NET Site Project", "Carson");
            AddOrUpdateDevelopers(context, "ASP.NET Site Project", "Ivanov");
            AddOrUpdateDevelopers(context, "ASP.NET Site Project", "Blade");
            AddOrUpdateDevelopers(context, "ASP.NET Site Project", "Derst");

            context.SaveChanges();
        }
        void AddOrUpdateDevelopers(PMSiteContext context, string projectName, string developerName)
        {
            var crs = context.Projects.SingleOrDefault(c => c.Name == projectName);
            var inst = crs.Developers.SingleOrDefault(i => i.Lastname == developerName);
            if (inst == null)
                crs.Developers.Add(context.People.Single(i => i.Lastname == developerName));
        }
    }
}
