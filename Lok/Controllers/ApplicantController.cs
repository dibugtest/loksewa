﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lok.Data.Interface;
using Lok.Models;
using Lok.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

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
                                    IVargaRepository Varga,IDistrictRepository District, IBoardNameRepository BoardName, IEducationLevelRepository EducationLevel
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


        // GET: Applicant
        public async Task<ActionResult> Index()
        {
            var Applicants = await _Applicant.GetAll();
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
                        _Applicant.Update(registeredApplicant, register.Id);

                        TempData["Message"] = "Successfully Registered. Please check your Email and reset your password.";
                        return RedirectToAction("PasswordReset", register.Id);
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "Failed to send mail. Please Try Again.");
                        return View(register);
                    }
                }

            }
            else
            {

                Applicant applicant = new Applicant();
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
                        string EmailBody = "Email Verification and Password Setup link." + "<a href='" + "http://localhost:5000/applicant/emailVerification/'" + applicant.Id + ">Link</a>";
                        try
                        {
                            SendEMail(register.Email, "Email Verification Link.", EmailBody);
                            TempData["Message"] = "Successfully Registered. Please check your Email and reset your password.";
                            return RedirectToAction("PasswordReset", register.Id);

                        }
                        catch
                        {
                            register.Id = applicant.Id.ToString();
                            ModelState.AddModelError(string.Empty, "Failed to send mail.");
                            return View(register);
                        }

                    }
                }
            }

            return View(register);
        }


        public async Task<ActionResult<ResetPasswordVM>> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Applicant applicant = await _Applicant.GetById(id);
                if (applicant == null)
                {
                    return RedirectToAction("ApplicantLogin");
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
                    applicant.EmailVerification = true;
                    applicant.RandomPassword = "";
                    applicant.Password.Hash = reset.Password;
                    TempData["Message"] = "Successfully Reset Password";
                    return View();
                }
            }
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
                            ModelState.AddModelError(string.Empty, "Email and Password not Matched.");
                            return View(login);
                        }

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Check your mail and reset your password.";
                        return RedirectToAction("ResetPassword", applicant.Id.ToString());
                    }
                }
                else
                {
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
                _Applicant.Update(applicant, personalVM.Id);
                TempData["Message"] = "Success";
                return View();

            }
            else
            {
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
                ExtraVM extraVM = _mapper.Map<ExtraVM>(applicant.PersonalInformation);
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
                _Applicant.Update(applicant, extraVM.Id);
                TempData["Message"] = "Success";
                return View();

            }
            else
            {
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

            string id = Request.Cookies != null ? Request.Cookies["Id"] : null;
            if (id != null)
            {
                Applicant applicant = await _Applicant.GetById(id);
                ContactVM contactVM = _mapper.Map<ContactVM>(applicant.ContactInformation);
                contactVM.Id = applicant.Id.ToString();
                contactVM.Districts = new SelectList(Districts, "Id", "Name");
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
                TempData["Message"] = "Success";
                return View();

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error");
                return View(contactVM);
            }
        }


        public async Task<ActionResult<EducationVM>> Education()
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
                educationVM.Id = applicant.Id.ToString();
                educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
                educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
                educationVM.Faculties = new SelectList(Faculties, "Id", "Name");
                return View(educationVM);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContactVM>> Education(EducationVM educationVM)
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
                Applicant applicant = await _Applicant.GetById(educationVM.Id);
                EducationInfo eduInfo = _mapper.Map<EducationInfo>(educationVM);
                _Applicant.UpdateEducationInfo(eduInfo, educationVM.Id);
               
                TempData["Message"] = "Success";
                return View();

            }
            else
            {
                educationVM.BoardNames = new SelectList(BoardNames, "Id", "Name");
                educationVM.EducationLevels = new SelectList(EducationLevels, "Id", "Name");
                educationVM.Faculties = new SelectList(Faculties, "Id", "Name");

                ModelState.AddModelError(string.Empty, "Error");
                return View(educationVM);
            }
        }




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