public class Timestamp {

    public static long ConvertToTimestamp(DateTime value)
    {
        long epoch = (value.Ticks - 621355968000000000) / 10000000;
        return epoch;
    }
}