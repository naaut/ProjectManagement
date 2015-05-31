using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PMSite.Models
{
    public enum CompanyType
    {
        Action,
        Customer
    }


    public class Company
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }
                
        [Display(Name = "Company Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // Physical address
        public string  Address { get; set; }

        // property specifies the type of Company, Action or Customer
        public CompanyType? CompanyType { get; set; }
    
        public virtual ICollection<Project> Projects { get; set; }


        //private ICollection<Project> _projects;
        //// Navigation property        
        //public virtual ICollection<Project> Projects
        //{
        //    get
        //    {
        //        return _projects ?? (_projects = new List<Project>());
        //    }
        //    set
        //    {
        //        _projects = value;
        //    }
        //}

    }
}