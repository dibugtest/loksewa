using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Lok.Controllers
{
    public class ReligionController : Controller
    {
        private readonly IReligionRepository _religion;
        private readonly IUnitOfWork _uow;

        public ReligionController(IReligionRepository religion, IUnitOfWork uow)
        {
            _religion = religion;
            _uow = uow;
        }
        // GET: religion
        public async Task<ActionResult> Index()
        {
            var religions = await _religion.GetAll();
            return View(religions);
        }

        public ActionResult<Religion> Create()
        {
            Religion value = new Religion();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Religion>> Create(Religion value)
        {
            //religion obj = new religion(value);
            _religion.Add(value);

            // it will be null
            //var testreligion = await _religion.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Religion>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var religion = await _religion.GetById(id);
                return View(religion);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Religion>> Edit(string id, Religion value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _religion.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _religion.Remove(id);

            // it won't be null
           // var testreligion = await _religion.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testreligion = await _religion.GetById(id);

            return RedirectToAction("Index");
        }
    }
}