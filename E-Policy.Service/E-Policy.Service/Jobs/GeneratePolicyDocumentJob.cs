using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;

using E_Policy.Service.Models.Constant;
using System.Linq;

namespace E_Policy.Service.Jobs
{
    public class GeneratePolicyDocumentJob : BaseJob
    {
        private void GeneratePolicyScheduleByApi(string apiUrl, string destinationFile)
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
                        throw new Exception("Failed Policy Generated via API !");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UpdateRequest(Dictionary<string, object> parameter)
        {
            try
            {
                string query = "SELECT * FROM [EPolicy].[Request] WITH(NOLOCK) WHERE Id = {0}";

                DataTable dataTable = _SQLDatabase.ExecuteQuery(string.Format(query, Convert.ToInt32(parameter["Id"])), CommandType.Text);

                foreach (DataRow dtRowUpdate in dataTable.Rows)
                {
                    if (parameter["Status"].ToString() == RequestStatus.FAILED)
                    {
                        dtRowUpdate["FailedCount"] = Convert.ToInt32(dtRowUpdate["FailedCount"]) + Convert.ToInt32(parameter["FailedCount"]);
                        if (Convert.ToInt32(dtRowUpdate["FailedCount"]) > 2) dtRowUpdate["Message"] = parameter["ErrorMessage"];
                        if (Convert.ToInt32(dtRowUpdate["FailedCount"]) < 3) parameter["Status"] = RequestStatus.PROCESS;
                    }
                    
                    dtRowUpdate["DownloadUrl"] = parameter["DownloadUrl"];
                    dtRowUpdate["Status"] = parameter["Status"];
                    dtRowUpdate["UpdatedBy"] = "system";
                    dtRowUpdate["UpdatedDate"] = DateTime.Now;
                }

                _SQLDatabase.TransferToDataBase(dataTable, "[EPolicy].[Request]");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateDocument(DataTable dataTable, int parentAno, string destinationPath)
        {
            try
            {
                List<string> careUploads = new List<string>();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (Convert.ToBoolean(dataRow["IsTax"]))
                    {
                        //---Generate PN---
                        string rptFile = string.Format(@"{0}\RPT\PN_Tax.rpt", ApplicationConfiguration.ApplicationPath);
                        string fileName = string.Format("{0} (PN).pdf", dataRow["PolicyNo"]);

                        //---Remove File Existing---
                        if (File.Exists(destinationPath + "\\" + fileName))
                        {
                            File.Delete(destinationPath + "\\" + fileName);
                        }

                        if (File.Exists(rptFile))
                        {
                            _CareService.GeneratePremiumNote(Convert.ToInt32(dataRow["Ano"]), destinationPath + "\\" + fileName, rptFile);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dataRow["ApiUrl"].ToString()))
                        {
                            string rptFile = string.Empty;
                            string fileName = string.Empty;

                            if (dataRow["DocumentType"].ToString() == "ALL" || dataRow["DocumentType"].ToString() == "PS")
                            {

                                //---Generate PS---
                                rptFile = string.Format(@"{0}\RPT\{1}\{2}_{3}.rpt", ApplicationConfiguration.ApplicationPath, dataRow["CompanyCode"], "PS", dataRow["TOC"]);
                                if (dataRow["CompanyCode"].ToString() == "Default") rptFile = string.Format(@"{0}\RPT\Default\PS.rpt", ApplicationConfiguration.ApplicationPath);
                                fileName = string.Format("{0} (PS).pdf", dataRow["PolicyNo"]);

                                //---Remove File Existing---
                                if (File.Exists(destinationPath + "\\" + fileName))
                                {
                                    File.Delete(destinationPath + "\\" + fileName);
                                }

                                _CareService.GeneratePolicySchedule(Convert.ToInt32(dataRow["Ano"]), destinationPath + "\\" + fileName, rptFile);
                                if (Convert.ToBoolean(dataRow["IsUploadToCare"])) careUploads.Add(destinationPath + "\\" + fileName);

                            }

                            if (dataRow["DocumentType"].ToString() == "ALL" || dataRow["DocumentType"].ToString() == "PN")
                            {
                                //---Generate PN---
                                rptFile = string.Format(@"{0}\RPT\{1}\{2}_{3}.rpt", ApplicationConfiguration.ApplicationPath, dataRow["CompanyCode"], "PN", dataRow["TOC"]);
                                fileName = string.Format("{0} (PN).pdf", dataRow["PolicyNo"]);

                                //---Remove File Existing---
                                if (File.Exists(destinationPath + "\\" + fileName))
                                {
                                    File.Delete(destinationPath + "\\" + fileName);
                                }

                                if (File.Exists(rptFile))
                                {
                                    _CareService.GeneratePremiumNote(Convert.ToInt32(dataRow["Ano"]), destinationPath + "\\" + fileName, rptFile);
                                    if (Convert.ToBoolean(dataRow["IsUploadToCare"])) careUploads.Add(destinationPath + "\\" + fileName);
                                }
                            }
                        }
                        else
                        {
                            string apiUrl = string.Format("{0}?PolicyNo={1}", dataRow["ApiUrl"], dataRow["PolicyNo"]);
                            string fileName = string.Format("{0} (PS).pdf", dataRow["PolicyNo"]);

                            //---Remove File Existing---
                            if (File.Exists(destinationPath + "\\" + fileName))
                            {
                                File.Delete(destinationPath + "\\" + fileName);
                            }

                            GeneratePolicyScheduleByApi(apiUrl, destinationPath + "\\" + fileName);
                        }
                    }
                }

                //---Upload To Care System---
                foreach (string careUpload in careUploads)
                {
                    _CareService.UploadPolicyDocument(parentAno, careUpload);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return "";
        }

        private DataTable GetOutstanding()
        {
            DataTable result = new DataTable("MyData");

            try
            {
                string query = @"SELECT 
                                  A.Id
                                 ,A.ParentAno
                                 ,A.PolicyNo As ParentPolicyNo
                                 ,A.TOC
                                 ,A.CompanyCode
                                 ,A.DocumentType
                                 ,A.ApiUrl
                                 ,A.IsCertificate
                                 ,A.IsUploadToCare
                                 ,A.IsTax                                 
                                 ,B.Ano
                                 ,B.PolicyNo
                                 FROM [EPolicy].[Request] A WITH(NOLOCK) 
                                 INNER JOIN [EPolicy].[RequestDetail] B WITH(NOLOCK)
                                 ON A.Id = B.Headerid
                                 WHERE A.Status = 'P'
                                 ORDER BY A.IsTax, A.Id";

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

                DataView dvOutstanding = new DataView(dataTable);
                DataTable dtOutstanding = dvOutstanding.ToTable(true, new[] { "Id", "ParentAno", "ParentPolicyNo", "IsUploadToCare", "IsTax" });

                //---Processing Data---
                foreach (DataRow dataRow in dtOutstanding.Rows)
                {
                    Dictionary<string, object> parameter = new Dictionary<string, object>();

                    //---Create Directory---
                    string destinationPath = string.Format(@"{0}\Result\{1}\{2}", ApplicationConfiguration.ApplicationPath, DateTime.Now.ToString("yyyy-MM-dd"), dataRow["ParentPolicyNo"]);

                    if (Convert.ToBoolean(dataRow["IsTax"]))
                    {
                        destinationPath = string.Format(@"{0}\Result\{1}\{2}-TAX", ApplicationConfiguration.ApplicationPath, DateTime.Now.ToString("yyyy-MM-dd"), dataRow["ParentPolicyNo"]);
                    }

                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }

                    try
                    {
                        DataRow[] drRequestDetailArray = dataTable.Select(string.Format("Id = {0}", dataRow["Id"]));

                        if (drRequestDetailArray.Any())
                        {
                            DataTable dtRequestDetail = drRequestDetailArray.CopyToDataTable();
                            GenerateDocument(dtRequestDetail, Convert.ToInt32(dataRow["ParentAno"]), destinationPath);
                        }

                        parameter.Add("Id", Convert.ToInt32(dataRow["Id"]));
                        parameter.Add("DownloadUrl", "");
                        parameter["Status"] = RequestStatus.SUCCESS;

                        //---Compress File-- -
                        string fileCompression = string.Format("{0}.zip", destinationPath);
                        if (File.Exists(fileCompression)) File.Delete(fileCompression);
                        ZipFile.CreateFromDirectory(destinationPath, fileCompression);

                        parameter["DownloadUrl"] = fileCompression;

                        UpdateRequest(parameter);
                    }
                    catch (Exception ex)
                    {
                        parameter.Add("Id", Convert.ToInt32(dataRow["Id"]));
                        parameter.Add("Status", RequestStatus.FAILED);
                        parameter.Add("ErrorMessage", ex.Message);
                        parameter.Add("FailedCount", 1);
                        UpdateRequest(parameter);

                        MessageLog.Error(string.Format("GeneratePolicyDocumentJob : [Id = {0} | Message = {1}]", Convert.ToInt32(dataRow["Id"]), ex.Message));
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
