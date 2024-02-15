using static Program;
public class UserPresence {
    public UserPresence(int UserId,string username,int TimeZone,int CountryId, int Permissions, float Longitude, float Latitude, int Rank)
    {
        this.UserId = UserId;
        this.Username = username;
        this.TimeZone = TimeZone;
        this.CountryId = CountryId;
        this.Permissions = Permissions;
        this.Longitude = Longitude;
        this.Latitude = Latitude;
        this.Rank = Rank;
    }
    public void WriteToStream(Writer bw)
    {
        bw.Write(this.UserId);
        bw.Write(this.Username);
        bw.Write(this.TimeZone);
        bw.Write(this.CountryId);
        bw.Write(this.Permissions);
        bw.Write(this.Longitude);
        bw.Write(this.Latitude);
        bw.Write(this.Rank);
        bw.Flush();
    }
    public int UserId;
    public string Username;
    public int TimeZone;
    public int CountryId;
    public int Permissions;
    public float Longitude;
    public float Latitude;
    public int Rank;
}