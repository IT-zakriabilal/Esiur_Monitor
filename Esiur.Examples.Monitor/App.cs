using Esiur.Examples.Monitor.Model;
using Esiur.Net.HTTP;
using Esiur.Net.IIP;
using Esiur.Resource;
using Esiur.Stores;
using Esiur.Stores.EntityCore;
using System;
using System.Threading.Tasks;
using Esiur.Examples.Monitor.Controller;
using Esiur.Core;
using System.Diagnostics;

namespace Esiur.Examples.Monitor
{
    class App
    {
        // hardcoded constants
        public const string ConnectionString
        = "host=surus.db.elephantsql.com;" +
            "port=5432;" +
            "database=msipuibk;" +
            "password=De6hl6auBXGDgc1dhtcNnvrmYNFklMZi;" +
            "username=msipuibk;" +
            "Pooling=true;" +
            "MinPoolSize=10;" +
            "MaxPoolSize=20;" +
            "CommandTimeout=300";


        public static EntityStore EntityStore;

        static async Task Main(string[] args)
        {
            await Warehouse.Put("sys", new MemoryStore());

            EntityStore = await Warehouse.Put("ent", new EntityStore()
            {
                Getter = () => new Model.DB()
            });


            await Warehouse.Put("sys/live", new Live());
            await Warehouse.Put("sys/history", new History());
 
 
            var iipServer = await Warehouse.Put("sys/iip", new DistributedServer()
            {
                // Port = 10518, // add to your firewall
                // EntryPoint = Home,
                ExceptionLevel = ExceptionLevel.Code | ExceptionLevel.Source | ExceptionLevel.Message | ExceptionLevel.Trace,
                // Set membership which handles authentication.
                Membership = new Membership()
            });

            var http = await Warehouse.Put("sys/http", new HTTPServer()
            {
                Port = Environment.OSVersion.Platform == PlatformID.Win32NT ? (ushort)8080 : (ushort)80
            });

 
 
            // Add HTTP Files handler
            await Warehouse.Put("sys/http/web", new Web() { RootPath = "View" });

            // Create IIP over Websocket HTTP module and give it to HTTP server.
            await Warehouse.Put("sys/http/iip", new IIPoWS() { Server = iipServer });

            // create db
            var db = new DB();
            db.Database.EnsureCreated();

            await Warehouse.Open();

            // 1 - open browser and type localhost:8080
            // 2 - click on Live button to see real time cpu and memory  
            // 3 - click on Process to see all processes runnning in that machine 
            

            // * -- all the data is transfered and returned using esiur 
            // * -- view in iui framework 

            //best regards 

            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    var path = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            //    Process.Start(path, "--remote-debugging-port=9222 http://localhost:8080");
            //}

            Console.WriteLine("Delta Monitor is online.");

        }
    }
}
