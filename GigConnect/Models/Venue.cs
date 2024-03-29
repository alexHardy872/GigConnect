﻿using GigConnect.Models.EnumClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required]
        [Display(Name = "Venue Name")]
        public string venueName { get; set; }


        [Required]
        [Display(Name = "Town/Region")]
        public string town { get; set; }


        public string description { get; set; }


        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Venue Website")]
        public string websiteUrl { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public GenreList genre { get; set; }


        [ForeignKey("ApplicationUser")]//fk attr
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }//the class the fk attr is referencing

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }




    }
}