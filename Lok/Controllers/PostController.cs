﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Lok.Data.Interface;
using Lok.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Lok.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize("Admin", AuthenticationSchemes = "AdminCookie")]

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

            var a = HttpContext.User;
            string name= a.Claims.Where(c => c.Type == ClaimTypes.Name)
                                           .Select(c => c.Value).SingleOrDefault();

            var Posts = await _Post.GetAll();
                return View(Posts);
            }

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
            [HttpPost]
            public async Task<ActionResult<Post>> Edit(string id, Post value)
            {
                // var product = new Product(value.Id);
                value.Id = ObjectId.Parse(id);
                _Post.Update(value, id);

                await _uow.Commit();

                return RedirectToAction("Index");
            }

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