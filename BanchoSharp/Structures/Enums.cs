namespace BanchoSharp.Structures {
    public enum SlotStatus
    {
        Open = 1,
        Locked = 2,
        NotReady = 4,
        Ready = 8,
        NoMap = 16,
        Playing = 32,
        Complete = 64,
        HasPlayer = 124
    }
    public enum MatchTypes
    {
        Standard,
        Powerplay
    }
    public enum Mods
    {

        None,
        NoFail,
        Easy,
        NoVideo = 4,
        Hidden = 8,
        HardRock = 16,
        SuddenDeath = 32,
        DoubleTime = 64,
        Relax = 128,
        HalfTime = 256,
        Taiko = 512
    }
}