using SurveyAPI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SurveyAPI.DataLayer
{
    public class DBOperations
    {
        string con = ConfigurationManager.AppSettings["DBConnection"];
        SqlConnection sqlConnection;
        public DBOperations()
        {
            sqlConnection = new SqlConnection(con);
        }
        public Survey GetSurvey(SurveyRequestModel surveyRequestModel)
        {
            Survey survey = new Survey();
            Question ques = new Question();
            List<Question> questionsList = new List<Question>();
            SqlDataReader dataReader = null;
            var user = new User();
            using (var cmd = new SqlCommand("UPS_GetSurveyDetails", sqlConnection))
            {
                cmd.Parameters.AddWithValue("@MarketName", surveyRequestModel.MarketName);
                cmd.Parameters.AddWithValue("@UserId", surveyRequestModel.UserId.ToString());
                sqlConnection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    survey.SurveyId = Convert.ToInt32(dataReader.GetValue(0));
                    survey.CreatedBy = Convert.ToInt32(dataReader.GetValue(1));
                    survey.SurveyName = Convert.ToString(dataReader.GetValue(2));
                    survey.NumQuestions = Convert.ToInt32(dataReader.GetValue(3));
                    survey.SurveyType = Convert.ToString(dataReader.GetValue(4));
                    survey.SurveyActiveFrom = Convert.ToDateTime(dataReader.GetValue(5));
                    survey.SurveyActiveTill = Convert.ToDateTime(dataReader.GetValue(6));
                    survey.AdditionalInformation = Convert.ToString(dataReader.GetValue(7));
                    ques.Id = Convert.ToInt32(dataReader.GetValue(8));
                    ques.Description = Convert.ToString(dataReader.GetValue(9));
                    questionsList.Add(ques);
                }
            }
            return survey;
        }
        public bool ValidateCredentials(string UserName, string UserPassword)
        {
            var user = new User();
            SqlCommand cmd = new SqlCommand("UPS_GET_AkshiLogin", sqlConnection);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@UserPassword", UserPassword);
            sqlConnection.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader DataReader = cmd.ExecuteReader();
            int IsExist = 0;
            while (DataReader.Read())
            {
                IsExist = Convert.ToInt32(DataReader.GetValue(0));
            }
            sqlConnection.Close();
            return (IsExist == 0) ? false : true;
        }
        public bool InsertOrUpdateQuestion(Question question, int UserId)
        {
            int Id = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("select MAX(Id) from Akshi_Questions;", sqlConnection);
                SqlDataReader rdr = cmd.ExecuteReader();
                int MaxQuestionID = 0;
                while (rdr.Read())
                {
                    MaxQuestionID = Convert.ToInt32(rdr.GetValue(0));
                }
                using (var InsertData = new SqlCommand("UPS_InsertOrUpdateQuestions", sqlConnection))
                {

                    InsertData.CommandType = CommandType.StoredProcedure;
                    InsertData.Parameters.AddWithValue("@Id", MaxQuestionID);
                    InsertData.Parameters.AddWithValue("@Description", question.Description);
                    InsertData.Parameters.AddWithValue("@UserId", UserId);
                    InsertData.Parameters.AddWithValue("@DateTime", DateTime.Now);

                    var outparameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    InsertData.Parameters.Add(outparameter);
                    InsertData.ExecuteNonQuery();
                    Id = Convert.ToInt32(outparameter.Value);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            { sqlConnection.Close(); }
            return true;
        }
        public bool InsertOrUpdateSurvey(Survey survey)
        {
            int Id = 0;
            bool result = false;
            List<Question> ques = new List<Question>();
            try
            {
                sqlConnection.Open();
                using (var InsertData = new SqlCommand("UPS_InsertOrUpdateSurvey", sqlConnection))
                {
                    InsertData.CommandType = CommandType.StoredProcedure;

                    InsertData.Parameters.AddWithValue("@UserId", survey.UserId);
                    InsertData.Parameters.AddWithValue("@SurveyId", survey.SurveyId);
                    InsertData.Parameters.AddWithValue("@SurveyName", survey.SurveyName);
                    InsertData.Parameters.AddWithValue("@NumQuestions", survey.NumQuestions);
                    InsertData.Parameters.AddWithValue("@SurveyType", survey.SurveyType);
                    InsertData.Parameters.AddWithValue("@SurveyActiveFrom", survey.SurveyActiveFrom);
                    InsertData.Parameters.AddWithValue("@SurveyActiveTill", survey.SurveyActiveTill);
                    InsertData.Parameters.AddWithValue("@AdditionalInformation", survey.AdditionalInformation);
                    InsertData.Parameters.AddWithValue("@AdditionalInformation", survey.AdditionalInformation);
                    foreach (Question q in survey.Questions)
                    {
                        result = InsertOrUpdateQuestion(q, survey.UserId);
                    }
                    var outparameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    InsertData.Parameters.Add(outparameter);
                    InsertData.ExecuteNonQuery();
                    Id = Convert.ToInt32(outparameter.Value);
                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            { sqlConnection.Close(); }
            return result;
        }
        public int InsertDetials(User user)
        {
            int Id = 0;
            try
            {
                sqlConnection.Open();
                SqlCommand InsertData = new SqlCommand("USP_SaveOrUpdate_AkshiLogin", sqlConnection);
                InsertData.CommandType = CommandType.StoredProcedure;

                InsertData.Parameters.AddWithValue("@UserName", user.UserName);
                InsertData.Parameters.AddWithValue("@UserPassword", user.UserPassword);

                var outparameter = new SqlParameter
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                InsertData.Parameters.Add(outparameter);
                InsertData.ExecuteNonQuery();
                Id = Convert.ToInt32(outparameter.Value);

            }
            catch (Exception ex)
            { }
            finally
            { sqlConnection.Close(); }
            return Id;
        }
    }
}