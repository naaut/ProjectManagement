using PMSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMSite.ViewModels
{
    public class PeopleIndexData
    {
        public IEnumerable<Person> People { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}