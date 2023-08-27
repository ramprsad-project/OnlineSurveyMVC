using SurveyAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SurveyAPI.DataLayer
{
    public class DBOperations
    {
        string con = "Data Source=100.21.241.183;Initial Catalog=DevelopersPOC;User ID=DevelopersPOC;Password=QF2p9MEGHtjMUr6GQw";

        public SqlDataReader ReadOperaton(Dictionary<string, string> args, string spName)
        {
            SqlDataReader dataReader = null;
            using (var connection2 = new SqlConnection(con))
            {
                var user = new User();
                using (var cmd = new SqlCommand(spName, connection2))
                {
                    foreach (var arg in args)
                    {
                        cmd.Parameters.AddWithValue(arg.Key, arg.Value);
                    }
                    connection2.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    dataReader = cmd.ExecuteReader();
                }
            }
            return dataReader;
        }
        public int InsertDetials(User user)
        {
            int Id = 0;
            try
            {
                using (SqlConnection connection1 = new SqlConnection(con))
                {
                    connection1.Open();
                    SqlCommand InsertData = new SqlCommand("USP_SaveOrUpdate_AkshiLogin", connection1);
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
                    connection1.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return Id;

        }

        public bool ValidateCredentials(string UserName, string UserPassword)
        {
            SqlConnection connection2 = new SqlConnection(con);
            var user = new User();
            SqlCommand cmd = new SqlCommand("UPS_GET_AkshiLogin", connection2);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@UserPassword", UserPassword);
            connection2.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader DataReader = cmd.ExecuteReader();
            int IsExist = 0;
            while (DataReader.Read())
            {
                IsExist = Convert.ToInt32(DataReader.GetValue(0));
            }
            connection2.Close();
            return (IsExist == 0) ? false : true;
        }

        public bool InsertOrUpdateQuestion(Question question, int UserId)
        {
            int Id = 0;
            try
            {
                using (SqlConnection connection1 = new SqlConnection(con))
                {
                    connection1.Open();
                    SqlCommand cmd = new SqlCommand("select MAX(Id) from Akshi_Questions;", connection1);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    int MaxQuestionID = 0;
                    while (rdr.Read())
                    {
                        MaxQuestionID = Convert.ToInt32(rdr.GetValue(0));
                    }
                    using (var InsertData = new SqlCommand("UPS_InsertOrUpdateQuestions", connection1))
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
                        connection1.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
            return true;
        }

        public bool InsertOrUpdateSurvey(Survey survey)
        {
            int Id = 0;
            bool result = false;
            List<Question> ques = new List<Question>();
            try
            {
                using (SqlConnection connection1 = new SqlConnection(con))
                {
                    connection1.Open();

                    using (var InsertData = new SqlCommand("UPS_InsertOrUpdateSurvey", connection1))
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
                        connection1.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
            return result;
        }

        public Survey GetSurvey(SurveyRequestModel surveyRequestModel)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("@MarketName", surveyRequestModel.MarketName);
            dic.Add("@UserId", surveyRequestModel.UserId.ToString());

            Survey survey = new Survey();
            Question ques = new Question();
            List<Question> questionsList = new List<Question>();

            SqlDataReader rdr = ReadOperaton(dic, "UPS_GetSurveyDetails");

            while (rdr.Read())
            {
                survey.SurveyId = Convert.ToInt32(rdr.GetValue(0));
                survey.CreatedBy = Convert.ToInt32(rdr.GetValue(1));
                survey.SurveyName = Convert.ToString(rdr.GetValue(2));
                survey.NumQuestions = Convert.ToInt32(rdr.GetValue(3));
                survey.SurveyType = Convert.ToString(rdr.GetValue(4));
                survey.SurveyActiveFrom = Convert.ToDateTime(rdr.GetValue(5));
                survey.SurveyActiveTill = Convert.ToDateTime(rdr.GetValue(6));
                survey.AdditionalInformation = Convert.ToString(rdr.GetValue(7));
                ques.Id = Convert.ToInt32(rdr.GetValue(8));
                ques.Description = Convert.ToString(rdr.GetValue(9));
                questionsList.Add(ques);
            }
            return survey;
        }
    }
}
