using ServiceStack;
using System;
using Funq;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using SMSBackend.Models;
using SMSBackend.Model;
using SMSBackend.Repositories;

namespace SMSBackend
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            new SMSAppHost().Init();
        }

      
    }
    public class SMSAppHost : AppHostBase
    {
        public SMSAppHost() : base("SMSService Stack",  typeof(SMSAppHost).Assembly)
        {
        }


        public override void Configure(Container container)
        {
            var stringConn = "MySQL ConnectionString Here";
            SetConfig(new HostConfig() { HandlerFactoryPath = "api" });
            var dbConnectionFactory =
                 new OrmLiteConnectionFactory(stringConn, MySqlDialect.Provider);                
            container.Register<IDbConnectionFactory>(dbConnectionFactory);
            container.Register<IDataRepository>(c=> new DataRepository(dbConnectionFactory));

            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                //Dropping existing schema
                db.DropTable<Sms>();
                db.DropTable<Countries>();

                //Creating Schema
                db.CreateTableIfNotExists<Sms>();
                db.CreateTableIfNotExists<Countries>();

                //Insertion with provided data
                db.Insert(new Countries() { Name = "Germany", Mcc = 262, Cc = 49, PricePerSms = 0.055M, CreatedAt=DateTime.UtcNow });
                db.Insert(new Countries() { Name = "Austria", Mcc = 232, Cc = 43, PricePerSms = 0.053M, CreatedAt = DateTime.UtcNow });
                db.Insert(new Countries() { Name = "Poland", Mcc = 260, Cc = 48, PricePerSms = 0.032M , CreatedAt = DateTime.UtcNow });

            }
        }
    }
}