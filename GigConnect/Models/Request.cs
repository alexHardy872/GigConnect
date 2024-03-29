﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }


        [ForeignKey("Gig")]    //// for later, checking if a gig is null differentiates a custom request
        public int? eventId { get; set; }
        public Gig Gig { get; set; }


        [ForeignKey("Band")]
        public int bandId { get; set; }
        public Band Band { get; set; }


        [ForeignKey("Venue")]
        public int venueId { get; set; }
        public Venue Venue { get; set; }


        public DateTime? gigTime { get; set; }

        public DateTime timeStamp { get; set; }

        public string message { get; set; }


        public bool fromBand { get; set; }

        public bool fromVenue { get; set; }

        public bool approved { get; set; }

        public bool denied { get; set; }





    }
}