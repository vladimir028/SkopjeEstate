using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealEstateProject.Models
{
    public class Agent
    {
       
        public String Name { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String Username { get; set; }

    }
}