// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tri.SmsEventGridFunc.Domain;
using Newtonsoft.Json.Linq;
using Twillio.SmsGridFunc.Domain;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;

namespace Tri.SmsEventGridFunc
{
    public static class SmsEventGridFunc
    {
          private const string SmsPartitionKey = "SMS";

        [FunctionName("SmsEventGridFunc")]
        [return: Queue("smsitems")]
        public static async Task<SmsQueueMessage> Run([EventGridTrigger]EventGridEvent eventGridEvent,
        [Table("smsmessage")] CloudTable testTable, 
        ILogger log)
        {
            log.LogInformation("Yay!");
            log.LogInformation(eventGridEvent.Data.ToString());

            var data = eventGridEvent.Data as JObject; 
            var dObject = data.ToObject<SmsQueueMessage>();
            var messagePartitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                                                                             SmsPartitionKey);
            var messageCodeFilter = TableQuery.GenerateFilterCondition("MessageCode", QueryComparisons.Equal,
                                                                             "SmsMessage");
            var messageQuery = new TableQuery<SmsMessage>().Where(TableQuery.CombineFilters(messagePartitionFilter, TableOperators.And, messageCodeFilter));
            var result = await testTable.ExecuteQuerySegmentedAsync(messageQuery, null);
            var message =  result.Select(s => s.Message).FirstOrDefault();
            if (message == null) {
                log.LogWarning("Could not find message with code", "SmsMessage");
                return null;
            } 
            else {
                log.LogInformation("Success Message: {message}", message);
                return new SmsQueueMessage {
                    To = dObject.To,
                    Message = message
                };
            }
        }
    }
}
