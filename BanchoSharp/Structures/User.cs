namespace BanchoSharp.Structures {
    public class User {
        public User(int userid, string username, string password, bUserStats stats)
        {
            this.userid = userid;
            this.username = username;
            this.password = password;
            this.userstats = stats;
        }
        public int userid;
        public string username;
        public string password;
        public bUserStats userstats;
    }
}