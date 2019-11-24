﻿using System;
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


        public string reviewOf { get; set; } // band or venue?

        [Required]
        [Display(Name = "Rating")]
        public int rating { get; set; }

        [Display(Name = "Review")]
        public string content { get; set; }

        [ForeignKey("Band")]
        public int? bandId { get; set; }
        public Band Band { get; set; }

        [ForeignKey("Venue")]
        public int? venueId { get; set; }
        public Venue Venue { get; set; }


    }
}