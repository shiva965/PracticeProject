using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTimeSheetManagement.Models
{
  [NotMapped]
   public class AssignedProjectsModel
    {
        public List<AdminModel> ListofAdmins { get; set; }

        [Required(ErrorMessage = "Choose Admin")]
        public int RegistrationID { get; set; }

        public List<ProjectModel> ListOfProjects{ get; set; }
        public string ProjectID { get; set; }
        public int? CreatedBy { get; set; }

        //created by shiva
        //public int prid { get; set; }


    }

    [NotMapped]
    public class ProjectModel
    {
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        //created by shiva for  check box for projects
        public int RegistrationID { get; set; }
        public string Name { get; set; }
        public bool SelectedProject { get; set; }
        public string Assigntoproject { get; set; }

    }

    // created by shiva 01/11/2022 for assign project to admin
    //[NotMapped]
    //public class ProjectsModel
    //{
    //    public int RegistrationID { get; set; }
    //    public string Name { get; set; }
    //    public bool SelectedProject { get; set; }
    //    public string Assigntoproject { get; set; }

    //}
}
