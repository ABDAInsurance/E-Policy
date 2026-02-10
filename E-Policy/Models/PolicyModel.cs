using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

using E_Policy.Models.Dao;

namespace E_Policy.Models
{
    public class PolicyModel : BaseModel
    {
        private void GenerateDocumentByCare(int ano, string destinationFile, string rptFile)
        {
            try
            {
                string errorMessage = _PSReportService.PolicyCertificateV2Report(ano, destinationFile, rptFile);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageException.NoLogMessageException(ex.Message);
            }
        }

        private void GenerateDocumentByApi(string apiUrl, string destinationFile)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(apiUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                    HttpResponseMessage httpResponseMessage = httpClient.GetAsync(apiUrl).Result;

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        HttpContent httpContent = httpResponseMessage.Content;
                        var contentStream = httpContent.ReadAsStreamAsync().Result; // get the actual content stream

                        using (var fileStream = new FileStream(destinationFile, FileMode.CreateNew))
                        {
                            contentStream.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        MessageException.NoLogMessageException("Failed Policy Generated via API !");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageException.NoLogMessageException(ex.Message);
            }
        }

        private void SaveHistory(string policyNo, int totalDocument, string userId)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string query = "[Abda].[SpTriggerTable]";

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@triggerEvent",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "I"
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@tableName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "[Abda].[epUserActivity]"
                });

                DataTable dataTable = _SQLDatabase.ExecuteQuery(query, CommandType.StoredProcedure, sqlParameters);

                DataRow dataRow = dataTable.NewRow();
                dataRow["PolicyNo"] = policyNo;
                dataRow["TotalDocument"] = totalDocument;
                dataRow["CreatedBy"] = userId;
                dataRow["CreatedDate"] = DateTime.Now;
                dataRow["UpdatedBy"] = userId;
                dataRow["UpdatedDate"] = DateTime.Now;
                dataTable.Rows.Add(dataRow);

                _SQLDatabase.TransferToDataBase(dataTable, "[Abda].[epUserActivity]");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object GetPolicy(Dictionary<string, object> request)
        {
            object data;

            try
            {
                string query = @"SELECT
                                 A.Ano
                                ,A.RegNo As RegisterNo
                                ,A.PolicyNo
                                ,A.CertificateNo
                                ,A.TOC As TOCCode
                                ,(SELECT Description + ' (' + TOC + ')' FROM TOC WITH(NOLOCK) WHERE TOC = A.TOC) As TOC
                                ,A.AId As InsuredId
                                ,(SELECT Name + ' (' + Id + ')' FROM Profile WITH(NOLOCK) WHERE Id = A.AId) As InsuredName
                                ,A.Source As SOBId
                                ,(SELECT Name + ' (' + Id + ')' FROM Profile WITH(NOLOCK) WHERE Id = A.Source) As SOB
                                ,B.PolicyType
                                ,A.AStatus As PolicyStatusCode
                                ,(CASE A.AStatus
                                    WHEN 'W' THEN 'Waiting'
                                    WHEN 'I' THEN 'Inforce'
                                    ELSE '-' END) PolicyStatus
                                FROM ACCEPTANCE A WITH(NOLOCK)
                                INNER JOIN Cover B WITH(NOLOCK)
                                ON A.Cno = B.Cno
                                WHERE A.AStatus IN ('W', 'I')
                                AND (A.RegNo = '{0}' OR A.PolicyNo = '{0}')";

                if(request.ContainsKey("StartCertificateNo") || request.ContainsKey("EndCertificateNo"))
                {
                    if(request["StartCertificateNo"].ToString().Length < 6 || request["EndCertificateNo"].ToString().Length < 6) MessageException.NoLogMessageException("Certificate Number Must be 6 (Six) Characters !");
                    if (request["StartCertificateNo"].ToString().Length > 6 || request["EndCertificateNo"].ToString().Length > 6) MessageException.NoLogMessageException("Certificate Number Must be 6 (Six) Characters !");
                    query += " AND A.CertificateNo BETWEEN '{1}' AND '{2}'";
                }
                else
                {
                    query += " AND (A.Ano = A.LAno OR A.LAno = -1)";
                    request.Add("StartCertificateNo", "");
                    request.Add("EndCertificateNo", "");
                }

                query += " ORDER BY A.Ano";

                DataTable dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, request["PolicyNo"], request["StartCertificateNo"], request["EndCertificateNo"]), CommandType.Text);

                if (dataTable.Rows.Count == 0)
                {
                    MessageException.NoLogMessageException(string.Format("CI Number / Polis Number : {0} Not Found !", request["PolicyNo"]));
                }
                else
                {
                    DirectApiDao directApiDao = IsDirectAPI(dataTable.Rows[0]["TOCCode"].ToString());

                    dataTable.Columns.Add("PartnerCode", typeof(string));
                    dataTable.Columns.Add("PartnerName", typeof(string));
                    dataTable.Columns.Add("ApiUrl", typeof(string));
                    dataTable.Columns.Add("IsTemplate", typeof(bool));
                    dataTable.Columns.Add("UserId", typeof(string));

                    if (directApiDao == null)
                    {
                        if (string.IsNullOrEmpty(request["StartCertificateNo"].ToString()) || string.IsNullOrEmpty(request["EndCertificateNo"].ToString()))
                        {
                            MessageException.NoLogMessageException("Certificate Number is Required !");
                        }

                        PartnerDao partnerDao = GetPartnerSetup(dataTable.Rows[0]["TOCCode"].ToString(),
                                                                dataTable.Rows[0]["InsuredId"].ToString(),
                                                                dataTable.Rows[0]["SOBId"].ToString(),
                                                                dataTable.Rows[0]["PolicyType"].ToString());

                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            dataRow["PartnerCode"] = partnerDao.Code;
                            dataRow["PartnerName"] = partnerDao.Name;
                            dataRow["ApiUrl"] = string.Empty;
                            dataRow["IsTemplate"] = true;
                            if (partnerDao.Code == "Default") dataRow["IsTemplate"] = false;
                            dataRow["UserId"] = request["UserId"];
                        }
                    }
                    else
                    {
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            dataRow["PartnerCode"] = string.Empty;
                            dataRow["PartnerName"] = string.Empty;
                            dataRow["ApiUrl"] = directApiDao.Url;
                            dataRow["IsTemplate"] = true;
                            dataRow["UserId"] = request["UserId"];
                        }
                    }
                }

                data = dataTable;
            }
            catch (Exception)
            {
                throw;
            }

            return data;
        }

        public string GenerateDocument(List<Dictionary<string, object>> requests)
        {
            string destinationPath = string.Empty;
            string documentNo = string.Empty;

            try
            {
                Dictionary<string, object> firstRequest = requests[0];
                documentNo = firstRequest["PolicyNo"].ToString();
                if (string.IsNullOrEmpty(firstRequest["PolicyNo"].ToString())) documentNo = firstRequest["RegisterNo"].ToString();
                
                destinationPath = string.Format(@"{0}\Result\{1}\{2}", _ApplicationPath, DateTime.Now.ToString("yyyy-MM-dd"), documentNo);

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                if (string.IsNullOrEmpty(firstRequest["ApiUrl"].ToString()))
                {
                    foreach (Dictionary<string, object> request in requests)
                    {
                        string rptFile = string.Format(@"{0}\RPT\{1}\{2}_{3}.rpt", _ApplicationPath, firstRequest["PartnerCode"], "PC", firstRequest["TOCCode"]);
                        if (firstRequest["PartnerCode"].ToString() == "Default") rptFile = string.Format(@"{0}\RPT\{1}\{2}.rpt", _ApplicationPath, firstRequest["PartnerCode"].ToString(), "PC");
                        string fileName = string.Format("{0}-{1} (PC).pdf", documentNo, request["CertificateNo"]);
                        GenerateDocumentByCare(Convert.ToInt32(request["Ano"]), destinationPath + "\\" + fileName, rptFile);
                    }
                }
                else
                {
                    foreach (Dictionary<string, object> request in requests)
                    {
                        string apiUrl = string.Format("{0}?PolicyNo={1}", firstRequest["ApiUrl"], documentNo);
                        string fileName = string.Format("{0} (PC).pdf", documentNo);
                        GenerateDocumentByApi(apiUrl, destinationPath + "\\" + fileName);
                    }
                }

                SaveHistory(documentNo, requests.Count, firstRequest["UserId"].ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return destinationPath;
        }
    }
}