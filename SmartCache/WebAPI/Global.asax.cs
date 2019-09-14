using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);

            var appConfig = ConfigurationManager.GetSection("appSettings") as NameValueCollection;

            int port = int.Parse(appConfig["SiloPort"]);

            var config = ClientConfiguration.LocalhostSilo(port);

            // --- simulates possible behaviour / scenario

            const int initializeAttemptsBeforeFailing = 5;

            int attempt = 0;

            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    break;
                }
                catch (SiloUnavailableException e)
                {
                    attempt++;

                    if (attempt >= initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
            }

            // ---
        }
    }
}
