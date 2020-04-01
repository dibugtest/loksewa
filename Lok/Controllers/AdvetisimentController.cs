using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace Lok.Controllers
{
    public class AdvetisimentController : Controller
    {
       

            private readonly IAdvertisiment _Advertisiment;
            private readonly IUnitOfWork _uow;
            private readonly IGroupRepository _Group;
        private readonly ISubGroupRepository _SubGroup;
        private readonly IServiceRepository _service;
        private readonly IEthinicalGroup _ethinicalGroup;
        private readonly ICategoryInterface _category;


        public AdvetisimentController(IAdvertisiment Advertisiment, IUnitOfWork uow, IGroupRepository Group,ISubGroupRepository subGroup,IServiceRepository service,IEthinicalGroup ethinicalGroup,ICategoryInterface Category)
            {
                _Advertisiment = Advertisiment;
                _uow = uow;
                _Group = Group;
            _service = service;
            _SubGroup = subGroup;
            _ethinicalGroup = ethinicalGroup;
            _category = Category;
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

            IEnumerable<EthinicalGroup> a =  await _ethinicalGroup.GetAll();
            ViewBag.EthinicalGroup = a.ToList();

            return View();
            }
            [HttpPost]
            public async Task<ActionResult<Advertisiment>> Create(Advertisiment value,IFormCollection Col)
            {
            //Advertisiment obj = new Advertisiment(value);
            List<EthinicalGroup> eths = new List<EthinicalGroup>();
            int i = 0;
            foreach(string key in Col.Keys)
            {
                if(key== "EthinicalGroup["+i+"]")
                {
                 EthinicalGroup eth = await _ethinicalGroup.GetById(Col["EthinicalGroup[" + i + "]"]);
                    eths.Add(eth);

                    i++;

                }
                // if(key["EthinicalGroup['"+i+"])
            }
            value.EthinicalGroups = eths;

                value.Group = await _Group.GetById(value.GroupId.ToString());
            value.SubGroup = await _SubGroup.GetById(value.SubGroupId.ToString());
            value.Service = await _service.GetById(value.ServiceId.ToString());
            value.Category = await _category.GetById(value.CategoryId.ToString());


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
            int i = 0;
            foreach (string key in Col.Keys)
            {
                if (key == "EthinicalGroup[" + i + "]")
                {
                    EthinicalGroup eth = await _ethinicalGroup.GetById(Col["EthinicalGroup[" + i + "]"]);
                    eths.Add(eth);

                    i++;

                }
                // if(key["EthinicalGroup['"+i+"])
            }
            value.EthinicalGroups = eths;

            value.Group = await _Group.GetById(value.GroupId.ToString());
            value.SubGroup = await _SubGroup.GetById(value.SubGroupId.ToString());
            value.Service = await _service.GetById(value.ServiceId.ToString());
            value.Category = await _category.GetById(value.CategoryId.ToString());



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