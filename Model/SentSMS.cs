using ServiceStack;
using SMSBackend.ViewModels;
using System;

namespace SMSBackend.Model
{
    [Route("/sms/{DateTimeFrom}/{DateTimeTo}/{Skip}/{Take}", "GET")]
    public class SentSMS : IReturn<SentSMSDTO>
    {
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public int Skip { get; set; }
        public int take { get; set; }

    }
}