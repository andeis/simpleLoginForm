using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SimpleTestLogin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SimpleTestLogin.Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.Username, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login Failed.");
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(SimpleTestLogin.Models.User user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new testEntities())
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    var enryptPass = crypto.Compute(user.Password);

                    var sysUser = db.admin.Create();

                    sysUser.Username = user.Username;
                    sysUser.Password = enryptPass;
                    sysUser.PasswordSalt = crypto.Salt;

                    db.admin.Add(sysUser);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home"); 
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        public ActionResult TestPageNoLayout()
        {
            return View();
        }

        private bool IsValid(string username, string password) {
            var crypto = new SimpleCrypto.PBKDF2();
            bool isValid = false;

            using (var db = new testEntities())
            {
                var user = db.admin.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

    }
}
