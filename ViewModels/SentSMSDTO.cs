using System.Collections.Generic;

namespace SMSBackend.ViewModels
{
    public class SentSMSDTO
    {
        public int TotalCount { get; set; }
        public List<SmsDTO> SmsDetail {get;set;}
    }
}