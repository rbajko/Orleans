using GrainInterfaces;
using Orleans;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class SmartCacheController : ApiController
    {
        // POST
        [Route("{email}")]
        public HttpResponseMessage Post(string email)
        {
            var grain = GrainClient.GrainFactory.GetGrain<ICacheGrain>("nomnio.com");

            HttpResponseMessage response;

            switch (grain.AddEmail(email).Result)
            {
                case "Created": response = Request.CreateResponse(HttpStatusCode.Created); break;

                case "Conflict": response = Request.CreateResponse(HttpStatusCode.Conflict); break;

                default: response = Request.CreateResponse(HttpStatusCode.InternalServerError); break;
            }

            return response;
        }

        // GET
        [Route("{email}")]
        public HttpResponseMessage Get(string email)
        {
            var grain = GrainClient.GrainFactory.GetGrain<ICacheGrain>("nomnio.com");

            HttpResponseMessage response;

            switch (grain.GetEmail(email).Result)
            {
                case "OK": response = Request.CreateResponse(HttpStatusCode.OK); break;

                case "NotFound": response = Request.CreateResponse(HttpStatusCode.NotFound); break;

                default: response = Request.CreateResponse(HttpStatusCode.InternalServerError); break;
            }

            return response;
        }
    }
}
