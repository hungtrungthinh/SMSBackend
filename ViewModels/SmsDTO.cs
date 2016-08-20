using ServiceStack;
using ServiceStack.DataAnnotations;
using System;

namespace SMSBackend.ViewModels
{
    
    public class SmsDTO
    {
       
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string State { get; set; }
        public int Mcc { get; set; }

        [DecimalLength(4, 3)]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}