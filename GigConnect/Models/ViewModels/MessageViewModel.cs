using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class MessageViewModel
    {

        public Message message { get; set; }

        public int recipientId { get; set; }

        public string recipientType { get; set; }
    }
}