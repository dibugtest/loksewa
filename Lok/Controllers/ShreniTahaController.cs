using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace Lok.Controllers
{
    public class ShreniTahaController : Controller
    {
        private readonly IShreniTahaRepository _ShreniTaha;
        private readonly IUnitOfWork _uow;

        public ShreniTahaController(IShreniTahaRepository ShreniTaha, IUnitOfWork uow)
        {
            _ShreniTaha = ShreniTaha;
            _uow = uow;
        }
        // GET: ShreniTaha
        public async Task<ActionResult> Index()
        {
            var ShreniTahas = await _ShreniTaha.GetAll();
            return View(ShreniTahas);
        }

        public ActionResult<ShreniTaha> Create()
        {
            ShreniTaha value = new ShreniTaha();
            ViewBag.Type = new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" },
                                                      new SelectListItem { Text = "Non-Government", Value = "Non-Government" } };
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ShreniTaha>> Create(ShreniTaha value)
        {
            ViewBag.Type = new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" },
                                                      new SelectListItem { Text = "Non-Government", Value = "Non-Government" } };
            //ShreniTaha obj = new ShreniTaha(value);
            _ShreniTaha.Add(value);

            // it will be null
            //var testShreniTaha = await _ShreniTaha.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<ShreniTaha>> Edit(string id)
        {
            ViewBag.Type = new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" },
                                                      new SelectListItem { Text = "Non-Government", Value = "Non-Government" } };
            if (!string.IsNullOrEmpty(id))
            {
                var ShreniTaha = await _ShreniTaha.GetById(id);
                return View(ShreniTaha);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<ShreniTaha>> Edit(string id, ShreniTaha value)
        {
            ViewBag.Type = new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" },
                                                      new SelectListItem { Text = "Non-Government", Value = "Non-Government" } };
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _ShreniTaha.Update(value, id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _ShreniTaha.Remove(id);

            // it won't be null
            // var testShreniTaha = await _ShreniTaha.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
            //  testShreniTaha = await _ShreniTaha.GetById(id);

            return RedirectToAction("Index");
        }
    }
}