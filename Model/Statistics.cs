using ServiceStack;
using System;

namespace SMSBackend.Model
{
    [Route("/Statistics", "GET")]
    public class Statistics
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public string mccList { get; set; }

    }
}