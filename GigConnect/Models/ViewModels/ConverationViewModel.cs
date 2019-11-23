using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class ConversationViewModel
    {
        public string other { get; set; }
        List<Message> messages { get; set; }


    }
}