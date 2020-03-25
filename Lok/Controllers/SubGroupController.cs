using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace Lok.Controllers
{
    public class SubGroupController : Controller
    {
        
            private readonly ISubGroupRepository _SubGroup;
            private readonly IUnitOfWork _uow;
            private readonly IGroupRepository _Group;


            public SubGroupController(ISubGroupRepository SubGroup, IUnitOfWork uow, IGroupRepository Group)
            {
                _SubGroup = SubGroup;
                _uow = uow;
                _Group = Group;
            }
            // GET: SubGroup
            public async Task<ActionResult> Index()
            {
                var SubGroups = await _SubGroup.GetAll();

                return View(SubGroups);
            }

            public async Task<ActionResult<SubGroup>> Create()
            {
                SubGroup value = new SubGroup();
                ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName");

                return View();
            }
            [HttpPost]
            public async Task<ActionResult<SubGroup>> Create(SubGroup value)
            {
                //SubGroup obj = new SubGroup(value);
                value.Group = await _Group.GetById(value.GroupId.ToString());

                _SubGroup.Add(value);

                // it will be null
                //var testSubGroup = await _SubGroup.GetById(value.);

                // If everything is ok then:
                await _uow.Commit();

                // The product will be added only after commit
                // testProduct = await _productRepository.GetById(product.Id);

                return RedirectToAction("Index");
            }
            [HttpGet]
            public async Task<ActionResult<SubGroup>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var SubGroup = await _SubGroup.GetById(id);

                    if (String.IsNullOrEmpty(SubGroup.GroupId))
                    {
                        ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName", SubGroup.GroupId);
                    }
                    else
                    {
                        ViewBag.GroupId = new SelectList(await _Group.GetAll(), "Id", "GroupName", SubGroup.GroupId);
                    }

                    return View(SubGroup);
                }
                else
                    return BadRequest();

            }
            [HttpPost]
            public async Task<ActionResult<SubGroup>> Edit(string id, SubGroup value)
            {
                value.Id = ObjectId.Parse(id);
                value.Group = await _Group.GetById(value.GroupId.ToString());

                _SubGroup.Update(value, id);

                await _uow.Commit();

                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<ActionResult> Delete(string id)
            {
                _SubGroup.Remove(id);

                // it won't be null
                var testSubGroup = await _SubGroup.GetById(id);

                // If everything is ok then:
                await _uow.Commit();

                // not it must by null
                testSubGroup = await _SubGroup.GetById(id);

                return RedirectToAction("Index");
            }

        

    }
}