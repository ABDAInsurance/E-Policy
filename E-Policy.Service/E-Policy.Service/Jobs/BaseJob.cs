using System;

using E_Policy.Service.Models;
using E_Policy.Service.Models.Utilities;

namespace E_Policy.Service.Jobs
{
    public class BaseJob
    {
        protected SQLDatabase _SQLDatabase;
        protected CareService _CareService;

        public BaseJob()
        {
            _SQLDatabase = new SQLDatabase(ApplicationConfiguration.ConnectionString);
            _CareService = new CareService();
        }
    }
}
