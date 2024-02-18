public class Score {
	public Score(string md5,string username, string score, string combo, string fc, string mods, string h300, string h100,string h50, string geki,string katu, string miss, DateTime time, int mode)
	{
		this.mapmd5 = md5;
		this.username = username;
		this.score = score;
		this.combo = combo;
		this.fc = fc;
		this.mods = mods;
		this.Count300 = h300;
		this.Count100 = h100;
		this.Count50 = h50;
		this.CountGeki = geki;
		this.CountKatu = katu;
		this.CountMiss = miss;
		this.Date = time;
		this.PlayMode = mode;
	}
	public string mapmd5;
	public string username;
	public string score;
	public string combo;
	public string fc;
	public string mods;
    public string Count100;
	public string Count300;
	public string Count50;
	public string CountGeki;
	public string CountKatu;
	public string CountMiss;
    public DateTime Date;
	public int PlayMode;
	
	
}