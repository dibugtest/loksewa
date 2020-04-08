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
    public class FacultyController : Controller
    {
        private readonly IFacultyRepository _Faculty;
        private readonly IUnitOfWork _uow;

        public FacultyController(IFacultyRepository Faculty, IUnitOfWork uow)
        {
            _Faculty = Faculty;
            _uow = uow;
        }
        // GET: Faculty
        public async Task<ActionResult> Index()
        {
            var Facultys = await _Faculty.GetAll();
            return View(Facultys);
        }

        public ActionResult<Faculty> Create()
        {
            Faculty value = new Faculty();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Faculty>> Create(Faculty value)
        {
            //Faculty obj = new Faculty(value);
            _Faculty.Add(value);

            // it will be null
            //var testFaculty = await _Faculty.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<Faculty>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Faculty = await _Faculty.GetById(id);
                return View(Faculty);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<Faculty>> Edit(string id, Faculty value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _Faculty.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _Faculty.Remove(id);

            // it won't be null
           // var testFaculty = await _Faculty.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testFaculty = await _Faculty.GetById(id);

            return RedirectToAction("Index");
        }
    }
}