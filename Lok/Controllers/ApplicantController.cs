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

namespace Lok.Controllers
{
    public class ApplicantController : Controller
    {
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

        private readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;


        public ApplicantController(IApplicantRepository Applicant, IReligionRepository Religion
                                    , IEmploymentRepository Employment, IOccupationRepository Occupation,
                                    IVargaRepository Varga, IDistrictRepository District, IBoardNameRepository BoardName, IEducationLevelRepository EducationLevel
                                    , IFacultyRepository Faculty, ISewaRepository Sewa,
                                    IShreniTahaRepository Shreni, IUnitOfWork uow, IMapper mapper)
        {
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
            _uow = uow;
            _mapper = mapper;
        }


        // GET: Applicant use authorize
        public async Task<ActionResult> Index()
        {
            string id = Request.Cookies["Id"].ToString();
            var Applicants = await _Applicant.GetById(id);
            return View(Applicants);
        }

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
                if (!string.IsNullOrEmpty(register.Id))
                {
                    Applicant registeredApplicant = await _Applicant.GetById(register.Id);

                    string EmailBody = "Your Reset Password is " + Base64Decode(registeredApplicant.RandomPassword) + ".Email Verification and Password Setup link." + "<a href='" + "http://localhost:5000/applicant/emailVerification/" + register.Id + "'>Link</a>";
                    try
                    {
                        SendEMail(register.Email, "Email Verification Link.", EmailBody);

                        registeredApplicant.PersonalInformation = _mapper.Map<PersonalInfo>(register);

                        registeredApplicant.ExtraInformation = _mapper.Map<ExtraInfo>(register);

                        registeredApplicant.EditedDate = DateTime.Now;
                        registeredApplicant.CreatedBy = "Admin";//from login
                        _Applicant.Update(registeredApplicant, register.Id);

                        TempData["Message"] = "Successfully Registered. Please check your Email and reset your password.";
                        return RedirectToAction("PasswordReset", register.Id);
                    }
                    catch
                    {
                        ViewBag.Error = "Error";
                        ModelState.AddModelError(string.Empty, "Failed to send mail. Please Try Again.");
                        return View(register);
                    }
                }
                else
                {

                    Applicant applicant = new Applicant();
                    applicant.CreatedDate = DateTime.Now;
                    applicant.EditedDate = DateTime.Now;
                    applicant.CreatedBy = "Admin";
                    applicant.PersonalInformation = _mapper.Map<PersonalInfo>(register);

                    applicant.ExtraInformation = _mapper.Map<ExtraInfo>(register);

                    //Check if the applicant exist with email 
                    if (_Applicant.GetByEmail(register.Email) != null)
                    {
                        string randString = RandomString(6);
                        applicant.RandomPassword = Base64Encode(randString);

                        //if not Add applicant to db
                        _Applicant.Add(applicant);

                        await _uow.Commit();


                        if (applicant.Id != null)
                        {
                            //send Email for email verification
                            string EmailBody = "Your Reset Password is " + Base64Decode(applicant.RandomPassword) + ".Email Verification and Password Setup link." + "<a href='" + "http://localhost:5000/applicant/emailVerification/'" + applicant.Id + ">Link</a>";
                            try
                            {
                                SendEMail(register.Email, "Email Verification Link.", EmailBody);
                                TempData["Message"] = "Successfully Registered. Please check your Email and reset your password.";
                                return RedirectToAction("PasswordReset", new { id = register.Id });

                            }
                            catch
                            {
                                register.Id = applicant.Id.ToString();
                                ViewBag.Error = "Error";
                                ModelState.AddModelError(string.Empty, "Failed to send mail.");
                                return View(register);
                            }

                        }
                    }
                }
            }

            return View(register);
        }


        public async Task<ActionResult<ResetPasswordVM>> ResetPassword(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Applicant applicant = await _Applicant.GetById(id);
                if (applicant == null)
                {
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "Please try Again!");
                    return View();
                }
                else
                {
                    ResetPasswordVM reset = new ResetPasswordVM();
                    reset.Id = applicant.Id.ToString();
                    reset.Name = applicant.PersonalInformation.FirstName + " " + applicant.PersonalInformation.MiddleName + " " + applicant.PersonalInformation.LastName;
                    return View(reset);
                }
            }
            else
            {
                return RedirectToAction("ApplicantLogin");
            }
        }
        [HttpPost]
        public async Task<ActionResult<ResetPasswordVM>> ResetPassword(ResetPasswordVM reset)
        {
            if (ModelState.IsValid)
            {
                Applicant applicant = await _Applicant.GetById(reset.Id);
                if (applicant != null)
                {
                    if (applicant.RandomPassword == Base64Encode(reset.RandPassword))
                    {
                        applicant.EmailVerification = true;
                        applicant.RandomPassword = "";

                        applicant.Password = new Passwords { Hash = reset.Password, Salt = reset.Password };

                        applicant.QuestionAnswer = _mapper.Map<SecurityQA>(reset);

                        _Applicant.Update(applicant, reset.Id);
                        await _uow.Commit();

                        TempData["Message"] = "Successfully Reset Password. Login using new password.";
                        return View(reset);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Reset Password doesnot match.");
                        ViewBag.Error = "Error";
                        return View();
                    }
                }
            }
            ViewBag.Error = "Error";
            ModelState.AddModelError(string.Empty, "Please try Again!");
            return View(reset);
        }



        public async Task<ActionResult<LoginVM>> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<LoginVM>> Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                Applicant applicant = await _Applicant.GetByEmail(login.Email);
                if (applicant != null)
                {
                    if (applicant.EmailVerification)
                    {
                        if (applicant.Password.Hash == login.Password)
                        {
                            CookieOptions mycookie = new CookieOptions();

                            if (Response.Cookies != null)
                            {
                                Response.Cookies.Delete("Id");
                            }

                            mycookie.Expires = DateTime.Now.AddMinutes(5);

                            Response.Cookies.Append("Id", applicant.Id.ToString(), mycookie);
                            //Response.Cookies.Delete(key);

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Error = "Error";
                            ModelState.AddModelError(string.Empty, "Email and Password not Matched.");
                            return View(login);
                        }

                    }
                    else
                    {
                        ViewBag.Error = "Error";
                        ModelState.AddModelError(String.Empty, "Check your mail and reset your password");
                        return View(login);
                        // TempData["ErrorMessage"] = "Check your mail and reset your password.";
                        //  return RedirectToAction("ResetPassword",new { Id = applicant.Id.ToString() });
                    }
                }
                else
                {
                    ViewBag.Error = "Error";
                    ModelState.AddModelError(string.Empty, "Please try Again.");
                    return View(login);
                }
            }
            return View(login);
        }

        public async Task<ActionResult<PersonalVM>> Personal()
        {
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                PersonalVM personalVM = _mapper.Map<PersonalVM>(applicant.PersonalInformation);
                personalVM.Id = applicant.Id.ToString();

                return View(personalVM);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PersonalVM>> Personal(PersonalVM personalVM)
        {
            if (ModelState.IsValid)
            {
                Applicant applicant = await _Applicant.GetById(personalVM.Id);
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

            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                ExtraVM extraVM = _mapper.Map<ExtraVM>(applicant.ExtraInformation);
                extraVM.Id = applicant.Id.ToString();

                extraVM.Religions = new SelectList(Religions, "Id", "Name");
                extraVM.Occupations = new SelectList(Occupations, "Id", "Name");
                extraVM.Employments = new SelectList(Employments, "Id", "Name");
                extraVM.Vargas = new SelectList(Vargas, "Id", "Name");
                return View(extraVM);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExtraVM>> Extra(ExtraVM extraVM)
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


            extraVM.Religions = new SelectList(Religions, "Id", "Name");
            extraVM.Occupations = new SelectList(Occupations, "Id", "Name");
            extraVM.Employments = new SelectList(Employments, "Id", "Name");
            extraVM.Vargas = new SelectList(Vargas, "Id", "Name");
            if (ModelState.IsValid)
            {
                Applicant applicant = await _Applicant.GetById(extraVM.Id);
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
            List<DropDownItem> Districts = (from r in await _District.GetAll()
                                            select new DropDownItem
                                            {
                                                Id = r.Id.ToString(),
                                                Name = r.Name
                                            }).ToList();


            Districts.Insert(0, new DropDownItem { Id = "", Name = "--Select--" });

            string id = Request.Cookies != null ? Request.Cookies["Id"].ToString() : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);

                ContactVM contactVM = applicant.ContactInformation != null ? _mapper.Map<ContactVM>(applicant.ContactInformation) : new ContactVM();
                contactVM.Id = applicant.Id.ToString();
                contactVM.Districts = new SelectList(Districts, "Id", "Name");
                contactVM.Email = applicant.PersonalInformation.Email;
                contactVM.MobileNo = applicant.PersonalInformation.Mobile;

                return View(contactVM);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContactVM>> Contact(ContactVM contactVM)
        {
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
                Applicant applicant = await _Applicant.GetById(contactVM.Id);
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
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                IEnumerable<EducationVM> edus = applicant.EducationInfos != null ? _mapper.Map<IEnumerable<EducationVM>>(applicant.EducationInfos) : null;
                return View(edus);
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public async Task<ActionResult<EducationVM>> Education(string EId)
        {
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


            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                EducationVM educationVM = new EducationVM();
                educationVM.EId = EId;
                if (EId != null)
                {
                    if (applicant.EducationInfos != null)
                    {
                        EducationInfo eduInfo = applicant.EducationInfos.FirstOrDefault(m => m.EId == EId);
                        if (eduInfo != null)
                        {
                            educationVM = _mapper.Map<EducationVM>(eduInfo);
                        }
                    }
                }
                educationVM.Id = applicant.Id.ToString();
                educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
                educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
                educationVM.Faculties = new SelectList(Faculties, "Id", "Name");

                //Check File Exists
                if (EId != null)
                {
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
                }


                return View(educationVM);

            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult<EducationVM>> Education(EducationVM educationVM, IFormFile FileMain, IFormFile FileEquivalent)
        {
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
                    Applicant applicant = await _Applicant.GetById(educationVM.Id);
                    List<EducationInfo> eduList = new List<EducationInfo>();
                    EducationInfo eduInfo = _mapper.Map<EducationInfo>(educationVM);
                    eduList.Add(eduInfo);
                    if (educationVM.EId == null)
                    {
                        eduInfo.EId = applicant.EducationInfos != null ? (applicant.EducationInfos.ToList().Count + 1).ToString() : "1";
                    }
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


        public async Task<ActionResult<TrainingVM>> TrainingIndex()
        {
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                IEnumerable<TrainingVM> training = applicant.TrainingInfos != null ? _mapper.Map<IEnumerable<TrainingVM>>(applicant.TrainingInfos) : null;
                return View(training);
            }
            else
            {
                return RedirectToAction("login");
            }

        }

        public async Task<ActionResult<TrainingVM>> TrainingCreate()
        {

            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                TrainingVM trainVM = new TrainingVM();
                trainVM.Id = applicant.Id.ToString();
                return View(trainVM);

            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public async Task<ActionResult<TrainingVM>> TrainingCreate(TrainingVM trainVM, IFormFile FileMain)
        {
            if (ModelState.IsValid)
            {
                if (FileMain != null)
                {
                    Applicant applicant = await _Applicant.GetById(trainVM.Id);
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
            if (TId==null)
            {
                return RedirectToAction("TrainingIndex");
            }
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                TrainingVM trainVM = new TrainingVM();
                trainVM.TId = TId;
                    if (applicant.EducationInfos != null)
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

            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        public async Task<ActionResult<TrainingVM>> TrainingEdit(TrainingVM trainVM, IFormFile FileMain)
        {
            if (ModelState.IsValid)
            {
                    Applicant applicant = await _Applicant.GetById(trainVM.Id);
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

        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilIndex()
        {
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                IEnumerable<ProfessionalCouncilVM> councils = applicant.ProfessionalCouncils != null ? _mapper.Map<IEnumerable<ProfessionalCouncilVM>>(applicant.ProfessionalCouncils) : null;
                return View(councils);
            }
            else
            {
                return RedirectToAction("login");
            }

        }

        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilCreate()
        {
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                ProfessionalCouncilVM councilVM = new ProfessionalCouncilVM();
                councilVM.Id = applicant.Id.ToString();
                return View(councilVM);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilCreate(ProfessionalCouncilVM councilVM, IFormFile FileMain)
        {
            if (ModelState.IsValid)
            {
                if (FileMain != null)
                {
                    Applicant applicant = await _Applicant.GetById(councilVM.Id);
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
            if (PId == null)
            {
                return RedirectToAction("CouncilIndex");
            }
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
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

            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        public async Task<ActionResult<ProfessionalCouncilVM>> CouncilEdit(ProfessionalCouncilVM councilVM, IFormFile FileMain)
        {
            if (ModelState.IsValid)
            {
                Applicant applicant = await _Applicant.GetById(councilVM.Id);
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
            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                IEnumerable<GovernmentExperienceVM> government = applicant.GovernmentInfos != null ? _mapper.Map<IEnumerable<GovernmentExperienceVM>>(applicant.GovernmentInfos) : null;
                IEnumerable<NonGovernmentExperienceVM> NonGovernment = applicant.NonGovernmentInfos != null ? _mapper.Map<IEnumerable<NonGovernmentExperienceVM>>(applicant.NonGovernmentInfos) : null;
                ExperienceVM exp = new ExperienceVM();
                exp.GovernmentExperience = government;
                exp.NonGovernmentExperience = NonGovernment;
                return View(exp);
            }
            else
            {
                return RedirectToAction("login");
            }

        }
        //Government Experience


        //Non Government Experience


        public ActionResult<Applicant> Create()
        {
            Applicant value = new Applicant();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Applicant>> Create(Applicant value)
        {
            //Applicant obj = new Applicant(value);
            _Applicant.Add(value);

            // it will be null
            //var testApplicant = await _Applicant.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Applicant>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Applicant = await _Applicant.GetById(id);
                return View(Applicant);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Applicant>> Edit(string id, Applicant value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _Applicant.Update(value, id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _Applicant.Remove(id);

            // it won't be null
            // var testApplicant = await _Applicant.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
            //  testApplicant = await _Applicant.GetById(id);

            return RedirectToAction("Index");
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
