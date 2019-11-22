﻿using System;
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


        [ForeignKey("Venue")]
        public int venueId { get; set; }
        public Venue Venue { get; set; }


        public string messageContent { get; set; }

        public DateTime timeStamp { get; set; }

    }
}