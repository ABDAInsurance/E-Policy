using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;
using E_Policy.Models.Constant;

namespace E_Policy.Models
{
    public class RequestModel : BaseModel
    {
        public void ValidateDataRequest(Dictionary<string, object> request)
        {
            try
            {
                string query = @"SELECT 
                                 CreatedBy As UserId
                                 FROM [EPolicy].[Request] WITH(NOLOCK)
                                 WHERE Status NOT IN ('U', 'S')
                                 AND IsTax = '{0}'                                 
                                 AND PolicyNo = '{1}'
                                 AND StartCertificateNo = '{2}'
                                 AND EndCertificateNo = '{3}'
                                 ";

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
                          WHERE Status NOT IN ('U', 'S')
                          AND IsTax = '{0}'      
                          AND CreatedBy = '{1}'";

                dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, request["IsTax"], request["UserId"]), CommandType.Text);

                if (dataTable.Rows.Count > 8)
                {
                    MessageException.NoLogMessageException("Please Complete Your Request !");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveRequest(Dictionary<string, object> request, DataTable dtPolicy)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string query = "[SpTriggerTable]";
                List<Dictionary<string, object>> payloadList = new List<Dictionary<string, object>>();

                foreach (DataRow drPolicy in dtPolicy.Rows)
                {
                    Dictionary<string, object> payload = new Dictionary<string, object>();

                    payload.Add("Ano", Convert.ToInt32(drPolicy["Ano"]));
                    payload.Add("PolicyNo", (drPolicy["PolicyStatusCode"].ToString() == "I" ? string.Format("{0}-{1}", drPolicy["PolicyNo"], drPolicy["CertificateNo"]) : string.Format("{0}-{1}", drPolicy["RegisterNo"], drPolicy["CertificateNo"])));
                    payload.Add("DocumentType", request["DocumentType"]);
                    payload.Add("IsUploadToCare", request["IsUploadToCare"]);
                    payload.Add("ApiUrl", request["ApiUrl"]);
                    payload.Add("IsCustomLayout", request["IsCustomLayout"]);
                    payload.Add("IsCertificate", request["IsCertificate"]);
                    payload.Add("TOC", request["TOC"]);
                    payload.Add("CompanyCode", request["CompanyCode"]);

                    payloadList.Add(payload);
                }

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
                    Value = "[EPolicy].[Request]"
                });

                DataTable dataTable = _SQLDatabase.ExecuteQuery(query, CommandType.StoredProcedure, sqlParameters);

                DataRow dataRow = dataTable.NewRow();
                dataRow["PolicyNo"] = request["PolicyNo"];
                dataRow["StartCertificateNo"] = request["StartCertificateNo"];
                dataRow["EndCertificateNo"] = request["EndCertificateNo"];
                dataRow["Payload"] = JsonConvert.SerializeObject(payloadList);
                dataRow["DownloadUrl"] = string.Empty;
                dataRow["IsTax"] = request["IsTax"];
                dataRow["FailedCount"] = 0;
                dataRow["Status"] = RequestStatus.PROCESS;
                dataRow["Message"] = "";
                dataRow["CreatedBy"] = request["UserId"];
                dataRow["CreatedDate"] = DateTime.Now;
                dataRow["UpdatedBy"] = request["UserId"];
                dataRow["UpdatedDate"] = DateTime.Now;
                dataTable.Rows.Add(dataRow);

                _SQLDatabase.TransferToDataBase(dataTable, "[EPolicy].[Request]");
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
                                  Id                                 
                                 ,PolicyNo
                                 ,StartCertificateNo
                                 ,EndCertificateNo
                                 ,DownloadUrl
                                 ,Status
                                 ,Message
                                 FROM [EPolicy].[Request] A WITH(NOLOCK)
                                 WHERE Status <> '{0}'
                                 AND IsTax = '{1}'      
                                 AND CreatedBy = '{2}'
                                 ORDER BY CreatedDate ASC";

                DataTable dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, RequestStatus.SUCCESS, request["IsTax"], request["UserId"]), CommandType.Text);

                data = dataTable;
            }
            catch (Exception)
            {
                throw;
            }

            return data;
        }
    }
}