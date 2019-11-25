using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class BandReview
    {

        [Key, Column(Order = 0)]
        [ForeignKey("Review")]
        public int reviewId { get; set; }
        public Review Review{ get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Band")]
        public int bandId { get; set; }
        public Band Band { get; set; }
    }
}