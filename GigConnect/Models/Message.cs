using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigConnect.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string from { get; set; }


        [ForeignKey("Venue")]
        public int venueId { get; set; }
        public Venue Venue { get; set; }


        [ForeignKey("Band")]
        public int bandId { get; set; }
        public Band band { get; set; }


        public string messageContent { get; set; }

        public DateTime timeStamp { get; set; }

        public bool read { get; set; }

    }
}