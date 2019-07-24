using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVF.K2.Bll.Models
{
    public class TaskViewModel
    {
        public TaskViewModel()
        {
            DataFields = new Dictionary<string, object>();
        }

        public string AllocatedUser { get; set; }
        public string Folio { get; set; }
        public DateTime StartDate { get; set; }
        public string Folder { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public Dictionary<string,object> DataFields { get; set; }
    }
}
