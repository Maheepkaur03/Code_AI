using HDFCReportAutomation_API.DataLayer;
using HDFCReportAutomation_API.Helpers.ExceptionLogHelper;
using HDFCReportAutomation_API.Helpers.ExternalTokenHelper;
using HDFCReportAutomation_API.Helpers.Misc;
using HDFCReportAutomation_API.Model;
using HDFCReportAutomation_API.Model.CommonServerResponse;
using HDFCReportAutomation_API.Model.ReportRequestByUserIdResponse;
using HDFCReportAutomation_API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static HDFCReportAutomation_API.Model.DashboardModels;

namespace HDFCReportAutomation_API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICompanyRepository _companyRepository;
        ServerResponse _response = new();
        ExeptionLogger _exceptionLogger = new();
        private readonly MiscHelper _miscHelper = new();
        private dynamic jsonResponse;
        DataTable _dt = new();
        ExternalTokenGen _externalTokenGen;
        DataAccess _dataAccess = new();

        public IConfiguration _configuration { get; }

        public ReportController(IReportRepository reportRepository, IHttpClientFactory httpClientFactory, IConfiguration configuration, ICompanyRepository companyRepository)
        {
            _reportRepository = reportRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _companyRepository = companyRepository;
            _externalTokenGen = new ExternalTokenGen(_configuration);
        }

        [HttpPost]
        [Route("insertIntoAuditLogs")]
        public ServerResponse InsertIntoAuditLogs([FromBody] ReportCompanyInformationAuditLog reportCompanyInformationAuditLog)
        {
            bool isAdded = false;
            try
            {
                isAdded = _reportRepository.InsertIntoAuditLog(reportCompanyInformationAuditLog);
                if (isAdded)
                {
                    _response.status = "success";
                    _response.msg = "Inserted Successfully";
                    _response.errorCode = 200;
                    _response.data = isAdded;
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not insert";
                    _response.errorCode = 500;
                    _response.data = isAdded;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("InsertIntoReportTable", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpGet]
        [Route("getAllRequestedReports")]
        public GenericTableModel GetAllRequestedReports()
        {
            GenericTableModel loaTable = new GenericTableModel();
            var data = _reportRepository.GetAllRequestedReports();

            if (data != null)
            {
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.Id).ToString(), ColumnName = "Sr.No.", ColumnType = "Text", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.CompanyName).ToString(), ColumnName = "Entity Name", ColumnType = "Text", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.CIN).ToString(), ColumnName = "CIN", ColumnType = "Text", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.RequestedBy).ToString(), ColumnName = "Requested By", ColumnType = "Text", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.CreatedOn).ToString(), ColumnName = "Requested On", ColumnType = "Numeric", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.FinalisedOn).ToString(), ColumnName = "Completed On", ColumnType = "Numeric", IsActive = true, IsColumnFixed = false });
                //loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.Remarks).ToString(), ColumnName = "Comments", ColumnType = "Text", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.Status).ToString(), ColumnName = "Status", ColumnType = "Text", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.Action).ToString(), ColumnName = "Action", ColumnType = "Text", IsActive = true, IsColumnFixed = false });
                loaTable.Header.Add(new EntityCategoryMapModel { ColumnId = ((int)enumReportRequestByUserIdResponse.CompanyId).ToString(), ColumnName = "Company Id", ColumnType = "Text", IsActive = true, IsColumnFixed = false });


                foreach (var item in data)
                {
                    Dictionary<string, object> columnwise = new Dictionary<string, object>();
                    foreach (var head in loaTable.Header)
                    {
                        ColumnValueModel columnvaluemodel = null;
                        switch ((enumReportRequestByUserIdResponse)Convert.ToInt32((head.ColumnId)))
                        {
                            case enumReportRequestByUserIdResponse.Id:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                columnvaluemodel.Display = item.Id.ToString();
                                break;
                            case enumReportRequestByUserIdResponse.CompanyName:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                columnvaluemodel.Display = item.CompanyName.ToString();
                                break;
                            case enumReportRequestByUserIdResponse.CIN:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                columnvaluemodel.Display = item.CIN.ToString();
                                break;
                            case enumReportRequestByUserIdResponse.RequestedBy:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                columnvaluemodel.Display = item.RequestedBy.ToString();
                                break;
                            case enumReportRequestByUserIdResponse.CreatedOn:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                //columnvaluemodel.Display = item.CreatedOn.ToString();
                                columnvaluemodel.Display = _miscHelper.GetFormattedDisplayDate(item.CreatedOn);
                                columnvaluemodel.Value = _miscHelper.GetEpochValue(item.CreatedOn);
                                break;
                            case enumReportRequestByUserIdResponse.FinalisedOn:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                //columnvaluemodel.Display = item.FinalisedOn.ToString();
                                columnvaluemodel.Display =  _miscHelper.GetFormattedDisplayDate(item.FinalisedOn);
                                columnvaluemodel.Value = _miscHelper.GetEpochValue(item.FinalisedOn);
                                break;
                            //case enumReportRequestByUserIdResponse.Remarks:
                            //    columnvaluemodel = new ColumnValueModel();
                            //    columnvaluemodel.ValueColumn = "ValueText";
                            //    columnvaluemodel.Display = item.Remarks.ToString();
                            //    break;
                            case enumReportRequestByUserIdResponse.Status:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                columnvaluemodel.Display = item.Status.ToString();
                                break;
                            case enumReportRequestByUserIdResponse.Action:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                columnvaluemodel.Display = item.Action.ToString();
                                break;
                            case enumReportRequestByUserIdResponse.CompanyId:
                                columnvaluemodel = new ColumnValueModel();
                                columnvaluemodel.ValueColumn = "ValueText";
                                columnvaluemodel.Display = item.CompanyId.ToString();
                                break;
                            default:
                                break;
                        }
                        columnwise.Add(head.ColumnId, columnvaluemodel);
                    }
                    loaTable.Data.Add(columnwise);
                }
            }
            return loaTable;
        }

        private async Task<CompanyBase> Generate(int customerCode, int CompanyId)
        { 
            var httpClient = _httpClientFactory.CreateClient();
            string _token = string.Empty;

            if(customerCode == 1034)
            {
                _token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjZCN0FDQzUyMDMwNUJGREI0RjcyNTJEQUVCMjE3N0NDMDkxRkFBRTEiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJhM3JNVWdNRnY5dFBjbExhNnlGM3pBa2ZxdUUifQ.eyJuYmYiOjE3NTEwMDYwNzQsImV4cCI6MTc1MTAyNzY3NCwiaXNzIjoiaHR0cHM6Ly9taXV1YXRzc28uYW11a2hhLmNvbSIsImF1ZCI6WyJodHRwczovL21pdXVhdHNzby5hbXVraGEuY29tL3Jlc291cmNlcyIsIklkc3J2QVBJIiwiVGVybWluYWxBUEkiLCJVc2VyUHJvZmlsZSIsIkVGSSIsIkZpbmFuY2lhbHMiLCJMaXRpZ2F0aW9uIiwiTmV3cyIsIk9wZW5EYXRhU2V0cyIsIlBlZXJzRnVuZGFtZW50YWxzIiwiUmlzayIsIkxvb2t1cCIsIkludGVybmFsUnVsZVdpemFyZCIsIkF1ZGl0VHJhaWwiXSwiY2xpZW50X2lkIjoiTUlVX1VBVF9DbGllbnQiLCJzdWIiOiJmYmJjN2JiNS0yNjA2LTRkNjctYmQwZi01NDQ2M2NlYzhlNDQiLCJhdXRoX3RpbWUiOjE3NTEwMDYwNzMsImlkcCI6ImxvY2FsIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIklkc3J2QVBJIiwiVGVybWluYWxBUEkiLCJVc2VyUHJvZmlsZSIsIkVGSSIsIkZpbmFuY2lhbHMiLCJMaXRpZ2F0aW9uIiwiTmV3cyIsIk9wZW5EYXRhU2V0cyIsIlBlZXJzRnVuZGFtZW50YWxzIiwiUmlzayIsIkxvb2t1cCIsIkludGVybmFsUnVsZVdpemFyZCIsIkF1ZGl0VHJhaWwiXSwiYW1yIjpbInB3ZCJdfQ.d9kVFCchx3Z2jU1tP6MHDOyamGhUCPoSrjXquZb-DLU2yPkZUJPpyWD5QGa-XlstuG1t-HY6yLf3JSrCnnHcRnsFc7NQYlzKchfDYQocTzOzEBr3zefecXc4KkbRsJ1zm2NX8BLHj4vfcNqKpC7btb3r-2KdQiQFM4vma3xi-b4Z2XK93LmV3SY0NOlS6iTcP1p3W8DsYJH-uUtFtWhpF4TpDK13wz8VFtyHPbvtHFfTtLDkr8PRnrNXQ9fJ5yS-EmyZZRcior5ROUZkbBFqig1AhSDjwFdF3cQsCVBi-CxNhCS7mBKFOFo-rVHC5VuOO4iAi9UNYLvD21awxoql3g";
            }
            else
            {
                _token = _externalTokenGen.RefreshToken();
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            string baseUrl = string.Empty;
            baseUrl = "https://ewsuat.amukha.com";

            if (customerCode == 1034)
            {
                baseUrl = "https://miuuat.amukha.com";
                customerCode = 1035;
            }
            

            DateTime endDate = DateTime.UtcNow.Date;
            DateTime startDate = endDate.AddYears(-3);

            long startEpoch = new DateTimeOffset(startDate).ToUnixTimeSeconds();
            long endEpoch = new DateTimeOffset(endDate).ToUnixTimeSeconds();

            //int customerCode = 1028;
            var filteredData = new Dictionary<string, object>();
            CompanyBase cb = new();
            var generateResponse = new CompanyBaseData();

            var apiRequests = new List<ApiRequest>
            {
                //new ApiRequest("companyData", HttpMethod.Get, $"{baseUrl}/API/Lookup/api/Lookup/Common/GetCompanyById?companyId={reportRequest.CompanyId}"),
                new ApiRequest("companyData", HttpMethod.Get, $"{baseUrl}/API/Lookup/api/Lookup/Common/GetCompanyById?companyId={CompanyId}"),
                //new ApiRequest("companyScore", HttpMethod.Post, $"{baseUrl}/API/HRisk/api/Risk/EWS/Overview/GetScoreByCompnayid?CustomerCode={reportRequest.CustomerCode}&Companyid={reportRequest.CompanyId}&EntityType=C")
                new ApiRequest("companyScore", HttpMethod.Post, $"{baseUrl}/API/HRisk/api/Risk/EWS/Overview/GetScoreByCompnayid?CustomerCode={customerCode}&Companyid={CompanyId}&EntityType=C")
            };

            var results = new Dictionary<string, object>();

            try
            {
                var tasks = apiRequests.Select(async request =>
                {
                    try
                    {
                        HttpResponseMessage response;

                        if (request.Method == HttpMethod.Get)
                        {
                            response = await httpClient.GetAsync(request.Url);
                        }
                        else
                        {
                            var content = new StringContent(JsonSerializer.Serialize(request.Body), Encoding.UTF8, "application/json");
                            response = await httpClient.PostAsync(request.Url, content);
                        }

                        var responseBody = await response.Content.ReadAsStringAsync();
                        results[request.Key] = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<object>(responseBody) : $"Error {response.StatusCode}: {responseBody}";
                    }
                    catch (Exception ex)
                    {
                        results[request.Key] = $"Exception: {ex.Message}";
                    }
                });

                await Task.WhenAll(tasks);
                

                if (results.TryGetValue("companyData", out var companyDataObj))
                {
                    generateResponse.CompanyData = JsonSerializer.Deserialize<CompanyData>(
                        JsonSerializer.Serialize(companyDataObj));
                }

                if (results.TryGetValue("companyScore", out var companyScoreObj) && companyScoreObj is JsonElement companyScoreElement)
                {
                    if (companyScoreElement.ValueKind == JsonValueKind.Object)
                    {
                        generateResponse.CompanyScore = JsonSerializer.Deserialize<CompanyScore>(
                            companyScoreElement.GetRawText());
                    }
                    else if (companyScoreElement.ValueKind == JsonValueKind.Array)
                    {
                        var overallScore = companyScoreElement.EnumerateArray()
                            .FirstOrDefault(scoreItem =>
                                scoreItem.TryGetProperty("Modulename", out var moduleName) &&
                                moduleName.GetString() == "Overall");

                        if (overallScore.ValueKind == JsonValueKind.Object)
                        {
                            generateResponse.CompanyScore = JsonSerializer.Deserialize<CompanyScore>(
                                overallScore.GetRawText());
                        }
                    }
                }

                string company_Name = generateResponse.CompanyData?.CompanyName;
                double? score = generateResponse.CompanyScore?.Score;

                
                cb.CompanyName = company_Name;
                cb.Score = score;


                return cb;
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("generateReportById", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return cb;
        }

        private dynamic GetCompanyDetails(string cin)
        {
            _dt = _companyRepository.GetCompanyDetailsByCIN(cin);
            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    jsonResponse = _miscHelper.DataTableSystemTextJson(_dt);
                }
                else
                {
                    _exceptionLogger.ExceptionLog("GetCompanyDetails", "No Company Details found for requested CIN");
                }
            }
            else
            {
                _exceptionLogger.ExceptionLog("GetCompanyDetails", "No Company Details found for requested CIN");
            }
            return jsonResponse;
        }

        private dynamic GetCompanyAddress(string cin)
        {
            _dt = _companyRepository.GetCompanyAddressByCin(cin);
            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    jsonResponse = _miscHelper.DataTableSystemTextJson(_dt);
                }
                else
                {
                    _exceptionLogger.ExceptionLog("GetCompanyAddress", "No Company Address found for requested CIN");
                }
            }
            else
            {
                _exceptionLogger.ExceptionLog("GetCompanyAddress", "No Company Address found for requested CIN");
            }
            return jsonResponse;
        }

        [HttpGet]
        [Route("GetCompanyWiseTemplate")]
        public async Task<ServerResponse> GetCompanyWiseTemplate(int ReportId, string clientId, int companyId, string cin)
        {
            ServerResponse response = new();
            DataAccess dataAccess = new();
            try
            {
                ReportData reportJson = new();
                var sections = new List<Section>(); 
                List<ClientTemplateSection> templateSections = new();
                CompanyBase companyBaseDetails = new();

                companyBaseDetails = await Generate(int.Parse(clientId), companyId);
                companyBaseDetails.CIN = cin;

                var template = _configuration.GetSection($"ClientTemplates:{clientId}")
                                             .Get<List<ClientTemplateSection>>();

                foreach (var item in template)
                {
                    if (item.Type == "TEXTAREA")
                    {
                        var result = _dataAccess.CallTextProcedure(item.Title, companyId, cin);
                        
                        sections.Add(new Section
                        {
                            Id = item.Id,
                            //Title = result.Count > 0 ? result[0]?.Title : "",
                            Title = ReplaceUnderscoreWithSpace(item.Title),
                            Type = "TEXTAREA",
                            Data = result.Count > 0 ? result[0]?.Data : ""
                        });
                    }
                    else if (item.Type == "TABLE")
                    {
                        var allRows = _dataAccess.GetTableDataByCompanyIdAndCIN(item.Title, companyId, cin);

                        List<ChildNode> filteredNodes = item.ChildNodes
                            .Select(config =>
                            {
                                var match = allRows.FirstOrDefault(r => r.Title == config.Title);
                                if (match != null)
                                {
                                    // Apply format if specified
                                    if (!string.IsNullOrEmpty(config.Format) && config.Format == "Amount")
                                    {
                                        match.Data = ConvertFormattedAmount(match.Data);
                                    }

                                    return new ChildNode
                                    {
                                        Title = ReplaceUnderscoreWithSpace(match.Title),
                                        Data = match.Data
                                    };
                                }
                                else
                                {
                                    return new ChildNode
                                    {
                                        Title = ReplaceUnderscoreWithSpace(config.Title),
                                        Data = null
                                    };
                                }
                            }).ToList();

                        sections.Add(new Section
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Type = "TABLE",
                            ChildNodes = filteredNodes
                        });
                    }

                    //else if (item.Type == "TABLE")
                    //{
                    //    var allRows = _dataAccess.GetTableDataByCompanyIdAndCIN(item.Title, companyId, cin);


                    //    List<ChildNode> filteredNodes = item.ChildNodes
                    //        .Select(childKey =>
                    //        {
                    //            var match = allRows.FirstOrDefault(r => r.Title == childKey);
                    //            if (match != null)
                    //            {
                    //                return match;
                    //            }
                    //            else
                    //            {
                    //                return new ChildNode
                    //                {
                    //                    Title = childKey,
                    //                    Data = null
                    //                };
                    //            }
                    //        }).ToList();

                    //    sections.Add(new Section
                    //    {
                    //        Id = item.Id,
                    //        Title = item.Title,
                    //        Type = "TABLE",
                    //        ChildNodes = filteredNodes
                    //    });
                    //}

                }

                reportJson.TemplateSection = sections;
                reportJson.CompanyBaseData = companyBaseDetails;

                InsertInitialDataRequest req = new();
                req.ReportId = ReportId;

                var finalJsonObject = new
                {
                    TemplateSection = sections,
                    CompanyBaseData = companyBaseDetails
                };

                req.InitialJsonData = JsonSerializer.Serialize(finalJsonObject);


                bool isAdded = InsertInitialDataForCompany(req);

                if (isAdded)
                {
                    response.status = "Success";
                    response.msg = "Data generated successfully";
                    response.errorCode = 200;
                    response.data = isAdded;

                    return response;
                }
                response.status = "Error";
                response.msg = "Could not generate data";
                response.errorCode = 500;
                response.data = isAdded;
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("GetCompanyWiseJson", ex.ToString());
                response.status = "Error";
                response.msg = ex.Message;
                response.errorCode = 500;
                response.data = "";
            }

            return response;
        }

        private bool InsertInitialDataForCompany([FromBody] InsertInitialDataRequest model)
        {
            var result = _reportRepository.InsertInitialTestData(model);
            try
            {
                if (model != null)
                {
                    if (result != null)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("Initial Test Data", ex.ToString());
                return false;
            }
            return false;
        }

        private string GetCINByCompanyId(int companyId)
        {
            string cin = _dataAccess.GetCINByCompanyId(companyId);
            return cin;
        }

        [HttpPost("report-request")]
        public ServerResponse ReportRequest([FromBody] List<ReportRequestModel> models)
        {
            try
            {
                List<int> failedCompanyIds = new();
                List<int> cinNotFoundCompanyIds = new();
                List<int> successCompanyIds = new();

                foreach (ReportRequestModel model in models)
                {
                    string cin = GetCINByCompanyId(model.CompanyId);
                    model.CIN = cin;

                    if (string.IsNullOrEmpty(cin))
                    {
                        cinNotFoundCompanyIds.Add(model.CompanyId);
                        _exceptionLogger.ExceptionLog("Report Request", $"CIN for CompanyId : {model.CompanyId} not found");
                        continue;
                    }

                    bool isAdded = _reportRepository.CreateReportRequest(model);
                    if (isAdded)
                    {
                        successCompanyIds.Add(model.CompanyId);
                    }
                    else
                    {
                        failedCompanyIds.Add(model.CompanyId);
                        _exceptionLogger.ExceptionLog("Report Request", $"Could not add CompanyId: {model.CompanyId}");
                    }
                }

                if (successCompanyIds.Count > 0 && (failedCompanyIds.Count > 0 || cinNotFoundCompanyIds.Count > 0))
                {
                    _response.status = "Partial Success";
                    _response.msg = "Some reports were requested successfully.";
                    _response.errorCode = 207;
                    _response.data = new
                    {
                        Created = successCompanyIds,
                        Failed = failedCompanyIds,
                        CINNotFound = cinNotFoundCompanyIds
                    };
                }
                else if (successCompanyIds.Count == models.Count)
                {
                    _response.status = "Success";
                    _response.msg = "Reports requested successfully";
                    _response.errorCode = 201;
                    _response.data = successCompanyIds;
                }
                else
                {
                    _response.status = "Failure";
                    _response.msg = "No reports were requested.";
                    _response.errorCode = 500;
                    _response.data = new
                    {
                        Failed = failedCompanyIds,
                        CINNotFound = cinNotFoundCompanyIds
                    };
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("CreateReport", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }

            return _response;
        }

        [HttpGet("getCheckerReports")]
        public ServerResponse GetCheckerReports(int clientId)
        {
            try
            {
                var dt = _reportRepository.GetReportRequestsForChecker(clientId);

                if (clientId != 0 && clientId != null)
                {
                    if (dt != null)
                    {
                        var json = _miscHelper.DataTableSystemTextJson(dt);
                        _response.status = "success";
                        _response.msg = "Successfully Got Checker Reports";
                        _response.errorCode = 200;
                        _response.data = json;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not GetCheckerReports";
                    _response.errorCode = 500;
                    _response.data = dt;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("GetCheckerReports", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpPost("approve-and-assign")]
        public ServerResponse ApproveAndAssign(int reportId, int makerId, string makerName)
        {
            var result = _reportRepository.ApproveAndAssignReport(reportId, makerId, makerName);
            try
            {
                if (reportId != 0 && reportId != null && makerId != 0 && makerId != null)
                {
                    if (result)
                    {
                        _response.status = "success";
                        _response.msg = "Approved and Assigned Report Successfully";
                        _response.errorCode = 200;
                        _response.data = result;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not Approve and Assign Report";
                    _response.errorCode = 500;
                    _response.data = result;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("ApproveAndAssign", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpGet("getMakerReports")]
        public ServerResponse GetMakerReports(int makerId, int clientId)
        {
            var dt = _reportRepository.GetReportsForMaker(makerId, clientId);
            try
            {
                if (makerId != 0 && makerId != null && clientId != 0 && clientId != null)
                {
                    if (dt != null)
                    {
                        var result = ConvertDataTableToList(dt);
                        _response.status = "success";
                        _response.msg = "Successfully Got Reports for Maker";
                        _response.errorCode = 200;
                        _response.data = result;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not GetReport for Maker";
                    _response.errorCode = 500;
                    _response.data = dt;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("GetMakerReports", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpPost("insert-initial-data")]
        public ServerResponse InsertInitialTestData([FromBody] InsertInitialDataRequest model)
        {
            var result = _reportRepository.InsertInitialTestData(model);
            try
            {
                if (model != null)
                {
                    if (result != null)
                    {
                        _response.status = "success";
                        _response.msg = "Inserted Initial Test Data Successfully";
                        _response.errorCode = 200;
                        _response.data = result;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not Insert Initial Test Data";
                    _response.errorCode = 500;
                    _response.data = result;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("Initial Test Data", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpGet("getReportDataByReportId")]
        public ServerResponse GetReportData(int reportId)
        {
            var dt = _reportRepository.GetReportData(reportId);
            try
            {
                //var result = ConvertDataTableToList(dt);
                if (reportId != 0 && reportId != null)
                {
                    if (dt != null)
                    {
                        var json = _miscHelper.DataTableSystemTextJson(dt);

                        _response.status = "success";
                        _response.msg = "Successfully Got Report Data";
                        _response.errorCode = 200;
                        _response.data = json;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not GetReportData";
                    _response.errorCode = 500;
                    _response.data = dt;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("GetReportData", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpPost("updateReportData")]
        public ServerResponse UpdateReportData([FromBody] InsertUpdatedData model)
        {
            var result = _reportRepository.UpdateReportData(model);
            try
            {
                if (model != null)
                {
                    if (result)
                    {
                        _response.status = "success";
                        _response.msg = "Updated Report Data Successfully";
                        _response.errorCode = 200;
                        _response.data = result;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not do UpdateReortData";
                    _response.errorCode = 500;
                    _response.data = result;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("UpdateReportData", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpPost("send-for-review")]
        public ServerResponse SendForReview(int reportId)
        {
            var result = _reportRepository.SendReportForReview(reportId);
            try
            {
                if (reportId != null && reportId != 0)
                {
                    if (result)
                    {
                        _response.status = "success";
                        _response.msg = "Successfully Send For Review";
                        _response.errorCode = 200;
                        _response.data = result;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not Send For Review";
                    _response.errorCode = 500;
                    _response.data = result;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("SendForReview", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        [HttpPost("finalize")]
        public ServerResponse FinalizeReport(int reportId)
        {
            var result = _reportRepository.FinalizeReport(reportId);
            try
            {
                if (reportId != null && reportId != 0)
                {
                    if (result)
                    {
                        _response.status = "success";
                        _response.msg = "Finalize Report Successfully";
                        _response.errorCode = 200;
                        _response.data = result;
                    }
                }
                else
                {
                    _response.status = "Error";
                    _response.msg = "Could not Finalize Report";
                    _response.errorCode = 500;
                    _response.data = result;
                }
            }
            catch (Exception ex)
            {
                _exceptionLogger.ExceptionLog("FinalizeReport", ex.ToString());
                _response.status = "Error";
                _response.msg = ex.Message;
                _response.errorCode = 500;
                _response.data = "";
            }
            return _response;
        }

        private static List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return list;
        }

        private static string ConvertFormattedAmount(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "N/A";

            var parts = input.Split('|');
            if (parts.Length == 0 || !decimal.TryParse(parts[0].Trim(), out decimal amount))
                return "N/A";

            string formattedAmount = Math.Abs(amount) >= 10000000
                ? $"Rs {(amount / 10000000):0.00} Cr"
                : $"Rs {(amount / 100000):0.00} Lakh";

            if (parts.Length > 1)
            {
                string suffix = parts[1].Trim();
                if (!string.IsNullOrEmpty(suffix))
                    formattedAmount += $" | {suffix}";
            }

            return formattedAmount;
        }

        private static string ReplaceUnderscoreWithSpace(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input.Replace("_", " ");
        }

    }
}
