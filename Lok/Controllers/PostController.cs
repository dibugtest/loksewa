using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Lok.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    public class PostController : Controller
    {
        
            private readonly IPostRepository _Post;
            private readonly IUnitOfWork _uow;

            public PostController(IPostRepository Post, IUnitOfWork uow)
            {
                _Post = Post;
                _uow = uow;
            }
            // GET: Post
            [Authorize("Admin", AuthenticationSchemes = "AdminCookie")]
            public async Task<ActionResult> Index()
            {
                var Posts = await _Post.GetAll();
                return View(Posts);
            }
        [Authorize("Admin", AuthenticationSchemes = "AdminCookie")]

        public ActionResult<Post> Create()
            {
                Post value = new Post();
                return View();
            }
            [HttpPost]
            public async Task<ActionResult<Post>> Create(Post value)
            {
                //Post obj = new Post(value);
                _Post.Add(value);

                // it will be null
                //var testPost = await _Post.GetById(value.);

                // If everything is ok then:
                await _uow.Commit();

                // The product will be added only after commit
                // testProduct = await _productRepository.GetById(product.Id);

                return RedirectToAction("Index");
            }
        [Authorize("Admin", AuthenticationSchemes = "AdminCookie")]

        public async Task<ActionResult> Details(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Post = await _Post.GetById(id);

                    return View(Post);
                }
                else
                    return BadRequest();


            }
        [Authorize("Admin", AuthenticationSchemes = "AdminCookie")]

        [HttpGet]
            public async Task<ActionResult<Post>> Edit(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var Post = await _Post.GetById(id);
                    return View(Post);
                }
                else
                    return BadRequest();

            }
        [Authorize("Admin", AuthenticationSchemes = "AdminCookie")]

        [HttpPost]
            public async Task<ActionResult<Post>> Edit(string id, Post value)
            {
                // var product = new Product(value.Id);
                value.Id = ObjectId.Parse(id);
                _Post.Update(value, id);

                await _uow.Commit();

                return RedirectToAction("Index");
            }
        [Authorize("Admin", AuthenticationSchemes = "AdminCookie")]

        [HttpGet]
            public async Task<ActionResult> Delete(string id)
            {
                _Post.Remove(id);

                // it won't be null
                var testPost = await _Post.GetById(id);

                // If everything is ok then:
                await _uow.Commit();

                // not it must by null
                testPost = await _Post.GetById(id);

                return RedirectToAction("Index");
            }
        

    }
}