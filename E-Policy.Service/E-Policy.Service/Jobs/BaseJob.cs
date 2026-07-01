using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using E_Policy.Service.Models.Utilities;

namespace E_Policy.Service.Jobs
{
    public class BaseJob
    {
        protected SQLDatabase _SQLDatabase;

        public BaseJob()
        {
            _SQLDatabase = new SQLDatabase(ApplicationConfiguration.ConnectionString);
        }

    }
}
