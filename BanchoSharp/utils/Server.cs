using System.Collections.Specialized;
using System.Net;
using System.Text;
using BanchoSharp;
using Helpers;

namespace utils {
    public class Server {
        public delegate void RequestHandler(HttpListenerContext context);
        public void StartServer(string url, RequestHandler handler, string Servicename)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            while(true)
            {
                var httpContext = listener.GetContext();
                handler(httpContext);
            }
        }
        public void StartMultiServer(string[] urls, RequestHandler handler, string Servicename)
        {
            var listener = new HttpListener();
            foreach(var url in urls)
            {
                listener.Prefixes.Add(url);
            }
            listener.Start();
            while(true)
            {
                var httpContext = listener.GetContext();
                handler(httpContext);
            }
        }
        public void StartRedirectEndpoint(string url, string newurl)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            while(true)
            {
                var httpContext = listener.GetContext();
                RedirectRequest(httpContext,newurl);
            }
        
        }

    static void RedirectRequest(HttpListenerContext context, string destinationUrl)
    {
        try
        {
            context.Response.ContentType = "text/html";

            string url = destinationUrl + context.Request.RawUrl.Replace("/web/", "web/");

            if (context.Request.HttpMethod == "POST")
            {
                if(url.Contains("submit-modular"))
                {
                    
                    var boundary = Multipart.GetBoundary(context.Request.ContentType);
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        var formData = Multipart.ParseMultipartFormData(reader, boundary);
                        Console.WriteLine($"Name from Content-Disposition header: {context.Request.ContentType}");
                        if(formData.ContainsKey("score"))
                        {
                            string score = formData["score"];
                            string iv = formData["iv"];
                            string reply = new WebClient().DownloadString(Config.BACKEND_URL+$"web/osu-submit-modular.php?iv={iv}&score={score}");

                            StreamWriter sw = new StreamWriter(context.Response.OutputStream);
                            sw.Write(reply);
                            sw.Flush();
                        } else {
                            Console.WriteLine("Blyaaat");
                        }
                        

                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.Close();
                    }
                }
            }
            else // Handle GET requests
            {
                string reply = new WebClient().DownloadString(url);
                StreamWriter sw = new StreamWriter(context.Response.OutputStream);
                sw.Write(reply);
                sw.Flush();
            }

            context.Response.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }



    }
}