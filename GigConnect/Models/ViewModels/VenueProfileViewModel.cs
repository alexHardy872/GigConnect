using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class VenueProfileViewModel
    {

        public Venue venue { get; set; }

        public List<GigInfoViewModel> gigs { get; set; }

        public List<Review> reviews { get; set; }

        public double score { get; set; }

        




    }
}