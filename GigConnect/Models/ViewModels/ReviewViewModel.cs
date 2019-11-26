using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigConnect.Models.ViewModels
{
    public class ReviewViewModel
    {

        public Review review { get; set; }

        public int reviewedId { get; set; }

        public string reviewOf { get; set; }
    }
}