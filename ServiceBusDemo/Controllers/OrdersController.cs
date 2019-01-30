using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        const string ServiceBusConnectionString = "Endpoint=sb://adahidemo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=uQFJEg3p8UJa5DusjB9ophQyXNd9lBrTbpq6OMky8n8=";
        const string QueueName = "orders";
        static IQueueClient queueClient;

        public OrdersController()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
        }

        // POST api/order
        [HttpPost]
        public async Task<IActionResult> PostOrderAsync([FromBody] dynamic value)
        {
            try
            {
                var messageBody = Encoding.UTF8.GetBytes(Convert.ToString(value));
                var timePerParse = Stopwatch.StartNew();
                await queueClient.SendAsync(new Message(messageBody));
                timePerParse.Stop();
                var details = new TimeDetails()
                {
                    MessagePushTime = timePerParse.ElapsedMilliseconds
                };
                return Ok(details);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    public class TimeDetails
    {
        public long MessagePushTime { get; set; }
    }
}
