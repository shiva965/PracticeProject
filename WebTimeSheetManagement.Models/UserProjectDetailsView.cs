using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTimeSheetManagement.Models
{
   public class UserProjectDetailsView
    {
        public string ProjectName { get; set; }
        public int? Hours { get; set; }
        public string Period { get; set; }
        public int? TotalHours { get; set; }
        public string Description { get; set; }

    }
}
