// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.WindowsAzure.Storage.Table;

namespace Twillio.SmsGridFunc.Domain
{
    public class SmsMessage : TableEntity
    {
        public string MessageCode { get; set; }
        public string Message { get; set; }
    }
}
