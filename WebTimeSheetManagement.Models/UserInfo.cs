using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTimeSheetManagement.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public int UID { get; set; }

        [Required(ErrorMessage = "Enter FName")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Enter LName")]
        public string MName { get; set; }

        [Required(ErrorMessage = "Enter MName")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        public string UName { get; set; }
    }
}
