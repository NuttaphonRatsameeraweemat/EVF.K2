using EVF.K2.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVF.K2.Api.Models
{
    public class WorklistViewModel
    {
        public K2ProfileModel K2Connect { get; set; }
        public string FromUser { get; set; }
        public string ProcessFolder { get; set; }
    }
}