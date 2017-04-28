using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAL_Demo.Controllers
{
    public class PostsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        // GET: Posts
        public ActionResult Get(int id)
        {
            Models.Post post = DB.Posts.Get(id);
            return new JsonResult { Data = post, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult List()
        {
            var posts = DB.Posts.ToList();
            return PartialView("_Posts", posts);
        }

        public ActionResult Edit(int id)
        {
            Models.Post post = null;
            if (id == 0)
                post = new Models.Post();
            else
                post = DB.Posts.Get(id);

            return PartialView("_Edit", post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.Post post)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                bool error = false;
                try
                {
                    DB.DataBase.BeginTransaction();
                    if (post.Id == 0)
                        DB.Posts.Add(post);
                    else
                        DB.Posts.Update(post);
                }
                catch (Exception)
                {
                    error = true;
                }
                finally
                {
                    status = !error;
                    DB.DataBase.EndTransaction(error);
                }
            }
            return new JsonResult { Data = new { status = status } };
        }


        public ActionResult Delete(int id)
        {
            bool status = false;
            Models.Post post = DB.Posts.Get(id);
            if (post != null)
            {
                bool error = false;
                try
                {
                    DB.DataBase.BeginTransaction();
                    DB.Posts.Delete(id);
                }
                catch (Exception)
                {
                    error = true;
                }
                finally
                {
                    status = !error;
                    DB.DataBase.EndTransaction(error);
                }
            }
            return new JsonResult { Data = new { status = status }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}