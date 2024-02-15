using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;

public class Program {
    static bool test = false;
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
    static byte[] StringToByteArray(string hex)
    {
        int length = hex.Length / 2;
        byte[] byteArray = new byte[length];
        for (int i = 0; i < length; i++)
        {
            byteArray[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return byteArray;
    }
    static void Main(string[] args)
    {
        HttpListener srv = new HttpListener();
        srv.Prefixes.Add("http://127.0.0.1:80/");
        srv.Prefixes.Add("http://localhost:80/");
        srv.Start();
        Console.WriteLine("[X] Server started listening at port 80");
        while(true)
        {
            HttpListenerContext context = srv.GetContext();
            
            HttpListenerResponse response = context.Response;
            response.AddHeader("cho-token","shittytoken");
            
            response.AddHeader("cho-protocol", "19");
            Stream ns = response.OutputStream;
            Stream iss = context.Request.InputStream;
            if(context.Request.HttpMethod == "GET" && !context.Request.Url.ToString().Contains("favicon.ico"))
            {
                if(context.Request.UserAgent == "osu!")
                    Console.WriteLine($"[X] Got connection with osu user agent");
                Console.WriteLine("[X] Path: "+context.Request.Url);
            }
            if(context.Request.Url.ToString().Contains("ppy.sh") && context.Request.HttpMethod == "POST" && context.Request.Url.ToString().StartsWith("http://c"))
            {
                //response.ContentType = "application/octet-stream";
                MemoryStream ms = new MemoryStream();
                Writer bw = new Writer(ms);
                
                
                if(context.Request.Headers["osu-token"] == null)
                {
                    
                    StreamReader br = new StreamReader(context.Request.InputStream,Encoding.UTF8);
                    
                    try {
                        
                        
                        var test1 = br.ReadLine(); // i need to fix problem which makes the string getting only partially readed
                        var test2 = br.ReadLine();
                        var test3 = br.ReadLine();
                        
                        string[] lines2 = test3.Split('|');
                        string username = test1;
                        string password = test2;
                        string version = lines2[0];
                        Console.WriteLine($"[X] Player {username} attempted to login from {version}");
                        bw.Write(0);
                        bw.Flush();
                        SendPacket(92,false,ms,ns); // SilenceInfo Packet
                        bw.Write(1);
                        bw.Flush();
                        SendPacket(5,false,ms,ns); // LoginReply Packet
                        bw.Write(19);
                        bw.Flush();
                        SendPacket(75,false,ms,ns); // Protocol Version Packet
                        bw.Write(0);
                        bw.Flush();
                        SendPacket(71,false,ms,ns); // Login Permissions Packet
                        bw.Write("Welcome to BanchoSharp!");
                        bw.Flush();
                        SendPacket(24,false,ms,ns);
                        new UserPresence(1,username,24,1,5,0f,0f,1).WriteToStream(bw);
                        SendPacket(83,false,ms,ns); // UserPresence Packet
                        new bUserStats(1,new bUserStatus(0,"idle","md5",0,0,0),2,1f,1,1,1,1).WriteToStream(bw);
                        SendPacket(11,false,ms,ns); // UserStats Packet
                        SendPacket(96,false,ms,ns); // UserPresence Bundle Packet
                        SendPacket(89,false,ms,ns); // Channel Info Complete Packet
                        bw.Write("#osu");
                        bw.Flush();
                        SendPacket(64,false,ms,ns);
                        bw.Write("#updates");
                        bw.Flush();
                        SendPacket(64,false,ms,ns); //Channel Join Success Packet
                    } catch {}
                    

                
                } else {
                    
                    BinaryReader br = new BinaryReader(context.Request.InputStream);
                    int pid = br.ReadInt16();
                    if(pid != 4)
                    {
                        //Console.WriteLine($"[X] Got connection with user agent: {context.Request.UserAgent}");
                        //Console.WriteLine("[X] Path: "+context.Request.Url);
                        //Console.WriteLine($"Got packet with token: {context.Request.Headers["osu-token"]}");
                        Console.WriteLine("Got packet "+pid);
                    }
                }
                
            } else {
                
            }
            if(context.Request.Url.ToString().Contains("c.ppy.sh") && context.Request.HttpMethod == "GET")
                {
                    response.ContentType = "text/html";
                    StreamWriter sw = new StreamWriter(ns);
                    sw.Write("<pre style='color:red'>");
                    sw.Write(@"  ____                   _           _____ _                      <br/>");
                    sw.Write(@" |  _ \                 | |         / ____| |                     <br/>");
                    sw.Write(@" | |_) | __ _ _ __   ___| |__   ___| (___ | |__   __ _ _ __ _ __  <br/>");
                    sw.Write(@" |  _ < / _` | '_ \ / __| '_ \ / _ \\___ \| '_ \ / _` | '__| '_ \ <br/>");
                    sw.Write(@" | |_) | (_| | | | | (__| | | | (_) |___) | | | | (_| | |  | |_) |<br/>");
                    sw.Write(@" |____/ \__,_|_| |_|\___|_| |_|\___/_____/|_| |_|\__,_|_|  | .__/ <br/>");
                    sw.Write("                                                           | |    <br/>");
                    sw.Write("                                                           |_|    <br/>");
                    sw.Write("<br/>");
                    sw.Write("<br/>");
                    sw.Write("Server powered by <a href='https://github.com/Zordon1337/BanchoSharp'>BanchoSharp</a>, an open source bancho server mainly for osu versions from 2014-2016</pre>");
                    sw.Flush();
                }
            if(context.Request.Url.ToString().Contains("bancho_connect.php"))
            {
                response.ContentType = "text/plain";
                StreamWriter sw = new StreamWriter(ns);
                sw.Write("us");
                sw.Flush();
                
            }
            if(context.Request.Url.ToString().Contains("/web/osu-osz2-getscores.php"))
            {
                string beatmapHash = context.Request.QueryString.Get("c");
                if(beatmapHash != null)
                {
                    response.ContentType = "text/html";
                    StreamWriter sw = new StreamWriter(ns);
                    sw.Write("2|False\r\n");
                    sw.Flush();
                }
            }
            if(context.Request.Url.ToString().Contains("/web/osu-submit-modular.php"))
            {
                Console.WriteLine(context.Request.Url.ToString());
                StreamReader reader = new StreamReader(iss);
                NameValueCollection formData = ReadMultipartFormData(context.Request);

                // Extract values from formData
                string score = formData["score"];
                string iv = formData["iv"];
                if(score != null && iv != null)
                {
                    string decryptedscore = CryptoHelper.DecryptString(score,Encoding.UTF8.GetBytes(CryptoHelper.key),iv);
                    Console.WriteLine($"Decrypted score: {decryptedscore}");
                } else {
                    Console.WriteLine("score is null?");
                }

            }
            context.Response.OutputStream.Close();
        }
    }
    static string ExtractValueFromPostData(string postData, string key)
    {
        // Extract the value of the specified key from the POST data
        string searchKey = $"{key}=";
        int startIndex = postData.IndexOf(searchKey);

        if (startIndex != -1)
        {
            int endIndex = postData.IndexOf("&", startIndex);
            if (endIndex == -1)
            {
                endIndex = postData.Length;
            }

            return Uri.UnescapeDataString(postData.Substring(startIndex + searchKey.Length, endIndex - startIndex - searchKey.Length));
        }

        return null;
    }
    public static void SendPacket(int packet, bool compression, MemoryStream ms, Stream ns)
    {
        Console.WriteLine($"[X] Writing Packet {packet}");
        BinaryWriter bw = new BinaryWriter(ns);
        bw.Write((UInt16)packet);
        bw.Write((bool)compression);
        bw.Write((UInt32)ms.Length);
        bw.Write(ms.ToArray());
        bw.Flush();
        ms.Position = 0;
    }
    static NameValueCollection ReadMultipartFormData(HttpListenerRequest request)
    {
        string boundary = GetBoundary(request.ContentType);

        using (Stream body = request.InputStream)
        {
            using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
            {
                string formDataString = reader.ReadToEnd();

                // Split the formDataString using the boundary
                string[] parts = formDataString.Split(new[] { boundary }, StringSplitOptions.RemoveEmptyEntries);

                NameValueCollection formData = new NameValueCollection();

                foreach (string part in parts)
                {
                    // Extract name and value from each part
                    Match nameMatch = Regex.Match(part, @"name=""(?<name>.*?)""");
                    Match valueMatch = Regex.Match(part, @"\r\n\r\n(?<value>.*?)\r\n");

                    if (nameMatch.Success && valueMatch.Success)
                    {
                        string name = nameMatch.Groups["name"].Value;
                        string value = valueMatch.Groups["value"].Value;

                        formData[name] = value;
                    }
                }

                return formData;
            }
        }
    }

    static string GetBoundary(string contentType)
    {
        string[] elements = contentType.Split(';');
        string boundaryElement = Array.Find(elements, e => e.Trim().StartsWith("boundary="));

        if (boundaryElement != null)
        {
            return boundaryElement.Trim().Substring("boundary=".Length);
        }

        throw new ArgumentException("Boundary not found in Content-Type header");
    }
}