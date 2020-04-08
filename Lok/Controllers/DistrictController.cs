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
    public class DistrictController : Controller
    {
        private readonly IDistrictRepository _District;
        private readonly IUnitOfWork _uow;

        public DistrictController(IDistrictRepository District, IUnitOfWork uow)
        {   
            _District = District;
            _uow = uow;
        }
        // GET: District
        public async Task<ActionResult> Index()
        {
            var Districts = await _District.GetAll();
            return View(Districts);
        }

        public ActionResult<District> Create()
        {
            District value = new District();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<District>> Create(District value)
        {
            //District obj = new District(value);
            _District.Add(value);

            // it will be null
            //var testDistrict = await _District.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<District>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var District = await _District.GetById(id);
                return View(District);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<District>> Edit(string id, District value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _District.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _District.Remove(id);

            // it won't be null
           // var testDistrict = await _District.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testDistrict = await _District.GetById(id);

            return RedirectToAction("Index");
        }
    }
}