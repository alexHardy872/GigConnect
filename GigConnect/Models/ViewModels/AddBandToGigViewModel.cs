using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class AddBandToGigViewModel
    {

        public List<GigInfoViewModel> gigs { get; set; }

        public Band band { get; set; }
    }
}