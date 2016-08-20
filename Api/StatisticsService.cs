using ServiceStack;
using SMSBackend.Model;
using SMSBackend.Repositories;
using SMSBackend.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SMSBackend.Api
{
    public class StatisticsService : Service
    {
        public IDataRepository Repository { get; set; }        

        public async Task<List<StatisticsDTO>> Get(Statistics statistics) 
        {          
            var responseData = await Repository.GetStatisticsAsync(statistics);           
            if (responseData == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return responseData;
           
        }

    }
}