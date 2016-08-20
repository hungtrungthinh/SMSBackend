using System;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SMSBackend.Models;
using SMSBackend.ViewModels;
using System.Collections.Generic;
using SMSBackend.Model;
using System.Linq;
using System.Threading.Tasks;

namespace SMSBackend.Repositories
{
    public interface IDataRepository
    {
        Task<List<CountriesDTO>> GetCountriesAsync();
        Task<List<StatisticsDTO>> GetStatisticsAsync(Statistics statistics);
        Task<SentSMSDTO> GetSentSMSAsync(SentSMS sentSMS);
        Task<bool> SendSMSAsync(Sms sms, string state);
    }
    /// <summary>
    /// Data Layer Repository
    /// </summary>
    public class DataRepository : IDataRepository
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public DataRepository(OrmLiteConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
        }
        
        /// <summary>
        /// Get Countries Details
        /// </summary>
        /// <returns></returns>
        public async  Task<List<CountriesDTO>> GetCountriesAsync()
        {
            List<CountriesDTO> countryDTOList = new List<CountriesDTO>();
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                var countries =  db.SelectAsync<Countries>();
                if (countries.Result.Count > 0)
                {

                    foreach (var country in countries.Result)
                    {
                        countryDTOList.Add(new CountriesDTO() { Name = country.Name, Cc = country.Cc, Mcc = country.Mcc, PricePerSms = country.PricePerSms });
                    }

                }
                return await Task.FromResult(countryDTOList);
            }
        }
        /// <summary>
        /// Get SMS Statistics
        /// </summary>
        /// <param name="statistics"></param>
        /// <returns></returns>
        public async Task<List<StatisticsDTO>> GetStatisticsAsync(Statistics statistics)
        {
            List<StatisticsDTO> statisticsDTOList = new List<StatisticsDTO>();
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                var smsResults =  db.SelectAsync<Sms>();
                if (smsResults.Result.Count > 0)
                {
                    var filteredSMSResults = smsResults.Result.GroupBy(g => new { g.Mcc }).Select(g => g.First()).ToList();
                    statisticsDTOList.AddRange(filteredSMSResults.Select(n => new StatisticsDTO() { Day = DateTime.Now, Mcc = n.Mcc, Price = n.Price, TotalPrice = (n.Price * smsResults.Result.Where(p => p.Mcc == n.Mcc).Count()), Count = smsResults.Result.Where(p => p.Mcc == n.Mcc).Count() }));
                    return await Task.FromResult(statisticsDTOList);
                }
                return null;
            }
        }
        /// <summary>
        /// Get Sent SMS Data
        /// </summary>
        /// <param name="sentSMS"></param>
        /// <returns></returns>
        public async Task<SentSMSDTO> GetSentSMSAsync(SentSMS sentSMS)
        {
            SentSMSDTO sentSMSDTO = new SentSMSDTO();
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                //here is room for better improvement.
                var smsResults = db.SelectAsync<Sms>();
                var dataresult = smsResults.Result.Where(t => t.CreatedAt >= sentSMS.DateTimeFrom && t.CreatedAt <= sentSMS.DateTimeTo);


                var smsData = dataresult.Skip((int)sentSMS.Skip).Take((int)sentSMS.take).ToList();
                if (smsData.Count > 0)
                {
                    sentSMSDTO.TotalCount = smsData.Count();
                    sentSMSDTO.SmsDetail = new List<SmsDTO>();
                    foreach (var data in smsData)
                    {
                        sentSMSDTO.SmsDetail.Add(new SmsDTO() { To = data.To, Text = data.Text, From = data.From, State = data.State, CreatedAt = data.CreatedAt, Mcc = data.Mcc, Price = data.Price });
                    }
                }
                return await Task.FromResult(sentSMSDTO); 
            }
        }
        /// <summary>
        /// Save SMS to Table
        /// </summary>
        /// <param name="sms"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<bool> SendSMSAsync(Sms sms, string state)
        {
            bool isSent = false;
            //Grab 3 digits from phone number & grab country code
            var mobileCC = Convert.ToInt32(string.IsNullOrEmpty(sms.To) ? 0 : int.Parse(sms.To.Substring(0, 3)));
            if (mobileCC > 0)
            {
                //Check with Countries data & fetch associated country for SMS and commercial detail
                var appliedCountryDetail =  GetCountriesAsync().Result.Where(c => c.Mcc == mobileCC).FirstOrDefault();
                if (appliedCountryDetail != null)
                {
                    Sms smsObj = new Sms();
                    smsObj.From = sms.From;
                    smsObj.To = sms.To;
                    smsObj.Text = sms.To;
                    smsObj.Mcc = appliedCountryDetail.Mcc;
                    smsObj.Price = appliedCountryDetail.PricePerSms;
                    smsObj.State = state;
                    smsObj.CreatedAt = System.DateTime.UtcNow;
                    using (var db = DbConnectionFactory.OpenDbConnection())
                    {                        
                        var lastInsertedId =  await db.InsertAsync(smsObj);
                        if (lastInsertedId > 0)
                        {
                            isSent = true;
                        }
                    }
                }               
            }
            return await Task.FromResult(isSent);
        }
    }
}