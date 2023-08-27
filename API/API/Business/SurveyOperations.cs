using SurveyAPI.Model;
using SurveyAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAPI.Business
{
    public class SurveyOperations
    {
        public Survey GetSurveyDetails(SurveyRequestModel surveyRequestModel)
        {
            Survey survey = new DBOperatons().GetSurvey(surveyRequestModel);
            return survey;
        }

        public bool InsertOrUpdateSurvey(Survey survey)
        {
            bool IsTransactionSuccessful = new DBOperatons().InsertOrUpdateSurvey(survey);
            return IsTransactionSuccessful;
        }
    }
}
