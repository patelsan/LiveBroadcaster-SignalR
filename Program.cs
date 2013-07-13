using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Owin.Hosting;
using Microsoft.AspNet.SignalR;
using Owin;

namespace LiveBroadcaster
{
    class Program
    {
        static void Main(string[] args)
        {
			string url = "http://localhost:8080";

			using(WebApp.Start<Startup>(url))
			{
				Console.WriteLine("Server is ready to take connections on " + url);
				Broadcaster.Start();
				Console.ReadLine();
			}
        }
    }

    public static class Broadcaster
    {
        private static int counter = 1;
        private static Random cpuRandom;
        private static Random memoryRandom;
		private static IHubContext hubContext;

        public static void Start()
        {
            cpuRandom = new Random(30);
            memoryRandom = new Random(6);

			//You need to create instance using the GlobalHost, creating a direct instance of your hub would result in to runtime exception
			hubContext = GlobalHost.ConnectionManager.GetHubContext<StatsHub>();
			new Timer(BroadcastStats, null, 1500, 1500);
        }

        private static void BroadcastStats(object state)
        {
			hubContext.Clients.All.updateStats(new { time = counter, cpu = cpuRandom.Next(10, 70), memory = memoryRandom .Next(1,32)});
            
            counter++;
            Console.WriteLine(String.Format("Broadcasted..."));
        }
    }

	public class StatsHub:Hub
	{
		//In out case, this method is not being used as we are directly calling the hub
		public void Send(int time, int cpu, int memory)
		{
			Clients.All.updateStats(new { time = time, cpu = cpu, memory = memory});
		}
	}

	class Startup
    {
        public void Configuration(IAppBuilder app)
        {     
            // Turn cross domain on 
            var config = new HubConfiguration { EnableCrossDomain = true };
            app.MapHubs(config);
        }
    }
}
