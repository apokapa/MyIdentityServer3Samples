using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace SecureApi.Controllers
{

    [RoutePrefix("api")]
    public class ValuesController : ApiController

    {

        // GET api/values
        [Route("values")]
        [ScopeAuthorize("read")]
        public IEnumerable<string> Get()
        {
            //var claimsPrincipal = User as ClaimsPrincipal;
            //var username = claimsPrincipal?.FindFirst("").Value;

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }



    }
}