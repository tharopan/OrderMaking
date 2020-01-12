using System;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace OrderMaking.ConsoleApp
{
    public class Program
    {
        //public const string ServiceName = "OrderMakingServices";

        //public class Service : ServiceBase
        //{
        //    public Service()
        //    {
        //        ServiceName = Program.ServiceName;
        //    }

        //    protected override void OnStart(string[] args)
        //    {
        //        Program.Start(args);
        //    }

        //    protected override void OnStop()
        //    {
        //        Program.Stop();
        //    }
        //}

        static void Main(string[] args)
        {
            //if (!Environment.UserInteractive)
            //    // running as service
            //    using (var service = new Service())
            //        ServiceBase.Run(service);
            //else
            //{
            //    // running as console app
            //    Start(args);
            //    Console.WriteLine("Press any key to stop...");
            //    Console.ReadKey(true);
            //    Stop();
            //}

            //string domainAddress = "Http://192.168.0.28:8081/";

            string domainAddress = "Http://192.168.99.3:8081/";

            //using (WebApp.Start(url: domainAddress))
            //{
            //    Console.WriteLine("Service Hosted " + domainAddress);
            //    System.Threading.Thread.Sleep(-1);
            //}
            //var config = new HttpSelfHostConfiguration("Http://192.168.0.28:8081/");

            var config = new HttpSelfHostConfiguration(domainAddress);

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Service Hosted " + domainAddress);
                System.Threading.Thread.Sleep(-1); Console.ReadLine();
            }

            Console.ReadKey();
        }

        private static void Start(string[] args)
        {
            // onstart code here
        }

        private static void Stop()
        {
            // onstop code here
        }
    }
}
