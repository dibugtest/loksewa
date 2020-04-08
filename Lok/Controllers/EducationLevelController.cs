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
    public class EducationLevelController : Controller
    {
        private readonly IEducationLevelRepository _EducationLevel;
        private readonly IUnitOfWork _uow;

        public EducationLevelController(IEducationLevelRepository EducationLevel, IUnitOfWork uow)
        {
            _EducationLevel = EducationLevel;
            _uow = uow;
        }
        // GET: EducationLevel
        public async Task<ActionResult> Index()
        {
            var EducationLevels = await _EducationLevel.GetAll();
            return View(EducationLevels);
        }

        public ActionResult<EducationLevel> Create()
        {
            EducationLevel value = new EducationLevel();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<EducationLevel>> Create(EducationLevel value)
        {
            //EducationLevel obj = new EducationLevel(value);
            _EducationLevel.Add(value);

            // it will be null
            //var testEducationLevel = await _EducationLevel.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<EducationLevel>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var EducationLevel = await _EducationLevel.GetById(id);
                return View(EducationLevel);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<EducationLevel>> Edit(string id, EducationLevel value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _EducationLevel.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _EducationLevel.Remove(id);

            // it won't be null
           // var testEducationLevel = await _EducationLevel.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testEducationLevel = await _EducationLevel.GetById(id);

            return RedirectToAction("Index");
        }
    }
}