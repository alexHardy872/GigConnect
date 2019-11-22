using GigConnect.Models.EnumClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class Band
    {
        [Key]
        public int BandId { get; set; }


        public string bandName { get; set; }

        [Display(Name = "Genre")]
        public GenreList genre { get; set; }

        public string facebookPageId { get; set; }

        public string twitterPageHandle { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }


        [ForeignKey("ApplicationUser")]//fk attr
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }//the class the fk attr is referencing

      





    }
}