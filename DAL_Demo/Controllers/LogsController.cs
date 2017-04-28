using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAL_Demo.Controllers
{
    [Models.ConnectedUserOnly]
    public class LogsController : Controller
    {
        // GET: Logs
        public ActionResult Index()
        {
            List<Models.Log> logs = null;
            if (((Models.User)Session["CurrentUser"]).UserType == Models.UserType.admin)
                logs = DB.Logs.ToList();
            else
                logs = DB.Logs.UserLogs(((Models.User)Session["CurrentUser"]).Id);
            return View(logs);
        }
    }
}