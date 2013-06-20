using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LiveBroadcaster
{
    class Program
    {
        static void Main(string[] args)
        {
            Broadcaster.Start();
            Console.WriteLine("Started broadcasting events. Press any key to exit.");
            Console.ReadLine();
        }
    }

    public static class Broadcaster
    {
		//Update with your application keys
        private readonly static string APP_ID = "46568";
        private readonly static string APP_KEY = "b0d95df00dd6be817af7";
        private readonly static string APP_SECRET = "d5fa096323689eba8026";
        private static int counter = 1;
        private static Random cpuRandom;
        private static Random memoryRandom;
        private static Pusher pusher;
        

        public static void Start()
        {
            pusher = new Pusher(APP_ID, APP_KEY, APP_SECRET);
            new Timer(BroadcastStats, null, 500, 1500);
            cpuRandom = new Random(30);
            memoryRandom = new Random(6);
        }

        private static void BroadcastStats(object state)
        {
            var result = pusher.Trigger("stats_channel", "update_event", new { time = counter, cpu = cpuRandom.Next(10, 70), memory = memoryRandom .Next(1,32)});
            counter++;
            Console.WriteLine(String.Format("Broadcasted...status: {0}", result.StatusCode));
        }
    }
}
