using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class SocialMediaIds
    {
        [Key]
        public int SocialId { get; set; }

        public string facebookPageId { get; set; }

        public string twitterHandle { get; set; }

        public string youtubeChannelId { get; set; }

        [ForeignKey("Band")]
        public int bandId { get; set; }
        public Band Band { get; set; }



    }
}