using System.Net;
using BanchoSharp.Lists;
using BanchoSharp.Structures;

namespace BanchoSharp.Utils {
    public class Auth {
        
        public static string TryAuth(string username, string password)
        {
            string reply = new WebClient().DownloadString(Config.BACKEND_URL+$"api/auth.php?username={username}&password={password}");
            if(reply.StartsWith("0"))
            {
                return "fail";
            } else {
                string[] parsed = reply.Split("|");
                if( parsed.Length < 3)
                {
                    return "fail";
                } else {
                    return reply;
                }
            }
        }
        public static string GetByUsername(string username)
        {
            string reply = new WebClient().DownloadString(Config.BACKEND_URL+$"api/FindUser.php?username={username}");
            string[] parsed = reply.Split("|");
            if( parsed.Length < 2)
            {
                return "fail";
            } else {
                return reply;
            }
        }
        public static User CreateUser(string username, string password)
        {
            User user = new User(Users.users.Count+1,username,password,new bUserStats(Users.users.Count+1, new bUserStatus(0,"","",0,1,0),0,1f,0,0,1,0));
            Users.users.Add(user);
            return user;
        }
    }
}