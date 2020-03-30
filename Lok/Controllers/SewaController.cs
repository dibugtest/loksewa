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
    public class SewaController : Controller
    {
        private readonly ISewaRepository _Sewa;
        private readonly IUnitOfWork _uow;

        public SewaController(ISewaRepository Sewa, IUnitOfWork uow)
        {
            _Sewa = Sewa;
            _uow = uow;
        }
        // GET: Sewa
        public async Task<ActionResult> Index()
        {
            var Sewas = await _Sewa.GetAll();
            return View(Sewas);
        }

        public ActionResult<Sewa> Create()
        {
            Sewa value = new Sewa();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Sewa>> Create(Sewa value)
        {
            //Sewa obj = new Sewa(value);
            _Sewa.Add(value);

            // it will be null
            //var testSewa = await _Sewa.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Sewa>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Sewa = await _Sewa.GetById(id);
                return View(Sewa);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Sewa>> Edit(string id, Sewa value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _Sewa.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _Sewa.Remove(id);

            // it won't be null
           // var testSewa = await _Sewa.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testSewa = await _Sewa.GetById(id);

            return RedirectToAction("Index");
        }
    }
}