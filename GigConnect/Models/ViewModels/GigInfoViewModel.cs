using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class GigInfoViewModel
    {

        public Gig gig { get; set; }

        public List<Band> bands { get; set; }

    }
}