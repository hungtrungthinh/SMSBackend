using ServiceStack;
using SMSBackend.Model;
using SMSBackend.Repositories;
using SMSBackend.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SMSBackend.Api
{
   
    public class CountryService : Service
    {
        public IDataRepository Repository { get; set; }

        /// <summary>
        /// This Method gets the countries details with COuntry code , price
        /// </summary>
        /// <param name="sms"></param>
        /// <returns>CustomerDTO</returns>    
        public async Task<List<CountriesDTO>> Get(Countries countries)
        {
            var responseData = await Repository.GetCountriesAsync();
            if(responseData == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;               
            }
            return responseData;
        }
    }
}