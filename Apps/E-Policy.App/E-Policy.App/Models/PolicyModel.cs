using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using E_Policy.App.Models.Dao;

namespace E_Policy.App.Models
{
    public class PolicyModel : BaseModel
    {
        public void ValidateRequest(Dictionary<string, object> request)
        {
            try
            {
                string query = @"SELECT 
                                 CreatedBy As UserId
                                 FROM [EPolicy].[Request] WITH(NOLOCK)
                                 WHERE Status <> 'S'
                                 AND IsTax = '{0}'                                 
                                 AND PolicyNo = '{1}'
                                 AND StartCertificateNo = '{2}'
                                 AND EndCertificateNo = '{3}'";

                DataTable dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, request["IsTax"], request["PolicyNo"], request["StartCertificateNo"], request["EndCertificateNo"]), CommandType.Text);

                if (dataTable.Rows.Count > 0)
                {
                    if (request["UserId"].ToString() == dataTable.Rows[0]["UserId"].ToString())
                    {
                        MessageException.NoLogMessageException("Your Request is Being Processed !");
                    }
                    else
                    {
                        MessageException.NoLogMessageException(string.Format("Your Request is Being Processed By : {0} !", dataTable.Rows[0]["UserId"].ToString()));
                    }
                }

                query = @"SELECT 
                          PolicyNo, StartCertificateNo, EndCertificateNo, Status
                          FROM [EPolicy].[Request] WITH(NOLOCK)
                          WHERE Status <> 'S'
                          AND IsTax = '{0}'      
                          AND CreatedBy = '{1}'";

                dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, request["IsTax"], request["UserId"]), CommandType.Text);

                if (dataTable.Rows.Count > 8)
                {
                    MessageException.NoLogMessageException("Please Complete Your Request !");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveRequest(Dictionary<string, object> request, DataTable dtPolicy)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();

                DataRow drPolicyFirstRow = dtPolicy.Rows[0];
                string documentNo = drPolicyFirstRow["PolicyNo"].ToString();
                if (drPolicyFirstRow["PolicyStatus"].ToString() == "W") documentNo = drPolicyFirstRow["RegisterNo"].ToString();

                //---Insert Table Request---
                string query = @"INSERT [EPolicy].[Request]([ParentAno],[PolicyNo],[StartCertificateNo],[EndCertificateNo],[TOC],[CompanyCode],[DocumentType],[ApiUrl],[IsCertificate],[IsCustomLayout],[IsUploadToCare],[DownloadUrl],[IsTax],[FailedCount],[Status],[Message],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate])
                                 VALUES(@ParentAno,@PolicyNo,@StartCertificateNo,@EndCertificateNo,@TOC,@CompanyCode,@DocumentType,@ApiUrl,@IsCertificate,@IsCustomLayout,@IsUploadToCare,'',@IsTax,0,'P','',@UserId,@CurrentDate,@UserId,@CurrentDate)";

                sqlParameters.Clear();
                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@ParentAno",
                    Value = Convert.ToInt32(dtPolicy.Rows[0]["ParentAno"])
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@PolicyNo",
                    Value = documentNo
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@StartCertificateNo",
                    Value = request["StartCertificateNo"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@EndCertificateNo",
                    Value = request["EndCertificateNo"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@TOC",
                    Value = drPolicyFirstRow["TOC"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@CompanyCode",
                    Value = request["CompanyCode"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@DocumentType",
                    Value = request["DocumentType"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@ApiUrl",
                    Value = request["ApiUrl"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@IsCertificate",
                    Value = request["IsCertificate"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@IsCustomLayout",
                    Value = request["IsCustomLayout"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@IsUploadToCare",
                    Value = request["IsUploadToCare"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@IsTax",
                    Value = request["IsTax"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@UserId",
                    Value = request["UserId"]
                });

                sqlParameters.Add(new SqlParameter()
                {
                    ParameterName = "@CurrentDate",
                    Value = DateTime.Now
                });

                int newId = _SQLDatabase.ExecuteNonQuery(query, CommandType.Text, true, sqlParameters);

                //---Insert Table Request Detail---
                query = "[SpTriggerTable]";

                sqlParameters.Clear();
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
                    Value = "[EPolicy].[RequestDetail]"
                });

                DataTable dataTable = _SQLDatabase.ExecuteQuery(query, CommandType.StoredProcedure, sqlParameters);

                foreach (DataRow drPolicy in dtPolicy.Rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["HeaderId"] = newId;
                    dataRow["Ano"] = drPolicy["Ano"];
                    dataRow["PolicyNo"] = documentNo;
                    if (Convert.ToInt32(drPolicy["LAno"]) > 0) dataRow["PolicyNo"] = string.Format("{0}-{1}", documentNo, drPolicy["CertificateNo"]);
                    dataRow["CreatedBy"] = request["UserId"];
                    dataRow["CreatedDate"] = DateTime.Now;
                    dataRow["UpdatedBy"] = request["UserId"];
                    dataRow["UpdatedDate"] = DateTime.Now;
                    dataTable.Rows.Add(dataRow);
                }

                _SQLDatabase.TransferToDataBase(dataTable, "[EPolicy].[RequestDetail]");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GenerateRequest(Dictionary<string, object> request)
        {
            try
            {
                ValidateRequest(request);

                string query = @"SELECT
                                 A.Ano
                                ,A.LAno                                
                                ,(CASE WHEN A.LAno = -1 THEN A.Ano ELSE A.LAno END) As ParentAno
                                ,A.RegNo As RegisterNo
                                ,A.PolicyNo
                                ,A.CertificateNo
                                ,A.TOC
                                ,A.AId As InsuredId
                                ,A.Source As SourceId
                                ,B.PolicyType
                                ,A.AType As PolicyStatusType                                
                                ,A.AStatus As PolicyStatus
                                FROM ACCEPTANCE A WITH(NOLOCK)
                                INNER JOIN Cover B WITH(NOLOCK)
                                ON A.Cno = B.Cno
                                WHERE A.AType <> 'C' 
                                AND A.AStatus IN ('W', 'I')
                                AND (A.RegNo = '{0}' OR A.PolicyNo = '{0}')";

                if (!string.IsNullOrEmpty(request["StartCertificateNo"].ToString()) || !string.IsNullOrEmpty(request["EndCertificateNo"].ToString()))
                {
                    if (request["StartCertificateNo"].ToString().Length < 6 || request["EndCertificateNo"].ToString().Length < 6) MessageException.NoLogMessageException("Certificate Number Must be 6 (Six) Characters !");
                    if (request["StartCertificateNo"].ToString().Length > 6 || request["EndCertificateNo"].ToString().Length > 6) MessageException.NoLogMessageException("Certificate Number Must be 6 (Six) Characters !");
                    query += " AND A.CertificateNo BETWEEN '{1}' AND '{2}'";
                }
                else
                {
                    query += " AND (A.Ano = A.LAno OR A.LAno = -1)";
                }

                query += " ORDER BY A.Ano";

                DataTable dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, request["PolicyNo"], request["StartCertificateNo"], request["EndCertificateNo"]), CommandType.Text);

                if (dataTable.Rows.Count == 0)
                {
                    MessageException.NoLogMessageException(string.Format("CI Number / Polis Number : {0} Not Found !", request["PolicyNo"]));
                }
                else
                {
                    DataRow drPolicy = dataTable.Rows[0];

                    if (dataTable.Rows.Count > 30)
                    {
                        MessageException.NoLogMessageException("Maximum of 30 Policies Per Request!");
                    }

                    if (Convert.ToBoolean(request["IsTax"]))
                    {
                        if (Convert.ToInt32(drPolicy["LAno"]) != -1)
                        {
                            if (string.IsNullOrEmpty(request["StartCertificateNo"].ToString()) || string.IsNullOrEmpty(request["EndCertificateNo"].ToString()))
                            {
                                MessageException.NoLogMessageException("Certificate Number is Required !");
                            }
                        }

                        request.Add("ApiUrl", "");
                        request.Add("IsCertificate", true);
                        request.Add("IsCustomLayout", true);
                        request.Add("CompanyCode", "");

                        if (string.IsNullOrEmpty(request["StartCertificateNo"].ToString()) || string.IsNullOrEmpty(request["EndCertificateNo"].ToString()))
                        {
                            request["IsCertificate"] = false;
                        }
                    }
                    else
                    {
                        ApiSetupDao ApiSetupDao = GetApiSetup(drPolicy["TOC"].ToString());

                        if (ApiSetupDao == null)
                        {
                            if (Convert.ToInt32(drPolicy["LAno"]) != -1)
                            {
                                if (string.IsNullOrEmpty(request["StartCertificateNo"].ToString()) || string.IsNullOrEmpty(request["EndCertificateNo"].ToString()))
                                {
                                    MessageException.NoLogMessageException("Certificate Number is Required !");
                                }
                            }

                            DataRow drPartner = GetPartnerSetup(request["DocumentType"].ToString(),
                                                                drPolicy["TOC"].ToString(),
                                                                drPolicy["SourceId"].ToString(),
                                                                drPolicy["InsuredId"].ToString());

                            request.Add("ApiUrl", "");
                            request.Add("IsCertificate", drPartner["IsCertificate"]);
                            request.Add("IsCustomLayout", true);
                            request.Add("CompanyCode", drPartner["CompanyCode"]);
                        }
                        else
                        {
                            if (request["DocumentType"].ToString() != "ALL" && request["DocumentType"].ToString() != "PS") MessageException.NoLogMessageException("This Policy Support Policy Schedule Only !");

                            request.Add("ApiUrl", ApiSetupDao.Url);
                            request.Add("IsCertificate", false);
                            request.Add("IsCustomLayout", true);
                            request.Add("CompanyCode", "");
                        }
                    }
                }

                SaveRequest(request, dataTable);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object GetRequest(Dictionary<string, object> request)
        {
            object data;

            try
            {
                string query = @"SELECT
                                *
                                FROM 
                                (
                                    SELECT
                                     Id                                 
                                    ,PolicyNo
                                    ,StartCertificateNo
                                    ,EndCertificateNo
                                    ,DownloadUrl
                                    ,Status
                                    ,Message
                                    ,CreatedDate
                                    FROM [EPolicy].[Request] A WITH(NOLOCK)
                                    WHERE Status <> 'S'
                                    AND IsTax = '{0}'      
                                    AND CreatedBy = '{1}'
                                    UNION
                                    SELECT
                                     Id                                 
                                    ,PolicyNo
                                    ,StartCertificateNo
                                    ,EndCertificateNo
                                    ,DownloadUrl
                                    ,Status
                                    ,Message
                                    ,CreatedDate
                                    FROM [EPolicy].[Request] A WITH(NOLOCK)
                                    WHERE Status = 'S'
                                    AND IsTax = '{0}'      
                                    AND CreatedBy = '{1}'
                                    AND UpdatedDate >= DATEADD(hour, -24, GETDATE())
                                ) As A
                                ORDER BY CreatedDate DESC";

                DataTable dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, request["IsTax"], request["UserId"]), CommandType.Text);

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

                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                }

                Directory.CreateDirectory(destinationPath);

                if (string.IsNullOrEmpty(firstRequest["ApiUrl"].ToString()))
                {
                    string rptFile = string.Empty;
                    string fileName = string.Empty;

                    foreach (Dictionary<string, object> request in requests)
                    {
                        
                    }
                }
                else
                {
                    foreach (Dictionary<string, object> request in requests)
                    {
                        string apiUrl = string.Format("{0}?PolicyNo={1}", firstRequest["ApiUrl"], documentNo);
                        string fileName = string.Format("{0} (PC).pdf", documentNo);
                        //GenerateDocumentByApi(apiUrl, destinationPath + "\\" + fileName);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return destinationPath;
        }
    }
}