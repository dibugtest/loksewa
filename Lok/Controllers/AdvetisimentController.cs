using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
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


            public AdvetisimentController(IAdvertisiment Advertisiment, IUnitOfWork uow, IGroupRepository Group,ISubGroupRepository subGroup,IServiceRepository service,IEthinicalGroup ethinicalGroup)
            {
                _Advertisiment = Advertisiment;
                _uow = uow;
                _Group = Group;
            _service = service;
            _SubGroup = subGroup;
            _ethinicalGroup = ethinicalGroup;
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
            ViewBag.ServiceId = new SelectList(await _service.GetAll(), "Id", "GroupName");
            ViewBag.SubGroupId = new SelectList(await _SubGroup.GetAll(), "Id", "GroupName");
            ViewBag.EthinicalGroup = new SelectList(await _ethinicalGroup.GetAll(), "Id", "GroupName");


            return View();
            }
            [HttpPost]
            public async Task<ActionResult<Advertisiment>> Create(Advertisiment value)
            {
                //Advertisiment obj = new Advertisiment(value);
                value.Group = await _Group.GetById(value.GroupId.ToString());

                _Advertisiment.Add(value);

                // it will be null
                //var testAdvertisiment = await _Advertisiment.GetById(value.);

                // If everything is ok then:
                await _uow.Commit();

                // The product will be added only after commit
                // testProduct = await _productRepository.GetById(product.Id);

                return RedirectToAction("Index");
            }
            [HttpGet]
            public async Task<ActionResult<Advertisiment>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Advertisiment = await _Advertisiment.GetById(id);

                    if (String.IsNullOrEmpty(Advertisiment.GroupId))
                    {
                        ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName", Advertisiment.GroupId);
                    }
                    else
                    {
                        ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName", Advertisiment.GroupId);
                    }

                    return View(Advertisiment);
                }
                else
                    return BadRequest();

            }
            [HttpPost]
            public async Task<ActionResult<Advertisiment>> Edit(string id, Advertisiment value)
            {
                value.Id = ObjectId.Parse(id);
                value.Group = await _Group.GetById(value.GroupId.ToString());

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