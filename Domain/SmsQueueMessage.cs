// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

namespace Tri.SmsEventGridFunc.Domain
{
    public class SmsQueueMessage 
    {
        public string To { get; set; }
        public string Message { get; set; }
    }
}
