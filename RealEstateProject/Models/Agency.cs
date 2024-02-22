using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealEstateProject.Models
{
    public class Agency
    {
        [Key]
        public int Id { get; set; }
        
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        //public virtual List<Property> Properties { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        public string Lattitude { get; set; }
        public string Longtitude { get; set; }
        public string Address { get; set; }
    }
}