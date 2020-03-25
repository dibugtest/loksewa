using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Lok.Controllers
{
    public class GroupController : Controller
    {
            private readonly IGroupRepository _Group;
            private readonly IUnitOfWork _uow;

            public GroupController(IGroupRepository Group, IUnitOfWork uow)
            {
                _Group = Group;
                _uow = uow;
            }
            // GET: Group
            public async Task<ActionResult> Index()
            {
                var Groups = await _Group.GetAll();
                return View(Groups);
            }

            public ActionResult<Group> Create()
            {
                Group value = new Group();
                return View();
            }
            [HttpPost]
            public async Task<ActionResult<Group>> Create(Group value)
            {
                //Group obj = new Group(value);
                _Group.Add(value);

                // it will be null
                //var testGroup = await _Group.GetById(value.);

                // If everything is ok then:
                await _uow.Commit();

                // The product will be added only after commit
                // testProduct = await _productRepository.GetById(product.Id);

                return RedirectToAction("Index");
            }
            [HttpGet]
            public async Task<ActionResult<Group>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Group = await _Group.GetById(id);
                    return View(Group);
                }
                else
                    return BadRequest();

            }
            [HttpPost]
            public async Task<ActionResult<Group>> Edit(string id, Group value)
            {
                // var product = new Product(value.Id);
                value.Id = ObjectId.Parse(id);
                _Group.Update(value, id);

                await _uow.Commit();

                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<ActionResult> Delete(string id)
            {
                _Group.Remove(id);

                // it won't be null
                var testGroup = await _Group.GetById(id);

                // If everything is ok then:
                await _uow.Commit();

                // not it must by null
                testGroup = await _Group.GetById(id);

                return RedirectToAction("Index");
            }
        

    }
}