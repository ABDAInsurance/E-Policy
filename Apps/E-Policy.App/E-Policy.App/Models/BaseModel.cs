using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;

using Newtonsoft.Json;
using E_Policy.App.Models.Dao;
using E_Policy.App.Models.Utilities;
using System.Linq;

namespace E_Policy.App.Models
{
    public class BaseModel
    {
        protected SQLDatabase _SQLDatabase;

        protected string _ApplicationPath = ConfigurationManager.AppSettings["ApplicationPath"];
        string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        
        public BaseModel()
        {
            _SQLDatabase = new SQLDatabase(connectionString);
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

        public DataRow GetPartnerSetup(string documentType, string toc, string sourceId, string insuredId, string topro, bool isCertificate)
        {
            DataRow result;
            
            try
            {
                string query = @"SELECT 
                                  TOC
                                 ,SourceId
                                 ,InsuredId
                                 ,CompanyCode
                                 ,DocumentType
                                 ,IsCertificate                                 
                                 ,IsCustomLayout
                                 ,Topro
                                 FROM [EPolicy].[ProductSetup] WITH(NOLOCK)
                                 WHERE TOC = '{0}' AND ISCertificate = {1}";


                if (!isCertificate)
                {
                    query += " AND TOPRO = '{2}' ";
                } 
                

                query = string.Format(query,toc,isCertificate?1:0, topro);

                DataTable dataTable = _SQLDatabase.ExecuteQuery(query, CommandType.Text);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow[] drCustomLayoutArray = dataTable.Select("IsCustomLayout = 1");

                    if (drCustomLayoutArray.Any())
                    {
                        DataTable dtCustomLayout = drCustomLayoutArray.CopyToDataTable();
                        DataRow[] drProductSetupArray = dataTable.Select(string.Format("SourceId = '{0}' AND InsuredId = '{1}'", sourceId, insuredId));

                        if (drProductSetupArray.Any())
                        {
                            dataTable = drProductSetupArray.CopyToDataTable();
                        }
                        else
                        {
                            drProductSetupArray = dataTable.Select("SourceId = '' and InsuredId = ''");
                            if (drProductSetupArray.Any())
                            {
                                dataTable = drProductSetupArray.CopyToDataTable();
                            }
                            else
                            {
                                dataTable.Rows.Clear();
                            }
                        }
                    }

                    if (dataTable.Rows.Count > 0 && documentType != "ALL")
                    {
                        DataRow[] foundDocument = dataTable.Select(string.Format("DocumentType = '{0}'", documentType));

                        if (foundDocument.Length == 0)
                        {
                            MessageException.NoLogMessageException(string.Format("Document Type Selected isn't Available !", documentType));
                        }
                    }
                }

                if (dataTable.Rows.Count == 0) dataTable.Rows.Add(toc, "", "", "Default", "PS", 1, 1);
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
                if(!File.Exists(source)) throw new Exception("File Not Found !");

                FileInfo fileInfo = new FileInfo(source);
                result["FileName"] = fileInfo.Name;
                result["File"] = File.ReadAllBytes(source);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}