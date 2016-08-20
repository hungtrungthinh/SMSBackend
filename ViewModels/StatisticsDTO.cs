using ServiceStack;
using ServiceStack.DataAnnotations;
using System;

namespace SMSBackend.ViewModels
{
    public class StatisticsDTO : SmsDTO, IReturn<SmsDTO>
    {
        public  DateTime Day { get; set; }
        public int Count { get; set; }

        [DecimalLength(4, 2)]
        public decimal TotalPrice { get; set; }

    }
}