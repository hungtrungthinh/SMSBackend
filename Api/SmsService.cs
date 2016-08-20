using ServiceStack;
using SMSBackend.Models;
using SMSBackend.Repositories;
using System.Net;
using System;
using System.IO;
using SMSBackend.Model;
using SMSBackend.ViewModels;
using System.Threading.Tasks;

namespace SMSBackend.Api
{
    public class SmsService : Service
    {    
        public IDataRepository Repository { get; set; }

        /// <summary>
        /// This Method inserts the SMS data and log for dummy SMS implementations
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        public async Task<bool> Get(Sms sms)
        {
            var state = await  WriteSMSToLogFileAsync(sms);           
            var responseData = await Repository.SendSMSAsync(sms, state);
            if (!responseData)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return responseData;
        }

        /// <summary>
        /// Get the SMS Details sent 
        /// </summary>
        /// <param name="sentSMS"></param>
        /// <returns></returns>
        public async Task<SentSMSDTO> Get(SentSMS sentSMS)
        {
            var responseData = await Repository.GetSentSMSAsync(sentSMS);


            if (responseData == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return responseData;
        }
        #region Helper Method
       /// <summary>
       /// Helper method write SMS log to file.
       /// </summary>
       /// <param name="sms"></param>
       /// <returns></returns>
        private async Task<string> WriteSMSToLogFileAsync(Sms sms)
        {
            try
            {
                StreamWriter log;
                if (!File.Exists(@"logfile.txt"))
                {
                    log = new StreamWriter(@"logfile.txt");
                }
                else
                {
                    log = File.AppendText(@"logfile.txt");
                }
                log.WriteLine(sms.From + " : " + sms.To + ":" + sms.Text + " : " + System.DateTime.UtcNow);
                log.Close();
                return await Task.FromResult("Success"); 
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult("Failed");
        }
        #endregion
    }
}