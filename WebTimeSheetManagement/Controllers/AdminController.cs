using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTimeSheetManagement.Concrete;
using WebTimeSheetManagement.Filters;
using WebTimeSheetManagement.Interface;
using EventApplicationCore.Library;
using WebTimeSheetManagement.Models;


namespace WebTimeSheetManagement.Controllers
{
    [ValidateAdminSession]
    public class AdminController : Controller
    {
        private ITimeSheet _ITimeSheet;
        private IExpense _IExpense;
        ILogin _ILogin;
        IRegistration _IRegistration;
        public AdminController()
        {
            _ITimeSheet = new TimeSheetConcrete();
            _IExpense = new ExpenseConcrete();
            _ILogin = new LoginConcrete();
            _IRegistration = new RegistrationConcrete();
        }
        // GET: Admin
        [HttpGet]
        public ActionResult Dashboard()
        {
            try
            {
                var timesheetResult = _ITimeSheet.GetTimeSheetsCountByAdminID(Convert.ToString(Session["AdminUser"]));

                if (timesheetResult != null)
                {
                    ViewBag.SubmittedTimesheetCount = timesheetResult.SubmittedCount;
                    ViewBag.ApprovedTimesheetCount = timesheetResult.ApprovedCount;
                    ViewBag.RejectedTimesheetCount = timesheetResult.RejectedCount;
                }
                else
                {
                    ViewBag.SubmittedTimesheetCount = 0;
                    ViewBag.ApprovedTimesheetCount = 0;
                    ViewBag.RejectedTimesheetCount = 0;
                }


                var expenseResult = _IExpense.GetExpenseAuditCountByAdminID(Convert.ToString(Session["AdminUser"]));

                if (expenseResult != null)
                {
                    ViewBag.SubmittedExpenseCount = expenseResult.SubmittedCount;
                    ViewBag.ApprovedExpenseCount = expenseResult.ApprovedCount;
                    ViewBag.RejectedExpenseCount = expenseResult.RejectedCount;
                }
                else
                {
                    ViewBag.SubmittedExpenseCount = 0;
                    ViewBag.ApprovedExpenseCount = 0;
                    ViewBag.RejectedExpenseCount = 0;
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel changepasswordmodel)
        {
            try
            {
                var password = EncryptionLibrary.EncryptText(changepasswordmodel.OldPassword);

                var storedPassword = _ILogin.GetPasswordbyUserID(Convert.ToInt32(Session["AdminUser"]));

                if (storedPassword == password)
                {
                    var result = _ILogin.UpdatePassword(EncryptionLibrary.EncryptText(changepasswordmodel.NewPassword), Convert.ToInt32(Session["AdminUser"]));

                    if (result)
                    {
                        ModelState.Clear();
                        ViewBag.message = "Password Changed Successfully";
                        return View(changepasswordmodel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something Went Wrong Please try Again after some time");
                        return View(changepasswordmodel);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Entered Wrong Old Password");
                    return View(changepasswordmodel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserInfo());
        }

        [HttpGet]
        public ActionResult Assignproject()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Create(UserInfo userInfo)
        //{
        //    try
        //    {
        //        var isUsernameExists = _IRegistration.CheckUserNameExists(userInfo.UName);

        //        if (isUsernameExists)
        //        {
        //            ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
        //        }
        //        else
        //        {
        //            //registration.CreatedOn = DateTime.Now;
        //            //registration.RoleID = _IRoles.getRolesofUserbyRolename("Users");
        //            //registration.Password = EncryptionLibrary.EncryptText(registration.Password);
        //            //registration.ConfirmPassword = EncryptionLibrary.EncryptText(registration.ConfirmPassword);
        //            if (_IRegistration.AddUser(userInfo) > 0)
        //            {
        //                TempData["MessageRegistration"] = "Data Saved Successfully!";
        //                return RedirectToAction("UserInfo");
        //            }
        //            else
        //            {
        //                return View(userInfo);
        //            }
        //        }
        //        return RedirectToAction("UserInfo");
        //    }
        //    catch
        //    {
        //        return View(userInfo);
        //    }
        //}

    }
}