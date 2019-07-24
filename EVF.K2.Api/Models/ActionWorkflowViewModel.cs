using EVF.K2.Bll.Models;
using System.Collections.Generic;

namespace EVF.K2.Api.Models
{
    public class ActionWorkflowViewModel
    {
        public K2ProfileModel K2Connect { get; set; }
        public string SerialNumber { get; set; }
        public string Action { get; set; }
        public string AllocatedUser { get; set; }
        public Dictionary<string, object> Datafields { get; set; }
    }
}