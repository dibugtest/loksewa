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
    public class AwasthaController : Controller
    {
        private readonly IAwasthaRepository _Awastha;
        private readonly IUnitOfWork _uow;

        public AwasthaController(IAwasthaRepository Awastha, IUnitOfWork uow)
        {
            _Awastha = Awastha;
            _uow = uow;
        }
        // GET: Awastha
        public async Task<ActionResult> Index()
        {
            var Awasthas = await _Awastha.GetAll();
            return View(Awasthas);
        }

        public ActionResult<Awastha> Create()
        {
            Awastha value = new Awastha();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Awastha>> Create(Awastha value)
        {
            //Awastha obj = new Awastha(value);
            _Awastha.Add(value);

            // it will be null
            //var testAwastha = await _Awastha.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Awastha>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Awastha = await _Awastha.GetById(id);
                return View(Awastha);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Awastha>> Edit(string id, Awastha value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _Awastha.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _Awastha.Remove(id);

            // it won't be null
           // var testAwastha = await _Awastha.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testAwastha = await _Awastha.GetById(id);

            return RedirectToAction("Index");
        }
    }
}