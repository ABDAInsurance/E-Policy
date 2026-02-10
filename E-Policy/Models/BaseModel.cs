using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;

using Newtonsoft.Json;
using E_Policy.Models.Dao;
using E_Policy.Models.Utilities;

namespace E_Policy.Models
{
    public class BaseModel
    {
        protected SQLDatabase _SQLDatabase;
        protected PSReportService.BuildClient _PSReportService;

        protected string _ApplicationPath = ConfigurationManager.AppSettings["ApplicationPath"];
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        
        public BaseModel()
        {
            _SQLDatabase = new SQLDatabase(connectionString);
            _PSReportService = new PSReportService.BuildClient();
        }

        public DirectApiDao IsDirectAPI(string toc)
        {
            DirectApiDao directApiDao;

            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\DirectAPI.json";
                if (!File.Exists(filePath))
                {
                    MessageException.NoLogMessageException("DirectAPI.Json File Not Found !");
                }

                string jsonPayload = File.ReadAllText(filePath);
                List<DirectApiDao> DirectApis = JsonConvert.DeserializeObject<List<DirectApiDao>>(jsonPayload);
                directApiDao = DirectApis.Find(x => x.TOC == toc);
            }
            catch (Exception)
            {
                throw;
            }

            return directApiDao;
        }

        public PartnerDao GetPartnerSetup(string toc, string insuredId, string sobId, string policyType)
        {
            PartnerDao partnerDao;

            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\PartnerSetup.json";
                if (!File.Exists(filePath))
                {
                    MessageException.NoLogMessageException("PartnerSetup.Json File Not Found !");
                }

                string jsonPayload = File.ReadAllText(filePath);
                List<PartnerDao> PartnerSetups = JsonConvert.DeserializeObject<List<PartnerDao>>(jsonPayload);
                partnerDao = PartnerSetups.Find(x => x.TOC == toc & x.InsuredId == insuredId & x.SOBId == sobId & x.PolicyType == policyType);
                if (partnerDao == null)
                {
                    partnerDao = new PartnerDao();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return partnerDao;
        }

        public Dictionary<string, object> DownloadFile(string source)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("FileName", "");
            result.Add("File", null);

            try
            {
                if (!Directory.Exists(source)) throw new Exception("File Not Found !");

                string fileCompression = string.Format("{0}.zip", source);
                if (File.Exists(fileCompression)) File.Delete(fileCompression);
                ZipFile.CreateFromDirectory(source, fileCompression);

                FileInfo fileInfo = new FileInfo(fileCompression);

                result["FileName"] = fileInfo.Name;
                result["File"] = File.ReadAllBytes(fileCompression);

                //---Delete File---
                Directory.Delete(source, true);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}