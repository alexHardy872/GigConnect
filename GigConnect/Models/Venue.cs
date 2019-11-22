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


        public string venueName { get; set; }

        [ForeignKey("ApplicationUser")]//fk attr
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }//the class the fk attr is referencing

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }




    }
}