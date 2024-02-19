using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BanchoSharp;
using BanchoSharp.Structures;
using BanchoSharp.Utils;
using StreamUtils;
using utils;
public class Bancho {
    public static void Handler(HttpListenerContext context)
    {
        HttpListenerResponse response = context.Response;
        response.AddHeader("cho-token","shittytoken");
        response.AddHeader("cho-protocol", "19");
        Stream ns = response.OutputStream;
        Stream iss = context.Request.InputStream;
        if(context.Request.HttpMethod == "POST")
        {
            //response.ContentType = "application/octet-stream";
            MemoryStream ms = new MemoryStream();
            Writer bw = new Writer(ms);
            if(context.Request.Headers["osu-token"] == null)
            {
                StreamReader br = new StreamReader(context.Request.InputStream,Encoding.UTF8);
                try {
                    var test1 = br.ReadLine();
                    var test2 = br.ReadLine();
                    var test3 = br.ReadLine();
                    string[] lines2 = test3.Split('|');
                    string username = test1;
                    string password = test2;
                    string version = lines2[0];
                    Console.WriteLine($"[X] Player {username} attempted to login from {version}");
                    #region Login
                    string response2 = Auth.TryAuth(username,password);
                    if(response2 != "fail")
                    {
                        string[] parsed = response2.Split("|");
                        int userid = int.Parse(parsed[1]);
                        long totalscore = long.Parse(parsed[3]);
                        float accuracy = float.Parse(parsed[2]);
                        short PP = 0;
                        PP = (short)int.Parse(parsed[4]);
                        response.Headers.Set("cho-token",username.ToString());
                        username = username.Trim();
                        //Console.WriteLine($"{userid} {totalscore} {accuracy} {PP}");
                        bw.Write(0);
                        bw.Flush();
                        SendPacket(92,false,ms,ns); // SilenceInfo Packet
                        bw.Write(userid);
                        bw.Flush();
                        SendPacket(5,false,ms,ns); // LoginReply Packet
                        bw.Write(19);
                        bw.Flush();
                        SendPacket(75,false,ms,ns); // Protocol Version Packet
                        bw.Write(20);
                        bw.Flush();
                        SendPacket(71,false,ms,ns); // Login Permissions Packet
                        bw.Write("Welcome to BanchoSharp!");
                        bw.Flush();
                        SendPacket(24,false,ms,ns);
                        new UserPresence(userid,username,24,1,5,0f,0f,1).WriteToStream(bw);
                        SendPacket(83,false,ms,ns); // UserPresence Packet
                        new bUserStats(userid,new bUserStatus(0,"idle","md5",0,0,0),totalscore,accuracy,1,totalscore,1,PP).WriteToStream(bw);
                        SendPacket(11,false,ms,ns); // UserStats Packet
                        SendPacket(96,false,ms,ns); // UserPresence Bundle Packet
                        SendPacket(89,false,ms,ns); // Channel Info Complete Packet
                        bw.Write("#osu");
                        bw.Flush();
                        SendPacket(64,false,ms,ns);
                        bw.Write("#updates");
                        bw.Flush();
                        SendPacket(64,false,ms,ns); //Channel Join Success Packet
                    } else {
                        bw.Write(0);
                        bw.Flush();
                        SendPacket(92,false,ms,ns); // SilenceInfo Packet
                        bw.Write(-1);
                        bw.Flush();
                        SendPacket(5,false,ms,ns); // LoginReply Packet
                    }
                    #endregion
                    } catch(Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                } else {
                    /*

                    TODO: Maybe some token checking lol
                    */
                    BinaryReader br = new BinaryReader(context.Request.InputStream);
                    int pid = br.ReadInt16();
                    if(pid != 4)
                    {
                        //Console.WriteLine($"[X] Got connection with user agent: {context.Request.UserAgent}");
                        //Console.WriteLine("[X] Path: "+context.Request.Url);
                        //Console.WriteLine($"Got packet with token: {context.Request.Headers["osu-token"]}");
                        Console.WriteLine("[X] Got packet "+pid);
                        switch(pid)
                        {
                            case 31: // MatchCreate
                            {
                                //bMatch match = new bMatch(context.Request.InputStream);
                                Writer w = new Writer(ns);
                                //MemoryStream MS = new MemoryStream();
                                //match.WriteToStream(w);
                                SendEmptyPacket(37,ns);
                                //bw.Write("#lobby");
                                //bw.Flush();
                                //SendPacket(64,false,ms,ns);
                                //bw.Write("#multi_1");
                                //bw.Flush();
                                //SendPacket(64,false,ms,ns);
                                break;
                            }
                            case 85:
                            {
                                Console.WriteLine(br.ReadInt16());
                                string username = context.Request.Headers.Get("osu-token");
                                string response2 = Auth.GetByUsername(username);
                                if(response2 != "fail")
                                {
                                    string[] parsed = response2.Split("|");
                                    int userid = int.Parse(parsed[0]);
                                    long totalscore = long.Parse(parsed[2]);
                                    float accuracy = float.Parse(parsed[1]);
                                    short PP = 0;
                                    PP = (short)int.Parse(parsed[3]);
                                    new bUserStats(userid,new bUserStatus(0,"idle","md5",0,0,0),totalscore,accuracy,1,totalscore,1,PP).WriteToStream(bw);
                                    SendPacket(11,false,ms,ns); // UserStats Packet
                                    
                                }
                                
                                break;
                            }
                        }
                    }
                }

            } else {
            }
            if(context.Request.HttpMethod == "GET")
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
            context.Response.OutputStream.Close();

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
    public static void SendEmptyPacket(int packet, Stream ns)
    {
        Console.WriteLine($"[X] Writing Packet {packet}");
        BinaryWriter bw = new BinaryWriter(ns);
        bw.Write((UInt16)packet);
        bw.Write(false);
        bw.Write(0);
        bw.Flush();
    }
}

public class AvatarServer {
    public static void Handler(HttpListenerContext context)
    {
        Console.WriteLine($"[X] Got request to avatar server {context.Request.RawUrl}");
        try {
            context.Response.ContentType = "image/png";
            if(File.Exists("C:\\lol.png"))
            {
                byte[] imageData = File.ReadAllBytes("C:\\lol.png");
                context.Response.OutputStream.Write(imageData, 0, imageData.Length);
            }
            context.Response.OutputStream.Close();
        } catch {
            
        }
    }
}
public class Program {
    static void Main(string[] args)
    {
        Console.WriteLine("[X] Server started listening at port 80");
        Server server = new Server();
        new Thread(()=>server.StartMultiServer(new string[]{"http://c.ppy.sh:7270/","http://c1.ppy.sh:7270/"},Bancho.Handler, "Bancho")).Start();
        new Thread(()=>server.StartServer("http://a.ppy.sh:6666/", AvatarServer.Handler,"Avatar server")).Start();
        
    }
}