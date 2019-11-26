using GigConnect.Models.EnumClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        //[Required]
        //[Display(Name = "Rating")]
        //public int rating { get; set; }


        [Display(Name = "Rating")]
        public Rating rating { get; set; }


        [Display(Name = "Review")]
        public string content { get; set; }

        public DateTime timeStamp { get; set; }

      




    }
}