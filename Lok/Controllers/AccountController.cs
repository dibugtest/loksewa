using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Lok.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets.Internal;

namespace Lok.Controllers
{
    public class AccountController : Controller
    {
        private IAuthinterface auth;
        public AccountController(IAuthinterface Auth)
        {
            this.auth = Auth;
        }
        // GET: Admin/Admins
     




        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        Role role = new Role();
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel l, string ReturnUrl)
        {

            ViewBag.ReturnUrl = ReturnUrl;


            if (await auth.IsUserExists(l.Email))
            {
                var login = auth.Login(l.Email, l.Password);
                Login user = await auth.GetUser(l.Email);
                string pass = user.RandomPass;


                if (login.Result != null)
                {
                    var Admin = login;

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        //var objAdmin = context.login.FirstOrDefault(a => (a.Email == l.Email));

                        //FormsAuthentication.SetAuthCookie(l.Email, false);

                        HttpContext.Session.SetString("id", Convert.ToString(user.Id));
                        HttpContext.Session.SetString("userEmail", user.Email);
                        //Session.Add("category", Admin.Role);

                        return Redirect(ReturnUrl);

                    }
                    else
                    {
                        const string Issuer = "my issuer";
                        
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, l.Email, ClaimValueTypes.String, Issuer));
                        //   claims.Add(new Claim(Constants., user., ClaimValueTypes.String, Constants.Issuer));
                        //  claims.Add(new Claim(Constants.CompanyClaimType, user.Company, ClaimValueTypes.String, Constants.Issuer));
                        claims.Add(new Claim(ClaimTypes.Role, user.Role, ClaimValueTypes.String, Issuer));

                        var userIdentity = new ClaimsIdentity("Debugsoft");
                        userIdentity.AddClaims(claims);

                        var userPrincipal = new ClaimsPrincipal(userIdentity);

                        await HttpContext.SignInAsync(
              "Cookie",userPrincipal,
               new AuthenticationProperties
               {
                   ExpiresUtc = DateTime.UtcNow.AddMinutes(100),
                   IsPersistent = false,
                   AllowRefresh = false
               });



                        return RedirectToAction("Index", "Post");
                       
                    }

                }
                else if (l.Password == pass)
                {
                    TempData["message"] = l.Email;
                    return RedirectToAction("NewPassword");
                }


                else
                {
                    ModelState.AddModelError("", "Invalid Password");
                }

            }
            ModelState.AddModelError("", "Invalid User");

            return View();


        }

        public ActionResult NewPassword()
        {

            return PartialView();

        }
        [HttpPost]
        public async Task<ActionResult> NewPassword(PasswordConform pass)
        {
            if (ModelState.IsValid)
            {
                string message = TempData["message"].ToString();
                var query = await auth.GetUser(message);
                    string password = pass.Password;
                Login login =await auth.ChangePass(query, password);
                //query.RandomPass = null;

                return RedirectToAction("Login");



            }
            return PartialView();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        //  public ActionResult Logout()
        //        {


        //            FormsAuthentication.SignOut();

        //            Session.Abandon();
        //            return RedirectToAction("Login");


        //        }



        //}
    }
}