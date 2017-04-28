using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAL_Demo.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult Login()
        {
            Models.LoginView loginView = new Models.LoginView();
            return View(loginView);
        }
        [HttpPost]
        public ActionResult Login(Models.LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                Models.User user = DB.Users.GetByName(loginView.UserName);
                if (user != null)
                {
                    if (user.Password == loginView.Password)
                    {
                        Session["CurrentUser"] = user;
                        bool error = false;
                        try
                        {
                            DB.DataBase.BeginTransaction();
                            DB.Logs.Login(user.Id);
                        }
                        catch (Exception)
                        {
                            error = true;
                        }
                        finally
                        {
                            DB.DataBase.EndTransaction(error);
                        }
                        return RedirectToAction("Index", "Logs");
                    }
                    else
                        ModelState.AddModelError("Password", "Mode de passe incorrect!");
                }
                else
                    ModelState.AddModelError("UserName", "Ce nom d'usager n'existe pas!");
            }
            return View(loginView);
        }

        public ActionResult Logout()
        {
            bool error = false;
            try
            {
                DB.DataBase.BeginTransaction();
                DB.Logs.Logout(((Models.User)Session["CurrentUser"]).Id);
            }
            catch (Exception)
            {
                error = true;
            }
            finally
            {
                DB.DataBase.EndTransaction(error);
            }

            Session["CurrentUser"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            Models.UserView userView = new Models.UserView();
            return View(userView);
        }
        [HttpPost]
        public ActionResult Create(Models.UserView userview)
        {
            if (ModelState.IsValid)
            {
                Models.User userFound = DB.Users.GetByName(userview.Name);
                if (userFound == null)
                {
                    bool error = false;
                    try
                    {
                        DB.DataBase.BeginTransaction();
                        DB.Users.Add(userview.ToUser());
                    }
                    catch (Exception)   
                    {
                        error = true;
                    }
                    finally
                    {
                        DB.DataBase.EndTransaction(error);
                    }   
                    return RedirectToAction("Login", "Accounts");
                }
                else
                    ModelState.AddModelError("Name", "Ce nom d'usager existe déjà!");
            }
            return View(userview);
        }

        public ActionResult Edit(int id)
        {
                Models.User user = DB.Users.Get(id);
                if (user != null)
                {
                    Models.UserView userView = new Models.UserView(user);
                    return View(userView);
                }
                return RedirectToAction("Index", "Home");           
        }
        [HttpPost]
        public ActionResult Edit(Models.UserView userView)
        {
            if (ModelState.IsValid)
            {
                Session["CurrentUser"] = userView.ToUser();
                bool error = false;
                try
                {
                    DB.DataBase.BeginTransaction();
                    DB.Users.Update((Models.User)Session["CurrentUser"]);
                }
                catch (Exception)
                {
                    error = true;
                }
                finally
                {
                    DB.DataBase.EndTransaction(error);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(userView);
        }
    }
}