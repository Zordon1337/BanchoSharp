using System.Text;

namespace StreamUtils {
    public class Writer : BinaryWriter
    {
        public Writer(Stream s) : base(s) { }
        public void WriteUnsigned(uint value)
        {
            do
            {
                byte byteValue = (byte)(value & 0x7F);
                value >>= 7;

                if (value != 0)
                    byteValue |= 0x80;

                this.Write(byteValue);
            } while (value != 0);
        }
        
        public override void Write(string value)
        {
            if (value.Length != 0)
            {
                this.Write((byte)11); 
                WriteUnsigned((uint)value.Length);
                this.Write(Encoding.ASCII.GetBytes(value));
            }
            else
            {
                this.Write((byte)0);
            }
        }
        
    }
    public class Reader : BinaryReader
    {
        public Reader(Stream s, Encoding en): base(s,en){}
        public uint ReadUnsigned()
        {
            uint result = 0;
            int shift = 0;

            while (true)
            {
                byte byteValue = this.ReadByte();
                result |= (uint)((byteValue & 0x7F) << shift);

                if ((byteValue & 0x80) == 0)
                    break;

                shift += 7;
            }

            return result;
        }
        public override string ReadString()
        {

            uint length = this.ReadUnsigned();
            byte[] data = this.ReadBytes((int)length);

            return Encoding.ASCII.GetString(data);
        }
    }
    public class Utils {
        public static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length / 2;
            byte[] byteArray = new byte[length];
            for (int i = 0; i < length; i++)
            {
                byteArray[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return byteArray;
        }
    }
}