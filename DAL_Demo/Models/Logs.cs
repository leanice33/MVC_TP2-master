using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL_Demo.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Login { get; set; }
        public DateTime Logout { get; set; }

        public Log()
        {
            Login = Logout = DateTime.Now;
        }

        public Log(int userId)
        {
            UserId = userId;
            Login = Logout = DateTime.Now;
        }
    }

    public class Logs : DAL.RecordsDB<Log>
    {
        public Logs(DAL.DataBase dataBase) : base(dataBase)
        {
            SetCache(true, "SELECT * FROM Logs ORDER BY Login DESC");
        }
        public void Login(int UserId)
        {
            Add(new Log(UserId));
        }

        public List<Log> UserLogs(int userId)
        {
            return ToList().
                   Where(log => log.UserId == userId).
                   OrderByDescending(log => log.Login).
                   ToList();
        }
        public void Logout(int userId)
        {
            List<Log> userLogs = UserLogs(userId);
            if (userLogs.Count > 0)
            {
                userLogs[0].Logout = DateTime.Now;
                Update(userLogs[0]);
            }
        }
    }

}