using SurveyAPI.Business;
using SurveyAPI.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace API.Controllers
{
    public class AuthenticateController : ApiController
    {
        // GET: api/Authentication
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Authentication/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Authentication
        public bool ValidateUser([FromBody]User user)
        {
            return new UserAuthentication().IsAuthenticatedUser(user);
        }

        // PUT: api/Authentication/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Authentication/5
        public void Delete(int id)
        {
        }
    }
}
