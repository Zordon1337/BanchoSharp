using StreamUtils;
using static Program;

public class bUserStatus {
    public bUserStatus(int status, string beatmapHash, string beatmap, int mods, int playMode,int beatmapId)
    {
        this.status = status;
        this.beatmap = beatmap;
        this.beatmapHash = beatmapHash;
        this.mods = mods;
        this.playMode = playMode;
        this.beatmapId = beatmapId;
    }

    public void WriteToStream(Writer bw)
    {
        bw.Write((byte) status);
        bw.Write(beatmapHash);
        bw.Write(beatmap);
        bw.Write((uint) mods);
        bw.Write((byte) playMode);
        bw.Write(beatmapId);
       

    }
    public string beatmap;
    public string beatmapHash;
    public int beatmapId;
    public int mods;
    public int playMode;
    public int status;
}