using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace OrderMaking.ConsoleApp
{
    public class Program
    {
        //    HttpSelfHostConfiguration config = new HttpSelfHostConfiguration("http://localhost:8080");


        //    config.Routes.MapHttpRoute(
        //"API Default", "api/{controller}/{id}", 
        //new { id = RouteParameter.Optional
        //});

        //    using (HttpSelfHostServer server = new HttpSelfHostServer(config))
        //    {
        //        server.OpenAsync().Wait();
        //        Console.WriteLine("Press Enter to quit.");
        //        Console.ReadLine();
        //    }

        static void Main(string[] args)
        {
            string domainAddress = "Http://192.168.0.28:8081/";

            using (WebApp.Start(url: domainAddress))
            {
                Console.WriteLine("Service Hosted " + domainAddress);
                System.Threading.Thread.Sleep(-1);
            }
        }
    }
}
