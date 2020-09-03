using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ReceiveData
{
    public static class ReceivePayloadFromGithub
    {
        [FunctionName("ReceiveIssue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Github",
                collectionName: "Issues",
                ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<Event> eventsOut,
            ILogger log)
        {
            log.LogInformation("Received event");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                string location = data.location.user.location;
                string repos = data.repository;
                string username = data.username;

                var eventObj = new Event()
                {
                    Location = location,
                    Username = username,
                    EventDate = DateTime.Now,
                    Repository = repos
                };

                await eventsOut.AddAsync(eventObj);

                return new OkObjectResult("Event inserted");
            }
            catch(Exception ex)
            {
                log.LogError(ex, "Error insert event");
                return new BadRequestObjectResult("Event not valid");
            }
        }
    }

    public class Event
    {
        public string Location { get; set; }
        public string Username { get; set; }
        public DateTime EventDate { get; set; }
        public string Repository { get; set; }
    }
}
