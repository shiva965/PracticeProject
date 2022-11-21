using EventApplicationCore.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebTimeSheetManagement.Concrete;
using WebTimeSheetManagement.Filters;
using WebTimeSheetManagement.Helpers;
using WebTimeSheetManagement.Interface;
using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Controllers
{
    [ValidateSuperAdminSession]
    public class SuperAdminController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        private IRegistration _IRegistration;
        private IRoles _IRoles;
        private IAssignRoles _IAssignRoles;
        private ICacheManager _ICacheManager;
        private IUsers _IUsers;
        private IProject _IProject;
        

        public SuperAdminController()
        {
            _IRegistration = new RegistrationConcrete();
            _IRoles = new RolesConcrete();
            _IAssignRoles = new AssignRolesConcrete();
            _ICacheManager = new CacheManager();
            _IUsers = new UsersConcrete();
            _IProject = new ProjectConcrete();
        }

        // GET: SuperAdmin
        public ActionResult Dashboard()
        {
            try
            {

                var adminCount = _ICacheManager.Get<object>("AdminCount");

                if (adminCount == null)
                {
                    var admincount = _IUsers.GetTotalAdminsCount();
                    _ICacheManager.Add("AdminCount", admincount);
                    ViewBag.AdminCount = admincount;
                }
                else
                {
                    ViewBag.AdminCount = adminCount;
                }

                var usersCount = _ICacheManager.Get<object>("UsersCount");

                if (usersCount == null)
                {
                    var userscount = _IUsers.GetTotalUsersCount();
                    _ICacheManager.Add("UsersCount", userscount);
                    ViewBag.UsersCount = userscount;
                }
                else
                {
                    ViewBag.UsersCount = usersCount;
                }

                var projectCount = _ICacheManager.Get<object>("ProjectCount");

                if (projectCount == null)
                {
                    var projectcount = _IProject.GetTotalProjectsCounts();
                    _ICacheManager.Add("ProjectCount", projectcount);
                    ViewBag.ProjectCount = projectcount;
                }
                else
                {
                    ViewBag.ProjectCount = projectCount;
                }

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {

            int id = 0;
            Registration reg = new Registration();
            var item = db.Registration.OrderByDescending(m => m.RegistrationID).FirstOrDefault();
            if (id != 0)
            {
                reg = db.Registration.Where(x => x.RegistrationID == id).FirstOrDefault<Registration>();
            }
            else if (item == null)
            {
                reg.EmployeeID = "EMP001";
            }
            else
            {
                //D3 means in UserId last 3 digits
                reg.EmployeeID = "EMP" + (Convert.ToInt32(item.EmployeeID.Substring(4, item.EmployeeID.Length - 4)) + 1).ToString("D3"); 
            }
            return View(reg);
        }

        [HttpPost]
        public ActionResult CreateAdmin(Registration registration)
        {

            try
            {
                var isUsernameExists = _IRegistration.CheckUserNameExists(registration.Username);

                if (isUsernameExists)
                {
                    ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                }
                else
                {
                    registration.CreatedOn = DateTime.Now;
                    registration.RoleID = _IRoles.getRolesofUserbyRolename("Admin");
                    registration.Password = EncryptionLibrary.EncryptText(registration.Password);
                    registration.ConfirmPassword = EncryptionLibrary.EncryptText(registration.ConfirmPassword);
                    if (_IRegistration.AddUser(registration) > 0)
                    {
                        TempData["MessageRegistration"] = "Data Saved Successfully!";
                        return RedirectToAction("CreateAdmin");
                    }
                    else
                    {
                        return View("CreateAdmin", registration);
                    }
                }

                return RedirectToAction("Dashboard");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult AssignRoles()
        {
            try
            {
                AssignRolesModel assignRolesModel = new AssignRolesModel();
                assignRolesModel.ListofAdmins = _IAssignRoles.ListofAdmins();
                assignRolesModel.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                return View(assignRolesModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult AssignRoles(AssignRolesModel objassign)
        {
            try
            {
                if (objassign.ListofUser == null)
                {
                    TempData["MessageErrorRoles"] = "There are no Users to Assign Roles";
                    objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                    return View(objassign);
                }


                var SelectedCount = (from User in objassign.ListofUser
                                     where User.selectedUsers == true
                                     select User).Count();

                if (SelectedCount == 0)
                {
                    TempData["MessageErrorRoles"] = "You have not Selected any User to Assign Roles";
                    objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                    return View(objassign);
                }

                if (ModelState.IsValid)
                {
                    objassign.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
                    _IAssignRoles.SaveAssignedRoles(objassign);
                    TempData["MessageRoles"] = "Roles Assigned Successfully!";
                }

                objassign = new AssignRolesModel();
                objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();

                return RedirectToAction("AssignRoles");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //created by shiva for assign the projects to admin by super admin

        [HttpGet]
        public ActionResult AssignedProjects()
        {
             try
            {
                AssignedProjectsModel assignedprojectsmodel = new AssignedProjectsModel();
                assignedprojectsmodel.ListofAdmins = _IAssignRoles.ListofAdmins();
                assignedprojectsmodel.ListOfProjects = _IAssignRoles.ListOfProjects();
           

                return View(assignedprojectsmodel);
            }
            catch (Exception)
            {

                throw;
            }
        }

///not completed work in pogresss.....
        [HttpPost]
        public ActionResult AssignedProjects(AssignedProjectsModel assignedProjectsModel)
        {
            try
            {
                if(assignedProjectsModel.ListOfProjects==null)
                 {
                    TempData["MessageErrorProject"] = "There are no Users to Assign Project";
                    assignedProjectsModel.ListofAdmins = _IAssignRoles.ListofAdmins();
                    assignedProjectsModel.ListOfProjects = _IAssignRoles.ListOfProjects();
                    return View(assignedProjectsModel);
                }

                var SelectedCount = (from User in assignedProjectsModel.ListOfProjects
                                     where User.SelectedProject == true
                                     select User).Count();
                if (SelectedCount == 0)
                {
                    TempData["MessageErrorProject"] = "You have not Selected any Project";
                    assignedProjectsModel.ListofAdmins = _IAssignRoles.ListofAdmins();
                    assignedProjectsModel.ListOfProjects = _IAssignRoles.ListOfProjects();
                    return View(assignedProjectsModel);
                }

                if (ModelState.IsValid)
                {
                    assignedProjectsModel.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
                    _IAssignRoles.SaveAssignProjects(assignedProjectsModel);
                    TempData["MessageProject"] = "Project Assigned Successfully!";
                }

                assignedProjectsModel = new AssignedProjectsModel();
                assignedProjectsModel.ListofAdmins = _IAssignRoles.ListofAdmins();
                assignedProjectsModel.ListOfProjects = _IAssignRoles.ListOfProjects();

                return RedirectToAction("AssignedProjects");

            }
            catch(Exception)
            {
                throw;
            }
            //return View();
        }

        //[HttpPost]
        //public ActionResult AssignedProjects(AssignedProjectsModel assignedProjectsModel)
        //{
        //    try
        //    {
        //        if (assignedProjectsModel.ListofAdmins == null)
        //        {
        //            TempData["MessageErrorRoles"] = "There are no Users to Assign Roles";
        //            assignedProjectsModel.ListofAdmins = _IAssignRoles.ListofAdmins();
        //            assignedProjectsModel.ListOfProjects = _IAssignRoles.ListOfProjects();
        //            //objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
        //            return View(assignedProjectsModel);
        //        }


        //        //var SelectedCount = (from User in objassign.ListofUser
        //        //                     where User.selectedUsers == true
        //        //                     select User).Count();
        //        var SelectedCount=(from admin in assignedProjectsModel.ListOfProjects where admin.)

        //        if (SelectedCount == 0)
        //        {
        //            TempData["MessageErrorRoles"] = "You have not Selected any User to Assign Roles";
        //            assignedProjectsModel.ListofAdmins = _IAssignRoles.ListofAdmins();
        //            assignedProjectsModel.l = _IAssignRoles.GetListofUnAssignedUsers();
        //            return View(assignedProjectsModel);
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            assignedProjectsModel.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
        //            _IAssignRoles.SaveAssignedRoles(assignedProjectsModel);
        //            TempData["MessageRoles"] = "Roles Assigned Successfully!";
        //        }

        //        assignedProjectsModel = new AssignedProjectsModel();
        //        assignedProjectsModel.ListofAdmins = _IAssignRoles.ListofAdmins();

        //        return RedirectToAction("AssignedProjects");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

    }
}