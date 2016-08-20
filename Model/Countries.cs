using ServiceStack;
using ServiceStack.DataAnnotations;
using SMSBackend.ViewModels;
using System;

namespace SMSBackend.Model
{
    [Route("/Countries", "GET")]
    public class Countries : IReturn<CountriesDTO>
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Mcc { get; set; }  
        public int Cc { get; set; }
        [DecimalLength(4, 3)]
        public decimal PricePerSms { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}