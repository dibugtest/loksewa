using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace PPMS.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _service;
        private readonly IUnitOfWork _uow;

        public ServiceController(IServiceRepository service, IUnitOfWork uow)
        {
            _service = service;
            _uow = uow;
        }
        // GET: Service
        public async Task<ActionResult> Index()
        {
            var Services = await _service.GetAll();
            return View(Services);
        }

        public ActionResult<Service> Create()
        {
            Service value = new Service();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Service>> Create(Service value)
        {
            //Service obj = new Service(value);
            _service.Add(value);

            // it will be null
            //var testService = await _service.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Details(string id) {
            if (!string.IsNullOrEmpty(id))
            {
                var Service = await _service.GetById(id);
               
                return View(Service);
            }
            else
                return BadRequest();


        }
        [HttpGet]
        public async Task<ActionResult<Service>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Service = await _service.GetById(id);
                return View(Service);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Service>> Edit(string id, Service value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _service.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _service.Remove(id);

            // it won't be null
            var testService = await _service.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
            testService = await _service.GetById(id);

            return RedirectToAction("Index");
        }
    }
}