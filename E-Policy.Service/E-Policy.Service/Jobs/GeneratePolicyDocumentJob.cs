using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace E_Policy.Service.Jobs
{
    public class GeneratePolicyDocumentJob : BaseJob
    {
        private void SaveTransaction(string policyNo, int totalDocument, string userId)
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
                    Value = "[EPolicy].[Transaction]"
                });

                DataTable dataTable = _SQLDatabase.ExecuteQuery(query, CommandType.StoredProcedure, sqlParameters);

                DataRow dataRow = dataTable.NewRow();
                dataRow["PolicyNo"] = policyNo;
                dataRow["DocumentType"] = totalDocument;
                dataRow["IsUploadToCare"] = totalDocument;
                dataRow["CreatedBy"] = userId;
                dataRow["CreatedDate"] = DateTime.Now;
                dataRow["UpdatedBy"] = userId;
                dataRow["UpdatedDate"] = DateTime.Now;
                dataTable.Rows.Add(dataRow);

                _SQLDatabase.TransferToDataBase(dataTable, "[EPolicy].[Transaction]");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetOutstanding()
        {
            DataTable result = new DataTable("MyData");

            try
            {
                string query = @"SELECT Id, Payload FROM [epolicy].[Request] WHERE Status = 'P'";

                result = _SQLDatabase.ExecuteQuery(query, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public void Run()
        {
            try
            {
                DataTable dataTable = GetOutstanding();

                //---Processing Data---
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    try
                    {
                        List<Dictionary<string, object>> payloads = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(dataRow["Payload"].ToString());
                    }
                    catch (Exception ex)
                    {
                        //MessageLog.Error(string.Format("Generate Policy Document : [Ano = {0} | PolicyNo = {1} | Message = {2}]", processingParameter.Ano, processingParameter.PolicyNo, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageLog.Error(ex.Message);
            }
        }
    }
}
