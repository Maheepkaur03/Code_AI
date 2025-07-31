using Amukha.WorkflowCSBAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amukha.WorkflowCSBAPI.Cache.Repo;
using Heckyl.Terminal.Utility.Logging;
using System.Data.SqlClient;
using System.Reflection;
using Heckyl.Terminal.Utility.Core.ExtensionMethods;

namespace Amukha.WorkflowCSBAPI.Cache.Repo
{
    public static class GetFileId
    {
        public static int fileId { get; set; }
    }

    public class AppWorkflowRepository : DataBaseAccess
    {
        const string procGetFileId = "Ui.InsertUploadFilesMaster";
        const string procUploadFile = "Ui.InsertUploadFilesMaster";
        const string procInsertMinutesFile = "UI.InsertMinutesFile";
        const string procInsertNPAFile = "UI.InsertNPAFile";
        const string procInsertEWSFile = "[UI].[InsertEWSFile]";
        const string procInsertRFAFraudFile = "UI.Insert_RFAFraudBorrwersReport";
        const string procInsertPortfolioFile = "UI.InsertPortfolioFile";
        const string procInsertHistoricalFile = "UI.InsertOneTimeHistoricalFile";
        const string procGetWorkflowTableData = "Ui.GetUploadFilesMaster";

        const string procGetNPAFile = "UI.Get_NPAFile_View";
        const string procGetEWSFile = "UI.Get_EWSFile_View";
        const string procGetEWSLatestFile = "[UI].[Get_EWSFile_View_Latest]";
        const string procGetMinutesFile = "UI.Get_MinutesFile_View";
        const string procGetPortfolioFile = "UI.Get_PortfolioFile_View";
        const string procGetHistoricalFile = "UI.Get_OneTimeHistoricalFile_View";
        const string procgetEWSPeriods = "UI.GetFileDatesTypeWise";
        const string procgetLOAPeriods = "UI.Get_ListOfAccountDates";
        const string procgetExternalRating = "MIU.Get_ExternalRating_Workflow";

        const string procInsertFileStatusComments = "UI.Insert_FileStatusComments";
        const string procInsertFileCommentsTrail = "[UI].[Insert_FileCommentsTrail]";
        const string procGetFileCommentsTrail = "[UI].[Get_FileCommentsTrail]";
        const string procUpdateMOMRecordFromView = "UI.[UpdateMomFileFromViewTab]";
        const string procInsert_FileCommentAttachment = "[UI].[Insert_FileCommentsAttachements]";
        const string procGetWorkflowPredifinedReports = "UI.Get_PreDefinedReportsParametersWise";
        const string procGetSlippageReport = "UI.Get_SlippageReport";
        const string procMigrationReport = "[UI].[Get_StaticMigrationReport]";
        const string procCustomReport = "UI.Get_CustomReport";
        const string procGetCustomReportVaribles = "[UI].[Get_CustomReportVaribles]";
        const string procCheckIfFileExists = "[UI].[Upload_CheckAttachmentFileExists]";
        const string procGetCustomDefinedParametersReports = "[UI].[Get_CustomDefinedParameters_Reports]";
        const string procGetPreDefinedParametersReports = "[UI].[Get_PreDefinedParameters_Reports]";
        const string procGetEWSSummaryReport = "UI.Get_EWSSummary";
        const string procPopulateValuesForEWS = "Process.PopulateValuesForEWS";

        const string procEwsFileDataForAlertReport = "UI.GetEwsFileDataForAlertReport";
        const string procPortfolioLevelEwsFileWithUserComments = "UI.GetPortfolioLevelEwsFileWithUserComments";
        const string procPortfolioLevelEwsFileWithRMComments = "UI.GetPortfolioLevelTriggerComments";

        const string procGetMinutesFileTextCommentVersions = "UI.Get_MinutesFileTextCommentVersions_View";
        const string procUpdateMomFileFromViewTab = "[UI].[UpdateMomFileFromViewTab]";
        const string procInsert_UserActionsAgainstAlert = "[UI].[Insert_UserActionsAgainstAlert]";
        const string procWorkflowActionStatusMaster = "[UI].[Get_AuditActionStatusMaster]";
        const string procInsertUserActionsAttachements = "UI.Insert_UserActionsAttachements";
        const string procInsertUserRecordIdComments = "UI.Insert_UserRecordIdComments";
        const string procInsertUserRecordIdAttachements = "UI.Insert_UserRecordIdAttachements";
        const string procGetActionDetailsByRecordId = "[UI].[Get_ActionDetailsByRecordId]";
        const string procGetRatingDistribution = "[UI].[Get_RatingDistribution]";
        const string procGetEWSTrendReport = "[UI].[Get_EWSTrend]";
        const string procGetLOATable = "[UI].[Get_ListOfAccountByRM]";
        const string procGetActionTable = "[UI].[Get_ListOfAccountByRM_ActionRequired]";
        const string procGetCaseTransferLogs = "UI.GetCaseTransferLogs";
        const string procGetCaseReopenLogs = "[UI].[GetCaseReopenLogs]";
        const string procGetAttachments = "UI.GetCaseRemarksAndAttachementTrails";
        const string procGetTrfEvents = "[UI].[GetCaseTrfEventsMaster]";
        const string procCaseTrfLog = "[UI].InsertCaseTransferLogs";
        const string procCaseReopen = "[UI].[InsertCaseSubmissionStatusToReopen]";
        const string procGetFileAccountName = "UI.GetFileAccountName";
        const string procGetListCommentTrails = "UI.GetCaseAuditTrailCommentsAttachement";
        const string procGetListRemarkTrails = "UI.Get_CaseSubmissionRemarksAndAttachment";
        const string procAccountActionDetails = "UI.Get_ListOfAccountByRecordId";
        const string procInsertUpdateCase = "UI.Insert_CaseSubmissionStatus";
        const string procGetRemarkAndAttch = "UI.GetAllCaseRemarksTrails";
        const string procInsertCaseActionRemarksAndAttch = "UI.InsertCaseTrfEventsActionRemarksAndAttachment";
        const string procInsertCommentAttchments = "UI.InsertCaseAuditTrailCommentsAndAttachment";
        const string procgetAccountReportingUser = "UI.Get_AccountwiseReportingUserMapping";
        const string procInsertAccUserMapping = "[UI].[Insert_AccountwiseReportingUserMapping]";
        const string procGetRFAFraudFile = "UI.GetRFAFraudBorrwersReport";
        const string procInsertEmailGroup = "UI.Insert_EmaildGroup";
        const string procGetEmailGroup = "UI.GetEmaildGroup";
        const string procDeleteFileFromDB = "UI.DeleteUploadFilesOnFailure";
        const string procGetPeriodDropdown = "[RiskUIClient].[Terminal].[Proc_Get_RiskCategoryPeriods]";
        const string procGetPieChartSummary = "[UI].[GetAccountsCompletionStatus]";
        const string procAccountLevel = "[UI].[GetAccountLevelBreakDownReport]";
        const string procHighriskborrower = "[UI].[GetHighRiskBorrowerAccessControlDashboard]";
        public AppWorkflowRepository() : base("WorkflowDB")
        {

        }
        public bool populateValuesForEWS()
        {
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procPopulateValuesForEWS);
                var result = ExecuteDataSet(dbCommand);
            }
            catch (Exception e)
            {
                Logger.Error("Error in populateValuesForEWS " + e.StackTrace);
                return false;
            }

            return true;
        }

        public bool DeleteFileFromDB(int? FileId, DateTime? MappingMonth, string FileType)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    var dbCommand = DataBase.GetStoredProcCommand(procDeleteFileFromDB);
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, FileId);
                    DataBase.AddInParameter(dbCommand, "@MappingMonth", DbType.DateTime, MappingMonth);
                    DataBase.AddInParameter(dbCommand, "@FileType", DbType.String, FileType);

                    var result = ExecuteDataSet(dbCommand, true);
                }
                catch (Exception e)
                {
                    Logger.Error("Error in DeleteFileFromDB " + e.StackTrace);
                    return false;
                }
            }

            return true;
        }

        public FileCommentsTrailStatus addFileCommentsTrail(FileCommentsTrailModel obj)
        {
            bool isAdded = false;

            FileCommentsTrailStatus resp = new FileCommentsTrailStatus();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertFileCommentsTrail);
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, obj.FileId);
                DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, obj.RecordIds);
                DataBase.AddInParameter(dbCommand, "@FileType", DbType.String, obj.FileType);
                DataBase.AddInParameter(dbCommand, "@Userid", DbType.Guid, obj.Userid);
                DataBase.AddInParameter(dbCommand, "@Comments", DbType.String, obj.Comments);

                DataBase.AddOutParameter(dbCommand, "@CommentId", DbType.Int32, int.MaxValue);

                var result = ExecuteDataSet(dbCommand, true);



                resp.CommentId = Convert.ToInt32(DataBase.GetParameterValue(dbCommand, "@CommentId"));

                resp.isCommentAdded = true;
                // return resp;

            }
            catch (Exception e)
            {
                Logger.Error("Error while adding FileCommentsTrail  to DB , Error : ", e.Message);
                isAdded = false;
            }
            return resp; ;
        }
        public bool InsertEmailGroup(EmaildGroup emaildGroup)
        {
            bool isAddedEmail = false;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertEmailGroup);
                //DataBase.AddInParameter(dbCommand, "@Id", DbType.Int32, emaildGroup.Id);
                DataBase.AddInParameter(dbCommand, "@Flag", DbType.String, "Add");
                DataBase.AddInParameter(dbCommand, "@GroupName", DbType.String, emaildGroup.GroupName);
                DataBase.AddInParameter(dbCommand, "@EmailId", DbType.String, emaildGroup.EmailId);
                var result = ExecuteDataSet(dbCommand, true);
                isAddedEmail = true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while adding InsertEmailGroup  to DB , Error : ", ex.Message);
                isAddedEmail = false;
            }
            return isAddedEmail;
        }
        public bool UpdateEmailGroup(EmaildGroup emaildGroup)
        {
            bool isAddedEmail = false;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertEmailGroup);
                DataBase.AddInParameter(dbCommand, "@Id", DbType.Int32, emaildGroup.Id);
                DataBase.AddInParameter(dbCommand, "@Flag", DbType.String, emaildGroup.Flag);
                DataBase.AddInParameter(dbCommand, "@GroupName", DbType.String, emaildGroup.GroupName);
                DataBase.AddInParameter(dbCommand, "@EmailId", DbType.String, emaildGroup.EmailId);
                var result = ExecuteDataSet(dbCommand, true);
                isAddedEmail = true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while adding InsertEmailGroup  to DB , Error : ", ex.Message);
                isAddedEmail = false;
            }
            return isAddedEmail;
        }
        public bool AddToUploadMaster(UploadedFilesModel obj, int noOfAccounts)
        {
            bool isAddedToMaster = false;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procUploadFile);
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.String, obj.File_ID);
                DataBase.AddInParameter(dbCommand, "@UploadeBy", DbType.String, obj.UploadedBy);
                DataBase.AddInParameter(dbCommand, "@Topic", DbType.String, obj.Topic);
                DataBase.AddInParameter(dbCommand, "@Month", DbType.String, obj.Month);
                DataBase.AddInParameter(dbCommand, "@NoOfAccount", DbType.Int32, noOfAccounts);
                DataBase.AddInParameter(dbCommand, "@Notes", DbType.String, obj.Notes);
                //DataBase.AddInParameter(dbCommand, "@Attachment", DbType.String, AttachmentDetails.FileName);
                DataBase.AddInParameter(dbCommand, "@UserFileName", DbType.String, DownloadFileModel.UserFileName);
                DataBase.AddInParameter(dbCommand, "@ServerFileName", DbType.String, DownloadFileModel.ServerFileName);
                DataBase.AddOutParameter(dbCommand, "@Id", DbType.Int32, int.MaxValue);
                DataBase.AddInParameter(dbCommand, "@UserId", DbType.String, obj.UploadedById);
                DataBase.AddInParameter(dbCommand, "@FileUploadedDate", DbType.DateTime, obj.FileUploadDate);
                DataBase.AddInParameter(dbCommand, "@EndDate", DbType.DateTime, obj.EndDate);

                var result = ExecuteDataSet(dbCommand, true);
                GetFileId.fileId = Convert.ToInt32(DataBase.GetParameterValue(dbCommand, "@Id"));

                //may return this FileId later to confirm that record is created

                isAddedToMaster = true;

            }
            catch (Exception e)
            {
                Logger.Error("Error while adding row to upload master , Error : ", e.Message);
                isAddedToMaster = false;
            }
            return isAddedToMaster;
        }
        public List<EmaildGroupDetails> GetEmailGroupDetails()
        {
            List<EmaildGroupDetails> emaildGroupDetails = new List<EmaildGroupDetails>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetEmailGroup);
                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    emaildGroupDetails.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                select new EmaildGroupDetails
                                                {
                                                    Id = dr["Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["Id"]),
                                                    GroupName = dr["GroupName"] == DBNull.Value ? String.Empty : dr["GroupName"].ToString(),
                                                    EmailId = dr["EmailId"] == DBNull.Value ? String.Empty : dr["EmailId"].ToString(),

                                                });
                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetEmailGroupDetails");
                Logger.Error(e.StackTrace);

                //data = null;
                // throw;
            }
            catch (Exception e)
            {
                Logger.Error("Unable to GetEmailGroupDetails");
                Logger.Error(e.StackTrace);

                //data = null;
            }
            return emaildGroupDetails;
        }
        public List<UploadedFilesModel> getWorkflowTable()
        {
            List<UploadedFilesModel> data = new List<UploadedFilesModel>();
            int rowCount = 0;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetWorkflowTableData);
                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new UploadedFilesModel
                                  {
                                      File_ID = dr["FileId"].ToString(),
                                      UploadedBy = dr["UploadeBy"].ToString(),
                                      UploadedDate = dr["FileUploadedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FileUploadedDate"]),
                                      Topic = dr["Topic"].ToString(),
                                      Month = dr["Month"].ToString(),
                                      //NoOfAccounts = Convert.ToInt32(dr["NoOfAccount"]),
                                      NoOfAccounts = Convert.ToInt32(dr["NoOfAccount"]),
                                      Attachment = dr["UserFileName"].ToString(),
                                      ServerFileName = dr["ServerFileName"].ToString(),
                                      Notes = dr["Notes"].ToString(),
                                      LastActivityOn = dr["LastActivityOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["LastActivityOn"]),
                                      LastActivityBy = dr["LastActivityBy"] == DBNull.Value ? null : Convert.ToString(dr["LastActivityBy"])
                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getWorkflowTable");
                Logger.Error(e.StackTrace);

                //data = null;
                // throw;
            }
            catch (Exception e)
            {
                Logger.Error("Unable to getWorkflowTable");
                Logger.Error(e.StackTrace);

                //data = null;
            }
            return data;

        }

        #region Add rows to DB
        public bool addRowToDB(MinutesFile dr)
        {
            bool addedRowToDB = false;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertMinutesFile);

                //DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, dr.FileId);
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, GetFileId.fileId);
                DataBase.AddInParameter(dbCommand, "@HeckylId", DbType.String, dr.stringAmukhaID);
                DataBase.AddInParameter(dbCommand, "@ClientName", DbType.String, dr.ClientName);
                DataBase.AddInParameter(dbCommand, "@Segment", DbType.String, dr.Segment);
                DataBase.AddInParameter(dbCommand, "@FileTriggers", DbType.String, dr.FileTriggers);
                DataBase.AddInParameter(dbCommand, "@Region", DbType.String, dr.Region);
                DataBase.AddInParameter(dbCommand, "@InternalRating", DbType.String, dr.InternalRating);
                DataBase.AddInParameter(dbCommand, "@OsInCrs", DbType.Decimal, dr.OsInCrs);
                DataBase.AddInParameter(dbCommand, "@ExposureInCrs", DbType.Decimal, dr.ExposureInCrs);
                DataBase.AddInParameter(dbCommand, "@Strategy", DbType.String, dr.Strategy);
                DataBase.AddInParameter(dbCommand, "@Minutes", DbType.String, dr.Minutes);
                DataBase.AddInParameter(dbCommand, "@MinutesMonth", DbType.DateTime, dr.MinutesMonth);

                var result = ExecuteDataSet(dbCommand, true);

                addedRowToDB = true;
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding minutes row to DB , Error : ", e.Message);
                addedRowToDB = false;
                throw;
            }
            return addedRowToDB;
        }
        public bool addRowToDB(NPAFile dr)
        {
            bool addedRowToDB = false;
            try
            {

                var dbCommand = DataBase.GetStoredProcCommand(procInsertNPAFile);

                //DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, dr.FileId);
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, GetFileId.fileId);
                DataBase.AddInParameter(dbCommand, "@HeckylId", DbType.String, dr.stringAmukhaID);
                DataBase.AddInParameter(dbCommand, "@ClientName", DbType.String, dr.ClientName);
                DataBase.AddInParameter(dbCommand, "@Segment", DbType.String, dr.Segment);
                DataBase.AddInParameter(dbCommand, "@NPADate", DbType.DateTime, dr.NPADate);
                DataBase.AddInParameter(dbCommand, "@NPAMonth", DbType.DateTime, dr.NPAMonth);
                var result = ExecuteDataSet(dbCommand, true);
                addedRowToDB = true;
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding NPA row to DB , Error : ", e.Message);
                addedRowToDB = false;
                throw;
            }
            return addedRowToDB;
        }
        public bool addRowToDB(DataTable ewsFileTable)
        {
            bool addedRowToDB = false;
            try
            {
                using (var dbCommand = DataBase.GetStoredProcCommand(procInsertEWSFile))
                {
                    var parameter = new SqlParameter
                    {
                        ParameterName = "@EwsFile",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.TblType_EwsFile",
                        Value = ewsFileTable
                    };
                    dbCommand.Parameters.Add(parameter);
                    var result = ExecuteDataSet(dbCommand, true);
                    addedRowToDB = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding EWS row to DB , Error : ", e.Message);
                Logger.Error(e.StackTrace);
                addedRowToDB = false;
                throw;
            }
            return addedRowToDB;
        }
        public bool uploadWorkflowUserMapping(DataTable mappingTable, string fileMonth, string storedProcName, string tvpTypeName, string parameterName)
        {
            bool isInserted = false;
            try
            {
                using (var dbCommand = DataBase.GetStoredProcCommand(storedProcName))
                {
                    var mappingParam = new SqlParameter
                    {
                        ParameterName = parameterName,
                        SqlDbType = SqlDbType.Structured,
                        TypeName = tvpTypeName,
                        Value = mappingTable
                    };
                    dbCommand.Parameters.Add(mappingParam);

                    dbCommand.Parameters.Add(new SqlParameter("@FileMonth", SqlDbType.VarChar) { Value = fileMonth });

                    var result = ExecuteDataSet(dbCommand, true);
                    isInserted = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while uploading mapping: " + ex.Message);
                Logger.Error(ex.StackTrace);
                throw;
            }
            return isInserted;
        }
        public bool addRowToDB(PortfolioFile dr)
        {
            bool addedRowToDB = false;
            try
            {

                var dbCommand = DataBase.GetStoredProcCommand(procInsertPortfolioFile);

                //DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, dr.FileId);
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, GetFileId.fileId);
                DataBase.AddInParameter(dbCommand, "@HeckylId", DbType.String, dr.stringAmukhaID);
                DataBase.AddInParameter(dbCommand, "@ClientName", DbType.String, dr.ClientName);
                DataBase.AddInParameter(dbCommand, "@Segment", DbType.String, dr.Segment);
                DataBase.AddInParameter(dbCommand, "@OsInCrs", DbType.Decimal, dr.OsInCrs);
                DataBase.AddInParameter(dbCommand, "@ExposureInCrs", DbType.Decimal, dr.ExposureInCrs);
                DataBase.AddInParameter(dbCommand, "@InternalRating", DbType.String, dr.InternalRating);
                DataBase.AddInParameter(dbCommand, "@PortfolioMonth", DbType.DateTime, dr.PortfolioMonth);
                DataBase.AddInParameter(dbCommand, "@GroupName", DbType.String, dr.GroupName);

                var result = ExecuteDataSet(dbCommand, true);
                addedRowToDB = true;
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding portfolio row to DB , Error : ", e.Message);
                addedRowToDB = false;
                throw;
            }
            return addedRowToDB;
        }
        public bool addRowToDB(HistoricalFile dr)
        {
            bool addedRowToDB = false;
            try
            {

                var dbCommand = DataBase.GetStoredProcCommand(procInsertHistoricalFile);

                //DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, dr.FileId);

                DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, GetFileId.fileId);
                DataBase.AddInParameter(dbCommand, "@HeckylId", DbType.String, dr.stringAmukhaID);
                DataBase.AddInParameter(dbCommand, "@ClientName", DbType.String, dr.ClientName);
                DataBase.AddInParameter(dbCommand, "@Segment", DbType.String, dr.Segment);
                DataBase.AddInParameter(dbCommand, "@Region", DbType.String, dr.Region);
                DataBase.AddInParameter(dbCommand, "@EWSInclusionDate", DbType.DateTime, dr.EWSInclusionDate);
                DataBase.AddInParameter(dbCommand, "@HistoricalMonth", DbType.DateTime, dr.HistoricalMonth);
                DataBase.AddInParameter(dbCommand, "@Category", DbType.String, dr.Category);
                var result = ExecuteDataSet(dbCommand, true);
                addedRowToDB = true;
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding Historical row to DB , Error : ", e.StackTrace);
                addedRowToDB = false;
                throw;
            }
            return addedRowToDB;
        }

        public bool addRowToDB(RFAFraudFile dr)
        {
            bool addedRowToDB = false;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertRFAFraudFile);

                DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, GetFileId.fileId);
                DataBase.AddInParameter(dbCommand, "@Name_as_per_RBI", DbType.String, dr.Name_as_per_RBI);
                DataBase.AddInParameter(dbCommand, "@Bank_Name", DbType.String, dr.Bank_Name);
                DataBase.AddInParameter(dbCommand, "@PAN", DbType.String, dr.PanNo);
                DataBase.AddInParameter(dbCommand, "@Classification", DbType.String, dr.Classification);
                DataBase.AddInParameter(dbCommand, "@CRILC_Date", DbType.DateTime, dr.CRILC_Date);
                DataBase.AddInParameter(dbCommand, "@RFAFraud_Date", DbType.DateTime, dr.RFAFraud_Date);
                DataBase.AddInParameter(dbCommand, "@MatchWithRBLCustomerList", DbType.String, dr.MatchWithRBLCustomerList);
                DataBase.AddInParameter(dbCommand, "@Cif_id", DbType.String, dr.Cif_id);
                DataBase.AddInParameter(dbCommand, "@Customer_name_as_per_exposure_report", DbType.String, dr.Customer_name_as_per_exposure_report);
                DataBase.AddInParameter(dbCommand, "@business_segment_code", DbType.String, dr.business_segment_code);
                DataBase.AddInParameter(dbCommand, "@business_segment_desc", DbType.String, dr.business_segment_desc);
                DataBase.AddInParameter(dbCommand, "@borrower_status", DbType.String, dr.borrower_status);
                DataBase.AddInParameter(dbCommand, "@gross_total_exposure", DbType.Double, dr.gross_total_exposure);
                DataBase.AddInParameter(dbCommand, "@Email_id", DbType.String, dr.Email_id);

                var result = ExecuteDataSet(dbCommand, true);
                addedRowToDB = true;
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding EWS row to DB , Error : ", e.Message);
                Logger.Error(e.StackTrace);
                addedRowToDB = false;
                throw;
            }
            return addedRowToDB;
        }

        #endregion

        #region GetFiles for View Tab
        public List<NPAFile> GetNPAFile(string fileMonth = null)
        {
            //    if (fileMonth == "null")
            //        fileMonth = null;

            List<NPAFile> data = new List<NPAFile>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetNPAFile);

                DataBase.AddInParameter(dbCommand, "@FileMonth ", DbType.String, fileMonth);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new NPAFile
                                  {
                                      FileId = Convert.ToInt32(dr["FileId"]),
                                      //HeckylID = Convert.(dr["HeckylId"]),
                                      stringAmukhaID = dr["HeckylId"].ToString(),
                                      ClientName = dr["ClientName"].ToString(),
                                      Segment = dr["Segment"].GetType() == typeof(DBNull) ? null : dr["Segment"].ToString(),
                                      NPADate = dr["NPADate"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["NPADate"],
                                      NPAMonth = dr["NPAMonth"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["NPAMonth"]
                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getNPAFile");
                Logger.Error(e.StackTrace);
                data = null;
                // throw;
            }
            return data;


        }
        public List<EWSFile> GetEWSFile(string fileMonth)
        {
            List<EWSFile> data = new List<EWSFile>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetEWSFile);

                DataBase.AddInParameter(dbCommand, "@FileMonth ", DbType.String, fileMonth);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new EWSFile
                                  {
                                      FileId = Convert.ToInt32(dr["FileId"]),
                                      AmukhaId = dr["AmukhaId"].ToString(),
                                      UCICID = dr["UCICID"].ToString(),
                                      AccountName = dr["AccountName"].ToString(),
                                      LevelOneBusiness = dr["LevelOneBusiness"].ToString(),
                                      LevelTwoBusiness = dr["LevelTwoBusiness"].ToString(),
                                      LevelOneCredit = dr["LevelOneCredit"].ToString(),
                                      LevelTwoCredit = dr["LevelTwoCredit"].ToString(),
                                      EWSMonth = dr["EWSMonth"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["EWSMonth"],
                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getEWSFile");
                Logger.Error(e.StackTrace);
                data = null;
                // throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to getEWSFile");
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
                data = null;
            }
            return data;


        }

        public List<PortfolioFile> GetPortfolioFile(string fileMonth)
        {
            List<PortfolioFile> data = new List<PortfolioFile>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetPortfolioFile);

                DataBase.AddInParameter(dbCommand, "@FileMonth ", DbType.String, fileMonth);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new PortfolioFile
                                  {
                                      FileId = Convert.ToInt32(dr["FileId"]),
                                      //HeckylID = Convert.ToInt32(dr["HeckylId"]),
                                      stringAmukhaID = dr["HeckylId"].ToString(),
                                      ClientName = dr["ClientName"].ToString(),
                                      Segment = dr["Segment"].GetType() == typeof(DBNull) ? null : dr["Segment"].ToString(),
                                      OsInCrs = dr["OsInCrs"].GetType() == typeof(DBNull) ? (Decimal?)null : Convert.ToDecimal((dr["OsInCrs"])),
                                      ExposureInCrs = dr["ExposureInCrs"].GetType() == typeof(DBNull) ? (Decimal?)null : Convert.ToDecimal((dr["ExposureInCrs"])),
                                      InternalRating = dr["InternalRating"].GetType() == typeof(DBNull) ? null : dr["InternalRating"].ToString(),
                                      PortfolioMonth = dr["PortfolioMonth"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["PortfolioMonth"]
                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getPortfolioFile");
                Logger.Error(e.StackTrace);
                data = null;
                //throw;
            }
            return data;


        }
        public List<MinutesFile> GetMinutesFile(string fileMonth)
        {
            List<MinutesFile> data = new List<MinutesFile>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetMinutesFile);

                DataBase.AddInParameter(dbCommand, "@FileMonth ", DbType.String, fileMonth);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new MinutesFile
                                  {
                                      Id = Convert.ToInt32(dr["Id"]),
                                      FileId = Convert.ToInt32(dr["FileId"]),
                                      //HeckylID = Convert.ToInt32(dr["HeckylId"]),
                                      stringAmukhaID = dr["HeckylId"].ToString(),
                                      ClientName = dr["ClientName"].ToString(),
                                      Segment = dr["Segment"].GetType() == typeof(DBNull) ? null : dr["Segment"].ToString(),
                                      FileTriggers = dr["FileTriggers"].GetType() == typeof(DBNull) ? null : dr["FileTriggers"].ToString(),
                                      GroupName = dr["GroupName"].GetType() == typeof(DBNull) ? null : dr["GroupName"].ToString(),
                                      Region = dr["Region"].GetType() == typeof(DBNull) ? null : dr["Region"].ToString(),
                                      InternalRating = dr["InternalRating"].GetType() == typeof(DBNull) ? null : dr["InternalRating"].ToString(),
                                      OsInCrs = dr["OsInCrs"].GetType() == typeof(DBNull) ? (Decimal?)null : (Convert.ToDecimal(dr["OsInCrs"])),
                                      ExposureInCrs = dr["ExposureInCrs"].GetType() == typeof(DBNull) ? (Decimal?)null : (Convert.ToDecimal(dr["ExposureInCrs"])),
                                      Strategy = dr["Strategy"].GetType() == typeof(DBNull) ? null : dr["Strategy"].ToString(),
                                      Minutes = dr["Minutes"].GetType() == typeof(DBNull) ? null : dr["Minutes"].ToString(),
                                      MinutesMonth = dr["MinutesMonth"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["MinutesMonth"],
                                      Comments = dr["Comments"].GetType() == typeof(DBNull) ? null : dr["Comments"].ToString(),
                                      PrevMonthMinutes = dr["PrevMonthMinutes"].GetType() == typeof(DBNull) ? null : dr["PrevMonthMinutes"].ToString(),
                                      PrevMonthStrategy = dr["PrevMonthStrategy"].GetType() == typeof(DBNull) ? null : dr["PrevMonthStrategy"].ToString(),
                                      FileStatus = dr["FileStatus"].GetType() == typeof(DBNull) ? null : dr["FileStatus"].ToString(),
                                      MinutesModifiedDate = dr["MinutesModifiedDate"].GetType() == typeof(DBNull) ? null : (DateTime?)Convert.ToDateTime(dr["MinutesModifiedDate"]),
                                      FileTriggersModifiedDate = dr["FileTriggersModifiedDate"].GetType() == typeof(DBNull) ? null : (DateTime?)Convert.ToDateTime(dr["FileTriggersModifiedDate"]),
                                      RMName = dr["RMName"].GetType() == typeof(DBNull) ? null : dr["RMName"].ToString(),
                                      MonthObserveEwsMonitor = dr["MonthObserveEWSMonitor"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["MonthObserveEWSMonitor"],
                                      ActionId = dr["ActionId"].GetType() == typeof(DBNull) ? (int?)null : string.IsNullOrWhiteSpace(dr["ActionId"].ToString()) ? (int?)null : (int?)Convert.ToInt32(dr["ActionId"])

                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getMinutesData");
                Logger.Error(e.StackTrace);
                data = null;
                // throw;
            }
            return data;


        }
        public List<HistoricalFile> GetHistoricalFile(string fileMonth)
        {
            List<HistoricalFile> data = new List<HistoricalFile>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetHistoricalFile);

                DataBase.AddInParameter(dbCommand, "@FileMonth ", DbType.String, fileMonth);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new HistoricalFile
                                  {
                                      FileId = Convert.ToInt32(dr["FileId"]),
                                      //HeckylID = Convert.ToInt32(dr["HeckylId"]),
                                      stringAmukhaID = dr["HeckylId"].ToString(),
                                      ClientName = dr["ClientName"].ToString(),
                                      Segment = dr["Segment"].GetType() == typeof(DBNull) ? null : dr["Segment"].ToString(),
                                      Region = dr["Region"].GetType() == typeof(DBNull) ? null : dr["Region"].ToString(),
                                      EWSInclusionDate = dr["EWSInclusionDate"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["EWSInclusionDate"],
                                      HistoricalMonth = dr["HistoricalMonth"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["HistoricalMonth"],
                                      Category = dr["Category"].GetType() == typeof(DBNull) ? null : dr["Category"].ToString(),
                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getWorkflowTable");
                Logger.Error(e.StackTrace);
                data = null;
                // throw;
            }
            return data;


        }

        public List<RFAFraudFile> GetRFAFraudFile(string fileMonth)
        {
            List<RFAFraudFile> data = new List<RFAFraudFile>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetRFAFraudFile);

                DataBase.AddInParameter(dbCommand, "@FileMonth ", DbType.String, fileMonth);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new RFAFraudFile
                                  {
                                      FileId = dr["FileId"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(dr["FileId"]),
                                      Name_as_per_RBI = dr["Name_as_per_RBI"].GetType() == typeof(DBNull) ? string.Empty : dr["Name_as_per_RBI"].ToString(),
                                      Bank_Name = dr["Bank_Name"].GetType() == typeof(DBNull) ? string.Empty : dr["Bank_Name"].ToString(),
                                      PanNo = dr["PAN"].GetType() == typeof(DBNull) ? string.Empty : dr["PAN"].ToString(),
                                      Classification = dr["Classification"].GetType() == typeof(DBNull) ? string.Empty : dr["Classification"].ToString(),
                                      CRILC_Date = dr["CRILC_Date"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["CRILC_Date"],
                                      RFAFraud_Date = dr["RFAFraud_Date"].GetType() == typeof(DBNull) ? (DateTime?)null : (DateTime)dr["RFAFraud_Date"],
                                      MatchWithRBLCustomerList = dr["MatchWithRBLCustomerList"].GetType() == typeof(DBNull) ? string.Empty : dr["MatchWithRBLCustomerList"].ToString(),
                                      Cif_id = dr["Cif_id"].GetType() == typeof(DBNull) ? string.Empty : dr["Cif_id"].ToString(),
                                      Customer_name_as_per_exposure_report = dr["Customer_name_as_per_exposure_report"].GetType() == typeof(DBNull) ? string.Empty : dr["Customer_name_as_per_exposure_report"].ToString(),
                                      business_segment_code = dr["business_segment_code"].GetType() == typeof(DBNull) ? string.Empty : dr["business_segment_code"].ToString(),
                                      business_segment_desc = dr["business_segment_desc"].GetType() == typeof(DBNull) ? string.Empty : dr["business_segment_desc"].ToString(),
                                      borrower_status = dr["borrower_status"].GetType() == typeof(DBNull) ? string.Empty : dr["borrower_status"].ToString(),
                                      Email_id = dr["Email_id"].GetType() == typeof(DBNull) ? string.Empty : dr["Email_id"].ToString()
                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getRFAFraudFile");
                Logger.Error(e.StackTrace);
                data = null;
            }
            return data;
        }
        #endregion

        public List<PeriodDdnModel> getViewPeriodDdn(string fileType = null)
        {
            //if (fileType == "Minutes upload")
            //    fileType = "MOM";
            //string fileMonth = "";
            if (fileType == "EWS Latest")
                fileType = "EWS";
            List<PeriodDdnModel> data = new List<PeriodDdnModel>();
            try
            {
                //string type = "EWS";
                var dbCommand = DataBase.GetStoredProcCommand(procgetEWSPeriods);

                DataBase.AddInParameter(dbCommand, "@FileType ", DbType.String, fileType);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new PeriodDdnModel
                                  {

                                      Key = Convert.ToDateTime(dr["Period"]).ToString("yyyy-MM"),
                                      Value = Convert.ToDateTime(dr["Period"]).ToString("yyyy-MM"),

                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to get Period DDN");
                Logger.Error(e.StackTrace);
                data = null;
                // throw;
            }
            return data;


        }
        public List<LOADdnModel> getLOAPeriodDdn()
        {
            List<LOADdnModel> data = new List<LOADdnModel>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procgetLOAPeriods);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new LOADdnModel
                                  {

                                      Month = (dr["Month"]).ToString(),
                                      MonthDate = Convert.ToDateTime(dr["MonthDt"]).ToString("yyyy-MM-dd")

                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to get LOA Period DDN");
                Logger.Error(e.StackTrace);
                data = null;
                // throw;
            }
            return data;


        }
        public bool AddMOMViewComments(int recordId, MOMViewComments obj = null)
        {
            bool addedComments = false;
            //List<MOMViewComments> data = new List<MOMViewComments>();
            try
            {
                //string type = "EWS";

                var dbCommand = DataBase.GetStoredProcCommand(procInsertFileStatusComments);

                DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.String, recordId);

                DataBase.AddInParameter(dbCommand, "@FileId ", DbType.Int32, obj.FileId);

                DataBase.AddInParameter(dbCommand, "@FileType ", DbType.String, obj.FileType);

                DataBase.AddInParameter(dbCommand, "@Userid ", DbType.Guid, Guid.Parse(obj.Userid));

                DataBase.AddInParameter(dbCommand, "@FileStatus ", DbType.String, obj.FileStatus);

                DataBase.AddInParameter(dbCommand, "@Comments ", DbType.String, obj.Comments);

                var result = ExecuteDataSet(dbCommand, true);
                addedComments = true;
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding EWS row to DB , Error : ", e.Message);
                addedComments = false;
                throw;
            }

            return addedComments;


        }
        public List<ExternalRatingResponseModel> getExternalRating(string dimIdList, string fileMonth)
        {
            List<ExternalRatingResponseModel> data = new List<ExternalRatingResponseModel>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procgetExternalRating);

                DataBase.AddInParameter(dbCommand, "@DimValueList ", DbType.String, dimIdList);
                DataBase.AddInParameter(dbCommand, "@FileMonth ", DbType.String, fileMonth);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null)
                {
                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new ExternalRatingResponseModel
                                  {
                                      dimId = Convert.ToInt32(dr["CompanyId"]),
                                      ExternalRating = (dr["ExternalRating"]).ToString(),

                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to get External Rating");
                Logger.Error(e.StackTrace);
                data = null;
                // throw;
            }
            return data;


        }
        public bool AddCommentsTrailAttachment(int commentId, string CDNFileName, string UserFileName, Guid commentedBy, int attachmentOrder)
        {
            bool isCommentAdded = false;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsert_FileCommentAttachment);

                DataBase.AddInParameter(dbCommand, "@CommentId", DbType.Int32, Convert.ToInt32(commentId));
                DataBase.AddInParameter(dbCommand, "@CDNFileName", DbType.String, CDNFileName);
                DataBase.AddInParameter(dbCommand, "@UserFileName", DbType.String, UserFileName);
                DataBase.AddInParameter(dbCommand, "@CommentedBy", DbType.Guid, commentedBy);
                DataBase.AddInParameter(dbCommand, "@AttachementsOrder", DbType.Int32, attachmentOrder);

                var ds = ExecuteDataSet(dbCommand, true);
                isCommentAdded = true;
            }
            catch (Exception e)
            {
                isCommentAdded = false;
                Logger.Error("Failed to insert attachment details to table - ", e.Message);
            }

            return isCommentAdded;

        }
        public List<PopupVersion> GetVersiondetails(int? RecordId, string Type)
        {
            List<PopupVersion> popupverisonData = new List<PopupVersion>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetMinutesFileTextCommentVersions);

                DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.Int32, RecordId);
                DataBase.AddInParameter(dbCommand, "@Type ", DbType.String, Type);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {

                    popupverisonData.AddRange(from DataRow dr in ds.Tables[0].Rows
                                              select new PopupVersion
                                              {
                                                  UserId = dr["UserId"] == DBNull.Value ? (Guid?)null : (Guid)dr["UserId"],
                                                  CreatedDate = dr["CreatedDate"] == DBNull.Value ? Convert.ToDateTime(DBNull.Value) : Convert.ToDateTime(dr["CreatedDate"]),
                                                  RecordText = dr["RecordText"] == DBNull.Value ? String.Empty : dr["RecordText"].ToString(),
                                                  RecordComments = dr["RecordComments"] == DBNull.Value ? String.Empty : dr["RecordComments"].ToString(),
                                                  VersionNumber = dr["VersionNumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["VersionNumber"]),
                                              });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to getPopupVersioncomments");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                popupverisonData = null;
                //throw;
            }
            return popupverisonData;


        }
        public List<MOMViewComments> getCommentsTrail(int recordId, int fileId)
        {
            List<MOMViewComments> data = new List<MOMViewComments>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetFileCommentsTrail);

                DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.Int32, recordId);
                DataBase.AddInParameter(dbCommand, "@FileId ", DbType.Int32, fileId);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {

                    data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                  select new MOMViewComments
                                  {
                                      Comments = dr["Comments"] == DBNull.Value ? null : Convert.ToString(dr["Comments"]),
                                      CreatedDate = dr["CreatedDate"] == DBNull.Value ? Convert.ToDateTime(DBNull.Value) : Convert.ToDateTime(dr["CreatedDate"]),

                                      FileType = dr["FileType"] == DBNull.Value ? null : dr["FileType"].ToString(),
                                      Userid = dr["UserId"] == DBNull.Value ? null : dr["UserId"].ToString(),
                                      UserFileName = dr["UserFileName"] == DBNull.Value ? null : dr["UserFileName"].ToString(),
                                      CommentedBy = dr["CommentedBy"] == DBNull.Value ? (Guid?)null : (Guid)dr["CommentedBy"],
                                      AttachmentOrder = dr["AttachementsOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AttachementsOrder"]),
                                      AttachmentCreatedDate = dr["AttachementsCreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AttachementsCreatedDate"])
                                  });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to get file commentsTrail");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                data = null;
                //throw;
            }
            return data;


        }
        public List<SlippageReport> GetSlippageReport(DateTime? FromMonth, DateTime? ToMonth)
        {
            List<SlippageReport> slippageReports = new List<SlippageReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetSlippageReport);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@FromMonth ", DbType.DateTime, FromMonth);
                    DataBase.AddInParameter(dbCommand, "@ToMonth ", DbType.DateTime, ToMonth);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        //slippageReports.AddRange(from DataRow dr in ds.Tables[0].Rows
                        //              select new SlippageReport
                        //              {
                        //                  Tag = dr["Tag"] == DBNull.Value ? string.Empty : dr["Tag"].ToString(),
                        //                  Movement = dr["Movement"] == DBNull.Value ? string.Empty : dr["Movement"].ToString(),
                        //                  AvgPercentOutput = dr["AvgPercentOutput"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["AvgPercentOutput"]),
                        //                  EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["EWSMonth"]),
                        //                  PercentOutput = dr["PercentOutput"] == DBNull.Value ? -1 : Convert.ToDecimal(dr["PercentOutput"]),
                        //              });

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DateTime? EWSMonth = dr["EWSMonth"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["EWSMonth"]);
                            slippageReports.Add(new SlippageReport
                            {
                                Tag = dr["Tag"] == DBNull.Value ? string.Empty : dr["Tag"].ToString(),
                                Movement = dr["Movement"] == DBNull.Value ? string.Empty : dr["Movement"].ToString(),
                                AvgPercentOutput = dr["AvgPercentOutput"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["AvgPercentOutput"]),
                                EWSMonth = EWSMonth,
                                PercentOutput = dr["PercentOutput"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["PercentOutput"]),
                            });
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return slippageReports;
            }

        }
        public List<FileAccountName> GetFileAccountName(string FileType)
        {
            List<FileAccountName> fileAccounts = new List<FileAccountName>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetFileAccountName);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@FileType", DbType.String, FileType);

                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        fileAccounts.AddRange(from DataRow dr in ds.Tables[0].Rows
                                              select new FileAccountName
                                              {
                                                  Value = dr["HeckylId"] == DBNull.Value ? string.Empty : dr["HeckylId"].ToString(),
                                                  Label = dr["AccountName"] == DBNull.Value ? string.Empty : dr["AccountName"].ToString(),
                                              });
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return fileAccounts;
            }
        }

        public Dictionary<string, ListTrails> GetListCommentTrails(int? RecordId, int? FileId)
        {
            Dictionary<string, ListTrails> auditListComments = new Dictionary<string, ListTrails>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetListCommentTrails);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, FileId);
                    DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, RecordId);

                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Attachement attach_ = new Attachement
                            {
                                CDNFileName = dr["CDNFileName"] == DBNull.Value ? string.Empty : dr["CDNFileName"].ToString(),
                                UserFileName = dr["UserFileName"] == DBNull.Value ? string.Empty : dr["UserFileName"].ToString(),
                                AttachementOrder = dr["AttachementsOrder"] == DBNull.Value ? string.Empty : dr["AttachementsOrder"].ToString(),
                            };
                            string Comments = dr["Comments"] == DBNull.Value ? string.Empty : dr["Comments"].ToString();
                            string CommentedBy = dr["CommentedBy"] == DBNull.Value ? string.Empty : dr["CommentedBy"].ToString();
                            DateTime? CommentedOn = dr["CommentedOn"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["CommentedOn"]);

                            DateTime? AttachementCreatedOn = dr["AttachementCreatedOn"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["AttachementCreatedOn"]);

                            string dict_key = CommentedBy + "_" + CommentedOn.Value.FromEpoch().ToString();
                            if (!auditListComments.ContainsKey(dict_key))
                            {
                                auditListComments.Add(dict_key, new ListTrails
                                {
                                    CommentedBy = CommentedBy,
                                    CommentedOn = CommentedOn,
                                    Comments = Comments,
                                    AttachementCreatedOn = AttachementCreatedOn,
                                    Attachements = new List<Attachement> { attach_ }
                                });
                            }
                            else
                            {
                                auditListComments[dict_key].Attachements.Add(attach_);
                            }

                        }

                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return auditListComments;
            }
        }

        public Dictionary<string, ListRemarks> GetListRemarkTrails(int? RecordId, int? FileId)
        {
            Dictionary<string, ListRemarks> auditListRemarks = new Dictionary<string, ListRemarks>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetListRemarkTrails);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, FileId);
                    DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, RecordId);

                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Attachement attach_ = new Attachement
                            {
                                CDNFileName = dr["CDNFileName"] == DBNull.Value ? string.Empty : dr["CDNFileName"].ToString(),
                                UserFileName = dr["UserFileName"] == DBNull.Value ? string.Empty : dr["UserFileName"].ToString(),
                                AttachementOrder = dr["AttachmentsOrder"] == DBNull.Value ? string.Empty : dr["AttachmentsOrder"].ToString(),
                            };
                            string SubmittedByUserId = dr["SubmittedByUserId"] == DBNull.Value ? string.Empty : dr["SubmittedByUserId"].ToString();
                            string SubmittedToUserId = dr["SubmittedToUserId"] == DBNull.Value ? string.Empty : dr["SubmittedToUserId"].ToString();
                            string Remarks = dr["Remarks"] == DBNull.Value ? string.Empty : dr["Remarks"].ToString();

                            DateTime? SubmissionDate = dr["SubmissionDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["SubmissionDate"]);
                            string RFAConcern = dr["RFAConcern"] == DBNull.Value ? string.Empty : dr["RFAConcern"].ToString();
                            string dictKey = SubmittedByUserId + "_" + SubmissionDate.Value.FromEpoch().ToString();
                            if (!auditListRemarks.ContainsKey(dictKey))
                            {
                                auditListRemarks.Add(dictKey,
                                                    new ListRemarks
                                                    {
                                                        SubmittedByUserId = SubmittedByUserId,
                                                        SubmittedToUserId = SubmittedToUserId,
                                                        Remarks = Remarks,
                                                        RFAConcern = RFAConcern,
                                                        SubmissionDate = SubmissionDate,
                                                        Attachements = new List<Attachement> { attach_ }
                                                    });
                            }
                            else
                            {
                                auditListRemarks[dictKey].Attachements.Add(attach_);
                            }
                        }

                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return auditListRemarks;
            }
        }
        public List<MigrationReport> GetMigrationReport(DateTime? FromDate, DateTime? ToDate)
        {
            List<MigrationReport> migrationReports = new List<MigrationReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procMigrationReport);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@FromDate", DbType.DateTime, FromDate);
                    DataBase.AddInParameter(dbCommand, "@ToDate", DbType.DateTime, ToDate);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        migrationReports.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                  select new MigrationReport
                                                  {
                                                      PreviousCategory = dr["PrevCategory"] == DBNull.Value ? string.Empty : dr["PrevCategory"].ToString(),
                                                      Category = dr["Category"] == DBNull.Value ? string.Empty : dr["Category"].ToString(),
                                                      MovementRemark = dr["MovementRemark"] == DBNull.Value ? string.Empty : dr["MovementRemark"].ToString(),
                                                      AvgMonthPrevCategory = dr["AvgMonthPrevCategory"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["AvgMonthPrevCategory"]),
                                                      MaxMonthPrevCategory = dr["MaxMonthPrevCategory"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["MaxMonthPrevCategory"]),
                                                  });
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return migrationReports;
            }

        }
        public List<EwsFileDataForAlertReport> GetEwsFileDataForAlertReport(DateTime? FromDate, DateTime? ToDate)
        {
            List<EwsFileDataForAlertReport> migrationReports = new List<EwsFileDataForAlertReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procEwsFileDataForAlertReport);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@FromDate", DbType.DateTime, FromDate);
                    DataBase.AddInParameter(dbCommand, "@ToDate", DbType.DateTime, ToDate);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        migrationReports.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                  select new EwsFileDataForAlertReport
                                                  {
                                                      HeckylIdentifier = dr["HeckylIdentifier"] == DBNull.Value ? 0 : Convert.ToInt32(dr["HeckylIdentifier"]),
                                                      Segment = dr["Segment"] == DBNull.Value ? string.Empty : dr["Segment"].ToString(),
                                                      EWSAnalystName = dr["EWSAnalystName"] == DBNull.Value ? string.Empty : dr["EWSAnalystName"].ToString(),
                                                      Category = dr["Category"] == DBNull.Value ? string.Empty : dr["Category"].ToString(),
                                                      InternalRating = dr["InternalRating"] == DBNull.Value ? string.Empty : dr["InternalRating"].ToString(),
                                                      EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EWSMonth"]),
                                                  });
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return migrationReports;
            }

        }
        public List<PortfolioAuditReport> GetPortfolioLevelEwsFileWithRMComments(string EwsMonth, string AccountId = null)
        {
            List<PortfolioAuditReport> migrationReports = new List<PortfolioAuditReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procPortfolioLevelEwsFileWithRMComments);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@EwsMonth", DbType.String, EwsMonth);
                    if (AccountId != null)
                        DataBase.AddInParameter(dbCommand, "@AccountId", DbType.String, AccountId);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        migrationReports.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                  select new PortfolioAuditReport
                                                  {
                                                      AmukhaID = dr["HeckylId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["HeckylId"]),
                                                      TrgNo = dr["TrgNo"] == DBNull.Value ? string.Empty : dr["TrgNo"].ToString(),
                                                      EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EWSMonth"]),
                                                      RMName = dr["RMUserId"] == DBNull.Value ? string.Empty : dr["RMUserId"].ToString(),
                                                      RMComment = dr["RMComments"] == DBNull.Value ? string.Empty : dr["RMComments"].ToString(),
                                                      RMDateTimeStamp = dr["RMCommentsDt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["RMCommentsDt"]),
                                                  });
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return migrationReports;
            }

        }
        public List<PortfolioAuditReport> GetPortfolioLevelEwsFileWithUserComments(string EwsMonth, string AccountId = null)
        {
            List<PortfolioAuditReport> migrationReports = new List<PortfolioAuditReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procPortfolioLevelEwsFileWithUserComments);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@EwsMonth", DbType.String, EwsMonth);
                    if (AccountId != null)
                        DataBase.AddInParameter(dbCommand, "@AccountId", DbType.String, AccountId);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        migrationReports.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                  select new PortfolioAuditReport
                                                  {
                                                      AmukhaID = dr["HeckylId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["HeckylId"]),
                                                      NameOfAccount = dr["AccountName"] == DBNull.Value ? string.Empty : dr["AccountName"].ToString(),
                                                      Segment = dr["Segment"] == DBNull.Value ? string.Empty : dr["Segment"].ToString(),
                                                      Category = dr["Category"] == DBNull.Value ? string.Empty : dr["Category"].ToString(),
                                                      EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EWSMonth"]),
                                                      Status = dr["Status"] == DBNull.Value ? string.Empty : dr["Status"].ToString(),
                                                      AssignedDate = dr["AssignedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["AssignedDate"]),
                                                      Action = dr["Action"] == DBNull.Value ? string.Empty : dr["Action"].ToString(),
                                                      RFAConcernYN = dr["RFAConcern"] == DBNull.Value ? string.Empty : dr["RFAConcern"].ToString(),
                                                      ZBHName = dr["ZBHName"] == DBNull.Value ? string.Empty : dr["ZBHName"].ToString(),
                                                      RCHName = dr["RCHName"] == DBNull.Value ? string.Empty : dr["RCHName"].ToString(),
                                                      RMName = dr["RMName"] == DBNull.Value ? string.Empty : dr["RMName"].ToString(),
                                                      SupervisorName = dr["SupervisorName"] == DBNull.Value ? string.Empty : dr["SupervisorName"].ToString(),
                                                      SupervisorComment = dr["SupervisorComments"] == DBNull.Value ? string.Empty : dr["SupervisorComments"].ToString(),
                                                      SupervisorDateTimeStamp = dr["SupervisorCommentsDt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["SupervisorCommentsDt"]),
                                                      CreditAnalystName = dr["CreditAnalystName"] == DBNull.Value ? string.Empty : dr["CreditAnalystName"].ToString(),
                                                      CreditAnalystComment = dr["CAComments"] == DBNull.Value ? string.Empty : dr["CAComments"].ToString(),
                                                      CreditAnalystDateTimeStamp = dr["CACommentsDt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["CACommentsDt"]),
                                                      EWSAnalystName = dr["EWSAnalystName"] == DBNull.Value ? string.Empty : dr["EWSAnalystName"].ToString(),
                                                      EWSAnalystComment = dr["EWSAnalystComments"] == DBNull.Value ? string.Empty : dr["EWSAnalystComments"].ToString(),
                                                      EWSAnalystDateTimeStamp = dr["EWSAnalystCommentsDt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EWSAnalystCommentsDt"]),
                                                  });
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return migrationReports;
            }

        }


        public List<EWSSummaryReport> GetEwsSummaryReport(string ToMonth, int? ThreshHold)
        {
            List<EWSSummaryReport> ewsSummaryReports = new List<EWSSummaryReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetEWSSummaryReport);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@ToMonth", DbType.String, ToMonth);
                    DataBase.AddInParameter(dbCommand, "@ThreshHold", DbType.Int32, ThreshHold);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        ewsSummaryReports.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                   select new EWSSummaryReport
                                                   {
                                                       SrNo = dr["SrNo"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["SrNo"]),
                                                       Particulars = dr["Perticulars"] == DBNull.Value ? string.Empty : dr["Perticulars"].ToString(),
                                                       Category = dr["Category"] == DBNull.Value ? string.Empty : dr["Category"].ToString(),
                                                       TotalOsInCrs = dr["TotalOsInCrs"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["TotalOsInCrs"]),
                                                   });
                    }

                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return ewsSummaryReports;
            }

        }
        public List<EWSReportsFile> GetWorkflowReports(int? ParameterId, string ToMonth, int? ThreshHold, int? HeckylId)
        {
            List<EWSReportsFile> data = new List<EWSReportsFile>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetWorkflowPredifinedReports);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@ParameterId", DbType.Int32, ParameterId);
                    DataBase.AddInParameter(dbCommand, "@ToMonth", DbType.String, ToMonth);
                    DataBase.AddInParameter(dbCommand, "@ThreshHold", DbType.Int32, ThreshHold);
                    DataBase.AddInParameter(dbCommand, "@HeckylIdentifier", DbType.Int32, HeckylId);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        data.AddRange(from DataRow dr in ds.Tables[0].Rows
                                      select new EWSReportsFile
                                      {
                                          Id = dr["Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["Id"]),
                                          HeckylId = dr["HeckylId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["HeckylId"]),
                                          ClientName = dr["ClientName"] == DBNull.Value ? string.Empty : dr["ClientName"].ToString(),
                                          GroupName = dr["GroupName"] == DBNull.Value ? string.Empty : dr["GroupName"].ToString(),
                                          Segment = dr["Segment"] == DBNull.Value ? string.Empty : dr["Segment"].ToString(),
                                          Region = dr["Region"] == DBNull.Value ? string.Empty : dr["Region"].ToString(),
                                          EWSMonthTimeOfInclWLAL = ParameterId == 9 ? dr["EWSMonthTimeOfIncl_WL_AL"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["EWSMonthTimeOfIncl_WL_AL"]) : null,
                                          Category = ParameterId == 5 || ParameterId == 6 || ParameterId == 8 || ParameterId == 10 ? dr["Category"] == DBNull.Value ? string.Empty : dr["Category"].ToString() : "",
                                          CategoryTimeOfIncl = ParameterId == 5 || ParameterId == 6 ? dr["CategoryTimeOfIncl"] == DBNull.Value ? string.Empty : dr["CategoryTimeOfIncl"].ToString() : "",
                                          ExitEWSMonth = ParameterId == 2 ? dr["ExitEWSMonth"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["ExitEWSMonth"]) : null,
                                          EWSMonth = ParameterId == 3 || ParameterId == 4 || ParameterId == 5 || ParameterId == 6 || ParameterId == 7 || ParameterId == 9 ? dr["EWSMonth"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["EWSMonth"]) : null,
                                          Minutes = ParameterId == 3 || ParameterId == 4 || ParameterId == 7 || ParameterId == 8 || ParameterId == 10 ? dr["Minutes"] == DBNull.Value ? string.Empty : dr["Minutes"].ToString() : "",
                                          DiffInMonths = ParameterId == 4 || ParameterId == 5 || ParameterId == 6 || ParameterId == 9 ? dr["DiffInMonths"] == DBNull.Value ? string.Empty : dr["DiffInMonths"].ToString() : "",
                                          EWSMonthTimeOfIncl = ParameterId == 1 || ParameterId == 2 || ParameterId == 3 || ParameterId == 4 || ParameterId == 5 || ParameterId == 6 || ParameterId == 9 || ParameterId == 10 ? dr["EWSMonthTimeOfIncl"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["EWSMonthTimeOfIncl"]) : null,
                                          FileTriggers = ParameterId == 1 || ParameterId == 3 || ParameterId == 4 || ParameterId == 7 || ParameterId == 8 || ParameterId == 10 ? dr["FileTriggers"] == DBNull.Value ? string.Empty : dr["FileTriggers"].ToString() : "",
                                          OsInCrs = ParameterId == 1 || ParameterId == 3 || ParameterId == 4 || ParameterId == 5 || ParameterId == 6 || ParameterId == 7 || ParameterId == 8 ? dr["OsInCrs"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrs"]) : 0,
                                          ExposureInCrs = ParameterId == 1 || ParameterId == 3 || ParameterId == 4 || ParameterId == 5 || ParameterId == 6 || ParameterId == 7 || ParameterId == 8 ? dr["ExposureInCrs"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrs"]) : 0,
                                          InternalRating = ParameterId == 1 || ParameterId == 3 || ParameterId == 4 || ParameterId == 5 || ParameterId == 6 || ParameterId == 7 || ParameterId == 8 ? dr["InternalRating"] == DBNull.Value ? string.Empty : dr["InternalRating"].ToString() : "",
                                          OsInCrsTimeOfIncl = ParameterId == 1 || ParameterId == 2 || ParameterId == 3 || ParameterId == 4 ? dr["OsInCrsTimeOfIncl"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrsTimeOfIncl"]) : 0,
                                          ExposureInCrsTimeOfIncl = ParameterId == 1 || ParameterId == 2 || ParameterId == 3 || ParameterId == 4 ? dr["ExposureInCrsTimeOfIncl"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrsTimeOfIncl"]) : 0,
                                          InternalRatingTimeOfIncl = ParameterId == 1 || ParameterId == 2 || ParameterId == 3 || ParameterId == 4 ? dr["InternalRatingTimeOfIncl"] == DBNull.Value ? string.Empty : dr["InternalRatingTimeOfIncl"].ToString() : "",
                                          MonthTwoCategory = ParameterId == 10 ? dr["Month2Category"] == DBNull.Value ? string.Empty : dr["Month2Category"].ToString() : "",
                                          MonthThreeCategory = ParameterId == 10 ? dr["Month3Category"] == DBNull.Value ? string.Empty : dr["Month3Category"].ToString() : "",
                                          MonthTwoMinutes = ParameterId == 10 ? dr["Month2Minutes"] == DBNull.Value ? string.Empty : dr["Month2Minutes"].ToString() : "",
                                          MonthThreeMinutes = ParameterId == 10 ? dr["Month3Minutes"] == DBNull.Value ? string.Empty : dr["Month3Minutes"].ToString() : "",
                                          MonthMovementToWlAl = ParameterId == 10 ? dr["MonthMovementToWlAl"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["MonthMovementToWlAl"]) : null,
                                          NPADate = ParameterId == 10 ? dr["NPADate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["NPADate"]) : null,
                                      });

                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return data;


        }
        public List<CustomVariblesReports> GetCustomReports(string FilterClause, string SelectClause, string Priod, string PriodValue)
        {
            List<CustomVariblesReports> customReports = new List<CustomVariblesReports>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procCustomReport);
                string tmp = FilterClause.Replace("'", "~"); ;
                try
                {
                    DataBase.AddInParameter(dbCommand, "@FilterClause ", DbType.String, tmp);
                    DataBase.AddInParameter(dbCommand, "@SelectClause ", DbType.String, SelectClause);
                    DataBase.AddInParameter(dbCommand, "@Priod ", DbType.String, Priod);
                    DataBase.AddInParameter(dbCommand, "@PriodValue ", DbType.String, PriodValue);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        var table = ds.Tables[0].Columns;
                        customReports.AddRange(from DataRow dr in ds.Tables[0].Rows
                                               select new CustomVariblesReports
                                               {
                                                   HeckylId = dr["HeckylId"] == DBNull.Value ? string.Empty : dr["HeckylId"].ToString(),
                                                   ClientName = dr["ClientName"] == DBNull.Value ? string.Empty : dr["ClientName"].ToString(),
                                                   EWSMonth = dr["EWSMonth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EWSMonth"]),
                                                   Segment = dr["Segment"] == DBNull.Value ? string.Empty : dr["Segment"].ToString(),
                                                   Region = dr["Region"] == DBNull.Value ? string.Empty : dr["Region"].ToString(),
                                                   Category = dr["Category"] == DBNull.Value ? string.Empty : dr["Category"].ToString(),
                                                   InternalRating = dr["InternalRating"] == DBNull.Value ? string.Empty : dr["InternalRating"].ToString(),
                                                   OsInCrs = dr["OsInCrs"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrs"]),
                                                   ExposureInCrs = dr["ExposureInCrs"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrs"]),

                                                   FileTriggers = table.Contains("FileTriggers") ? dr["FileTriggers"] == DBNull.Value ? string.Empty : dr["FileTriggers"].ToString() : null,
                                                   Minutes = table.Contains("Minutes") ? dr["Minutes"] == DBNull.Value ? string.Empty : dr["Minutes"].ToString() : null,
                                                   OsInCrsInCategoryUpgrade = table.Contains("OsInCrsInCategoryUpgrade") ? dr["OsInCrsInCategoryUpgrade"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrsInCategoryUpgrade"]) : null,
                                                   OsInCrsInCategoryDowngrade = table.Contains("OsInCrsInCategoryDowngrade") ? dr["OsInCrsInCategoryDowngrade"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrsInCategoryDowngrade"]) : null,
                                                   OsInCrsInclusionInOME = table.Contains("OsInCrsInclusionInOME") ? dr["OsInCrsInclusionInOME"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrsInclusionInOME"]) : null,
                                                   OsInCrsInclusionInWLAL = table.Contains("OsInCrsInclusionInWLAL") ? dr["OsInCrsInclusionInWLAL"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrsInclusionInWLAL"]) : null,
                                                   OsInCrsInclusionInNPA = table.Contains("OsInCrsInclusionInNPA") ? dr["OsInCrsInclusionInNPA"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["OsInCrsInclusionInNPA"]) : null,
                                                   ExposureInCrsCategoryUpgrade = table.Contains("ExposureInCrsCategoryUpgrade") ? dr["ExposureInCrsCategoryUpgrade"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrsCategoryUpgrade"]) : null,
                                                   ExposureInCrsCategoryDowngrade = table.Contains("ExposureInCrsCategoryDowngrade") ? dr["ExposureInCrsCategoryDowngrade"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrsCategoryDowngrade"]) : null,
                                                   ExposureInCrsInclusionInOME = table.Contains("ExposureInCrsInclusionInOME") ? dr["ExposureInCrsInclusionInOME"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrsInclusionInOME"]) : null,
                                                   ExposureInCrsInclusionInWLAL = table.Contains("ExposureInCrsInclusionInWLAL") ? dr["ExposureInCrsInclusionInWLAL"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrsInclusionInWLAL"]) : null,
                                                   ExposureInCrsInclusionInNPA = table.Contains("ExposureInCrsInclusionInNPA") ? dr["ExposureInCrsInclusionInNPA"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["ExposureInCrsInclusionInNPA"]) : null,
                                                   InternalRatingCategoryUpgrade = table.Contains("InternalRatingCategoryUpgrade") ? dr["InternalRatingCategoryUpgrade"] == DBNull.Value ? string.Empty : dr["InternalRatingCategoryUpgrade"].ToString() : "",
                                                   InternalRatingCategoryDowngrade = table.Contains("InternalRatingCategoryDowngrade") ? dr["InternalRatingCategoryDowngrade"] == DBNull.Value ? string.Empty : dr["InternalRatingCategoryDowngrade"].ToString() : "",
                                                   InternalRatingInclusionInOME = table.Contains("InternalRatingInclusionInOME") ? dr["InternalRatingInclusionInOME"] == DBNull.Value ? string.Empty : dr["InternalRatingInclusionInOME"].ToString() : "",
                                                   InternalRatingInclusionInWLAL = table.Contains("InternalRatingInclusionInWLAL") ? dr["InternalRatingInclusionInWLAL"] == DBNull.Value ? string.Empty : dr["InternalRatingInclusionInWLAL"].ToString() : "",
                                                   InternalRatingInclusionInNPA = table.Contains("InternalRatingInclusionInNPA") ? dr["InternalRatingInclusionInNPA"] == DBNull.Value ? string.Empty : dr["InternalRatingInclusionInNPA"].ToString() : "",
                                                   MinutesCategoryUpgrade = table.Contains("MinutesCategoryUpgrade") ? dr["MinutesCategoryUpgrade"] == DBNull.Value ? string.Empty : dr["MinutesCategoryUpgrade"].ToString() : "",
                                                   MinutesCategoryDowngrade = table.Contains("MinutesCategoryDowngrade") ? dr["MinutesCategoryDowngrade"] == DBNull.Value ? string.Empty : dr["MinutesCategoryDowngrade"].ToString() : "",
                                                   MinutesInclusionInOME = table.Contains("MinutesInclusionInOME") ? dr["MinutesInclusionInOME"] == DBNull.Value ? string.Empty : dr["MinutesInclusionInOME"].ToString() : "",
                                                   MinutesInclusionInWLAL = table.Contains("MinutesInclusionInWLAL") ? dr["MinutesInclusionInWLAL"] == DBNull.Value ? string.Empty : dr["MinutesInclusionInWLAL"].ToString() : "",
                                                   MinutesInclusionInNPA = table.Contains("MinutesInclusionInNPA") ? dr["MinutesInclusionInNPA"] == DBNull.Value ? string.Empty : dr["MinutesInclusionInNPA"].ToString() : "",
                                                   FileTriggersCategoryUpgrade = table.Contains("FileTriggersCategoryUpgrade") ? dr["FileTriggersCategoryUpgrade"] == DBNull.Value ? string.Empty : dr["FileTriggersCategoryUpgrade"].ToString() : "",
                                                   FileTriggersCategoryDowngrade = table.Contains("FileTriggersCategoryDowngrade") ? dr["FileTriggersCategoryDowngrade"] == DBNull.Value ? string.Empty : dr["FileTriggersCategoryDowngrade"].ToString() : "",
                                                   FileTriggersInclusionInOME = table.Contains("FileTriggersInclusionInOME") ? dr["FileTriggersInclusionInOME"] == DBNull.Value ? string.Empty : dr["FileTriggersInclusionInOME"].ToString() : "",
                                                   FileTriggersInclusionInWLAL = table.Contains("FileTriggersInclusionInWLAL") ? dr["FileTriggersInclusionInWLAL"] == DBNull.Value ? string.Empty : dr["FileTriggersInclusionInWLAL"].ToString() : "",
                                                   FileTriggersInclusionInNPA = table.Contains("FileTriggersInclusionInNPA") ? dr["FileTriggersInclusionInNPA"] == DBNull.Value ? string.Empty : dr["FileTriggersInclusionInNPA"].ToString() : "",
                                                   EwsMonthInclusionInOME = table.Contains("EwsMonthInclusionInOME") ? dr["EwsMonthInclusionInOME"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EwsMonthInclusionInOME"]) : null,
                                                   EwsMonthInclusionInWLAL = table.Contains("EwsMonthInclusionInWLAL") ? dr["EwsMonthInclusionInWLAL"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EwsMonthInclusionInWLAL"]) : null,
                                                   EwsMonthInclusionInNPA = table.Contains("EwsMonthInclusionInNPA") ? dr["EwsMonthInclusionInNPA"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EwsMonthInclusionInNPA"]) : null,
                                                   EwsMonthExisted = table.Contains("EwsMonthExisted") ? dr["EwsMonthExisted"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EwsMonthExisted"]) : null,
                                                   EwsMonthCategoryUpgrade = table.Contains("EwsMonthCategoryUpgrade") ? dr["EwsMonthCategoryUpgrade"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EwsMonthCategoryUpgrade"]) : null,
                                                   EwsMonthCategoryDowngrade = table.Contains("EwsMonthCategoryDowngrade") ? dr["EwsMonthCategoryDowngrade"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EwsMonthCategoryDowngrade"]) : null,
                                                   NPADate = table.Contains("NPADate") ? dr["NPADate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["NPADate"]) : null,

                                               });

                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
                return customReports;
            }


        }

        public List<CustomReportVariable> GetCustomReportVaribles()
        {
            List<CustomReportVariable> data = new List<CustomReportVariable>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetCustomReportVaribles);
                try
                {
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            data.Add(new CustomReportVariable
                            {
                                DBColumnName = dr["DBColumnName"] == DBNull.Value ? string.Empty : dr["DBColumnName"].ToString(),
                                DisplayName = dr["DisplayName"] == DBNull.Value ? string.Empty : dr["DisplayName"].ToString(),
                                showInCustomReportPopup = (dr["DBColumnName"].ToString() != "Category" && dr["DBColumnName"].ToString() != "InternalRating" && dr["DBColumnName"].ToString() != "OsInCrs" && dr.ItemArray[0].ToString() != "ExposureInCrs") ? true : false

                            });
                        }

                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return data;
        }
        //updateMOMFileRecord(recordId, userId, minutesText, fileTrggerText);

        public bool updateMOMFileRecord(EditRecordFromMOMModel req)
        {
            bool isAdded = false;
            List<MOMViewComments> data = new List<MOMViewComments>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procUpdateMOMRecordFromView);

                DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.Int32, req.recordId);
                DataBase.AddInParameter(dbCommand, "@UserId ", DbType.Guid, Guid.Parse(req.userId));
                DataBase.AddInParameter(dbCommand, "@Minutes ", DbType.String, req.minutesText);
                DataBase.AddInParameter(dbCommand, "@FileTriggers ", DbType.String, req.fileTriggerText);

                var ds = ExecuteDataSet(dbCommand, true);
                isAdded = true;
            }
            catch (SqlException e)
            {

                isAdded = false;
                Logger.Error("Unable to update MOM record");
                Logger.Error(e.StackTrace);
                //throw;
            }
            return isAdded;

        }
        public int LogCaseTrf(LogCaseTrf req, DataTable dataTable)
        {
            int ticketID = 0;
            List<LogCaseTrf> data = new List<LogCaseTrf>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                SqlConnection connection = new SqlConnection(DataBase.ConnectionString);
                SqlCommand cmd = new SqlCommand(procCaseTrfLog, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    SqlParameter parameter = new SqlParameter();
                    cmd.Parameters.AddWithValue("@AccountMappingUpdate", dataTable);
                    cmd.Parameters.AddWithValue("@ToUserId", req.ToUserId);
                    cmd.Parameters.AddWithValue("@Role", req.userRole);
                    cmd.Parameters.AddWithValue("@Comments", req.Comments);
                    cmd.Parameters.AddWithValue("@UserName", req.UserName);
                    cmd.Parameters.AddWithValue("@CaseTrfBy", req.CaseTrfBy);
                    cmd.Parameters.AddWithValue("@AccountTrfCount", Convert.ToInt32(req.AccountTrfCount));
                    cmd.Parameters.Add("@TcktId", SqlDbType.Int, 500);
                    cmd.Parameters["@TcktId"].Direction = ParameterDirection.Output;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    ticketID = (int)cmd.Parameters["@TcktId"].Value;
                    connection.Close();
                }
                catch (SqlException e)
                {
                    ticketID = 0;
                    Logger.Error("Unable to log case transfer from WorkflowActionPage");
                    Logger.Error(e);
                    //throw;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            return ticketID;

        }

        public int LogCaseReopenState(LogCaseReopen obj, DataTable dataTable)
        {
            int ticketID = 0;
            List<LogCaseReopen> data = new List<LogCaseReopen>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                SqlConnection connection = new SqlConnection(DataBase.ConnectionString);
                SqlCommand cmd = new SqlCommand(procCaseReopen, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    SqlParameter parameter = new SqlParameter();
                    cmd.Parameters.AddWithValue("@CaseReopen", dataTable);
                    cmd.Parameters.AddWithValue("@SubmittedByUserId", obj.SubmittedByUserId);
                    cmd.Parameters.AddWithValue("@SubmittedByRole", obj.SubmittedByRole);
                    cmd.Parameters.AddWithValue("@SubmittedToRole", obj.SubmittedToRole);
                    cmd.Parameters.AddWithValue("@Remarks", obj.Remarks);
                    cmd.Parameters.AddWithValue("@AccountTrfCount", Convert.ToInt32(obj.AccountTrfCount));
                    cmd.Parameters.Add("@TcktId", SqlDbType.Int, 500);
                    cmd.Parameters["@TcktId"].Direction = ParameterDirection.Output;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    ticketID = (int)cmd.Parameters["@TcktId"].Value;
                    connection.Close();
                }
                catch (SqlException e)
                {
                    ticketID = 0;
                    Logger.Error("Unable to log case reopen from WorkflowActionPage");
                    Logger.Error(e.StackTrace);
                    //throw;
                }
            }
            return ticketID;

        }
        public bool CheckFileExistsInDB(string period, string fileType)
        {
            bool fileExistsInDb = false;
            //List<MOMViewComments> data = new List<MOMViewComments>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procCheckIfFileExists);

                DataBase.AddInParameter(dbCommand, "@Period ", DbType.String, period);
                DataBase.AddInParameter(dbCommand, "@FileType ", DbType.String, fileType);

                var ds = ExecuteDataSet(dbCommand, true);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        fileExistsInDb = Convert.ToInt32(dr["Cnt"]) > 0 ? true : false;

                    }

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to check CheckFileExistsInDB ");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                fileExistsInDb = false;
                //throw;
            }
            return fileExistsInDb;


        }



        public Dictionary<string, FilterParameter> GetCustomReportParameters()
        {
            Dictionary<string, FilterParameter> data = new Dictionary<string, FilterParameter>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var firstSelectionArray = "Account Type|Outstanding|Exposure|Upgrade|Downgrade".Split('|');
                var dbCommand = DataBase.GetStoredProcCommand(procGetCustomDefinedParametersReports);
                try
                {
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        int counter = 1;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var DbAlias = dr["DbAlias"] == DBNull.Value ? string.Empty : dr["DbAlias"].ToString();
                            var ParameterName = dr["Parameter"] == DBNull.Value ? string.Empty : dr["Parameter"].ToString();

                            if (!data.ContainsKey(ParameterName))
                            {
                                var ParamDataType = dr["ParamDataTypes"] == DBNull.Value ? string.Empty : dr["ParamDataTypes"].ToString();
                                FilterParameter filter = new FilterParameter();
                                filter.ParameterName = ParameterName;
                                filter.DbAlias = DbAlias;
                                filter.ParamDataTypes = ParamDataType;
                                filter.ChildFilterParameter = new List<FilterParameterChild>();
                                filter.FilterType = GetFilterType(filter);
                                filter.Value = counter;
                                data.Add(ParameterName, filter);

                                counter++;

                            }
                            var ParamValues = dr["ParamValues"] == DBNull.Value ? string.Empty : dr["ParamValues"].ToString();
                            if (!string.IsNullOrEmpty(ParamValues))
                            {
                                data[ParameterName].ChildFilterParameter.Add(new FilterParameterChild
                                {
                                    Label = ParamValues,
                                    Value = ParamValues,
                                    IsSelected = firstSelectionArray.Contains(ParameterName) && data[ParameterName].ChildFilterParameter.Count == 0 ? true : false
                                });

                            }
                            if (data[ParameterName].ParamDataTypes == "NUMBER" && ParameterName != "Account")
                            {
                                data[ParameterName].ChildFilterParameter.Add(new FilterParameterChild
                                {
                                    Label = "Greater than or Equal to",
                                    Value = "GTET",
                                    IsSelected = true
                                });
                                data[ParameterName].ChildFilterParameter.Add(new FilterParameterChild
                                {
                                    Label = "Less than or Equal to",
                                    Value = "LTET",
                                    IsSelected = false
                                });
                                data[ParameterName].ChildFilterParameter.Add(new FilterParameterChild
                                {
                                    Label = "Between",
                                    Value = "BTWN",
                                    IsSelected = false
                                });

                            }
                        }


                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return data;


        }


        private FilterType GetFilterType(FilterParameter filter)
        {
            FilterType type = FilterType.FreeText;
            switch (filter.ParameterName)
            {
                case "Period":
                    type = FilterType.DoubleSelectDropdown;
                    break;

                case "Account Type":
                case "Upgrade":
                case "Downgrade":
                case "Account":
                    type = FilterType.SingleSelectDropdown;
                    break;

                case "Segment":
                case "Category":
                case "Internal Rating":
                case "Region":
                    type = FilterType.DoubleSelectDropdownWithCheckBox;
                    break;


                case "Outstanding (INR in Cr)":
                case "Exposure in (INR in Cr)":
                    type = FilterType.SingleSelectDropdownNumericFilter;
                    break;

                case "Change in Category":
                case "Change in Internal Rating":
                    type = FilterType.DoubleDropdownWithCheckBoxANdFromTo;
                    break;

                default:
                    break;
            }

            return type;
        }


        public List<PreDefinedReport> GetPreDefinedReportsData()
        {
            List<PreDefinedReport> data = new List<PreDefinedReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                //var dbCommand = DataBase.GetStoredProcCommand(procGetPreDefinedParametersReports);
                var dbCommand = DataBase.GetStoredProcCommand("UI.Get_PreDefinedParameters_Reports");
                try
                {
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            data.Add(new PreDefinedReport
                            {
                                Name = dr["Parameter"] == DBNull.Value ? string.Empty : dr["Parameter"].ToString(),
                                Value = dr["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dr["Id"].ToString())

                            });
                        }

                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return data;
        }
        public bool UpdateMomFileViewTab(UpdateViewTab updateRequest)
        {
            bool isAdded = false;
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procUpdateMomFileFromViewTab);

                DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, updateRequest.recordId);
                DataBase.AddInParameter(dbCommand, "@UserId", DbType.Guid, Guid.Parse(updateRequest.userId));
                DataBase.AddInParameter(dbCommand, "@Minutes", DbType.String, updateRequest.minutes);
                DataBase.AddInParameter(dbCommand, "@FileTriggers", DbType.String, updateRequest.fileTriggers);
                DataBase.AddInParameter(dbCommand, "@MinutesComments", DbType.String, updateRequest.minutesComments);
                DataBase.AddInParameter(dbCommand, "@FileTriggersComments", DbType.String, updateRequest.fileTriggersComments);

                var ds = ExecuteDataSet(dbCommand, true);
                isAdded = true;
            }
            catch (SqlException e)
            {

                isAdded = false;
                Logger.Error("Unable to update MOM file");
                Logger.Error(e.StackTrace);
                //throw;
            }
            return isAdded;

        }



        public UserActionsAlertDataResponse InsertUserActionsAgainstAlert(UserActionsAgainstAlert userActionsAgainstRequest)
        {
            UserActionsAlertDataResponse response = new UserActionsAlertDataResponse();
            int actionId = -1;
            string ErrorMessage = string.Empty;
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsert_UserActionsAgainstAlert);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, userActionsAgainstRequest.recordId);
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.String, userActionsAgainstRequest.fileId);
                    DataBase.AddInParameter(dbCommand, "@CreatedBy", DbType.Guid, userActionsAgainstRequest.createdBy);
                    DataBase.AddInParameter(dbCommand, "@StatusId", DbType.String, userActionsAgainstRequest.statusId);
                    DataBase.AddInParameter(dbCommand, "@AssignedTo", DbType.Guid, userActionsAgainstRequest.assignedTo);
                    DataBase.AddInParameter(dbCommand, "@Comments", DbType.String, userActionsAgainstRequest.comments);
                    DataBase.AddInParameter(dbCommand, "@IsFirstInsert", DbType.Boolean, userActionsAgainstRequest.isFirstInsert);
                    DataBase.AddOutParameter(dbCommand, "@ActionId", DbType.Int32, -1);
                    DataBase.AddOutParameter(dbCommand, "@ErrorMessage", DbType.String, -1);

                    var ds = ExecuteDataSet(dbCommand, true);
                    actionId = DataBase.GetParameterValue(dbCommand, "@ActionId") != DBNull.Value ? (Int32)DataBase.GetParameterValue(dbCommand, "@ActionId") : -1;
                    ErrorMessage = DataBase.GetParameterValue(dbCommand, "@ErrorMessage") != DBNull.Value ? Convert.ToString(DataBase.GetParameterValue(dbCommand, "@ErrorMessage")) : String.Empty;
                    response.ActionId = actionId;
                    response.ErrorMessage = ErrorMessage;
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return response;

        }

        public UserRecordResponse InserUserActionsComments(UserActionComments userCommentsRequest)
        {
            UserRecordResponse response = new UserRecordResponse();
            int Id = -1;
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertUserRecordIdComments);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, userCommentsRequest.recordId);
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, userCommentsRequest.fileId);
                    DataBase.AddInParameter(dbCommand, "@Comments", DbType.String, userCommentsRequest.Comments);
                    DataBase.AddInParameter(dbCommand, "@CommentedBy", DbType.Guid, userCommentsRequest.commentedBy);
                    DataBase.AddOutParameter(dbCommand, "@ID", DbType.Int32, -1);

                    var ds = ExecuteDataSet(dbCommand, true);
                    Id = DataBase.GetParameterValue(dbCommand, "@ID") != DBNull.Value ? (Int32)DataBase.GetParameterValue(dbCommand, "@ID") : -1;
                    response.Id = Id;

                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return response;
        }
        public List<StatusMaster> GetAuditActionStatusMaster()
        {
            List<StatusMaster> statusMasterList = new List<StatusMaster>();

            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procWorkflowActionStatusMaster);
                try
                {

                    var bsData = ExecuteDataSet(dbCommand, true);
                    if (bsData != null)
                    {
                        statusMasterList.AddRange(from DataRow dr in bsData.Tables[0].Rows
                                                  select new StatusMaster
                                                  {
                                                      Status = dr["Status"] == DBNull.Value ? string.Empty : dr["Status"].ToString(),
                                                      StatusId = dr["StatusId"] == DBNull.Value ? -1 : Convert.ToInt16(dr["StatusId"].ToString()),
                                                      ColorCode = dr["ColourCode"] == DBNull.Value ? string.Empty : dr["ColourCode"].ToString(),
                                                  });

                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return statusMasterList;
        }
        public UserRecordResponse InsertUserRecordAttachements(int commentId, string userFileName, string cdnFileName, int order, Guid CommentedBy)
        {
            UserRecordResponse response = new UserRecordResponse();
            int Id = -1;
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertUserRecordIdAttachements);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@CommentId", DbType.Int32, commentId);
                    DataBase.AddInParameter(dbCommand, "@UserFileName", DbType.String, userFileName);
                    DataBase.AddInParameter(dbCommand, "@CDNFileName", DbType.String, cdnFileName);
                    DataBase.AddInParameter(dbCommand, "@AttachementsOrder", DbType.Int32, order);
                    DataBase.AddInParameter(dbCommand, "@CommentedBy", DbType.Guid, CommentedBy);
                    DataBase.AddOutParameter(dbCommand, "@ID", DbType.Int32, -1);

                    var ds = ExecuteDataSet(dbCommand, true);
                    Id = DataBase.GetParameterValue(dbCommand, "@ID") != DBNull.Value ? (Int32)DataBase.GetParameterValue(dbCommand, "@ID") : -1;
                    response.Id = Id;

                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return response;
        }

        public bool InsertUserActionsAttachement(int actionId, string userFileName, string cdnFileName, int order, Guid CommentedBy)
        {
            bool insertSuccessfull = false;

            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertUserActionsAttachements);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@Action_Id", DbType.Int32, actionId);
                    DataBase.AddInParameter(dbCommand, "@UserFileName", DbType.String, userFileName);
                    DataBase.AddInParameter(dbCommand, "@CDNFileName", DbType.String, cdnFileName);
                    DataBase.AddInParameter(dbCommand, "@AttachementsOrder", DbType.Int32, order);
                    DataBase.AddInParameter(dbCommand, "@CommentedBy", DbType.Guid, CommentedBy);


                    var bsData = ExecuteDataSet(dbCommand, true);

                    insertSuccessfull = true;

                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return insertSuccessfull;
        }

        public List<ActionDetails> GetActionDetailsByRecord(int? recordId, int? fileId)
        {
            List<ActionDetails> actionDetails = new List<ActionDetails>();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetActionDetailsByRecordId);

                DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.Int32, recordId);
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.String, fileId);

                var ds = ExecuteDataSet(dbCommand, true);

                if (ds != null)
                {
                    actionDetails.AddRange(from DataRow dr in ds.Tables[0].Rows
                                           select new ActionDetails
                                           {
                                               Comments = dr["Comments"] == DBNull.Value ? String.Empty : dr["Comments"].ToString(),
                                               CreatedBy = dr["CreatedBy"] == DBNull.Value ? (Guid?)null : (Guid)dr["CreatedBy"],
                                               AssignedTo = dr["AssignedTo"] == DBNull.Value ? (Guid?)null : (Guid)dr["AssignedTo"],
                                               CreatedDate = dr["CreatedDate"] == DBNull.Value ? Convert.ToDateTime(DBNull.Value) : Convert.ToDateTime(dr["CreatedDate"]),
                                               CDNFileName = dr["CDNFileName"] == DBNull.Value ? String.Empty : dr["CDNFileName"].ToString(),
                                               UserFileName = dr["UserFileName"] == DBNull.Value ? String.Empty : dr["UserFileName"].ToString(),
                                               ActionId = dr["ActionId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["ActionId"]),
                                               StatusId = dr["StatusId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["StatusId"]),
                                               CommentId = dr["CommentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["CommentID"]),
                                               IsAction = dr["IsAction"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IsAction"]),
                                               Status = dr["Status"] == DBNull.Value ? String.Empty : dr["Status"].ToString()
                                           });

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetActionDetailsByRecord");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                actionDetails = null;
            }
            return actionDetails;
        }

        public List<RatingDistribution> GetRatingDistribution(string toMonth, int? threshold)
        {
            List<RatingDistribution> ratingDistrubutionReport = new List<RatingDistribution>();
            DataSet generatedReport = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetRatingDistribution);

                DataBase.AddInParameter(dbCommand, "@ToMonth ", DbType.String, toMonth);
                DataBase.AddInParameter(dbCommand, "@ThreshHold", DbType.Int32, threshold);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {
                    ratingDistrubutionReport.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                      select new RatingDistribution
                                                      {
                                                          Rating = dr["InternalRating"] == DBNull.Value ? String.Empty : dr["InternalRating"].ToString(),
                                                          Category = dr["Category"] == DBNull.Value ? String.Empty : dr["Category"].ToString(),

                                                          SumOsInCrs = dr["SumOsInCrs"] == DBNull.Value ? (decimal?)null : (decimal)dr["SumOsInCrs"],
                                                          Percent = dr["Percents"] == DBNull.Value ? (decimal?)null : (decimal)dr["Percents"]
                                                      });

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetRatingDistributionReport");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                ratingDistrubutionReport = null;
            }
            return ratingDistrubutionReport;
        }

        public List<EWSTrendModel> GetEWSTrend(string toMonth, int? threshold)
        {
            List<EWSTrendModel> ewsTrendList = new List<EWSTrendModel>();
            DataSet generatedReport = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetEWSTrendReport);

                DataBase.AddInParameter(dbCommand, "@ToMonth ", DbType.String, toMonth);
                DataBase.AddInParameter(dbCommand, "@ThreshHold", DbType.Int32, threshold);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {
                    ewsTrendList.AddRange(from DataRow dr in ds.Tables[0].Rows
                                          select new EWSTrendModel
                                          {
                                              Category = dr["Category"] == DBNull.Value ? String.Empty : dr["Category"].ToString(),
                                              EWSMonth = dr["EWSMonth"] == DBNull.Value ? String.Empty : dr["EWSMonth"].ToString(),
                                              OsInCrs = dr["OsInCrs"] == DBNull.Value ? (decimal?)null : (decimal?)dr["OsInCrs"],
                                              NoOfAccounts = dr["NoOfAccounts"] == DBNull.Value ? (int?)null : (int?)dr["NoOfAccounts"]
                                          });

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetEWSTrendReport from Repo");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                ewsTrendList = null;
            }
            return ewsTrendList;
        }
        public List<userAccountReportingMaster> GetUserAccountReportingMaster()
        {
            List<userAccountReportingMaster> userList = new List<userAccountReportingMaster>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procgetAccountReportingUser);
                try
                {
                    var userMasterReportData = ExecuteDataSet(dbCommand, true);
                    if (userMasterReportData != null)
                    {
                        userList.AddRange(from DataRow userMasterReportDataRow in userMasterReportData.Tables[0].Rows
                                          select new userAccountReportingMaster
                                          {
                                              HeckylId = userMasterReportDataRow["HeckylId"] == DBNull.Value ? 0 : Convert.ToInt32(userMasterReportDataRow["HeckylId"]),
                                              RMId = userMasterReportDataRow["RMId"] == DBNull.Value ? "" : userMasterReportDataRow["RMId"].ToString(),
                                              RMName = userMasterReportDataRow["RMName"] == DBNull.Value ? "" : userMasterReportDataRow["RMName"].ToString(),
                                              SupervisorId = userMasterReportDataRow["RMId"] == DBNull.Value ? "" : userMasterReportDataRow["SupervisorId"].ToString(),
                                              SupervisorName = userMasterReportDataRow["RMName"] == DBNull.Value ? "" : userMasterReportDataRow["SupervisorName"].ToString(),
                                              CreditAnalystId = userMasterReportDataRow["CreditAnalystId"] == DBNull.Value ? "" : userMasterReportDataRow["CreditAnalystId"].ToString(),
                                              CreditAnalystName = userMasterReportDataRow["CreditAnalystName"] == DBNull.Value ? "" : userMasterReportDataRow["CreditAnalystName"].ToString(),
                                              EWSAnalystId = userMasterReportDataRow["EWSAnalystId"] == DBNull.Value ? "" : userMasterReportDataRow["EWSAnalystId"].ToString(),
                                              EWSAnalystName = userMasterReportDataRow["EWSAnalystName"] == DBNull.Value ? "" : userMasterReportDataRow["EWSAnalystName"].ToString(),
                                              RCHName = userMasterReportDataRow["RCHName"] == DBNull.Value ? "" : userMasterReportDataRow["RCHName"].ToString(),
                                              RCHId = userMasterReportDataRow["RCHEmployeeId"] == DBNull.Value ? "" : userMasterReportDataRow["RCHEmployeeId"].ToString(),
                                              ZBHName = userMasterReportDataRow["ZBHName"] == DBNull.Value ? "" : userMasterReportDataRow["ZBHName"].ToString(),
                                              ZBHId = userMasterReportDataRow["ZBHEmployeeId"] == DBNull.Value ? "" : userMasterReportDataRow["ZBHEmployeeId"].ToString(),
                                              MappingMonth = userMasterReportDataRow["MappingMonth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(userMasterReportDataRow["MappingMonth"]),
                                          });
                    }
                }

                catch (SqlException ex)
                {
                    Logger.Error(ex.Message, MethodBase.GetCurrentMethod().Name);
                    Logger.Error(ex.StackTrace, MethodBase.GetCurrentMethod().Name);
                    throw ex;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, MethodBase.GetCurrentMethod().Name);
                    Logger.Error(ex.StackTrace, MethodBase.GetCurrentMethod().Name);
                    throw ex;
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }
            return userList;
        }
        public List<ListOfAcctTableCSB> GetLOATable(ListAccountrequest accountrequest)
        {
            List<ListOfAcctTableCSB> LoaTable = new List<ListOfAcctTableCSB>();
            DataSet LoaDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetLOATable);

                DataBase.AddInParameter(dbCommand, "@RMEmployeeId ", DbType.String, accountrequest.empId);
                DataBase.AddInParameter(dbCommand, "@FromDate", DbType.String, accountrequest.fromDate);
                DataBase.AddInParameter(dbCommand, "@ToDate", DbType.String, accountrequest.toDate);
                DataBase.AddInParameter(dbCommand, "@RoleId", DbType.Int32, accountrequest.roleId);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {
                    LoaTable.AddRange(from DataRow dr in ds.Tables[0].Rows
                                      select new ListOfAcctTableCSB
                                      {
                                          Id = dr["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dr["Id"]),
                                          AmukhaId = dr["AmukhaId"] == DBNull.Value ? String.Empty : dr["AmukhaId"].ToString(),
                                          UCICID = dr["UCICID"] == DBNull.Value ? String.Empty : dr["UCICID"].ToString(),
                                          FileId = dr["FileId"] == DBNull.Value ? -1 : Convert.ToInt32(dr["FileId"]),
                                          AccountName = dr["AccountName"] == DBNull.Value ? String.Empty : dr["AccountName"].ToString(),
                                          EWSMonth = dr["EWSMonth"] == DBNull.Value ? String.Empty : dr["EWSMonth"].ToString(),
                                          prevcat = dr["PreviousCategory"] == DBNull.Value ? String.Empty : dr["PreviousCategory"].ToString(),
                                          curntcat = dr["CurrentCategory"] == DBNull.Value ? String.Empty : dr["CurrentCategory"].ToString(),
                                          ewsscore = dr["EwsScore"] == DBNull.Value ? String.Empty : dr["EwsScore"].ToString(),
                                          Action = dr["Action"] == DBNull.Value ? String.Empty : dr["Action"].ToString(),
                                          AssignedDate = dr["AssignedDate"] == DBNull.Value ? Convert.ToDateTime(String.Empty) : Convert.ToDateTime(dr["AssignedDate"]),

                                          AssignedTo = dr["AssignedTo"] == DBNull.Value ? String.Empty : dr["AssignedTo"].ToString().TrimLower(),
                                          RFAConcern = dr["RFAConcern"] == DBNull.Value ? String.Empty : dr["RFAConcern"].ToString(),
                                          Status = dr["Status"] == DBNull.Value ? String.Empty : dr["Status"].ToString(),
                                          AlertCount = dr["AlertCount"] == DBNull.Value ? String.Empty : dr["AlertCount"].ToString(),
                                          OverallTAT = dr["OverallTAT"] == DBNull.Value ? String.Empty : dr["OverallTAT"].ToString(),
                                          TATStatus = dr["TATStatus"] == DBNull.Value ? String.Empty : dr["TATStatus"].ToString(),


                                          LevelOneBusiness = dr["LevelOneBusiness"] == DBNull.Value ? String.Empty : dr["LevelOneBusiness"].ToString().TrimLower(),
                                          L1BusinessSubmissionDate = dr["L1BusinessSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L1BusinessSubmissionDate"]),

                                          LevelTwoBusiness = dr["LevelTwoBusiness"] == DBNull.Value ? String.Empty : dr["LevelTwoBusiness"].ToString().TrimLower(),
                                          L2BusinessSubmissionDate = dr["L2BusinessSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L2BusinessSubmissionDate"]),

                                          LevelOneCredit = dr["LevelOneCredit"] == DBNull.Value ? String.Empty : dr["LevelOneCredit"].ToString().TrimLower(),
                                          L1CreditSubmissionDate = dr["L1CreditSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L1CreditSubmissionDate"]),

                                          LevelTwoCredit = dr["LevelTwoCredit"] == DBNull.Value ? String.Empty : dr["LevelTwoCredit"].ToString().TrimLower(),
                                          L2CreditSubmissionDate = dr["L2CreditSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L2CreditSubmissionDate"]),

                                      });

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetLOATable from Repo, SqlException:");
                Logger.Error(e);
                LoaTable = null;
            }
            catch (Exception e)
            {
                Logger.Error("Unable to GetLOATable from Repo, Exception: ");
                Logger.Error(e);
                LoaTable = null;
            }
            return LoaTable;
        }
        public List<ListOfAcctTableCSB> GetActionTable(ListAccountrequest accountrequest)
        {
            List<ListOfAcctTableCSB> LoaTable = new List<ListOfAcctTableCSB>();
            DataSet LoaDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetActionTable);

                DataBase.AddInParameter(dbCommand, "@RMEmployeeId ", DbType.String, accountrequest.empId);
                DataBase.AddInParameter(dbCommand, "@FromDate", DbType.String, accountrequest.fromDate);
                DataBase.AddInParameter(dbCommand, "@ToDate", DbType.String, accountrequest.toDate);
                DataBase.AddInParameter(dbCommand, "@RoleId", DbType.Int32, accountrequest.roleId);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {
                    LoaTable.AddRange(from DataRow dr in ds.Tables[0].Rows
                                      select new ListOfAcctTableCSB
                                      {
                                          Id = dr["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dr["Id"]),
                                          AmukhaId = dr["AmukhaId"] == DBNull.Value ? String.Empty : dr["AmukhaId"].ToString(),
                                          UCICID = dr["UCICID"] == DBNull.Value ? String.Empty : dr["UCICID"].ToString(),
                                          FileId = dr["FileId"] == DBNull.Value ? -1 : Convert.ToInt32(dr["FileId"]),
                                          AccountName = dr["AccountName"] == DBNull.Value ? String.Empty : dr["AccountName"].ToString(),
                                          EWSMonth = dr["EWSMonth"] == DBNull.Value ? String.Empty : dr["EWSMonth"].ToString(),
                                          prevcat = dr["PreviousCategory"] == DBNull.Value ? String.Empty : dr["PreviousCategory"].ToString(),
                                          curntcat = dr["CurrentCategory"] == DBNull.Value ? String.Empty : dr["CurrentCategory"].ToString(),
                                          ewsscore = dr["EwsScore"] == DBNull.Value ? String.Empty : dr["EwsScore"].ToString(),
                                          ChangeCat = dr["CategoryChange"] == DBNull.Value ? String.Empty : dr["CategoryChange"].ToString(),
                                          Action = dr["Action"] == DBNull.Value ? String.Empty : dr["Action"].ToString(),
                                          AssignedDate = dr["AssignedDate"] == DBNull.Value ? Convert.ToDateTime(String.Empty) : Convert.ToDateTime(dr["AssignedDate"]),

                                          AssignedTo = dr["AssignedTo"] == DBNull.Value ? String.Empty : dr["AssignedTo"].ToString().TrimLower(),
                                          RFAConcern = dr["RFAConcern"] == DBNull.Value ? String.Empty : dr["RFAConcern"].ToString(),
                                          Status = dr["Status"] == DBNull.Value ? String.Empty : dr["Status"].ToString(),
                                          AlertCount = dr["AlertCount"] == DBNull.Value ? String.Empty : dr["AlertCount"].ToString(),
                                          OverallTAT = dr["OverallTAT"] == DBNull.Value ? String.Empty : dr["OverallTAT"].ToString(),
                                          TATStatus = dr["TATStatus"] == DBNull.Value ? String.Empty : dr["TATStatus"].ToString(),


                                          LevelOneBusiness = dr["LevelOneBusiness"] == DBNull.Value ? String.Empty : dr["LevelOneBusiness"].ToString().TrimLower(),
                                          L1BusinessSubmissionDate = dr["L1BusinessSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L1BusinessSubmissionDate"]),

                                          LevelTwoBusiness = dr["LevelTwoBusiness"] == DBNull.Value ? String.Empty : dr["LevelTwoBusiness"].ToString().TrimLower(),
                                          L2BusinessSubmissionDate = dr["L2BusinessSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L2BusinessSubmissionDate"]),

                                          LevelOneCredit = dr["LevelOneCredit"] == DBNull.Value ? String.Empty : dr["LevelOneCredit"].ToString().TrimLower(),
                                          L1CreditSubmissionDate = dr["L1CreditSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L1CreditSubmissionDate"]),

                                          LevelTwoCredit = dr["LevelTwoCredit"] == DBNull.Value ? String.Empty : dr["LevelTwoCredit"].ToString().TrimLower(),
                                          L2CreditSubmissionDate = dr["L2CreditSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["L2CreditSubmissionDate"]),

                                      });

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetLOATable from Repo, SqlException:");
                Logger.Error(e);
                LoaTable = null;
            }
            catch (Exception e)
            {
                Logger.Error("Unable to GetLOATable from Repo, Exception: ");
                Logger.Error(e);
                LoaTable = null;
            }
            return LoaTable;
        }

        public List<CaseTransferLogs> GetCaseTransferLogs(DateTime? FromDate, DateTime? ToDate)
        {
            List<CaseTransferLogs> transferredAccounts = new List<CaseTransferLogs>();
            DataSet LoaDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetCaseTransferLogs);

                DataBase.AddInParameter(dbCommand, "@FromDate ", DbType.DateTime, FromDate);
                DataBase.AddInParameter(dbCommand, "@ToDate", DbType.DateTime, ToDate);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {
                    transferredAccounts.AddRange(from DataRow dr in ds.Tables[0].Rows
                                                 select new CaseTransferLogs
                                                 {
                                                     AccountName = dr["AccountName"] == DBNull.Value ? String.Empty : dr["AccountName"].ToString(),
                                                     Id = dr["Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["Id"]),
                                                     RecordId = dr["RecordId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["RecordId"]),
                                                     FileId = dr["FileId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["FileId"]),
                                                     FromUserId = dr["FromUserId"] == DBNull.Value ? String.Empty : dr["FromUserId"].ToString(),
                                                     ToUserId = dr["ToUserId"] == DBNull.Value ? String.Empty : dr["ToUserId"].ToString(),
                                                     Role = dr["Role"] == DBNull.Value ? String.Empty : dr["Role"].ToString(),
                                                     Comments = dr["Comments"] == DBNull.Value ? String.Empty : dr["Comments"].ToString(),
                                                     CreatedDate = dr["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["CreatedDate"]),
                                                     TicketId = dr["TicketId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["TicketId"]),
                                                     CaseTrfBy = dr["CaseTrfBy"] == DBNull.Value ? String.Empty : dr["CaseTrfBy"].ToString(),
                                                     EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["EWSMonth"])
                                                 });

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetCaseTransferLogs from Repo");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                transferredAccounts = null;
            }
            return transferredAccounts;
        }

        public List<CaseReopeningAccounts> GetCaseReopenLogs(DateTime? FromDate, DateTime? ToDate)
        {
            List<CaseReopeningAccounts> reopeningAccounts = new List<CaseReopeningAccounts>();
            DataSet LoaDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetCaseReopenLogs);

                DataBase.AddInParameter(dbCommand, "@FromDate", DbType.DateTime, FromDate);
                DataBase.AddInParameter(dbCommand, "@ToDate", DbType.DateTime, ToDate);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null)
                {
                    reopeningAccounts.AddRange(from DataRow dr in ds.Tables[0].Rows
                                               select new CaseReopeningAccounts
                                               {
                                                   AccountName = dr["AccountName"] == DBNull.Value ? String.Empty : dr["AccountName"].ToString(),
                                                   ReassignedBy = dr["SubmittedByUserId"] == DBNull.Value ? String.Empty : dr["SubmittedByUserId"].ToString(),
                                                   AssignedTo = dr["SubmittedToUserId"] == DBNull.Value ? String.Empty : dr["SubmittedToUserId"].ToString(),
                                                   Comments = dr["Remarks"] == DBNull.Value ? String.Empty : dr["Remarks"].ToString(),
                                                   SubmittedToRole = dr["SubmittedToRole"] == DBNull.Value ? String.Empty : dr["SubmittedToRole"].ToString(),
                                                   ReassigningDate = dr["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["CreatedDate"]),
                                                   ReassignId = dr["TicketId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["TicketId"]),
                                                   EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["EWSMonth"])
                                               });

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetCaseReopeningLogs from Repo");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                reopeningAccounts = null;
            }
            return reopeningAccounts;
        }

        public Dictionary<int, List<LOA_AttachmentModel>> GetAttachments(int recordId, int fileId)
        {
            Dictionary<int, List<LOA_AttachmentModel>> DictAttachments = new Dictionary<int, List<LOA_AttachmentModel>>();
            DataSet LoaDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetAttachments);

                DataBase.AddInParameter(dbCommand, "@EventID ", DbType.String, 0);
                DataBase.AddInParameter(dbCommand, "@RecordId", DbType.String, recordId.ToString());
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.String, fileId.ToString());

                var ds = ExecuteDataSet(dbCommand, true);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int eId = dr["EventId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["EventId"]);
                        LOA_AttachmentModel attachment = new LOA_AttachmentModel()
                        {
                            AttachmentBy = dr["AttachementBy"] == DBNull.Value ? null : dr["AttachementBy"].ToString(),
                            CreatedOn = dr["AttachementCreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AttachementCreatedOn"]),
                            CDNFileName = dr["CDNFileName"] == DBNull.Value ? null : dr["CDNFileName"].ToString(),
                            UserFileName = dr["UserFileName"] == DBNull.Value ? null : dr["UserFileName"].ToString(),
                            AtttachmentOrder = dr["AttachementsOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["AttachementsOrder"])
                        };

                        if (DictAttachments.ContainsKey(eId))
                        {
                            DictAttachments[eId].Add(attachment);
                        }
                        else
                        {
                            List<LOA_AttachmentModel> attchList = new List<LOA_AttachmentModel>();
                            attchList.Add(attachment);
                            DictAttachments.Add(eId, attchList);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetAttachments from Repo");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                DictAttachments = null;
            }

            return DictAttachments;
        }
        public List<TransferEvents> GetTrfEvents()
        {
            List<TransferEvents> TrfEventTable = new List<TransferEvents>();
            DataSet TrfEventDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetTrfEvents);

                var ds = ExecuteDataSet(dbCommand, true);

                if (ds != null)
                {
                    TrfEventTable.AddRange(from DataRow dr in ds.Tables[0].Rows
                                           select new TransferEvents
                                           {
                                               Id = dr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Id"]),
                                               SrNo = dr["SrNo"] == DBNull.Value ? String.Empty : dr["SrNo"].ToString(),
                                               Events = dr["Events"] == DBNull.Value ? String.Empty : dr["Events"].ToString()
                                           });

                }

            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetTrfEvents from Repo");
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                TrfEventTable = null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                TrfEventTable = null;
            }
            return TrfEventTable;
        }

        public Dictionary<string, List<LOA_ActionsTableModel>> GetActions(int recordId, int fileId)
        {
            Dictionary<string, List<LOA_ActionsTableModel>> tblData = new Dictionary<string, List<LOA_ActionsTableModel>>();

            DataSet actiondDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetRemarkAndAttch);

                DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.Int32, recordId);
                DataBase.AddInParameter(dbCommand, "@FileId ", DbType.Int32, fileId);

                var ds = ExecuteDataSet(dbCommand, true);


                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string srNo = dr["SrNo"] == DBNull.Value ? String.Empty : dr["SrNo"].ToString();
                        var actionRemarks = new LOA_ActionsTableModel
                        {
                            Action = dr["Action"] == DBNull.Value ? String.Empty : dr["Action"].ToString(),
                            Remark = dr["Remarks"] == DBNull.Value ? String.Empty : dr["Remarks"].ToString(),
                            IsEdited = dr["IsEdited"] == DBNull.Value ? (short)0 : Convert.ToInt16(dr["IsEdited"]),
                            TrgStatusDB = dr["TrgStatus"] == DBNull.Value ? String.Empty : dr["TrgStatus"].ToString(),
                            RemarksBy = dr["RemarksAddedBy"] == DBNull.Value ? String.Empty : dr["RemarksAddedBy"].ToString(),
                            RemarkedOn = dr["RemarksCreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RemarksCreatedOn"]),
                            IsRemarksCompletion = dr["IsRemarksCompletion"] == DBNull.Value ? (short)0 : Convert.ToInt16(dr["IsRemarksCompletion"])
                        };

                        if (tblData.ContainsKey(srNo))
                        {
                            tblData[srNo].Add(actionRemarks);
                        }
                        else
                        {
                            List<LOA_ActionsTableModel> data = new List<LOA_ActionsTableModel>();
                            data.Add(actionRemarks);
                            tblData.Add(srNo, data);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetActions from Repo");
                Logger.Error(e);
                tblData = null;
            }
            return tblData;
        }

        public accountActionData GetAccountActionDetailsById(int RecordId, int FileId)
        {
            accountActionData aaDetails = new accountActionData();
            DataSet LoaDS = new DataSet();
            try
            {
                var dbCommand = DataBase.GetStoredProcCommand(procAccountActionDetails);

                DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.Int32, RecordId);
                DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, FileId);

                var ds = ExecuteDataSet(dbCommand, true);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    aaDetails.Id = ds.Tables[0].Rows[0]["Id"] == DBNull.Value ? -1 : Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                    aaDetails.AmukhaId = ds.Tables[0].Rows[0]["AmukhaId"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["AmukhaId"].ToString();
                    aaDetails.UCICID = ds.Tables[0].Rows[0]["UCICID"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["UCICID"].ToString();
                    aaDetails.FileId = ds.Tables[0].Rows[0]["FileId"] == DBNull.Value ? -1 : Convert.ToInt32(ds.Tables[0].Rows[0]["FileId"]);
                    aaDetails.AccountName = ds.Tables[0].Rows[0]["AccountName"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["AccountName"].ToString();
                    aaDetails.EWSMonth = ds.Tables[0].Rows[0]["EWSMonth"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["EWSMonth"].ToString();
                    aaDetails.Action = ds.Tables[0].Rows[0]["Action"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["Action"].ToString();
                    aaDetails.AssignedDate = ds.Tables[0].Rows[0]["AssignedDate"] == DBNull.Value ? Convert.ToDateTime(String.Empty) : Convert.ToDateTime(ds.Tables[0].Rows[0]["AssignedDate"]);

                    aaDetails.AssignedTo = ds.Tables[0].Rows[0]["AssignedTo"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["AssignedTo"].ToString().TrimLower();
                    aaDetails.RFAConcern = ds.Tables[0].Rows[0]["RFAConcern"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["RFAConcern"].ToString();
                    aaDetails.Status = ds.Tables[0].Rows[0]["Status"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["Status"].ToString();
                    aaDetails.AlertCount = ds.Tables[0].Rows[0]["AlertCount"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["AlertCount"].ToString();
                    aaDetails.OverallTAT = ds.Tables[0].Rows[0]["OverallTAT"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["OverallTAT"].ToString();
                    aaDetails.TATStatus = ds.Tables[0].Rows[0]["TATStatus"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["TATStatus"].ToString();


                    aaDetails.LevelOneBusiness = ds.Tables[0].Rows[0]["LevelOneBusiness"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["LevelOneBusiness"].ToString().TrimLower();
                    aaDetails.L1BusinessSubmissionDate = ds.Tables[0].Rows[0]["L1BusinessSubmissionDate"] == DBNull.Value ? (long?)null : Convert.ToDateTime(ds.Tables[0].Rows[0]["L1BusinessSubmissionDate"]).FromEpoch();

                    aaDetails.LevelTwoBusiness = ds.Tables[0].Rows[0]["LevelTwoBusiness"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["LevelTwoBusiness"].ToString().TrimLower();
                    aaDetails.L2BusinessSubmissionDate = ds.Tables[0].Rows[0]["L2BusinessSubmissionDate"] == DBNull.Value ? (long?)null : Convert.ToDateTime(ds.Tables[0].Rows[0]["L2BusinessSubmissionDate"]).FromEpoch();

                    aaDetails.LevelOneCredit = ds.Tables[0].Rows[0]["LevelOneCredit"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["LevelOneCredit"].ToString().TrimLower();
                    aaDetails.L1CreditSubmissionDate = ds.Tables[0].Rows[0]["L1CreditSubmissionDate"] == DBNull.Value ? (long?)null : Convert.ToDateTime(ds.Tables[0].Rows[0]["L1CreditSubmissionDate"]).FromEpoch();

                    aaDetails.LevelTwoCredit = ds.Tables[0].Rows[0]["LevelTwoCredit"] == DBNull.Value ? String.Empty : ds.Tables[0].Rows[0]["LevelTwoCredit"].ToString().TrimLower();
                    aaDetails.L2CreditSubmissionDate = ds.Tables[0].Rows[0]["L2CreditSubmissionDate"] == DBNull.Value ? (long?)null : Convert.ToDateTime(ds.Tables[0].Rows[0]["L2CreditSubmissionDate"]).FromEpoch();

                }
            }
            catch (SqlException e)
            {
                Logger.Error("Unable to GetAccountActionDetailsById from DB SQL Exception: ");
                Logger.Error(e);
                aaDetails = null;
            }
            catch (Exception e)
            {
                Logger.Error("Unable to GetAccountActionDetailsById from Exception: ");
                Logger.Error(e);
                aaDetails = null;
            }
            return aaDetails;
        }

        public bool InsertAuditTrailCommentsAndAttachment(InsertAuditCaseModel req)
        {
            bool insertSuccessfull = false;
            // DataSet LoaDS = new DataSet();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertCommentAttchments);
                try
                {

                    DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, req.RecordId);
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, req.FileId);
                    DataBase.AddInParameter(dbCommand, "@Comments", DbType.String, req.Comments);
                    DataBase.AddInParameter(dbCommand, "@CommentedBy", DbType.String, req.CommentedBy);
                    DataBase.AddInParameter(dbCommand, "@CDNFileName", DbType.String, req.CDNFileName);
                    DataBase.AddInParameter(dbCommand, "@UserFileName", DbType.String, req.UserFileName);
                    DataBase.AddInParameter(dbCommand, "@AttachementsOrder", DbType.String, req.AttachementsOrder);

                    var bsData = ExecuteDataSet(dbCommand, true);

                    insertSuccessfull = true;

                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }

            return insertSuccessfull;

        }
        public bool InsertUpdateCaseDetails(InsertUpdateCaseModel req)
        {
            bool insertSuccessfull = false;
            // DataSet LoaDS = new DataSet();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertUpdateCase);
                try
                {

                    DataBase.AddInParameter(dbCommand, "@RecordId ", DbType.Int32, req.RecordId);
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, req.FileId);
                    DataBase.AddInParameter(dbCommand, "@SubmittedByUserId", DbType.String, req.SubmittedByUserId);
                    DataBase.AddInParameter(dbCommand, "@SubmittedByRole", DbType.String, req.SubmittedByRole);
                    DataBase.AddInParameter(dbCommand, "@SubmittedToUserId", DbType.String, req.SubmittedToUserId);
                    DataBase.AddInParameter(dbCommand, "@SubmittedToRole", DbType.String, req.SubmittedToRole);
                    DataBase.AddInParameter(dbCommand, "@RFAConcern", DbType.String, req.RFAConcern);
                    DataBase.AddInParameter(dbCommand, "@Remarks", DbType.String, req.Remarks);
                    DataBase.AddInParameter(dbCommand, "@CDNFileName", DbType.String, req.CDNFileName);
                    DataBase.AddInParameter(dbCommand, "@UserFileName", DbType.String, req.UserFileName);
                    DataBase.AddInParameter(dbCommand, "@AttachmentsOrder", DbType.String, req.AttachementsOrder);

                    var bsData = ExecuteDataSet(dbCommand, true);

                    insertSuccessfull = true;

                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }

            return insertSuccessfull;

        }

        public bool InsertCaseTrfRemarksAndAttachments(LOA_ActionRemarksAttachments req)
        {
            bool insertSuccessfull = false;
            // DataSet LoaDS = new DataSet();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procInsertCaseActionRemarksAndAttch);
                try
                {

                    DataBase.AddInParameter(dbCommand, "@EventId ", DbType.Int32, req.EventId);
                    DataBase.AddInParameter(dbCommand, "@RecordId", DbType.Int32, req.RecordId);
                    DataBase.AddInParameter(dbCommand, "@FileId", DbType.Int32, req.FileId);
                    DataBase.AddInParameter(dbCommand, "@Action", DbType.String, req.Action);
                    DataBase.AddInParameter(dbCommand, "@Remarks", DbType.String, req.Remarks);
                    DataBase.AddInParameter(dbCommand, "@AddedBy", DbType.String, req.AddedBy);
                    DataBase.AddInParameter(dbCommand, "@CDNFileName", DbType.String, LOA_ActionRemarksAttachments.CDNFileName);
                    DataBase.AddInParameter(dbCommand, "@UserFileName", DbType.String, LOA_ActionRemarksAttachments.UserFileName);
                    DataBase.AddInParameter(dbCommand, "@AttachementBy", DbType.String, req.AttachmentBy);
                    DataBase.AddInParameter(dbCommand, "@AttachementsOrder", DbType.String, req.AtttachmentOrder);
                    DataBase.AddInParameter(dbCommand, "@TrgStatus", DbType.String, req.TrgStatus);
                    var bsData = ExecuteDataSet(dbCommand, true);

                    insertSuccessfull = true;

                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();

                    dbCommand.Dispose();
                }
            }

            return insertSuccessfull;

        }
        public bool addAccountUserMappingToDB(DataTable accountUserMappingTable)
        {
            bool addedRowToDB = false;
            try
            {
                using (var dbCommand = DataBase.GetStoredProcCommand(procInsertAccUserMapping))
                {
                    var parameter = new SqlParameter
                    {
                        ParameterName = "@AccountwiseReportingUserMapping",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.TblType_AccountwiseReportingUserMapping",
                        Value = accountUserMappingTable
                    };
                    dbCommand.Parameters.Add(parameter);
                    var result = ExecuteDataSet(dbCommand, true);
                    addedRowToDB = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error while adding account-wise mapping rows to DB, Error: " + e.Message);
                Logger.Error(e.StackTrace);
                addedRowToDB = false;
                throw;
            }
            return addedRowToDB;
        }
        public List<AlertDump> GetAlertDumpList()
        {
            List<AlertDump> alertDumps = new List<AlertDump>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand("UI.GetAlertDumpInExcel");
                try
                {
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        alertDumps.AddRange(from DataRow dr in ds.Tables[0].AsEnumerable()
                                            select new AlertDump
                                            {
                                                FileName = dr["FileName"] == DBNull.Value ? string.Empty : dr["FileName"].ToString(),
                                                FilePath = dr["FilePath"] == DBNull.Value ? string.Empty : dr["FilePath"].ToString()
                                            });
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();
                    dbCommand.Dispose();
                }
                return alertDumps;
            }
        }
        public List<Analytics> GetPeriodDropdown(int CompanyId)
        {
            List<Analytics> analytics = new List<Analytics>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetPeriodDropdown);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@ClientId", DbType.Int32, CompanyId);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        analytics.AddRange(from DataRow dr in ds.Tables[0].AsEnumerable()
                                            select new Analytics
                                            {
                                                Value = dr["ScoreDate"] == DBNull.Value ? string.Empty : dr["ScoreDate"].ToString(),
                                                Text = dr["EWSMonth"] == DBNull.Value ? string.Empty : dr["EWSMonth"].ToString()
                                            });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();
                    dbCommand.Dispose();
                }
                return analytics;
            }
        }
        public List<AnalyticsSummary> GetPieChartData(int CompanyId, string RiskCategory, string ScoreDate)
        {
            List<AnalyticsSummary> analytics = new List<AnalyticsSummary>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dbCommand = DataBase.GetStoredProcCommand(procGetPieChartSummary);
                try
                {
                    DataBase.AddInParameter(dbCommand, "@ClientId", DbType.Int32, CompanyId);
                    DataBase.AddInParameter(dbCommand, "@RiskCategory", DbType.String, RiskCategory);
                    DataBase.AddInParameter(dbCommand, "@ScoreDate", DbType.String, ScoreDate);
                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        analytics.AddRange(from DataRow dr in ds.Tables[0].AsEnumerable()
                                            select new AnalyticsSummary
                                            {
                                                RecordId = dr["RecordId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RecordId"]),
                                                FileId = dr["FileId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FileId"]),
                                                AmukhaId = dr["AmukhaId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AmukhaId"]),
                                                UCICID = dr["UCICID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UCICID"]),
                                                AccountName = dr["AccountName"] == DBNull.Value ? string.Empty : dr["AccountName"].ToString(),
                                                EWSMonth = dr["EWSMonth"] == DBNull.Value ? string.Empty : dr["EWSMonth"].ToString(),
                                                RiskCategory = dr["RiskCategory"] == DBNull.Value ? string.Empty : dr["RiskCategory"].ToString(),
                                                EntityType = dr["EntityType"] == DBNull.Value ? string.Empty : dr["EntityType"].ToString(),
                                                CaseStatus = dr["CaseStatus"] == DBNull.Value ? string.Empty : dr["CaseStatus"].ToString()
                                            });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                finally
                {
                    if (dbCommand.Connection.State == ConnectionState.Open)
                        dbCommand.Connection.Close();
                    dbCommand.Dispose();
                }
                return analytics;
            }
        }
        public List<AccountLevelReport> GetAccountLevelReport(ListAccountrequest accountrequest)
        {
            List<AccountLevelReport> data = new List<AccountLevelReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var userIds = (accountrequest.userid ?? "")
                .Split('|')
                .Select(x => x.Trim())
                .ToList();
                
                foreach (var userId in userIds)
                {
                    try
                    {
                        var dbCommand = DataBase.GetStoredProcCommand(procAccountLevel);
                        DataBase.AddInParameter(dbCommand, "@UserList", DbType.String, userId); // pass one user at a time
                        DataBase.AddInParameter(dbCommand, "@FromDate", DbType.Date, accountrequest.fromDate);
                        DataBase.AddInParameter(dbCommand, "@ToDate", DbType.Date, accountrequest.toDate);

                        var ds = ExecuteDataSet(dbCommand, true);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                AccountLevelReport obj = new AccountLevelReport
                                {
                                    User = dr["AssignedTo"] == DBNull.Value ? string.Empty : dr["AssignedTo"].ToString(),
                                    AccountName = dr["AccountName"] == DBNull.Value ? string.Empty : dr["AccountName"].ToString(),
                                    ClintId = dr["AmukhaId"] == DBNull.Value ? string.Empty : dr["AmukhaId"].ToString(),
                                    Status = dr["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Status"]),
                                    pendingdays = dr["DaysPending"] == DBNull.Value ? string.Empty : Convert.ToString(dr["DaysPending"]),
                                    Pendingat = dr["PendingAt"] == DBNull.Value ? string.Empty : Convert.ToString(dr["PendingAt"]),
                                    EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["EWSMonth"]),
                                    UserId = userId // ← this line adds user id
                                };

                                data.Add(obj);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Logger.Error(ex.Message);
                        Logger.Error(ex.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message);
                        Logger.Error(ex.StackTrace);
                    }
                }
            }
            return data;

        }
        public List<HighRiskBorrowerReport> GetHighRiskBorrowerReport(ListAccountrequest accountrequest)
        {
            List<HighRiskBorrowerReport> data = new List<HighRiskBorrowerReport>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    var dbCommand = DataBase.GetStoredProcCommand(procHighriskborrower);

                    DataBase.AddInParameter(dbCommand, "@FromDate", DbType.Date, accountrequest.fromDate);
                    DataBase.AddInParameter(dbCommand, "@ToDate", DbType.Date, accountrequest.toDate);

                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            HighRiskBorrowerReport obj = new HighRiskBorrowerReport();
                            obj.User = dr["AssignedUser"] == DBNull.Value ? string.Empty : dr["AssignedUser"].ToString();
                            obj.Noofaccount = dr["NoOfAccounts"] == DBNull.Value ? string.Empty : dr["NoOfAccounts"].ToString();
                            obj.pending = dr["Pending"] == DBNull.Value ? string.Empty : dr["Pending"].ToString();
                            obj.completed = dr["Completed"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Completed"]);
                            obj.typeofuser = dr["TypeOfUser"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TypeOfUser"]);
                            obj.EWSMonth = dr["EWSMonth"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["EWSMonth"]);
                            data.Add(obj);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
            }
            return data;

        }
        public List<BorrowerRiskInsight> GetBorrowerRiskInsightsReport(ListAccountrequest accountrequest)
        {
            List<BorrowerRiskInsight> data = new List<BorrowerRiskInsight>();
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    var dbCommand = DataBase.GetStoredProcCommand("UI.GetBorrowerRiskInSightsReport");

                    DataBase.AddInParameter(dbCommand, "@FromDate", DbType.Date, accountrequest.fromDate);
                    DataBase.AddInParameter(dbCommand, "@ToDate", DbType.Date, accountrequest.toDate);

                    // If null or empty, pass DBNull to fetch all categories
                    if (string.IsNullOrWhiteSpace(accountrequest.categoryList))
                        DataBase.AddInParameter(dbCommand, "@CategoryList", DbType.String, DBNull.Value);
                    else
                        DataBase.AddInParameter(dbCommand, "@CategoryList", DbType.String, accountrequest.categoryList);

                    var ds = ExecuteDataSet(dbCommand, true);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BorrowerRiskInsight obj = new BorrowerRiskInsight
                            {
                                Id = dr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Id"]),
                                AmukhaId = dr["AmukhaId"]?.ToString(),
                                UCICID = dr["UCICID"]?.ToString(),
                                AccountName = dr["AccountName"]?.ToString(),
                                EwsGrade = dr["EwsGrade"]?.ToString(),
                                Status = dr["Status"]?.ToString(),
                                AssignedTo = dr["AssignedTo"]?.ToString(),
                                TrgDescriptions = dr["TrgDescriptions"]?.ToString(),
                                AlertDate = dr["AlertDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AlertDate"]),
                                EWSMonth = dr["EWSMonth"]?.ToString(),

                                LevelOneBusiness = dr["LevelOneBusiness"]?.ToString(),
                                LevelTwoBusiness = dr["LevelTwoBusiness"]?.ToString(),
                                LevelOneCredit = dr["LevelOneCredit"]?.ToString(),
                                LevelTwoCredit = dr["LevelTwoCredit"]?.ToString(),

                                LevelOneBusinessRemarks = dr["LevelOneBusinessRemarks"]?.ToString(),
                                LevelTwoBusinessRemarks = dr["LevelTwoBusinessRemarks"]?.ToString(),
                                LevelOneCreditRemarks = dr["LevelOneCreditRemarks"]?.ToString(),
                                LevelTwoCreditRemarks = dr["LevelTwoCreditRemarks"]?.ToString(),

                                DateFoClosure = dr["DateFoClosure"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DateFoClosure"]),
                                RFAConcern = dr["RFAConcern"]?.ToString()
                            };
                            data.Add(obj);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
            }
            return data;
        }
    }
}


