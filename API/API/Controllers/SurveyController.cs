using SurveyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class SurveyController : ApiController
    {
        // GET: api/Survey
        public Survey Get()
        {
            return new Survey();
        }

        // GET: Search/Details/5
        public Survey GetSurveyByMarket([FromBody]SurveyRequestModel surveyRequestModel)
        {
            Survey surveyOperations = new Survey();
            return surveyOperations;
        }

        // GET: api/Survey/5
        public string GetById(int id)
        {
            return "value";
        }

        // POST: api/Survey
        public void Post([FromBody]string value)
        {
        }

        // GET: Search/Create
        public bool InserOrUpdateSurvey([FromBody]Survey survey)
        {
            // bool IsTrasactionSuccesful = new SurveyOperations().InsertOrUpdateSurvey(survey);
            return true; 
        }

        // PUT: api/Survey/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Survey/5
        public void Delete(int id)
        {
        }
    }
}
