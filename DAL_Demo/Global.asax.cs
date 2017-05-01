using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DAL_Demo
{
    public sealed class DB
    {
        private static readonly DB instance = new DB();
        private DB() { }
        public static Models.Logs Logs { get; set; }
        public static Models.Users Users { get; set; }
        public static Models.Posts Posts { get; set; }
        public static DAL.DataBase DataBase { get; set; }
        public static void Initialize(string DB_Path, string SQL_Journal_Path = "")
        {
            String MainDB_Connection_String = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" +
                                              DB_Path + 
                                              "'; Integrated Security=true;Max Pool Size=1024;Pooling=true;";
            DataBase = new DAL.DataBase(MainDB_Connection_String, SQL_Journal_Path);
            DataBase.TrackSQL = true;
            Logs = new Models.Logs(DataBase);
            Users = new Models.Users(DataBase);
            Posts = new Models.Posts(DataBase);
        }
    }
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DB.Initialize(Server.MapPath(@"~\App_Data\MainDB.mdf"), 
                          Server.MapPath(@"~\App_Data\SQL_Journal.txt"));
        }
    }
}
