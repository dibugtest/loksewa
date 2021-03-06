﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Lok.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace Lok.Controllers
{
    [Authorize("Admin",AuthenticationSchemes = "AdminCookie")]

    public class AdvetisimentController : Controller
    {
       

            private readonly IAdvertisiment _Advertisiment;
            private readonly IUnitOfWork _uow;
            private readonly IGroupRepository _Group;
        private readonly ISubGroupRepository _SubGroup;
        private readonly IServiceRepository _service;
        private readonly IEthinicalGroup _ethinicalGroup;
        private readonly ICategoryInterface _category;
        private readonly IPostRepository _post;
        private readonly IEducationLevelRepository _educationLavel;

        public AdvetisimentController(IAdvertisiment Advertisiment,IEducationLevelRepository educationLevel, IUnitOfWork uow, IGroupRepository Group,ISubGroupRepository subGroup,IServiceRepository service,IEthinicalGroup ethinicalGroup,ICategoryInterface Category,IPostRepository post)
            {
                _Advertisiment = Advertisiment;
                _uow = uow;
                _Group = Group;
            _service = service;
            _SubGroup = subGroup;
            _ethinicalGroup = ethinicalGroup;
            _post = post;
            _category = Category;
            _educationLavel = educationLevel;
            }
            // GET: Advertisiment
            public async Task<ActionResult> Index()
            {
                var Advertisiments = await _Advertisiment.GetAll();

                return View(Advertisiments);
            }

            public async Task<ActionResult<Advertisiment>> Create()
            {
                Advertisiment value = new Advertisiment();
                ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName");
            ViewBag.ServiceId = new SelectList(await _service.GetAll(), "Id", "ServiceName");
            ViewBag.SubGroupId = new SelectList(await _SubGroup.GetAll(), "Id", "SubGroupName");
            ViewBag.CategoryId = new SelectList(await _category.GetAll(), "Id", "CategoryName");
            ViewBag.PostId = new SelectList(await _post.GetAll(), "Id", "PostName");
            ViewBag.MInEdu = new SelectList(await _educationLavel.GetAll(), "Id","Name");
            IEnumerable<EthinicalGroup> a =  await _ethinicalGroup.GetAll();
            ViewBag.EthinicalGroup = a.ToList();
            List<ExamViewModel> ev = new List<ExamViewModel>(){new ExamViewModel
            {
                ExamType="Experimental"
            },
              new ExamViewModel {
                ExamType="Interview"
            },
               new ExamViewModel {
                ExamType="Written"
            },

            };
            //List<string> Exam = new List<string>()
            //{
            // "written", "Interview", "Experimental"

            //};

            ViewBag.Exam = ev;
            return View();
            }
            [HttpPost]
            public async Task<ActionResult<Advertisiment>> Create(Advertisiment value,IFormCollection Col)
            {
            List<EthinicalGroup> eths = new List<EthinicalGroup>();
            List<AdvAndEth> adv = new List<AdvAndEth> ();
            List<string> exam = new List<string>();

            int i = 0;
            int j = 0;
            foreach (string key in Col.Keys)
            {
                AdvAndEth adve = new AdvAndEth();

                int values = 0;
                if (key.Contains("EthinicalGroup") && !key.Contains("value"))
                {
                    EthinicalGroup eth = await _ethinicalGroup.GetById(Col[key]);
                    values = Convert.ToInt32(Col[key + "value"]);

                    eths.Add(eth);
                    adve.GetEthinicalGroup = eth;
                    adve.Value = values;
                    adv.Add(adve);

                }

                //if (key == "EthinicalGroup[" + i + "]")
                //{
                //    EthinicalGroup eth = await _ethinicalGroup.GetById(Col["EthinicalGroup[" + i + "]"]);
                //    values = Convert.ToInt32(Col["EthinicalGroup[" + i + "]value"]);

                //    eths.Add(eth);
                //    adve.GetEthinicalGroup = eth;
                //    adve.Value = values;
                //    adv.Add(adve);
                //    i++;


                //}
                if (key.Contains("Exam"))
                {
                    string examtype = Col[key];
                    exam.Add(examtype);
                    j++;
                }
            }
            value.Examtype = exam;
            value.EthinicalGroups = eths;
            value.AdvAndEths = adv;
                value.Group = await _Group.GetById(value.GroupId.ToString());
            value.SubGroup = await _SubGroup.GetById(value.SubGroupId.ToString());
            value.Service = await _service.GetById(value.ServiceId.ToString());
            value.Category = await _category.GetById(value.CategoryId.ToString());
            value.Post = await _post.GetById(value.PostId.ToString());
            value.Edu = await _educationLavel.GetById(value.EducationId.ToString());

            _Advertisiment.Add(value);

                // it will be null
                //var testAdvertisiment = await _Advertisiment.GetById(value.);

                // If everything is ok then:
                await _uow.Commit();

                // The product will be added only after commit
                // testProduct = await _productRepository.GetById(product.Id);

                return RedirectToAction("Index");
            }
        public async Task<ActionResult> Details(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Service = await _Advertisiment.GetById(id);

                return View(Service);
            }
            else
                return BadRequest();


        }
        [HttpGet]
            public async Task<ActionResult<Advertisiment>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Advertisiment = await _Advertisiment.GetById(id);
               
                IEnumerable<EthinicalGroup> a = await _ethinicalGroup.GetAll();

                ViewBag.EthinicalGroup = a.ToList();

                if (String.IsNullOrEmpty(Advertisiment.GroupId))
                    {
                        ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName");
                    }
                    else
                    {
                        ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName", Advertisiment.GroupId);
                    }
                if (String.IsNullOrEmpty(Advertisiment.PostId))
                {
                    ViewBag.PostId = new SelectList(await _post.GetAll(), "Id", "PosTName");
                }
                else
                {
                    ViewBag.PostId = new SelectList(await _post.GetAll(), "Id", "PostName", Advertisiment.PostId);
                }
                if (String.IsNullOrEmpty(Advertisiment.ServiceId))
                {
                    ViewBag.ServiceId = new SelectList(await _service.GetAll(), "Id", "ServiceName");
                }
                else
                {
                    ViewBag.ServiceId = new SelectList(await _service.GetAll(), "Id", "ServiceName", Advertisiment.GroupId);
                }
                if (String.IsNullOrEmpty(Advertisiment.SubGroupId))
                {
                    ViewBag.SubGroupId = new SelectList(await _SubGroup.GetAll(), "Id", "SubGroupName");
                }
                else
                {
                    ViewBag.SubGroupId = new SelectList(await _SubGroup.GetAll(), "Id", "SubGroupName", Advertisiment.SubGroupId);
                }
                if (String.IsNullOrEmpty(Advertisiment.CategoryId))
                {
                    ViewBag.CategoryId = new SelectList(await _category.GetAll(), "Id", "CategoryName");
                }
                else
                {
                    ViewBag.CategoryId = new SelectList(await _category.GetAll(), "Id", "CategoryName", Advertisiment.CategoryId);
                }
                if (String.IsNullOrEmpty(Advertisiment.EducationId))
                {
                    ViewBag.MInEdu = new SelectList(await _educationLavel.GetAll(), "Id", "Name");
                }
                else
                {
                    ViewBag.MInEdu = new SelectList(await _educationLavel.GetAll(), "Id", "Name",Advertisiment.EducationId);
                }
                List<ExamViewModel> ev = new List<ExamViewModel>(){new ExamViewModel
            {
                ExamType="Experimental"
            },
              new ExamViewModel {
                ExamType="Interview"
            },
               new ExamViewModel {
                ExamType="Written"
            },

            };
                

                ViewBag.Exam = ev;



                return View(Advertisiment);
                }

                else
                    return BadRequest();

            }
            [HttpPost]
            public async Task<ActionResult<Advertisiment>> Edit(string id, Advertisiment value,IFormCollection Col)
            {
                value.Id = ObjectId.Parse(id);
            List<EthinicalGroup> eths = new List<EthinicalGroup>();
            List<AdvAndEth> adv = new List<AdvAndEth>();
            List<string> exam = new List<string>();
            int i = 0;
            int j = 0;

            foreach (string key in Col.Keys)
            {
                AdvAndEth adve = new AdvAndEth();

                int values = 0;
                if (key.Contains("EthinicalGroup") && !key.Contains("value"))
                {
                    EthinicalGroup eth = await _ethinicalGroup.GetById(Col[key]);
                    values = Convert.ToInt32(Col[key + "value"]);

                    eths.Add(eth);
                    adve.GetEthinicalGroup = eth;
                    adve.Value = values;
                    adv.Add(adve);

                }

                //if (key == "EthinicalGroup[" + i + "]")
                //{
                //    EthinicalGroup eth = await _ethinicalGroup.GetById(Col["EthinicalGroup[" + i + "]"]);
                //    values = Convert.ToInt32(Col["EthinicalGroup[" + i + "]value"]);

                //    eths.Add(eth);
                //    adve.GetEthinicalGroup = eth;
                //    adve.Value = values;
                //    adv.Add(adve);
                //    i++;


                //}
                if (key.Contains("Exam"))
                {
                    string examtype = Col[key];
                    exam.Add(examtype);
                    j++;
                }

                // if(key["EthinicalGroup['"+i+"])
            }
            value.Examtype = exam;

            value.EthinicalGroups = eths;
            value.AdvAndEths = adv;

            value.Edu = await _educationLavel.GetById(value.EducationId.ToString());

            value.Group = await _Group.GetById(value.GroupId.ToString());
            value.SubGroup = await _SubGroup.GetById(value.SubGroupId.ToString());
            value.Service = await _service.GetById(value.ServiceId.ToString());
            value.Category = await _category.GetById(value.CategoryId.ToString());
            value.Post = await _post.GetById(value.PostId.ToString());



            _Advertisiment.Update(value, id);

                await _uow.Commit();

                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<ActionResult> Delete(string id)
            {
                _Advertisiment.Remove(id);

                // it won't be null
                var testAdvertisiment = await _Advertisiment.GetById(id);

                // If everything is ok then:
                await _uow.Commit();

                // not it must by null
                testAdvertisiment = await _Advertisiment.GetById(id);

                return RedirectToAction("Index");
            }



        

    }
}