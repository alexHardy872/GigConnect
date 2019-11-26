using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class Gig
    {
        [Key]
        public int GigId { get; set; }

        public string description { get; set; }


        //[ForeignKey("Location")]
        //public int locationId { get; set; }
        //public Location Location { get; set; }


        [ForeignKey("Venue")]
        public int venueId { get; set; }
        public Venue Venue { get; set; }

        //public string bandsOnVenue { get; set; } // coma 

        public DateTime timeOfGig { get; set; }

        public bool open { get; set; }








    }
}