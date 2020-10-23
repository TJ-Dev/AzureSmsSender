using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Tri.SmsEventGridFunc.Domain;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Tri.SmsQueueTriggerFunc
{
    public static class SmsQueueTriggerFunc
    {   
        [FunctionName("SmsQueueTriggerFunc")]
        [return: TwilioSms(
            AccountSidSetting = "AzureWebJobsTwilioAccountSid", 
            AuthTokenSetting = "AzureWebJobsTwilioAuthToken", 
            From = "+16506845422")]
        public static CreateMessageOptions Run(
            [QueueTrigger("smsitems", Connection = "AzureWebJobsStorage")]JObject queuedMessage, 
            ILogger log)
        {
            log.LogInformation($"From Queue!");
            log.LogInformation($"C# SMS Queue trigger function processed: {queuedMessage.ToString()}");
            var incommingMessage = queuedMessage.ToObject<SmsQueueMessage>();
            log.LogInformation($"SMS Queue Item. message: {incommingMessage.Message}, to: {incommingMessage.To}"); 
            
            var twilioMessage = new CreateMessageOptions(new PhoneNumber(incommingMessage.To))
            {
                Body = $"Hello sir, {incommingMessage.Message}"
            };
            log.LogInformation($"Send SMS!");
            return twilioMessage;
        }
    }
}
