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
    public class EmploymentController : Controller
    {
        private readonly IEmploymentRepository _Employment;
        private readonly IUnitOfWork _uow;

        public EmploymentController(IEmploymentRepository Employment, IUnitOfWork uow)
        {
            _Employment = Employment;
            _uow = uow;
        }
        // GET: Employment
        public async Task<ActionResult> Index()
        {
            var Employments = await _Employment.GetAll();
            return View(Employments);
        }

        public ActionResult<Employment> Create()
        {
            Employment value = new Employment();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Employment>> Create(Employment value)
        {
            //Employment obj = new Employment(value);
            _Employment.Add(value);

            // it will be null
            //var testEmployment = await _Employment.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Employment>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Employment = await _Employment.GetById(id);
                return View(Employment);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Employment>> Edit(string id, Employment value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _Employment.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _Employment.Remove(id);

            // it won't be null
           // var testEmployment = await _Employment.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testEmployment = await _Employment.GetById(id);

            return RedirectToAction("Index");
        }
    }
}