using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMSite.Models
{

    public enum PersonType
    {
        Developer,
        Manager
    }

    public class Person
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string Lastname { get; set; }

        // property  specifies the type of Person, Manager or Developer
        public PersonType PersonType { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
        
        // Navigation property
        public virtual ICollection<Project> Projects { get; set; }

    }
}