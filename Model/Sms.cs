using ServiceStack;
using ServiceStack.DataAnnotations;
using SMSBackend.ViewModels;
using System;

namespace SMSBackend.Models
{
   [Route("/sms/{from}/{to}/{text}", "GET")]
    public class Sms : IReturn<SmsDTO>
    {
       

        [AutoIncrement]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string State { get; set; }
        public int Mcc { get; set; }
      
        [DecimalLength(4,3)]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        internal bool In(string mccList)
        {
            throw new NotImplementedException();
        }
    }
}