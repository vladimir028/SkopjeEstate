using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealEstateProject.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string ImageURL { get; set; }
        public virtual Property Property { get; set; }

        public Image() { }

        public Image(String ImageURL)
        {
            this.ImageURL = ImageURL; 
        }
    }

   
}