using System;
using System.Configuration;

namespace E_Policy.Service
{
    public class ApplicationConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                string Value = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                return Value;
            }
        }

        public static string BccEmailAdditional
        {
            get
            {
                string Value = ConfigurationManager.AppSettings["BccEmailAdditional"];
                return Value;
            }
        }

        public static double IntervalTime
        {
            get
            {
                double Value = Convert.ToDouble(ConfigurationManager.AppSettings["IntervalTime"]);
                return Value * 60 * 1000; //Convert To Milisecond
            }
        }
    }
}