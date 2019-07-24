using EVF.K2.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVF.K2.Api.Models
{
    public class SetOutOfOfficeViewModel
    {
        public K2ProfileModel K2Connect { get; set; }
        public WorkflowDelegateModel WorkflowDelegate { get; set; }
    }
}