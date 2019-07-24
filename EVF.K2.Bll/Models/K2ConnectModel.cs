using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVF.K2.Bll.Models
{
    public class K2ConnectModel
    {
        public K2ProfileModel K2Profile { get; set; }
        public int Port { get; set; }
        public string Url { get; set; }
        public string SecurityLabelName { get; set; }
    }
}
