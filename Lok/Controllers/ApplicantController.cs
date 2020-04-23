using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Lok.Data.Interface;
using Lok.Models;
using Lok.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Lok.Filter;
using System.Threading;
using Lok.Extension;

namespace Lok.Controllers
{
    [Authorize("Applicant")]
    public class ApplicantController : Controller
    {
        private readonly IAuthinterface _auth;
        private readonly ILoginInterface _login;
        private readonly IApplicantRepository _Applicant;
        private readonly IReligionRepository _Religion;
        private readonly IEmploymentRepository _Employment;
        private readonly IOccupationRepository _Occupation;
        private readonly IVargaRepository _Varga;
        private readonly IDistrictRepository _District;
        private readonly IBoardNameRepository _BoardName;
        private readonly IEducationLevelRepository _EducationLevel;
        private readonly IFacultyRepository _Faculty;
        private readonly ISewaRepository _Sewa;
        private readonly IShreniTahaRepository _Shreni;
        private readonly IAwasthaRepository _Awastha;
        private readonly IApplicationRepository _Applications;
        private readonly IAdvertisiment _Advertisement;

        private readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;

        public ApplicantController(ILoginInterface Login, IAuthinterface Auth, IApplicantRepository Applicant, IReligionRepository Religion
                                    , IEmploymentRepository Employment, IOccupationRepository Occupation,
                                    IVargaRepository Varga, IDistrictRepository District, IBoardNameRepository BoardName, IEducationLevelRepository EducationLevel
                                    , IFacultyRepository Faculty, ISewaRepository Sewa, IAwasthaRepository Awastha,
                                    IShreniTahaRepository Shreni, IApplicationRepository Application, IAdvertisiment Advertisement, IUnitOfWork uow, IMapper mapper)
        {
            _auth = Auth;
            _login = Login;
            _Applicant = Applicant;
            _Religion = Religion;
            _Employment = Employment;
            _Occupation = Occupation;
            _Varga = Varga;
            _District = District;
            _BoardName = BoardName;
            _EducationLevel = EducationLevel;
            _Faculty = Faculty;
            _Sewa = Sewa;
            _Shreni = Shreni;
            _Awastha = Awastha;
            _uow = uow;
            _Applications = Application;
            _Advertisement = Advertisement;
            _mapper = mapper;
        }


        public async Task<ActionResult> Index()
        {
            var userIdentity = new ClaimsIdentity("Debugsoft");
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            Thread.CurrentPrincipal = userPrincipal;
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            var Email = ClaimTypes.Email;// identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                              //.Select(c => c.Value).SingleOrDefault();
            //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            //// Get the claims values
            //var name = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
            //                   .Select(c => c.Value).SingleOrDefault();

            //var principal = System.Security.Claims.ClaimsPrincipal.Current;
            //string Email = principal.FindFirst("Email").Value;

            //var identity = (System.Security.Claims.ClaimsIdentity)Context.User.Identity;
            //string fullname1 = identity.Claims.FirstOrDefault(c => c.Type == "FullName").Value;
            //Get the current claims principal
            // var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
           // string Email = ClaimTypes.Email;// ClaimsExtensions.GetId(identity);

            //if (identity == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //// Get the claims values
            Applicant applicant = await _Applicant.GetByEmail(Email);

            if (applicant != null)
            {
                return View(applicant);

            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationVM>> RegisterApplicant()
        {
            RegistrationVM register = new RegistrationVM();
            List<DropDownItem> Religions = (from r in await _Religion.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();

            List<DropDownItem> Occupations = (from r in await _Occupation.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Employments = (from r in await _Employment.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Vargas = (from r in await _Varga.GetAll()
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();

            Religions.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Occupations.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Employments.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Vargas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });


            register.Religions = new SelectList(Religions, "Id", "Name");
            register.Occupations = new SelectList(Occupations, "Id", "Name");
            register.Employments = new SelectList(Employments, "Id", "Name");
            register.Vargas = new SelectList(Vargas, "Id", "Name");
            return View(register);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Applicant>> RegisterApplicant(RegistrationVM register)
        {

            List<DropDownItem> Religions = (from r in await _Religion.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();

            List<DropDownItem> Occupations = (from r in await _Occupation.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Employments = (from r in await _Employment.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Vargas = (from r in await _Varga.GetAll()
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();

            Religions.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Occupations.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Employments.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Vargas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });


            register.Religions = new SelectList(Religions, "Id", "Name");
            register.Occupations = new SelectList(Occupations, "Id", "Name");
            register.Employments = new SelectList(Employments, "Id", "Name");
            register.Vargas = new SelectList(Vargas, "Id", "Name");

            if (ModelState.IsValid)
            {
                //if (!string.IsNullOrEmpty(register.Id))
                //{
                //    Applicant registeredApplicant = await _Applicant.GetById(register.Id);

                //    string EmailBody = "Your Reset Password is " + Base64Decode(registeredApplicant.RandomPassword) + ".Email Verification and Password Setup link." + "<a href='" + "http://localhost:5000/applicant/emailVerification/" + register.Id + "'>Link</a>";
                //    try
                //    {
                //        SendEMail(register.Email, "Email Verification Link.", EmailBody);

                //        registeredApplicant.PersonalInformation = _mapper.Map<PersonalInfo>(register);

                //        registeredApplicant.ExtraInformation = _mapper.Map<ExtraInfo>(register);

                //        registeredApplicant.EditedDate = DateTime.Now;
                //        registeredApplicant.CreatedBy = "Admin";//from login
                //        _Applicant.Update(registeredApplicant, register.Id);
                //        await _uow.Commit();

                //        TempData["Message"] = "Successfully Registered. Please check your Email and reset your password.";
                //        return RedirectToAction("PasswordReset", register.Id);
                //    }
                //    catch
                //    {
                //        ViewBag.Error = "Error";
                //        ModelState.AddModelError(string.Empty, "Failed to send mail. Please Try Again.");
                //        return View(register);
                //    }
                //}
                // if()
                {

                    Applicant applicant = new Applicant();
                    applicant.CreatedDate = DateTime.Now;
                    applicant.EditedDate = DateTime.Now;
                    applicant.CreatedBy = "Admin";
                    applicant.PersonalInformation = _mapper.Map<PersonalInfo>(register);

                    applicant.ExtraInformation = _mapper.Map<ExtraInfo>(register);

                    bool userExists = true;
                    if (await _login.GetAll() != null)
                    {
                        userExists = await _auth.GetUser(register.Email) != null ? true : false;
                    }
                    //Check if the applicant exist with email 
                    if (_Applicant.GetByEmail(register.Email) != null && !userExists)
                    {
                        string randString = RandomString(6);
                        applicant.RandomPassword = Base64Encode(randString);

                        //if not Add applicant to db
                        _Applicant.Add(applicant);

                        //  await _uow.Commit();

                        //Add User to Login
                        Login userLogin = new Login()
                        {
                            Email = register.Email,
                            RandomPass = Base64Encode(randString),
                            sentdate = DateTime.Now.ToString(),
                            Role = "Applicant"
                        };

                        _login.Add(userLogin);
                        await _uow.Commit();

                        if (applicant.Id != null && userLogin.Id != null)
                        {
                            //send Email for email verification
                            string EmailBody = "Your Reset Password is " + randString + ".Email Verification and Password Setup link." + "<a href='" + "http://localhost:5000/applicant/ResetPassword/'" + applicant.Id + ">Link</a>";
                            try
                            {
                                SendEMail(register.Email, "Email Verification Link.", EmailBody);
                                TempData["Message"] = "Successfully Registered. Please check your Email and reset your password.";
                                return View("ResetPassword", new { id = register.Id });

                            }
                            catch
                            {
                                register.Id = applicant.Id.ToString();
                                ViewBag.Error = "Error";
                                ModelState.AddModelError(string.Empty, "Failed to send mail.");
                                return View(register);
                            }

                        }
                        else
                        {
                            ViewBag.Error = "Error";
                            TempData["Message"] = "Successfully Registered. Email not sent.Your Random Password is " + randString + ".";
                            return View("ResetPassword");

                        }
                    }
                }
            }
            ViewBag.Error = "Error";
            ModelState.AddModelError(string.Empty, "Fill up the form properly.");
            return View(register);
        }

        [AllowAnonymous]
        public async Task<ActionResult<ResetPasswordVM>> ResetPassword(string id)
        {
            ResetPasswordVM reset = new ResetPasswordVM();
            return View(reset);
            //if (!string.IsNullOrEmpty(id))
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            //    if (applicant == null)
            //    {
            //        ViewBag.Error = "Error";
            //        ModelState.AddModelError(string.Empty, "Please try Again!");
            //        return View();
            //    }
            //    else
            //    {
            //        ResetPasswordVM reset = new ResetPasswordVM();
            //        reset.Id = applicant.Id.ToString();
            //        reset.Name = applicant.PersonalInformation.FirstName + " " + applicant.PersonalInformation.MiddleName + " " + applicant.PersonalInformation.LastName;
            //        return View(reset);
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("ApplicantLogin");
            //}
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ResetPasswordVM>> ResetPassword(ResetPasswordVM reset)
        {
            if (ModelState.IsValid)
            {
                Login userLogin = await _auth.GetUser(reset.Email);

                if (!String.IsNullOrEmpty(userLogin.RandomPass))
                {
                    await _auth.ChangePass(userLogin, reset.Password);
                    TempData["Message"] = "Successfully Reset Password. Login using new password.";
                    return RedirectToAction("Login");

                }
                else
                {
                    ModelState.AddModelError("", "Random Password Null.");
                    return View(reset);

                }
                //    if (userLogin != null)
                //    {
                //        if (Base64Decode(userLogin.RandomPass) == reset.RandPassword)
                //        {

                //        }
                //    }

                //    Applicant applicant = await _Applicant.GetById(reset.Id);
                //    if (applicant != null)
                //    {
                //        if (applicant.RandomPassword == Base64Encode(reset.RandPassword))
                //        {
                //            applicant.EmailVerification = true;
                //            applicant.RandomPassword = "";

                //            applicant.Password = new Passwords { Hash = reset.Password, Salt = reset.Password };

                //            applicant.QuestionAnswer = _mapper.Map<SecurityQA>(reset);

                //            _Applicant.Update(applicant, reset.Id);
                //            await _uow.Commit();

                //            TempData["Message"] = "Successfully Reset Password. Login using new password.";
                //            return View(reset);
                //        }
                //        else
                //        {
                //            ModelState.AddModelError(string.Empty, "Reset Password doesnot match.");
                //            ViewBag.Error = "Error";
                //            return View();
                //        }
                //    }
                //}
                //ViewBag.Error = "Error";
                //ModelState.AddModelError(string.Empty, "Please try Again!");
                //return View(reset);
            }
            else
            {
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Please try Again!");
                return View(reset);
            }
        }


        [AllowAnonymous]
        public async Task<ActionResult<LoginVM>> Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<LoginVM>> Login(LoginVM l)
        {
            if (await _auth.IsUserExists(l.Email))
            {
                var login = _auth.Login(l.Email, l.Password);
                Login user = await _auth.GetUser(l.Email);
                string pass = user.RandomPass;


                if (login.Result != null && user.Role == "Applicant")
                {
                    var Admin = login;
                    Applicant applicant = await _Applicant.GetByEmail(l.Email);

                    const string Issuer = "my issuer";

                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Email, l.Email, ClaimValueTypes.String, Issuer));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role, ClaimValueTypes.String, Issuer));
                    claims.Add(new Claim("Id", applicant.Id.ToString(), ClaimValueTypes.String, Issuer));

                    var userIdentity = new ClaimsIdentity("Debugsoft");
                    userIdentity.AddClaims(claims);

                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync("AdminCookie", userPrincipal,
                                                   new AuthenticationProperties
                                                   {
                                                       ExpiresUtc = DateTime.UtcNow.AddMinutes(100),
                                                       IsPersistent = false,
                                                       AllowRefresh = false
                                                   });

                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("", "Invalid User or  Password");
                }

            }
            ModelState.AddModelError("", "Invalid User or Password");

            return View();


            //    Applicant applicant = await _Applicant.GetByEmail(login.Email);
            //    if (applicant != null)
            //    {
            //        if (applicant.EmailVerification)
            //        {
            //            if (applicant.Password.Hash == login.Password)
            //            {
            //                CookieOptions mycookie = new CookieOptions();

            //                if (Response.Cookies != null)
            //                {
            //                    Response.Cookies.Delete("Id");
            //                }

            //                mycookie.Expires = DateTime.Now.AddMinutes(5);

            //                Response.Cookies.Append("Id", applicant.Id.ToString(), mycookie);
            //                //Response.Cookies.Delete(key);

            //                return RedirectToAction("Index");
            //            }
            //            else
            //            {
            //                ViewBag.Error = "Error";
            //                ModelState.AddModelError(string.Empty, "Email and Password not Matched.");
            //                return View(login);
            //            }

            //        }
            //        else
            //        {
            //            ViewBag.Error = "Error";
            //            ModelState.AddModelError(String.Empty, "Check your mail and reset your password");
            //            return View(login);
            //            // TempData["ErrorMessage"] = "Check your mail and reset your password.";
            //            //  return RedirectToAction("ResetPassword",new { Id = applicant.Id.ToString() });
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.Error = "Error";
            //        ModelState.AddModelError(string.Empty, "Please try Again.");
            //        return View(login);
            //    }
            //}
            // return View();

        }

        public async Task<ActionResult<PersonalVM>> Personal()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            PersonalVM personalVM = _mapper.Map<PersonalVM>(applicant.PersonalInformation);
            personalVM.Id = applicant.Id.ToString();

            return View(personalVM);


        }

        [HttpPost]
        public async Task<ActionResult<PersonalVM>> Personal(PersonalVM personalVM)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }

            if (ModelState.IsValid)
            {
                //Applicant applicant = await _Applicant.GetById(personalVM.Id);
                applicant.PersonalInformation = _mapper.Map<PersonalInfo>(personalVM);
                applicant.EditedDate = DateTime.Now;
                _Applicant.Update(applicant, personalVM.Id);
                await _uow.Commit();
                TempData["Message"] = "Successfully Updated Personal Information.";
                return RedirectToAction("Index");
                //return View();

            }
            else
            {
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(personalVM);
            }
        }

        public async Task<ActionResult<ExtraVM>> Extra()
        {
            //Get the current claims principal
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }


            List<DropDownItem> Religions = (from r in await _Religion.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();

            List<DropDownItem> Occupations = (from r in await _Occupation.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Employments = (from r in await _Employment.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Vargas = (from r in await _Varga.GetAll()
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();


            Religions.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Occupations.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Employments.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Vargas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;

            // Applicant applicant = await _Applicant.GetById(id);
            ExtraVM extraVM = _mapper.Map<ExtraVM>(applicant.ExtraInformation);
            extraVM.Id = applicant.Id.ToString();

            extraVM.Religions = new SelectList(Religions, "Id", "Name");
            extraVM.Occupations = new SelectList(Occupations, "Id", "Name");
            extraVM.Employments = new SelectList(Employments, "Id", "Name");
            extraVM.Vargas = new SelectList(Vargas, "Id", "Name");
            return View(extraVM);

        }

        [HttpPost]
        public async Task<ActionResult<ExtraVM>> Extra(ExtraVM extraVM)
        {
            //Get the current claims principal
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Religions = (from r in await _Religion.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();

            List<DropDownItem> Occupations = (from r in await _Occupation.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Employments = (from r in await _Employment.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
            List<DropDownItem> Vargas = (from r in await _Varga.GetAll()
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();


            Religions.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Occupations.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Employments.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Vargas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });


            extraVM.Religions = new SelectList(Religions, "Id", "Name");
            extraVM.Occupations = new SelectList(Occupations, "Id", "Name");
            extraVM.Employments = new SelectList(Employments, "Id", "Name");
            extraVM.Vargas = new SelectList(Vargas, "Id", "Name");
            if (ModelState.IsValid)
            {
                // Applicant applicant = await _Applicant.GetById(extraVM.Id);
                applicant.ExtraInformation = _mapper.Map<ExtraInfo>(extraVM);
                applicant.EditedDate = DateTime.Now;
                _Applicant.Update(applicant, extraVM.Id);
                await _uow.Commit();
                TempData["Message"] = "Successfully updated the extra informartion.";
                return RedirectToAction("Index");
                // return View();

            }
            else
            {
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(extraVM);
            }
        }

        public async Task<ActionResult<ContactVM>> Contact()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Districts = (from r in await _District.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();


            Districts.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            // string id = Request.Cookies != null ? Request.Cookies["Id"].ToString() : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);

            ContactVM contactVM = applicant.ContactInformation != null ? _mapper.Map<ContactVM>(applicant.ContactInformation) : new ContactVM();
            contactVM.Id = applicant.Id.ToString();
            contactVM.Districts = new SelectList(Districts, "Id", "Name");
            contactVM.Email = applicant.PersonalInformation.Email;
            contactVM.MobileNo = applicant.PersonalInformation.Mobile;

            return View(contactVM);
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        [HttpPost]
        public async Task<ActionResult<ContactVM>> Contact(ContactVM contactVM)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Districts = (from r in await _District.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();


            Districts.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            contactVM.Districts = new SelectList(Districts, "Id", "Name");
            if (ModelState.IsValid)
            {
                //Applicant applicant = await _Applicant.GetById(contactVM.Id);
                applicant.ContactInformation = _mapper.Map<ContactInfo>(contactVM);
                _Applicant.Update(applicant, contactVM.Id);
                await _uow.Commit();
                TempData["Message"] = "Successfully updated the contact information.";
                return RedirectToAction("Index");
                //return View();

            }
            else
            {
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(contactVM);
            }
        }

        public async Task<ActionResult> EducationIndex()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");
            }

            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            List<DropDownItem> BoardNames = (from r in await _BoardName.GetAll()
                                             select new DropDownItem
                                             {
                                                 Id = r.Id.ToString(),
                                                 Name = r.Name
                                             }).ToList();
            List<DropDownItem> EducationLevels = (from r in await _EducationLevel.GetAll()
                                                  select new DropDownItem
                                                  {
                                                      Id = r.Id.ToString(),
                                                      Name = r.Name
                                                  }).ToList();
            List<DropDownItem> Faculties = (from r in await _Faculty.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();

            // IEnumerable<EducationVM> edus = applicant.EducationInfos != null ? _mapper.Map<IEnumerable<EducationVM>>(applicant.EducationInfos) : null;


            IEnumerable<EducationVM> edus = (from e in applicant.EducationInfos
                                             from b in BoardNames
                                             where b.Id == e.BoardName
                                             from l in EducationLevels
                                             where l.Id == e.Level
                                             from f in Faculties
                                             where f.Id == e.Faculty
                                             select new EducationVM
                                             {
                                                 EId = e.EId,
                                                 BoardName = b.Name,
                                                 Level = l.Name,
                                                 Faculty = f.Name,
                                                 DivisionPercentage = e.DivisionPercentage,
                                                 MainSubject = e.MainSubject,
                                                 DegreeName = e.DegreeName,
                                                 EducationType = e.EducationType

                                             }).AsEnumerable();

            return View(edus);
            //}
            //else
            //{
            //    return RedirectToAction("login");
            //}
        }

        public async Task<ActionResult<EducationVM>> EducationCreate()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }

            List<DropDownItem> BoardNames = (from r in await _BoardName.GetAll()
                                             select new DropDownItem
                                             {
                                                 Id = r.Id.ToString(),
                                                 Name = r.Name
                                             }).ToList();
            List<DropDownItem> EducationLevels = (from r in await _EducationLevel.GetAll()
                                                  select new DropDownItem
                                                  {
                                                      Id = r.Id.ToString(),
                                                      Name = r.Name
                                                  }).ToList();
            List<DropDownItem> Faculties = (from r in await _Faculty.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();




            BoardNames.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            EducationLevels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Faculties.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });


            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            EducationVM educationVM = new EducationVM();
            educationVM.Id = applicant.Id.ToString();
            educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
            educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
            educationVM.Faculties = new SelectList(Faculties, "Id", "Name");

            return View(educationVM);

            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        [HttpPost]
        public async Task<ActionResult<EducationVM>> EducationCreate(EducationVM educationVM, IFormFile FileMain, IFormFile FileEquivalent)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> BoardNames = (from r in await _BoardName.GetAll()
                                             select new DropDownItem
                                             {
                                                 Id = r.Id.ToString(),
                                                 Name = r.Name
                                             }).ToList();
            List<DropDownItem> EducationLevels = (from r in await _EducationLevel.GetAll()
                                                  select new DropDownItem
                                                  {
                                                      Id = r.Id.ToString(),
                                                      Name = r.Name
                                                  }).ToList();
            List<DropDownItem> Faculties = (from r in await _Faculty.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();


            BoardNames.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            EducationLevels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Faculties.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            if (ModelState.IsValid)
            {
                if (FileMain != null || FileEquivalent != null)
                {
                    // Applicant applicant = await _Applicant.GetById(educationVM.Id);
                    List<EducationInfo> eduList = new List<EducationInfo>();
                    EducationInfo eduInfo = _mapper.Map<EducationInfo>(educationVM);
                    eduList.Add(eduInfo);
                    eduInfo.EId = applicant.EducationInfos != null ? (applicant.EducationInfos.ToList().Count + 1).ToString() : "1";
                    eduInfo.FileName = eduInfo.EId + ".pdf";
                    eduInfo.EquivalentFileName = eduInfo.EId + ".pdf";
                    applicant.EditedDate = DateTime.Now;
                    if (applicant.EducationInfos == null)
                    {
                        applicant.EducationInfos = eduList.AsEnumerable();
                        _Applicant.Update(applicant, educationVM.Id);
                    }
                    else
                    {
                        _Applicant.UpdateEducationInfo(eduInfo, educationVM.Id, educationVM.EId);
                    }

                    await _uow.Commit();



                    //File Upload

                    string directoryMain = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Education", "Main"
                                    );

                    string directoryEquivalent = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Education", "Equivalent"
                                    );

                    Directory.CreateDirectory(directoryMain); // no need to check if it exists
                    Directory.CreateDirectory(directoryEquivalent); // no need to check if it exists



                    if (FileMain != null)
                    {
                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Education", "Main",
                                    eduInfo.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileMain.CopyToAsync(stream);
                        }
                    }

                    if (FileEquivalent != null)
                    {
                        var path = Path.Combine(
                                   Directory.GetCurrentDirectory(), "wwwroot", "images",
                                   "applicant", applicant.Id.ToString(), "Education", "Equivalent",
                                   eduInfo.EquivalentFileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileEquivalent.CopyToAsync(stream);
                        }
                    }

                    TempData["Message"] = "Successfully Added education Information.";
                    return RedirectToAction("EducationIndex");
                }
                else
                {
                    educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
                    educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
                    educationVM.Faculties = new SelectList(Faculties, "Id", "Name");
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "File is necessary.");
                    return View(educationVM);
                }

            }
            else
            {
                educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
                educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
                educationVM.Faculties = new SelectList(Faculties, "Id", "Name");
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(educationVM);
            }
        }

        public async Task<ActionResult<EducationVM>> EducationEdit(string EId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            if (String.IsNullOrEmpty(EId))
            {
                return RedirectToAction("EducationIndex");
            }
            List<DropDownItem> BoardNames = (from r in await _BoardName.GetAll()
                                             select new DropDownItem
                                             {
                                                 Id = r.Id.ToString(),
                                                 Name = r.Name
                                             }).ToList();
            List<DropDownItem> EducationLevels = (from r in await _EducationLevel.GetAll()
                                                  select new DropDownItem
                                                  {
                                                      Id = r.Id.ToString(),
                                                      Name = r.Name
                                                  }).ToList();
            List<DropDownItem> Faculties = (from r in await _Faculty.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();




            BoardNames.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            EducationLevels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Faculties.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });


            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            EducationVM educationVM = new EducationVM();
            educationVM.EId = EId;

            if (applicant.EducationInfos != null)
            {
                EducationInfo eduInfo = applicant.EducationInfos.FirstOrDefault(m => m.EId == EId);
                if (eduInfo != null)
                {
                    educationVM = _mapper.Map<EducationVM>(eduInfo);
                }
            }

            educationVM.Id = applicant.Id.ToString();
            educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
            educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
            educationVM.Faculties = new SelectList(Faculties, "Id", "Name");

            //Check File Exists

            if (educationVM.FileName != null)
            {
                string mainFile = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Education", "Main", educationVM.FileName
                                    );

                if (System.IO.File.Exists(mainFile))
                {
                    educationVM.FileMainLink = "~/images/applicant/" + educationVM.Id + "/Education/Main/" + educationVM.FileName;
                }
            }
            if (educationVM.EquivalentFileName != null)
            {
                string EquivalentFile = Path.Combine(
                                   Directory.GetCurrentDirectory(), "wwwroot", "images",
                                   "applicant", applicant.Id.ToString(), "Education", "Equivalent", educationVM.EquivalentFileName
                                   );

                if (System.IO.File.Exists(EquivalentFile))
                {
                    educationVM.FileEquivalentLink = "~/images/applicant/" + educationVM.Id + "/Education/Equivalent/" + educationVM.EquivalentFileName;
                }
            }

            return View(educationVM);

            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        [HttpPost]
        public async Task<ActionResult<EducationVM>> EducationEdit(EducationVM educationVM, IFormFile FileMain, IFormFile FileEquivalent)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> BoardNames = (from r in await _BoardName.GetAll()
                                             select new DropDownItem
                                             {
                                                 Id = r.Id.ToString(),
                                                 Name = r.Name
                                             }).ToList();
            List<DropDownItem> EducationLevels = (from r in await _EducationLevel.GetAll()
                                                  select new DropDownItem
                                                  {
                                                      Id = r.Id.ToString(),
                                                      Name = r.Name
                                                  }).ToList();
            List<DropDownItem> Faculties = (from r in await _Faculty.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();


            BoardNames.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            EducationLevels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Faculties.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            if (ModelState.IsValid)
            {
                if (FileMain != null || FileEquivalent != null)
                {
                    // Applicant applicant = await _Applicant.GetById(educationVM.Id);
                    EducationInfo eduInfo = _mapper.Map<EducationInfo>(educationVM);
                    eduInfo.FileName = eduInfo.EId + ".pdf";
                    eduInfo.EquivalentFileName = eduInfo.EId + ".pdf";
                    applicant.EditedDate = DateTime.Now;
                    _Applicant.UpdateEducationInfo(eduInfo, educationVM.Id, educationVM.EId);
                    await _uow.Commit();

                    //File Upload

                    string directoryMain = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Education", "Main"
                                    );

                    string directoryEquivalent = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Education", "Equivalent"
                                    );

                    Directory.CreateDirectory(directoryMain); // no need to check if it exists
                    Directory.CreateDirectory(directoryEquivalent); // no need to check if it exists

                    if (FileMain != null)
                    {
                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Education", "Main",
                                    eduInfo.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileMain.CopyToAsync(stream);
                        }
                    }

                    if (FileEquivalent != null)
                    {
                        var path = Path.Combine(
                                   Directory.GetCurrentDirectory(), "wwwroot", "images",
                                   "applicant", applicant.Id.ToString(), "Education", "Equivalent",
                                   eduInfo.EquivalentFileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileEquivalent.CopyToAsync(stream);
                        }
                    }

                    TempData["Message"] = "Successfully Updated education Information.";
                    return RedirectToAction("EducationIndex");
                }
                else
                {
                    string mainFile = Path.Combine(
                                           Directory.GetCurrentDirectory(), "wwwroot", "images",
                                           "applicant", educationVM.Id.ToString(), "Education", "Main", educationVM.FileName
                                           );

                    if (System.IO.File.Exists(mainFile))
                    {
                        educationVM.FileMainLink = "~/images/applicant/" + educationVM.Id + "/Education/Main/" + educationVM.FileName;
                    }
                    string equivalentFile = Path.Combine(
                                           Directory.GetCurrentDirectory(), "wwwroot", "images",
                                           "applicant", educationVM.Id.ToString(), "Education", "Equivalent", educationVM.EquivalentFileName
                                           );

                    if (System.IO.File.Exists(equivalentFile))
                    {
                        educationVM.FileEquivalentLink = "~/images/applicant/" + educationVM.Id + "/Education/Equivalent/" + educationVM.EquivalentFileName;
                    }

                    educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
                    educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
                    educationVM.Faculties = new SelectList(Faculties, "Id", "Name");
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "File is necessary.");
                    return View(educationVM);
                }

            }
            else
            {
                string mainFile = Path.Combine(
                                           Directory.GetCurrentDirectory(), "wwwroot", "images",
                                           "applicant", educationVM.Id.ToString(), "Education", "Main", educationVM.FileName
                                           );

                if (System.IO.File.Exists(mainFile))
                {
                    educationVM.FileMainLink = "~/images/applicant/" + educationVM.Id + "/Education/Main/" + educationVM.FileName;
                }
                string equivalentFile = Path.Combine(
                                       Directory.GetCurrentDirectory(), "wwwroot", "images",
                                       "applicant", educationVM.Id.ToString(), "Education", "Equivalent", educationVM.EquivalentFileName
                                       );

                if (System.IO.File.Exists(equivalentFile))
                {
                    educationVM.FileEquivalentLink = "~/images/applicant/" + educationVM.Id + "/Education/Equivalent/" + educationVM.EquivalentFileName;
                }
                educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
                educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
                educationVM.Faculties = new SelectList(Faculties, "Id", "Name");
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(educationVM);
            }
        }


        public async Task<ActionResult> TrainingIndex()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }

            IEnumerable<TrainingVM> training = null;
            if (applicant.TrainingInfos != null)
            {
                training = (from t in applicant.TrainingInfos
                            select new TrainingVM
                            {
                                TId = t.TId,
                                DivisionPercentage = t.DivisionPercentage,
                                OrganizationName = t.OrganizationName,
                                TrainingName = t.TrainingName,
                                StartDate = t.StartDate,
                                EndDate = t.EndDate
                            }).AsEnumerable();
            }
            // IEnumerable<TrainingVM> training = applicant.TrainingInfos != null ? _mapper.Map<IEnumerable<TrainingInfo>, IEnumerable<TrainingVM>>(applicant.TrainingInfos.AsEnumerable()) : null;
            return View(training);


        }

        public async Task<ActionResult<TrainingVM>> TrainingCreate()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }


            TrainingVM trainVM = new TrainingVM();
            trainVM.Id = applicant.Id.ToString();
            return View(trainVM);

        }

        [HttpPost]
        public async Task<ActionResult<TrainingVM>> TrainingCreate(TrainingVM trainVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            if (ModelState.IsValid)
            {
                if (FileMain != null)
                {
                    // Applicant applicant = await _Applicant.GetById(trainVM.Id);
                    List<TrainingInfo> TrainList = new List<TrainingInfo>();
                    TrainingInfo trainInfo = _mapper.Map<TrainingInfo>(trainVM);
                    TrainList.Add(trainInfo);
                    trainInfo.TId = applicant.TrainingInfos != null ? (applicant.TrainingInfos.ToList().Count + 1).ToString() : "1";

                    trainInfo.FileName = trainInfo.TId + ".pdf";

                    applicant.EditedDate = DateTime.Now;
                    if (applicant.TrainingInfos == null)
                    {
                        applicant.TrainingInfos = TrainList.AsEnumerable();
                        _Applicant.Update(applicant, trainVM.Id);
                    }
                    else
                    {
                        _Applicant.UpdateTrainingInfo(trainInfo, trainVM.Id, null);

                    }
                    await _uow.Commit();



                    //File Upload

                    string directoryMain = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Training", "File"
                                    );

                    Directory.CreateDirectory(directoryMain); // no need to check if it exists


                    if (FileMain != null)
                    {
                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Training", "File",
                                    trainInfo.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileMain.CopyToAsync(stream);
                        }
                    }



                    TempData["Message"] = "Successfully Added Training Information.";
                    return RedirectToAction("TrainingIndex");
                }
                else
                {
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "File is Required.");
                    return View(trainVM);
                }

            }
            else
            {

                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(trainVM);
            }
        }

        public async Task<ActionResult<TrainingVM>> TrainingEdit(string TId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            if (TId == null)
            {
                return RedirectToAction("TrainingIndex");
            }
            //string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            TrainingVM trainVM = new TrainingVM();
            trainVM.TId = TId;
            if (applicant.TrainingInfos != null)
            {
                TrainingInfo trainInfo = applicant.TrainingInfos.FirstOrDefault(m => m.TId == TId);
                if (trainInfo != null)
                {
                    trainVM = _mapper.Map<TrainingVM>(trainInfo);
                }
            }

            trainVM.Id = applicant.Id.ToString();

            //Check File Exists

            if (trainVM.FileName != null)
            {
                string mainFile = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Training", "File", trainVM.FileName
                                    );

                if (System.IO.File.Exists(mainFile))
                {
                    trainVM.FileMainLink = "~/images/applicant/" + trainVM.Id + "/Training/File/" + trainVM.FileName;
                }
            }



            return View(trainVM);

            // }
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }

        [HttpPost]
        public async Task<ActionResult<TrainingVM>> TrainingEdit(TrainingVM trainVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            if (ModelState.IsValid)
            {
                // Applicant applicant = await _Applicant.GetById(trainVM.Id);
                TrainingInfo trainInfo = _mapper.Map<TrainingInfo>(trainVM);
                trainInfo.FileName = trainInfo.TId + ".pdf";
                applicant.EditedDate = DateTime.Now;
                _Applicant.UpdateTrainingInfo(trainInfo, trainVM.Id, trainVM.TId);

                await _uow.Commit();



                //File Upload

                string directoryMain = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "Training", "File"
                                );

                Directory.CreateDirectory(directoryMain); // no need to check if it exists


                if (FileMain != null)
                {
                    var path = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "Training", "File",
                                trainInfo.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await FileMain.CopyToAsync(stream);
                    }
                }

                TempData["Message"] = "Successfully Updated Training Information.";
                return RedirectToAction("TrainingIndex");
            }
            else
            {

                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error.");
                return View(trainVM);
            }

        }

        public async Task<ActionResult> CouncilIndex()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }

            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            // IEnumerable<ProfessionalCouncilVM> councils = applicant.ProfessionalCouncils != null ? _mapper.Map<IEnumerable<ProfessionalCouncilVM>>(applicant.ProfessionalCouncils) : null;
            if (applicant.ProfessionalCouncils != null)
            {
                IEnumerable<ProfessionalCouncilVM> councils = (from c in applicant.ProfessionalCouncils
                                                               select new ProfessionalCouncilVM
                                                               {
                                                                   PId = c.PId,
                                                                   ProviderName = c.ProviderName,
                                                                   RegistrationNo = c.RegistrationNo,
                                                                   RenewDate = c.RenewDate,
                                                                   ValidateFrom = c.ValidateFrom,
                                                                   Validity = c.Validity,
                                                                   Type = c.Type
                                                               }).AsEnumerable();
                return View(councils);
            }
            else
            {
                return View();
            }
            //}
            //else
            //{
            //    return RedirectToAction("login");
            //}

        }

        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilCreate()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }

            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //    if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            ProfessionalCouncilVM councilVM = new ProfessionalCouncilVM();
            councilVM.Id = applicant.Id.ToString();
            return View(councilVM);
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }
        [HttpPost]
        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilCreate(ProfessionalCouncilVM councilVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            if (ModelState.IsValid)
            {
                if (FileMain != null)
                {
                    // Applicant applicant = await _Applicant.GetById(councilVM.Id);
                    List<ProfessionalCouncil> councilList = new List<ProfessionalCouncil>();
                    ProfessionalCouncil council = _mapper.Map<ProfessionalCouncil>(councilVM);
                    councilList.Add(council);
                    council.PId = applicant.ProfessionalCouncils != null ? (applicant.ProfessionalCouncils.ToList().Count + 1).ToString() : "1";

                    council.FileName = council.PId + ".pdf";

                    applicant.EditedDate = DateTime.Now;
                    if (applicant.ProfessionalCouncils == null)
                    {
                        applicant.ProfessionalCouncils = councilList.AsEnumerable();
                        _Applicant.Update(applicant, councilVM.Id);
                    }
                    else
                    {
                        _Applicant.UpdateProfessionalCouncil(council, councilVM.Id, null);

                    }
                    await _uow.Commit();



                    //File Upload

                    string directoryMain = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Council", "File"
                                    );

                    Directory.CreateDirectory(directoryMain); // no need to check if it exists


                    if (FileMain != null)
                    {
                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Council", "File",
                                    council.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileMain.CopyToAsync(stream);
                        }
                    }



                    TempData["Message"] = "Successfully Added Professional Council Information.";
                    return RedirectToAction("CouncilIndex");
                }
                else
                {
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "File is Required.");
                    return View(councilVM);
                }

            }
            else
            {

                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(councilVM);
            }
        }
        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilEdit(string PId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            if (PId == null)
            {
                return RedirectToAction("CouncilIndex");
            }
            //string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            // Applicant applicant = await _Applicant.GetById(id);
            ProfessionalCouncilVM councilVM = new ProfessionalCouncilVM();
            councilVM.PId = PId;
            if (applicant.ProfessionalCouncils != null)
            {
                ProfessionalCouncil council = applicant.ProfessionalCouncils.FirstOrDefault(m => m.PId == PId);
                if (council != null)
                {
                    councilVM = _mapper.Map<ProfessionalCouncilVM>(council);
                }
            }

            councilVM.Id = applicant.Id.ToString();

            //Check File Exists

            if (councilVM.FileName != null)
            {
                string mainFile = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Council", "File", councilVM.FileName
                                    );

                if (System.IO.File.Exists(mainFile))
                {
                    councilVM.FileMainLink = "~/images/applicant/" + councilVM.Id + "/Council/File/" + councilVM.FileName;
                }
            }

            return View(councilVM);

            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }
        [HttpPost]
        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilEdit(ProfessionalCouncilVM councilVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            if (ModelState.IsValid)
            {
                // Applicant applicant = await _Applicant.GetById(councilVM.Id);
                ProfessionalCouncil council = _mapper.Map<ProfessionalCouncil>(councilVM);
                council.FileName = council.PId + ".pdf";
                applicant.EditedDate = DateTime.Now;
                _Applicant.UpdateProfessionalCouncil(council, councilVM.Id, councilVM.PId);

                await _uow.Commit();

                //File Upload

                string directoryMain = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "Council", "File"
                                );

                Directory.CreateDirectory(directoryMain); // no need to check if it exists


                if (FileMain != null)
                {
                    var path = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "Council", "File",
                                council.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await FileMain.CopyToAsync(stream);
                    }
                }

                TempData["Message"] = "Successfully Updated Professional Council Information.";
                return RedirectToAction("CouncilIndex");
            }
            else
            {

                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error.");
                return View(councilVM);
            }

        }


        public async Task<ActionResult<ExperienceVM>> ExperienceIndex()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //    if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            List<DropDownItem> Sewas = (from r in await _Sewa.GetAll()
                                        select new DropDownItem
                                        {
                                            Id = r.Id.ToString(),
                                            Name = r.Name
                                        }).ToList();

            List<DropDownItem> Shrenis = (from r in await _Shreni.GetAll()
                                          select new DropDownItem
                                          {
                                              Id = r.Id.ToString(),
                                              Name = r.Name
                                          }).ToList();
            List<DropDownItem> Awasthas = (from r in await _Awastha.GetAll()
                                           select new DropDownItem
                                           {
                                               Id = r.Id.ToString(),
                                               Name = r.Name
                                           }).ToList();
            ExperienceVM exp = new ExperienceVM();

            if (applicant.GovernmentInfos != null)
            {
                IEnumerable<GovernmentExperienceVM> government = (from g in applicant.GovernmentInfos
                                                                  from s in Sewas
                                                                  where s.Id == g.Sewa
                                                                  from a in Awasthas
                                                                  where a.Id == g.Awastha
                                                                  from t in Shrenis
                                                                  where t.Id == g.TahaShreni
                                                                  select new GovernmentExperienceVM
                                                                  {
                                                                      GId = g.GId,
                                                                      OfficeName = g.OfficeName,
                                                                      Sewa = s.Name,
                                                                      Awastha = a.Name,
                                                                      TahaShreni = t.Name,
                                                                      OfficeAddress = g.OfficeAddress,
                                                                      JobType = g.JobType,
                                                                      Post = g.Post,
                                                                      StartDate = g.StartDate,
                                                                      EndDate = g.EndDate
                                                                  }
                                                                ).AsEnumerable();
                exp.GovernmentExperience = government;
            }
            if (applicant.NonGovernmentInfos != null)
            {
                IEnumerable<NonGovernmentExperienceVM> nonGovernment = (from g in applicant.NonGovernmentInfos
                                                                        from s in Shrenis
                                                                        where s.Id == g.Level
                                                                        select new NonGovernmentExperienceVM
                                                                        {
                                                                            GId = g.GId,
                                                                            OfficeName = g.OfficeName,
                                                                            Post = g.Post,
                                                                            JobType = g.JobType,
                                                                            JobStartDate = g.JobStartDate,
                                                                            JobEndDate = g.JobEndDate
                                                                        }
                                                                ).AsEnumerable();
                exp.NonGovernmentExperience = nonGovernment;
            }
            // IEnumerable<GovernmentExperienceVM> government = applicant.GovernmentInfos != null ? _mapper.Map<IEnumerable<GovernmentExperienceVM>>(applicant.GovernmentInfos) : null;
            // IEnumerable<NonGovernmentExperienceVM> NonGovernment = applicant.NonGovernmentInfos != null ? _mapper.Map<IEnumerable<NonGovernmentExperienceVM>>(applicant.NonGovernmentInfos) : null;
            // exp.NonGovernmentExperience = NonGovernment;
            return View(exp);
            //}
            //else
            //{
            //    return RedirectToAction("login");
            //}

        }
        //Government Experience     

        public async Task<ActionResult<GovernmentExperienceVM>> Government()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Sewas = (from r in await _Sewa.GetAll()
                                        select new DropDownItem
                                        {
                                            Id = r.Id.ToString(),
                                            Name = r.Name
                                        }).ToList();

            List<DropDownItem> Shrenis = (from r in await _Shreni.GetAll()
                                          where r.Type == "Government"
                                          select new DropDownItem
                                          {
                                              Id = r.Id.ToString(),
                                              Name = r.Name
                                          }).ToList();
            List<DropDownItem> Awasthas = (from r in await _Awastha.GetAll()
                                           select new DropDownItem
                                           {
                                               Id = r.Id.ToString(),
                                               Name = r.Name
                                           }).ToList();

            Sewas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Awasthas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Shrenis.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });


            //string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            GovernmentExperienceVM governmentVM = new GovernmentExperienceVM();
            governmentVM.Id = applicant.Id.ToString();
            governmentVM.ShreniTahas = new SelectList(Shrenis, "Id", "Name");
            governmentVM.Awasthas = new SelectList(Awasthas, "Id", "Name");
            governmentVM.Sewas = new SelectList(Sewas, "Id", "Name");
            return View(governmentVM);

            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        [HttpPost]
        public async Task<ActionResult<GovernmentExperienceVM>> Government(GovernmentExperienceVM governmentVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Sewas = (from r in await _Sewa.GetAll()
                                        select new DropDownItem
                                        {
                                            Id = r.Id.ToString(),
                                            Name = r.Name
                                        }).ToList();

            List<DropDownItem> Shrenis = (from r in await _Shreni.GetAll()
                                          where r.Type == "Government"
                                          select new DropDownItem
                                          {
                                              Id = r.Id.ToString(),
                                              Name = r.Name
                                          }).ToList();
            List<DropDownItem> Awasthas = (from r in await _Awastha.GetAll()
                                           select new DropDownItem
                                           {
                                               Id = r.Id.ToString(),
                                               Name = r.Name
                                           }).ToList();

            Sewas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Awasthas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Shrenis.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            if (ModelState.IsValid)
            {
                if (FileMain != null)
                {
                    //  Applicant applicant = await _Applicant.GetById(governmentVM.Id);
                    List<GovernmentExperienceInfo> govList = new List<GovernmentExperienceInfo>();
                    GovernmentExperienceInfo governmentInfo = _mapper.Map<GovernmentExperienceInfo>(governmentVM);
                    govList.Add(governmentInfo);
                    governmentInfo.GId = applicant.GovernmentInfos != null ? (applicant.GovernmentInfos.ToList().Count + 1).ToString() : "1";
                    governmentInfo.FileName = governmentInfo.GId + ".pdf";
                    applicant.CreatedDate = DateTime.Now;
                    applicant.CreatedBy = "self";
                    applicant.EditedDate = DateTime.Now;
                    if (applicant.GovernmentInfos == null)
                    {
                        applicant.GovernmentInfos = govList.AsEnumerable();
                        _Applicant.Update(applicant, governmentVM.Id);
                    }
                    else
                    {
                        _Applicant.UpdateGovernmentInfo(governmentInfo, governmentVM.Id, governmentVM.GId);

                    }
                    await _uow.Commit();



                    //File Upload

                    string directoryMain = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Government", "File"
                                    );



                    Directory.CreateDirectory(directoryMain); // no need to check if it exists



                    if (FileMain != null)
                    {
                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "Government", "File",
                                    governmentInfo.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileMain.CopyToAsync(stream);
                        }
                    }



                    TempData["Message"] = "Successfully Added Government Experience Information.";
                    return RedirectToAction("ExperienceIndex");
                }
                else
                {
                    governmentVM.Awasthas = new SelectList(Awasthas, "Id", "Name");
                    governmentVM.Sewas = new SelectList(Sewas, "Id", "Name");
                    governmentVM.ShreniTahas = new SelectList(Shrenis, "Id", "Name");
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "File is Required.");
                    return View(governmentVM);
                }

            }
            else
            {
                governmentVM.Awasthas = new SelectList(Awasthas, "Id", "Name");
                governmentVM.Sewas = new SelectList(Sewas, "Id", "Name");
                governmentVM.ShreniTahas = new SelectList(Shrenis, "Id", "Name");
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(governmentVM);
            }
        }


        public async Task<ActionResult<GovernmentExperienceVM>> GovernmentEdit(string GId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Sewas = (from r in await _Sewa.GetAll()
                                        select new DropDownItem
                                        {
                                            Id = r.Id.ToString(),
                                            Name = r.Name
                                        }).ToList();

            List<DropDownItem> Shrenis = (from r in await _Shreni.GetAll()
                                          where r.Type == "Government"
                                          select new DropDownItem
                                          {
                                              Id = r.Id.ToString(),
                                              Name = r.Name
                                          }).ToList();
            List<DropDownItem> Awasthas = (from r in await _Awastha.GetAll()
                                           select new DropDownItem
                                           {
                                               Id = r.Id.ToString(),
                                               Name = r.Name
                                           }).ToList();

            Sewas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Awasthas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Shrenis.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            //string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            GovernmentExperienceVM governmentVM = new GovernmentExperienceVM();
            governmentVM.GId = GId;

            if (applicant.GovernmentInfos != null)
            {
                GovernmentExperienceInfo governmentInfo = applicant.GovernmentInfos.FirstOrDefault(m => m.GId == GId);
                if (governmentInfo != null)
                {
                    governmentVM = _mapper.Map<GovernmentExperienceVM>(governmentInfo);
                }
            }

            governmentVM.Id = applicant.Id.ToString();
            governmentVM.ShreniTahas = new SelectList(Shrenis, "Id", "Name");
            governmentVM.Awasthas = new SelectList(Awasthas, "Id", "Name");
            governmentVM.Sewas = new SelectList(Sewas, "Id", "Name");

            //Check File Exists
            if (GId != null)
            {
                if (governmentVM.FileName != null)
                {
                    string mainFile = Path.Combine(
                                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                                        "applicant", applicant.Id.ToString(), "Government", "File", governmentVM.FileName
                                        );

                    if (System.IO.File.Exists(mainFile))
                    {
                        governmentVM.FileMainLink = "~/images/applicant/" + governmentVM.Id + "/Government/File/" + governmentVM.FileName;
                    }
                }

            }


            return View(governmentVM);

            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        [HttpPost]
        public async Task<ActionResult<GovernmentExperienceVM>> GovernmentEdit(GovernmentExperienceVM governmentVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Sewas = (from r in await _Sewa.GetAll()
                                        select new DropDownItem
                                        {
                                            Id = r.Id.ToString(),
                                            Name = r.Name
                                        }).ToList();

            List<DropDownItem> Shrenis = (from r in await _Shreni.GetAll()
                                          where r.Type == "Government"
                                          select new DropDownItem
                                          {
                                              Id = r.Id.ToString(),
                                              Name = r.Name
                                          }).ToList();
            List<DropDownItem> Awasthas = (from r in await _Awastha.GetAll()
                                           select new DropDownItem
                                           {
                                               Id = r.Id.ToString(),
                                               Name = r.Name
                                           }).ToList();

            Sewas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Awasthas.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            Shrenis.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            if (ModelState.IsValid)
            {

                // Applicant applicant = await _Applicant.GetById(governmentVM.Id);

                GovernmentExperienceInfo govInfo = _mapper.Map<GovernmentExperienceInfo>(governmentVM);
                govInfo.FileName = govInfo.GId + ".pdf";
                applicant.EditedDate = DateTime.Now;
                _Applicant.UpdateGovernmentInfo(govInfo, governmentVM.Id, governmentVM.GId);
                await _uow.Commit();



                //File Upload

                string directoryMain = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "Government", "File"
                                );



                Directory.CreateDirectory(directoryMain); // no need to check if it exists


                if (FileMain != null)
                {
                    var path = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "Government", "File",
                                govInfo.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await FileMain.CopyToAsync(stream);
                    }
                }



                TempData["Message"] = "Successfully Updated Government Experience Information.";
                return RedirectToAction("ExperienceIndex");

            }
            else
            {
                governmentVM.ShreniTahas = new SelectList(Shrenis, "Id", "Name");
                governmentVM.Sewas = new SelectList(Sewas, "Id", "Name");
                governmentVM.Awasthas = new SelectList(Awasthas, "Id", "Name");
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(governmentVM);
            }
        }
        //Non Government Experience

        public async Task<ActionResult<NonGovernmentExperienceVM>> NonGovernment()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Levels = (from r in await _Shreni.GetAll()
                                         where r.Type == "Non-Government"
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();


            Levels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });


            //string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            NonGovernmentExperienceVM nonGovVM = new NonGovernmentExperienceVM();
            nonGovVM.Id = applicant.Id.ToString();
            nonGovVM.Levels = new SelectList(Levels, "Id", "Name");
            return View(nonGovVM);

            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        [HttpPost]
        public async Task<ActionResult<GovernmentExperienceVM>> NonGovernment(NonGovernmentExperienceVM nonGovVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Levels = (from r in await _Shreni.GetAll()
                                         where r.Type == "Non-Government"
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();

            Levels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            if (ModelState.IsValid)
            {
                if (FileMain != null)
                {
                    // Applicant applicant = await _Applicant.GetById(nonGovVM.Id);
                    List<NonGovernmentExperienceInfo> nonGovList = new List<NonGovernmentExperienceInfo>();
                    NonGovernmentExperienceInfo nonGovInfo = _mapper.Map<NonGovernmentExperienceInfo>(nonGovVM);
                    nonGovList.Add(nonGovInfo);
                    nonGovInfo.GId = applicant.NonGovernmentInfos != null ? (applicant.NonGovernmentInfos.ToList().Count + 1).ToString() : "1";
                    nonGovInfo.FileName = nonGovInfo.GId + ".pdf";
                    applicant.CreatedDate = DateTime.Now;
                    applicant.CreatedBy = "self";
                    applicant.EditedDate = DateTime.Now;
                    if (applicant.NonGovernmentInfos == null)
                    {
                        applicant.NonGovernmentInfos = nonGovList.AsEnumerable();
                        _Applicant.Update(applicant, nonGovVM.Id);
                    }
                    else
                    {
                        _Applicant.UpdateNonGovernmentInfo(nonGovInfo, nonGovVM.Id, nonGovVM.GId);

                    }
                    await _uow.Commit();



                    //File Upload

                    string directoryMain = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "NonGovernment", "File"
                                    );



                    Directory.CreateDirectory(directoryMain); // no need to check if it exists



                    if (FileMain != null)
                    {
                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "images",
                                    "applicant", applicant.Id.ToString(), "NonGovernment", "File",
                                    nonGovInfo.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await FileMain.CopyToAsync(stream);
                        }
                    }



                    TempData["Message"] = "Successfully Added Non Government Experience Information.";
                    return RedirectToAction("ExperienceIndex");
                }
                else
                {
                    nonGovVM.Levels = new SelectList(Levels, "Id", "Name");
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "File is Required.");
                    return View(nonGovVM);
                }

            }
            else
            {
                nonGovVM.Levels = new SelectList(Levels, "Id", "Name");
                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(nonGovVM);
            }
        }


        public async Task<ActionResult<NonGovernmentExperienceVM>> NonGovernmentEdit(string GId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }

            List<DropDownItem> Levels = (from r in await _Shreni.GetAll()
                                         where r.Type == "Non-Government"
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();

            Levels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            //string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            NonGovernmentExperienceVM nonGovVM = new NonGovernmentExperienceVM();
            nonGovVM.GId = GId;

            if (applicant.NonGovernmentInfos != null)
            {
                NonGovernmentExperienceInfo nonGovInfo = applicant.NonGovernmentInfos.FirstOrDefault(m => m.GId == GId);
                if (nonGovInfo != null)
                {
                    nonGovVM = _mapper.Map<NonGovernmentExperienceVM>(nonGovInfo);
                }
            }

            nonGovVM.Id = applicant.Id.ToString();
            nonGovVM.Levels = new SelectList(Levels, "Id", "Name");

            //Check File Exists
            if (GId != null)
            {
                if (nonGovVM.FileName != null)
                {
                    string mainFile = Path.Combine(
                                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                                        "applicant", applicant.Id.ToString(), "NonGovernment", "File", nonGovVM.FileName
                                        );

                    if (System.IO.File.Exists(mainFile))
                    {
                        nonGovVM.FileMainLink = "~/images/applicant/" + nonGovVM.Id + "/NonGovernment/File/" + nonGovVM.FileName;
                    }
                }

            }


            return View(nonGovVM);

            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        [HttpPost]
        public async Task<ActionResult<NonGovernmentExperienceVM>> NonGovernmentEdit(NonGovernmentExperienceVM nonGovVM, IFormFile FileMain)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Levels = (from r in await _Shreni.GetAll()
                                         where r.Type == "Non-Government"
                                         select new DropDownItem
                                         {
                                             Id = r.Id.ToString(),
                                             Name = r.Name
                                         }).ToList();


            Levels.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            if (ModelState.IsValid)
            {

                //  Applicant applicant = await _Applicant.GetById(nonGovVM.Id);

                NonGovernmentExperienceInfo nonGovInfo = _mapper.Map<NonGovernmentExperienceInfo>(nonGovVM);
                nonGovInfo.FileName = nonGovInfo.GId + ".pdf";
                applicant.EditedDate = DateTime.Now;
                _Applicant.UpdateNonGovernmentInfo(nonGovInfo, nonGovVM.Id, nonGovVM.GId);
                await _uow.Commit();



                //File Upload

                string directoryMain = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "NonGovernment", "File"
                                );



                Directory.CreateDirectory(directoryMain); // no need to check if it exists


                if (FileMain != null)
                {
                    var path = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images",
                                "applicant", applicant.Id.ToString(), "NonGovernment", "File",
                                nonGovInfo.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await FileMain.CopyToAsync(stream);
                    }
                }



                TempData["Message"] = "Successfully Updated NonGovernment Experience Information.";
                return RedirectToAction("ExperienceIndex");


            }
            else
            {
                nonGovVM.Levels = new SelectList(Levels, "Id", "Name");

                ViewBag.Error = "Error";
                ModelState.AddModelError(string.Empty, "Error");
                return View(nonGovVM);
            }
        }
        public async Task<ActionResult<UploadVM>> Upload()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //    if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            UploadVM uploadVM = new UploadVM();
            Upload upload = new Upload();
            if (applicant.Uploads != null)
            {
                upload = applicant.Uploads;
                uploadVM = _mapper.Map<UploadVM>(upload);
                uploadVM.PhotographLink = upload.Photograph != null ? "~/images/Applicant/" + applicant.Id.ToString() + "/Upload/" + upload.Photograph : "";
                uploadVM.CitizenshipLink = upload.Citizenship != null ? "~/images/Applicant/" + applicant.Id.ToString() + "/Upload/" + upload.Citizenship : "";
                uploadVM.SignatureLink = upload.Signature != null ? "~/images/Applicant/" + applicant.Id.ToString() + "/Upload/" + upload.Signature : "";
                uploadVM.InclusionGroupLink = upload.InclusionGroup != null ? "~/images/Applicant/" + applicant.Id.ToString() + "/Upload/" + upload.InclusionGroup : "";
            }
            uploadVM.Id = applicant.Id.ToString();

            return View(uploadVM);
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }
        [HttpPost]
        public async Task<ActionResult<UploadVM>> Upload(UploadVM uploadVM, IFormFile PhotographFile,
                                                       IFormFile SignatureFile, IFormFile CitizenshipFile, IFormFile InclusionGroupFile)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            try
            {
                if (uploadVM.Id != null)
                {
                    //  Applicant applicant = await _Applicant.GetById(uploadVM.Id);
                    if (applicant != null)
                    {

                        Upload upload = new Upload();
                        if (applicant.Uploads != null)
                        {
                            upload = applicant.Uploads;
                        }
                        string directoryUpload = Path.Combine(
                                       Directory.GetCurrentDirectory(), "wwwroot", "images",
                                       "applicant", applicant.Id.ToString(), "Upload"
                                       );


                        Directory.CreateDirectory(directoryUpload); // no need to check if it exists


                        if (PhotographFile != null)
                        {
                            upload.Photograph = "Photo.jpg";
                            var path = Path.Combine(
                                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                                        "applicant", applicant.Id.ToString(), "Upload",
                                        upload.Photograph);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await PhotographFile.CopyToAsync(stream);
                            }
                        }
                        if (SignatureFile != null)
                        {
                            upload.Signature = "Signature.jpg";
                            var path = Path.Combine(
                                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                                        "applicant", applicant.Id.ToString(), "Upload",
                                        upload.Signature);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await SignatureFile.CopyToAsync(stream);
                            }
                        }
                        if (CitizenshipFile != null)
                        {
                            upload.Citizenship = "Citizenship.jpg";
                            var path = Path.Combine(
                                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                                        "applicant", applicant.Id.ToString(), "Upload",
                                        upload.Citizenship);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await CitizenshipFile.CopyToAsync(stream);
                            }
                        }
                        if (InclusionGroupFile != null)
                        {
                            upload.InclusionGroup = "Inclusiongroup.jpg";
                            var path = Path.Combine(
                                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                                        "applicant", applicant.Id.ToString(), "Upload",
                                        upload.InclusionGroup);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await InclusionGroupFile.CopyToAsync(stream);
                            }
                        }

                        applicant.Uploads = upload;
                        _Applicant.Update(applicant, applicant.Id.ToString());
                        await _uow.Commit();
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                    TempData["Message"] = "Successfully uploded the files.";
                    return RedirectToAction("upload");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Upload");
            }
        }


        public async Task<ActionResult<PreviewVM>> Preview()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            //    //Get the current claims principal
            //    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            //if (identity == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //// Get the claims values
            //string id = identity.Claims.Where(c => c.Type.Contains("ApplicantId"))
            //                   .Select(c => c.Value).SingleOrDefault();

            ////string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            PreviewVM preview = new PreviewVM();
            //  Applicant applicant = await _Applicant.GetById(id);

            if (applicant != null)
            {
                List<DropDownItem> Religions = (from r in await _Religion.GetAll()
                                                select new DropDownItem
                                                {
                                                    Id = r.Id.ToString(),
                                                    Name = r.Name
                                                }).ToList();

                List<DropDownItem> Occupations = (from r in await _Occupation.GetAll()
                                                  select new DropDownItem
                                                  {
                                                      Id = r.Id.ToString(),
                                                      Name = r.Name
                                                  }).ToList();
                List<DropDownItem> Employments = (from r in await _Employment.GetAll()
                                                  select new DropDownItem
                                                  {
                                                      Id = r.Id.ToString(),
                                                      Name = r.Name
                                                  }).ToList();
                List<DropDownItem> Vargas = (from r in await _Varga.GetAll()
                                             select new DropDownItem
                                             {
                                                 Id = r.Id.ToString(),
                                                 Name = r.Name
                                             }).ToList();
                List<DropDownItem> Districts = (from r in await _District.GetAll()
                                                select new DropDownItem
                                                {
                                                    Id = r.Id.ToString(),
                                                    Name = r.Name
                                                }).ToList();

                List<DropDownItem> BoardNames = (from r in await _BoardName.GetAll()
                                                 select new DropDownItem
                                                 {
                                                     Id = r.Id.ToString(),
                                                     Name = r.Name
                                                 }).ToList();
                List<DropDownItem> EducationLevels = (from r in await _EducationLevel.GetAll()
                                                      select new DropDownItem
                                                      {
                                                          Id = r.Id.ToString(),
                                                          Name = r.Name
                                                      }).ToList();
                List<DropDownItem> Faculties = (from r in await _Faculty.GetAll()
                                                select new DropDownItem
                                                {
                                                    Id = r.Id.ToString(),
                                                    Name = r.Name
                                                }).ToList();

                List<DropDownItem> Sewas = (from r in await _Sewa.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();

                List<DropDownItem> Shrenis = (from r in await _Shreni.GetAll()
                                              select new DropDownItem
                                              {
                                                  Id = r.Id.ToString(),
                                                  Name = r.Name
                                              }).ToList();
                List<DropDownItem> Awasthas = (from r in await _Awastha.GetAll()
                                               select new DropDownItem
                                               {
                                                   Id = r.Id.ToString(),
                                                   Name = r.Name
                                               }).ToList();
                List<DropDownItem> District = (from r in await _District.GetAll()
                                               select new DropDownItem
                                               {
                                                   Id = r.Id.ToString(),
                                                   Name = r.Name
                                               }).ToList();

                preview.Personal = _mapper.Map<PersonalVM>(applicant.PersonalInformation);
                preview.Extra = _mapper.Map<ExtraVM>(applicant.ExtraInformation);
                preview.Contact = _mapper.Map<ContactVM>(applicant.ContactInformation);
                preview.Contact.District = District.FirstOrDefault(m => m.Id == preview.Contact.District).Name;

                //Uploaded documents
                preview.Upload = _mapper.Map<UploadVM>(applicant.Uploads);
                preview.Upload.PhotographLink = "~/images/applicant/" + applicant.Id.ToString() + "/upload/photo.jpg";
                preview.Upload.SignatureLink = "~/images/applicant/" + applicant.Id.ToString() + "/upload/signature.jpg";

                //education Informations
                if (applicant.EducationInfos != null)
                {
                    preview.Educations = (from e in applicant.EducationInfos
                                          from b in BoardNames
                                          where b.Id == e.BoardName
                                          from l in EducationLevels
                                          where l.Id == e.Level
                                          from f in Faculties
                                          where f.Id == e.Faculty
                                          select new EducationVM
                                          {
                                              EId = e.EId,
                                              BoardName = b.Name,
                                              Level = l.Name,
                                              Faculty = f.Name,
                                              DivisionPercentage = e.DivisionPercentage,
                                              MainSubject = e.MainSubject,
                                              DegreeName = e.DegreeName,
                                              EducationType = e.EducationType

                                          }).AsEnumerable();

                }

                //TrainingInfos 
                if (applicant.TrainingInfos != null)
                {
                    preview.Trainings = (from t in applicant.TrainingInfos
                                         select new TrainingVM
                                         {
                                             TId = t.TId,
                                             DivisionPercentage = t.DivisionPercentage,
                                             OrganizationName = t.OrganizationName,
                                             TrainingName = t.TrainingName,
                                             StartDate = t.StartDate,
                                             EndDate = t.EndDate
                                         }).AsEnumerable();
                }

                //Council Infos
                if (applicant.ProfessionalCouncils != null)
                {
                    preview.Councils = (from c in applicant.ProfessionalCouncils
                                        select new ProfessionalCouncilVM
                                        {
                                            PId = c.PId,
                                            ProviderName = c.ProviderName,
                                            RegistrationNo = c.RegistrationNo,
                                            RenewDate = c.RenewDate,
                                            ValidateFrom = c.ValidateFrom,
                                            Validity = c.Validity,
                                            Type = c.Type
                                        }).AsEnumerable();
                }

                if (applicant.GovernmentInfos != null)
                {
                    preview.Governments = (from g in applicant.GovernmentInfos
                                           from s in Sewas
                                           where s.Id == g.Sewa
                                           from a in Awasthas
                                           where a.Id == g.Awastha
                                           from t in Shrenis
                                           where t.Id == g.TahaShreni
                                           select new GovernmentExperienceVM
                                           {
                                               GId = g.GId,
                                               OfficeName = g.OfficeName,
                                               Sewa = s.Name,
                                               Awastha = a.Name,
                                               TahaShreni = t.Name,
                                               OfficeAddress = g.OfficeAddress,
                                               JobType = g.JobType,
                                               Post = g.Post,
                                               StartDate = g.StartDate,
                                               EndDate = g.EndDate
                                           }
                                                                    ).AsEnumerable();
                }

                //Non Government Infos
                if (applicant.NonGovernmentInfos != null)
                {
                    preview.NonGovernments = (from g in applicant.NonGovernmentInfos
                                              from s in Shrenis
                                              where s.Id == g.Level
                                              select new NonGovernmentExperienceVM
                                              {
                                                  GId = g.GId,
                                                  OfficeName = g.OfficeName,
                                                  Post = g.Post,
                                                  JobType = g.JobType,
                                                  JobStartDate = g.JobStartDate,
                                                  JobEndDate = g.JobEndDate
                                              }
                                                                    ).AsEnumerable();
                }

                return View(preview);
            }

            return View(preview);
            //}

            //return RedirectToAction("Login");

        }

        //Delete the Education Record
        public async Task<ActionResult> DeleteEducation(string EId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            //    //Get the current claims principal
            //    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            //if (identity == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //// Get the claims values
            //string id = identity.Claims.Where(c => c.Type.Contains("ApplicantId"))
            //                   .Select(c => c.Value).SingleOrDefault();
            ////string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            // Applicant applicant = await _Applicant.GetById(id);
            if (applicant != null)
            {
                if (applicant.EducationInfos != null)
                {
                    EducationInfo eduInfo = applicant.EducationInfos.FirstOrDefault(m => m.EId == EId);

                    _Applicant.DeleteEducationInfo(applicant.Id.ToString(), EId);
                    await _uow.Commit();


                    //Delete File if exists
                    string equivalent = Path.Combine(
                                            Directory.GetCurrentDirectory(), "wwwroot", "images",
                                            "applicant", applicant.Id.ToString(), "Education", "Equivalent", eduInfo.EquivalentFileName
                                         );
                    string main = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",
                                                                   "applicant", applicant.Id.ToString(), "Education", "main", eduInfo.FileName
                                                                );



                    if (System.IO.File.Exists(main))
                    {
                        System.IO.File.Delete(main);
                    }


                    if (System.IO.File.Exists(equivalent))
                    {
                        System.IO.File.Delete(equivalent);
                    }
                    TempData["Message"] = "Record Deleted Successfully.";
                    return RedirectToAction("EducationIndex");
                }
                else
                {
                    return RedirectToAction("EducationIndex");
                }
            }
            else
            {
                return RedirectToAction("login");
            }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }


        //Delete the Training Record
        public async Task<ActionResult> DeleteTraining(string TId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //    if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            if (applicant != null)
            {
                if (applicant.TrainingInfos != null)
                {
                    TrainingInfo trainInfo = applicant.TrainingInfos.FirstOrDefault(m => m.TId == TId);

                    _Applicant.DeleteTrainingInfo(applicant.Id.ToString(), TId);
                    await _uow.Commit();


                    //Delete File if exists
                    string main = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",
                                                                     "applicant", applicant.Id.ToString(), "Training", "File", trainInfo.FileName
                                                                  );



                    if (System.IO.File.Exists(main))
                    {
                        System.IO.File.Delete(main);
                    }


                    TempData["Message"] = "Record Deleted Successfully.";
                    return RedirectToAction("TrainingIndex");
                }
                else
                {
                    return RedirectToAction("TrainingINdex");
                }
            }
            else
            {
                return RedirectToAction("login");
            }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }

        //Delete the Council Record
        public async Task<ActionResult> DeleteCouncil(string PId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            ////string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            if (applicant != null)
            {
                if (applicant.ProfessionalCouncils != null)
                {
                    ProfessionalCouncil councilInfo = applicant.ProfessionalCouncils.FirstOrDefault(m => m.PId == PId);

                    _Applicant.DeleteProfessionalCouncil(applicant.Id.ToString(), PId);
                    await _uow.Commit();


                    //Delete File if exists
                    string main = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",
                                                                     "applicant", applicant.Id.ToString(), "Council", "File", councilInfo.FileName
                                                                  );



                    if (System.IO.File.Exists(main))
                    {
                        System.IO.File.Delete(main);
                    }


                    TempData["Message"] = "Record Deleted Successfully.";
                    return RedirectToAction("CouncilIndex");
                }
                else
                {
                    return RedirectToAction("CouncilIndex");
                }
            }
            else
            {
                return RedirectToAction("login");
            }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }

        //Delete the Government Record
        public async Task<ActionResult> DeleteGovernment(string GId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //    if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            if (applicant != null)
            {
                if (applicant.GovernmentInfos != null)
                {
                    GovernmentExperienceInfo govInfo = applicant.GovernmentInfos.FirstOrDefault(m => m.GId == GId);

                    _Applicant.DeleteGovernmentInfo(applicant.Id.ToString(), GId);
                    await _uow.Commit();


                    //Delete File if exists
                    string main = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",
                                                                     "applicant", applicant.Id.ToString(), "Government", "File", govInfo.FileName
                                                                  );



                    if (System.IO.File.Exists(main))
                    {
                        System.IO.File.Delete(main);
                    }


                    TempData["Message"] = "Record Deleted Successfully.";
                    return RedirectToAction("ExperienceIndex");
                }
                else
                {
                    return RedirectToAction("ExperienceIndex");
                }
            }
            else
            {
                return RedirectToAction("login");
            }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }

        //Delete the Government Record
        public async Task<ActionResult> DeleteNonGovernment(string GId)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            //string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            //    if (id != null)
            //{
            //    Applicant applicant = await _Applicant.GetById(id);
            if (applicant != null)
            {
                if (applicant.NonGovernmentInfos != null)
                {
                    NonGovernmentExperienceInfo govInfo = applicant.NonGovernmentInfos.FirstOrDefault(m => m.GId == GId);

                    _Applicant.DeleteNonGovernmentInfo(applicant.Id.ToString(), GId);
                    await _uow.Commit();


                    //Delete File if exists
                    string main = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",
                                                                     "applicant", applicant.Id.ToString(), "NonGovernment", "File", govInfo.FileName
                                                                  );



                    if (System.IO.File.Exists(main))
                    {
                        System.IO.File.Delete(main);
                    }


                    TempData["Message"] = "Record Deleted Successfully.";
                    return RedirectToAction("ExperienceIndex");
                }
                else
                {
                    return RedirectToAction("ExperienceIndex");
                }
            }
            else
            {
                return RedirectToAction("login");
            }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

        }

        //public ActionResult<Applicant> Create()
        //{
        //    Applicant value = new Applicant();
        //    return View();
        //}
        //[HttpPost]
        //public async Task<ActionResult<Applicant>> Create(Applicant value)
        //{
        //    //Applicant obj = new Applicant(value);
        //    _Applicant.Add(value);

        //    // it will be null
        //    //var testApplicant = await _Applicant.GetById(value.);

        //    // If everything is ok then:
        //    await _uow.Commit();

        //    // The product will be added only after commit
        //    // testProduct = await _productRepository.GetById(product.Id);

        //    return RedirectToAction("Index");
        //}
        //[HttpGet]
        //public async Task<ActionResult<Applicant>> Edit(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var Applicant = await _Applicant.GetById(id);
        //        return View(Applicant);
        //    }
        //    else
        //        return BadRequest();

        //}
        //[HttpPost]
        //public async Task<ActionResult<Applicant>> Edit(string id, Applicant value)
        //{
        //    // var product = new Product(value.Id);
        //    value.Id = ObjectId.Parse(id);
        //    _Applicant.Update(value, id);

        //    await _uow.Commit();

        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            _Applicant.Remove(id);

            DeleteFile(id);
            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
            //  testApplicant = await _Applicant.GetById(id);

            return RedirectToAction("Index");
        }

        public void DeleteFile(string id)
        {
            List<string> dirs = new List<string>();
            List<string> files = new List<string>();
            List<string> subdirs = new List<string>();
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "applicant", id);
            if (Directory.Exists(dir))
            {
                subdirs = Directory.GetDirectories(dir).Length != 0 ? Directory.GetDirectories(dir).ToList() : null;
                if (subdirs.Count > 0)
                    foreach (string item in subdirs)
                    {
                        List<string> subsubDirs = Directory.GetDirectories(item).Length != 0 ? Directory.GetDirectories(item).ToList() : null;
                        List<String> filesDir = Directory.GetFiles(item).Length != 0 ? Directory.GetFiles(item).ToList() : null;

                        if (subsubDirs.Count > 0)
                        {
                            subdirs.AddRange(subsubDirs);
                        }

                        if (filesDir.Count > 0)
                        {
                            foreach (string fileItem in filesDir)
                            {
                                System.IO.File.Delete(fileItem);
                            }
                        }
                    }

                foreach (string item in subdirs)
                {
                    Directory.Delete(item);
                }

                Directory.Delete(dir);
            }
        }

        public async Task<ActionResult> Application()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            ApplicationVM appVM = new ApplicationVM();
            List<DropDownItem> Faculties = (from f in await _Faculty.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = f.Id.ToString(),
                                                Name = f.Name
                                            }).ToList();
            Faculties.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            appVM.Faculties = new SelectList(Faculties, "Id", "Name");
            IEnumerable<Advertisiment> Ads = await _Advertisement.GetAll();
            appVM.Advertisements = Ads;
            appVM.Id = applicant.Id.ToString();
            return View(appVM);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationVM>> Application(ApplicationVM appVM)
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);

            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            List<DropDownItem> Faculties = (from f in await _Faculty.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = f.Id.ToString(),
                                                Name = f.Name
                                            }).ToList();
            Faculties.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });
            appVM.Faculties = new SelectList(Faculties, "Id", "Name");
            IEnumerable<Advertisiment> Ads = await _Advertisement.GetAll();
            appVM.Advertisements = Ads;
            if (ModelState.IsValid)
            {
                Applications app = _mapper.Map<Applications>(appVM);
                app.Applicant = Request.Cookies != null ? Request.Cookies["Id"] : null;
                app.EthnicalGroup = appVM.EthnicalGroups;
                app.AppliedDate = DateTime.Now;

                IEnumerable<Applications> apps = await _Applications.GetAll();
                if (apps != null)
                {
                    if (!apps.Any(m => m.Advertisement == appVM.Advertisement && m.Applicant == appVM.Id))
                    {
                        _Applications.Add(app);
                        app.PaymentStatus = "Pending";
                        await _uow.Commit();
                    }
                }
                else
                {
                    _Applications.Add(app);
                    app.PaymentStatus = "Pending";
                    await _uow.Commit();
                }
                TempData["Message"] = "Successfully Submitted the Application.";
                return RedirectToAction("Index");
            }
            else
            {

                return View(appVM);
            }
        }



        public async Task<ActionResult> Payment()
        {
            Applicant applicant = await _Applicant.GetByEmail(ClaimTypes.Email);
            if (applicant == null)
            {
                return RedirectToAction("Login");

            }
            string id = applicant.Id.ToString();

            PaymentVM payVM = new PaymentVM();
            // string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            var applications = await _Applications.GetAll();
            var advertisements = await _Advertisement.GetAll();

            IEnumerable<ApplicationPay> apps = (from a in applications
                                                from ad in advertisements
                                                where a.Advertisement == ad.Id.ToString() && a.Applicant == id
                                                select new ApplicationPay
                                                {
                                                    Id = a.Id.ToString(),
                                                    ObjAd = ad,
                                                    EthnicalGroups = String.Join(" , ", a.EthnicalGroup.ToList()),
                                                    PaymentStatus = a.PaymentStatus

                                                }).AsEnumerable();
            payVM.Applications = apps.ToList();
            payVM.Applicant = applicant;
            return View(payVM);

        }


        private static string RandomString(int length)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            const string pool = "ABCDEFGHIIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void SendEMail(string emailid, string Subject, string Body)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("dbugtest2016@gmail.com", "my@work#123$");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress("dbugtest2016@gmail.com", "Public Service Commission, Nepal");
            msg.To.Add(new MailAddress(emailid));
            msg.Subject = Subject;
            msg.IsBodyHtml = true;
            msg.Body = Body;

            client.Send(msg);
        }
    }

}
