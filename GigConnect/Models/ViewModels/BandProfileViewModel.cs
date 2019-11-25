using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class BandProfileViewModel
    {
        public Band band { get; set; }

        public List<GigInfoViewModel> gigs { get; set; }

       public  List<Review> reviews { get; set; }

        public List<string> facebookPermalinks { get; set; }

        public List<string> youtubeUrls { get; set; }

        public string twitterId { get; set; }








    }
}