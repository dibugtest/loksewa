using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Data.Repository;
using Lok.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Lok.Controllers
{
    public class CategoryController : Controller
    {
       
            private readonly ICategoryRepository _Category;
            private readonly IUnitOfWork _uow;

            public CategoryController(ICategoryRepository Category, IUnitOfWork uow)
            {
                _Category = Category;
                _uow = uow;
            }
            // GET: Category
            public async Task<ActionResult> Index()
            {
                var Categorys = await _Category.GetAll();
                return View(Categorys);
            }

            public ActionResult<Category> Create()
            {
                Category value = new Category();
                return View();
            }
            [HttpPost]
            public async Task<ActionResult<Category>> Create(Category value)
            {
                //Category obj = new Category(value);
                _Category.Add(value);

                // it will be null
                //var testCategory = await _Category.GetById(value.);

                // If everything is ok then:
                await _uow.Commit();

                // The product will be added only after commit
                // testProduct = await _productRepository.GetById(product.Id);

                return RedirectToAction("Index");
            }
            public async Task<ActionResult> Details(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Category = await _Category.GetById(id);

                    return View(Category);
                }
                else
                    return BadRequest();


            }
            [HttpGet]
            public async Task<ActionResult<Category>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Category = await _Category.GetById(id);
                    return View(Category);
                }
                else
                    return BadRequest();

            }
            [HttpPost]
            public async Task<ActionResult<Category>> Edit(string id, Category value)
            {
                // var product = new Product(value.Id);
                value.Id = ObjectId.Parse(id);
                _Category.Update(value, id);

                await _uow.Commit();

                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<ActionResult> Delete(string id)
            {
                _Category.Remove(id);

                // it won't be null
                var testCategory = await _Category.GetById(id);

                // If everything is ok then:
                await _uow.Commit();

                // not it must by null
                testCategory = await _Category.GetById(id);

                return RedirectToAction("Index");
            }
        }

    }
