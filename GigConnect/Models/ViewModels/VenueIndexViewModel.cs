using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class VenueIndexViewModel
    {


        public Venue venue { get; set; }

        public List<GigInfoViewModel> currentGigs { get; set; }

        public List<GigInfoViewModel> openGigs { get; set; }

        public List<Message> messagesIn { get; set; }

        public List<Message> messagesOut { get; set; }

        public List<Request> requestsIn { get; set; }

        public List<Request> requestsOut { get; set; }

        public List<Request> requestResponses { get; set; }

        public List<Review> reviews { get; set; }

        public double score { get; set; }



    }
}