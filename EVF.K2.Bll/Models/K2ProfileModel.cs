using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVF.K2.Bll.Models
{
    public class K2ProfileModel
    {
        public K2ProfileModel()
        {
            Impersonate = false;
            ImpersonateUser = string.Empty;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Impersonate { get; set; }
        public string ImpersonateUser { get; set; }
        public bool Management { get; set; }
    }
}
