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
    public class BoardNameController : Controller
    {
        private readonly IBoardNameRepository _BoardName;
        private readonly IUnitOfWork _uow;

        public BoardNameController(IBoardNameRepository BoardName, IUnitOfWork uow)
        {
            _BoardName = BoardName;
            _uow = uow;
        }
        // GET: BoardName
        public async Task<ActionResult> Index()
        {
            var BoardNames = await _BoardName.GetAll();
            return View(BoardNames);
        }

        public ActionResult<BoardName> Create()
        {
            BoardName value = new BoardName();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<BoardName>> Create(BoardName value)
        {
            //BoardName obj = new BoardName(value);
            _BoardName.Add(value);

            // it will be null
            //var testBoardName = await _BoardName.GetById(value.);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            // testProduct = await _productRepository.GetById(product.Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult<BoardName>> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var BoardName = await _BoardName.GetById(id);
                return View(BoardName);
            }
            else
                return BadRequest();

        }
        [HttpPost]
        public async Task<ActionResult<BoardName>> Edit(string id, BoardName value)
        {
            // var product = new Product(value.Id);
            value.Id = ObjectId.Parse(id);
            _BoardName.Update(value,id);

            await _uow.Commit();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            _BoardName.Remove(id);

            // it won't be null
           // var testBoardName = await _BoardName.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
          //  testBoardName = await _BoardName.GetById(id);

            return RedirectToAction("Index");
        }
    }
}