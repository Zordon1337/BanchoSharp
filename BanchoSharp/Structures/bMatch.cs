using System.Text;
using StreamUtils;

namespace BanchoSharp.Structures {
    public class bMatch {
        public bMatch(int MatchId,MatchTypes matchType, int playMode, string gameName, string beatmapName, string beatmapChecksum, int beatmapId, Mods activeMods, int hostId)
        {
            this.matchId = MatchId;
            this.matchType = matchType;
            this.playMode = playMode;
            this.gameName = gameName;
            this.beatmapName = beatmapName;
            this.beatmapChecksum = beatmapChecksum;
            this.beatmapId = beatmapId;
            this.activeMods = activeMods;
            this.hostId = hostId;
            for (int i = 0; i < 8; i++)
            {
                this.slotStatus[i] = SlotStatus.Open;
                this.slotId[i] = -1;
            }
        }

        public bMatch(Stream s)
        {
            Reader sr = new Reader(s,Encoding.UTF8);
            this.matchId = (int)sr.ReadByte();
            this.inProgress = sr.ReadBoolean();
            this.matchType = (MatchTypes)sr.ReadByte();
            this.activeMods = (Mods)sr.ReadInt16();
            this.gameName = sr.ReadString();
            this.beatmapName = sr.ReadString();
            this.beatmapId = sr.ReadInt32();
            this.beatmapChecksum = sr.ReadString();
            for (int i = 0; i < 8; i++)
            {
                this.slotStatus[i] = (SlotStatus)sr.ReadByte();
            }
            for (int j = 0; j < 8; j++)
            {
                this.slotId[j] = (((this.slotStatus[j] & SlotStatus.HasPlayer) > (SlotStatus)0) ? sr.ReadInt32() : -1);
            }
            this.hostId = sr.ReadInt32();
            this.playMode = (int)sr.ReadByte();
        }

        public int slotUsedCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    if ((this.slotStatus[i] & SlotStatus.HasPlayer) > (SlotStatus)0)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public int slotOpenCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (this.slotStatus[i] != SlotStatus.Locked)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public int slotReadyCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (this.slotStatus[i] == SlotStatus.Ready)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public void ReadFromStream(Reader sr)
        {
            throw new NotImplementedException();
        }

        public void WriteToStream(Writer sw)
        {
            sw.Write((byte)this.matchId);
            sw.Write(this.inProgress);
            sw.Write((byte)this.matchType);
            sw.Write((short)this.activeMods);
            sw.Write(this.gameName);
            sw.Write(this.beatmapName);
            sw.Write(this.beatmapId);
            sw.Write(this.beatmapChecksum);
            for (int i = 0; i < 8; i++)
            {
                sw.Write((byte)this.slotStatus[i]);
            }
            for (int j = 0; j < 8; j++)
            {
                if ((this.slotStatus[j] & SlotStatus.HasPlayer) > (SlotStatus)0)
                {
                    sw.Write(this.slotId[j]);
                }
            }
            sw.Write(this.hostId);
            sw.Write((byte)this.playMode);
            sw.Flush();
        }

        private static byte bools2byte(bool[] bools)
        {
            byte outbyte = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (bools[i])
                {
                    outbyte |= 1;
                }
                if (i > 0)
                {
                    outbyte = (byte)(outbyte << 1);
                }
            }
            return outbyte;
        }

        private static bool[] byte2bools(byte inbyte)
        {
            bool[] bools = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                bools[i] = ((inbyte >> i & 1) > 0);
            }
            return bools;
        }

        public int findPlayerFromId(int userId)
        {
            int pos = 0;
            while (pos < 8 && this.slotId[pos] != userId)
            {
                pos++;
            }
            if (pos > 7)
            {
                return -1;
            }
            return pos;
        }

        public string gameName;

        public int matchId;

        public MatchTypes matchType;

        public SlotStatus[] slotStatus = new SlotStatus[8];

        public int[] slotId = new int[8];

        public string beatmapName;

        public string beatmapChecksum;

        public int beatmapId = -1;

        public bool inProgress;

        public Mods activeMods;

        public int hostId;

        public int playMode;

        public const int slotCount = 8;
    }
}
