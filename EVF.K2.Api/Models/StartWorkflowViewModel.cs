using EVF.K2.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVF.K2.Api.Models
{
    public class StartWorkflowViewModel
    {
        public K2ProfileModel K2Connect { get; set; }
        public string ProcessName { get; set; }
        public string Folio { get; set; }
        public Dictionary<string, object> DataFields { get; set; }
    }
}