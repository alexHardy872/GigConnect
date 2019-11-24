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

        [Required]
        [Display(Name = "Band Name")]
        public string bandName { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public GenreList genre { get; set; }


        [Required]
        [Display(Name = "Hometown/Region")]
        public string town { get; set; }


        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Band Website")]
        public string bandWebsite { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }


        [ForeignKey("Social")]
        public int socialId { get; set; }
        public SocialMediaIds Social { get; set; }


        [ForeignKey("ApplicationUser")]//fk attr
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }//the class the fk attr is referencing

      





    }
}