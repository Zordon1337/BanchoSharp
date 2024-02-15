using static Program;

class bUserStats
{
        public bUserStatus Status;
        public float Accuracy;
        public int playCount;
        public int rankPosition;
        public int Id;
        public long totalScore;
        public long rankedScore;
        public short perfomancePoints;

        public bUserStats(int ID, bUserStatus info, long totalScore, float Accuracy, int playCount, long rankedScore, int rankPosition,
            short pp)
        {
            this.Id = ID;
            this.Status = info;
            this.totalScore = totalScore;
            this.Accuracy = Accuracy;
            this.playCount = playCount;
            this.rankedScore = rankedScore;
            this.rankPosition = rankPosition;
            this.perfomancePoints = pp;
        }

        

        public void WriteToStream(Writer writer)
        {
            writer.Write(Id);
            Status.WriteToStream(writer);
            writer.Write(totalScore);
            writer.Write(Accuracy);
            writer.Write(playCount);
            writer.Write(rankedScore);
            writer.Write(rankPosition);
            writer.Write(perfomancePoints);
        }
}

