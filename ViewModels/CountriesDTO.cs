using ServiceStack.DataAnnotations;

namespace SMSBackend.ViewModels
{
    public class CountriesDTO
    {
        public string Name { get; set; }
        public int Mcc { get; set; }
        public int Cc { get; set; }
       [DecimalLength(4,3)]
        public decimal PricePerSms { get; set; }
    }
}