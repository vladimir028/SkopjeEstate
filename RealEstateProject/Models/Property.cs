using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealEstateProject.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]

        public int Size { get; set;}
        public string Location { get; set; }
        public string Address { get; set; }
        public virtual List<Image> ImageURL { get; set; }
        public string BuyRent { get; set; }
        public virtual ApplicationUser Agent { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public string Lattitude { get; set; }
        public string Longtitude { get; set; }

    }
}