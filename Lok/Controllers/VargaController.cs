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
    public class VargaController : Controller
    {
        private readonly IVargaRepository _Varga;
        private readonly IUnitOfWork _uow;

        public VargaController(IVargaRepository Varga, IUnitOfWork uow)
        {
            _Varga = Varga;
            _uow = uow;
        }
        // GET: Varga
        public async Task<ActionResult> Index()
        {
            var Vargas = await _Varga.GetAll();
            return View(Vargas);
        }

        public ActionResult<Varga> Create()
        {
            Varga value = new Varga();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Varga>> Create(Varga value)
        {
            //Varga obj = new Varga(value);
            _Varga.Add(value);

            // it will be null
            //var testVarga = await _Varga.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Varga>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Varga = await _Varga.GetById(id);
                return View(Varga);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Varga>> Edit(string id, Varga value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _Varga.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _Varga.Remove(id);

            // it won't be null
           // var testVarga = await _Varga.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testVarga = await _Varga.GetById(id);

            return RedirectToAction("Index");
        }
    }
}