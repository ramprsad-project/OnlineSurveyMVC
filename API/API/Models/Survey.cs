using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAPI.Model
{
    public class Survey
    {
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public int NumQuestions { get; set; }
        public string SurveyType { get; set; }
        public DateTime SurveyActiveFrom { get; set; }
        public DateTime SurveyActiveTill { get; set; }
        public string AdditionalInformation { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
