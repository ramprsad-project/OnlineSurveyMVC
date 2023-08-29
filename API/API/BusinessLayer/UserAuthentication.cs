using SurveyAPI.DataLayer;
using SurveyAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyAPI.Business
{
    public class UserAuthentication
    {
        public bool IsAuthenticatedUser(User _user)
        {
            return new DBOperations().ValidateCredentials(_user.UserName, _user.Password);
        }
    }
}
