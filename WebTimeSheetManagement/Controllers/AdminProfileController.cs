using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTimeSheetManagement.Concrete;
using WebTimeSheetManagement.Filters;
using WebTimeSheetManagement.Interface;
using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Controllers
{
    public class AdminProfileController : Controller
    {
        // GET: AdminProfile
        ILogin _ILogin;
        public AdminProfileController()
        {
            _ILogin = new LoginConcrete();
        }

        // GET: UserProfile
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

    }
}