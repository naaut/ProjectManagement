using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PMSite.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        [Display(Name = "Project Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, 100)]
        public int Priority { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }
        
        public int? CustomerCompanyID { get; set; }
        // Navigation property
        public virtual Company CustomerCompany { get; set; }

        public int? ActionCompanyID { get; set; }
        // Navigation property
        public virtual Company ActionCompany { get; set; }

        public int? ManagerID { get; set; }
        // Navigation property
        public virtual Person Manager { get; set; }

        private ICollection<Person> _developers;
        // Navigation property        
        public virtual ICollection<Person> Developers
        {
            get
            {
                return _developers ?? (_developers = new List<Person>());
            }
            set
            {
                _developers = value;
            }
        }

    }
}