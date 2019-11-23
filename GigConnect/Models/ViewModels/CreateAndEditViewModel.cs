using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class CreateAndEditViewModel
    {
        public Band Band { get; set; }

        public Location Location { get; set; }

        public Venue Venue { get; set; }

        public SocialMediaIds Social { get; set; }



    }
}