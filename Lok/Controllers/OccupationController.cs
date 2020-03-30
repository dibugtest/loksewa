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
    public class OccupationController : Controller
    {
        private readonly IOccupationRepository _Occupation;
        private readonly IUnitOfWork _uow;

        public OccupationController(IOccupationRepository Occupation, IUnitOfWork uow)
        {
            _Occupation = Occupation;
            _uow = uow;
        }
        // GET: Occupation
        public async Task<ActionResult> Index()
        {
            var Occupations = await _Occupation.GetAll();
            return View(Occupations);
        }

        public ActionResult<Occupation> Create()
        {
            Occupation value = new Occupation();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Occupation>> Create(Occupation value)
        {
            //Occupation obj = new Occupation(value);
            _Occupation.Add(value);

            // it will be null
            //var testOccupation = await _Occupation.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Occupation>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Occupation = await _Occupation.GetById(id);
                return View(Occupation);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Occupation>> Edit(string id, Occupation value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _Occupation.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _Occupation.Remove(id);

            // it won't be null
           // var testOccupation = await _Occupation.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testOccupation = await _Occupation.GetById(id);

            return RedirectToAction("Index");
        }
    }
}