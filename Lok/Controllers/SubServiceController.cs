using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lok.Controllers
{
    public class SubServiceController : Controller
    {
        private readonly ISubService _SubService;
        private readonly IUnitOfWork _uow;
        private readonly IServiceRepository _service;


        public SubServiceController(ISubService SubService, IUnitOfWork uow,IServiceRepository service)
        {
            _SubService = SubService;
            _uow = uow;
            _service = service;
        }
        // GET: SubService
        public async Task<ActionResult> Index()
        {
            var SubServices = await _SubService.GetAll();
            
            return View(SubServices);
        }

        public async Task<ActionResult<SubService>> Create()
        {
            SubService value = new SubService();
            ViewBag.ServiceId = new SelectList(await _service.GetAll(), "Id", "ServiceName");

            return View();
        }
        [HttpPost]
        public async Task<ActionResult<SubService>> Create(SubService value)
        {
            //SubService obj = new SubService(value);
            value.Service = await _service.GetById(value.ServiceId.ToString());

            _SubService.Add(value);

            // it will be null
            //var testSubService = await _SubService.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<SubService>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var subservice = await _SubService.GetById(id);

                if (String.IsNullOrEmpty(subservice.ServiceId))
                {
                    ViewBag.ServiceId= new SelectList(await _service.GetAll(), "Id", "ServiceName", subservice.ServiceId);
                }
                else
                {
                    ViewBag.ServiceId = new SelectList(await _service.GetAll(), "Id", "ServiceName", subservice.ServiceId);
                }

                return View(subservice);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<SubService>> Edit(string id, SubService value)
        {
            value.Id = ObjectId.Parse(id);
            value.Service = await _service.GetById(value.ServiceId.ToString());

            _SubService.Update(value, id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _SubService.Remove(id);

            // it won't be null
            var testSubService = await _SubService.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
            testSubService = await _SubService.GetById(id);

            return RedirectToAction("Index");
        }

    }
}