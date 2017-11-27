using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace Recipes
{
    public class Program
    {
        private const string EndpointUri = "https://rcpt-cosmos-db.documents.azure.com:443/";
        private const string PrimaryKey = "b1KD3ujrFRXJpyFxRJDnyo4mdNp3jzV8e8QTe0msge3uAw36We9WbazevGTUc9pwjR8HmvQ2DKBAhlN12refXA==";
        private DocumentClient client;

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
