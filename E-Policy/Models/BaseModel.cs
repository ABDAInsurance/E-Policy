using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;

using Newtonsoft.Json;
using E_Policy.Models.Dao;
using E_Policy.Models.Utilities;
using System.Data;

namespace E_Policy.Models
{
    public class BaseModel
    {
        protected SQLDatabase _SQLDatabase;
        protected PSReportService.BuildClient _PSReportService;
        protected NonPSReportService.BuildClient _NonPSReportService;

        protected string _ApplicationPath = ConfigurationManager.AppSettings["ApplicationPath"];
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        
        public BaseModel()
        {
            _SQLDatabase = new SQLDatabase(connectionString);
            _PSReportService = new PSReportService.BuildClient();
            _NonPSReportService = new NonPSReportService.BuildClient();
        }

        public ApiSetupDao GetApiSetup(string toc)
        {
            ApiSetupDao ApiSetupDao;

            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\APISetup.json";
                if (!File.Exists(filePath))
                {
                    MessageException.NoLogMessageException("APISetup.Json File Not Found !");
                }

                string jsonPayload = File.ReadAllText(filePath);
                List<ApiSetupDao> DirectApis = JsonConvert.DeserializeObject<List<ApiSetupDao>>(jsonPayload);
                ApiSetupDao = DirectApis.Find(x => x.TOC == toc);
            }
            catch (Exception)
            {
                throw;
            }

            return ApiSetupDao;
        }

        public DataRow GetPartnerSetup(string toc, string insuredId, string sourceId)
        {
            DataRow result;

            try
            {
                string query = @"SELECT 
                                  A.CompanyCode
                                 ,A.CompanyName
                                 ,A.IsCertificate
                                 ,A.IsCustomLayout
                                 ,B.TOC
                                 ,B.DocumentType
                                 FROM [epolicy].[msPartner] A WITH(NOLOCK)
                                 INNER JOIN [epolicy].[ProductSetup] B WITH(NOLOCK)
                                 ON A.CompanyCode = B.CompanyCode
                                 WHERE B.TOC = '{0}' 
                                 AND B.InsuredId = '{1}' 
                                 AND B.SourceId = '{2}'";

                query = string.Format(query, toc, insuredId, sourceId);

                DataTable dataTable = _SQLDatabase.ExecuteQuery(query, CommandType.Text);

                result = dataTable.Rows[0];
            }
            catch (Exception)
            {
                throw;
            }

            return result;
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