using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models
{
    [Table("AssignedProjects")]
   public class AssignedProjects
    {
        [Key]
        public int AssignedProjectID { get; set; }
        public int? ProjectName { get; set; }
       
        public int AssignedTOAdmin { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
 