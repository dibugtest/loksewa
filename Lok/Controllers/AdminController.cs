using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace Lok.Controllers
{
    [Authorize("Admin",AuthenticationSchemes ="AdminCookie")]
    public class AdminController : Controller
    {
            private readonly IAdminRepository _Admins;
            private readonly IUnitOfWork _uow;
        private readonly ILoginInterface _login;
        private readonly IEmailSender sender;
        private readonly IAuthinterface Auth;

        public AdminController(IAdminRepository Admins, IUnitOfWork uow, ILoginInterface Logins,IEmailSender sender, IAuthinterface Auth)
            {
                _Admins = Admins;
                _uow = uow;
            _login = Logins;
            this.sender = sender;
            this.Auth= Auth;
    }
            // GET: Admins
            public async Task<ActionResult> Index()
            {
                var Adminss = await _Admins.GetAll();
                return View(Adminss);
            }

            public ActionResult<Admins> Create()        
            {

                Admins value = new Admins();
           
            ViewBag.Role = new List<SelectListItem> { new SelectListItem { Text = "Admin", Value = "Admin" },
                                                      new SelectListItem { Text = "Moderator", Value = "Moderator" },
                                                      new SelectListItem { Text = "SuperAdmin", Value = "SuperAdmin" },

            };

            return View();
            }
            [HttpPost]
            public async Task<ActionResult<Admins>> Create(Admins value)
            {

                _Admins.Add(value);
            Random generator = new Random();
            string password = generator.Next(0, 999999).ToString("D6");
           await sender.SendEmailAsync(value.Email, "Your account is successfully created", "Please use this <b>"+ password +"</b> for login ");
            Login Login = new Login(){
                Email = value.Email,
                Role = value.Role,
                RandomPass=password,
               sentdate = DateTime.Now.ToShortDateString()


        };
            _login.Add(Login);
                await _uow.Commit();

                
                return RedirectToAction("Index");
            }
            [HttpGet]
            public async Task<ActionResult<Admins>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Admins = await _Admins.GetById(id);
                ViewBag.Role = new List<SelectListItem> { new SelectListItem { Text = "Admin", Value = "Admin" },
                                                      new SelectListItem { Text = "Moderator", Value = "Moderator" },
                                                      new SelectListItem { Text = "SuperAdmin", Value = "SuperAdmin" }, };

                    return View(Admins);
                }
                else
                    return BadRequest();

            }
            [HttpPost]
            public async Task<ActionResult<Admins>> Edit(string id, Admins value)
            {
                // var product = new Product(value.Id);
                value.Id = ObjectId.Parse(id);
            Admins admin = await _Admins.GetById(id);
       _Admins.Update(value, id);
            Login Login = new Login();
            Login =await Auth.GetUser(admin.Email);
            if (admin.Email != value.Email)
            {
                Random generator = new Random();
                string password = generator.Next(0, 999999).ToString("D6");
                await sender.SendEmailAsync(value.Email, "Your account is successfully created", "Please use this code <b>" + password + "</b> for login ");
                Login.Email = value.Email;
                Login.RandomPass = password;
                Login.Role = value.Role;
                Login.PasswordHash = null;
                Login.PasswordSalt = null;
                Login.sentdate = DateTime.Now.ToShortDateString();

            }
            else
            {
                Login.Role = value.Role;
              
            }

            _login.Update(Login,Convert.ToString(Login.Id));


            await _uow.Commit();

                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<ActionResult> Delete(string id)
            {
                _Admins.Remove(id);

                // it won't be null
                // var testAdmins = await _Admins.GetById(id);

                // If everything is ok then:
                await _uow.Commit();

                // not it must by null
                //  testAdmins = await _Admins.GetById(id);

                return RedirectToAction("Index");
            }
        

    }
}