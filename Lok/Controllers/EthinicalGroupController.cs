using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Lok.Controllers
{
    public class EthinicalGroupController : Controller
    {
       
            private readonly IEthinicalGroup _EthinicalGroup;
            private readonly IUnitOfWork _uow;

            public EthinicalGroupController(IEthinicalGroup EthinicalGroup, IUnitOfWork uow)
            {
                _EthinicalGroup = EthinicalGroup;
                _uow = uow;
            }
            // GET: EthinicalGroup
            public async Task<ActionResult> Index()
            {
                var EthinicalGroups = await _EthinicalGroup.GetAll();
                return View(EthinicalGroups);
            }

            public ActionResult<EthinicalGroup> Create()
            {
                EthinicalGroup value = new EthinicalGroup();
                return View();
            }
            [HttpPost]
            public async Task<ActionResult<EthinicalGroup>> Create(EthinicalGroup value)
            {
                //EthinicalGroup obj = new EthinicalGroup(value);
                _EthinicalGroup.Add(value);

                // it will be null
                //var testEthinicalGroup = await _EthinicalGroup.GetById(value.);

                // If everything is ok then:
                await _uow.Commit();

                // The product will be added only after commit
                // testProduct = await _productRepository.GetById(product.Id);

                return RedirectToAction("Index");
            }
            [HttpGet]
            public async Task<ActionResult<EthinicalGroup>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var EthinicalGroup = await _EthinicalGroup.GetById(id);
                    return View(EthinicalGroup);
                }
                else
                    return BadRequest();

            }
            [HttpPost]
            public async Task<ActionResult<EthinicalGroup>> Edit(string id, EthinicalGroup value)
            {
                // var product = new Product(value.Id);
                value.Id = ObjectId.Parse(id);
                _EthinicalGroup.Update(value, id);

                await _uow.Commit();

                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<ActionResult> Delete(string id)
            {
                _EthinicalGroup.Remove(id);

                // it won't be null
                var testEthinicalGroup = await _EthinicalGroup.GetById(id);

                // If everything is ok then:
                await _uow.Commit();

                // not it must by null
                testEthinicalGroup = await _EthinicalGroup.GetById(id);

                return RedirectToAction("Index");
            }
        


    }
}