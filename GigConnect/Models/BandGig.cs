using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class BandGig
    {


        [Key, Column(Order = 0)]
        [ForeignKey("Gig")]
        public int gigId { get; set; }
        public Gig Gig{ get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Band")]
        public int bandId { get; set; }
        public Band Band { get; set; }
    }
}