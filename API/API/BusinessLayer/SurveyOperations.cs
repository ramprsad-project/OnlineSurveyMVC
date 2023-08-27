using SurveyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SurveyAPI.DataLayer;

namespace SurveyAPI.Business
{
    public class SurveyOperations
    {
        public Survey GetSurveyDetails(SurveyRequestModel surveyRequestModel)
        {
            Survey survey = new DBOperations().GetSurvey(surveyRequestModel);
            return survey;
        }

        public bool InsertOrUpdateSurvey(Survey survey)
        {
            bool IsTransactionSuccessful = new DBOperations().InsertOrUpdateSurvey(survey);
            return IsTransactionSuccessful;
        }
    }
}
