using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVF.K2.Bll.Models
{
    public class WorkflowDelegateModel
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Action { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
