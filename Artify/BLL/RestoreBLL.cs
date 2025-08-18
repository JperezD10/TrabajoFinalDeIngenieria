using BE;
using DAL;
using System;
using System.Data.SqlClient;

namespace BLL
{
    public class RestoreBLL
    {
        private readonly RestoreDAL _dal = new RestoreDAL();

        public Response<RestoreAnalysisData> AnalyzeBackup(string backupFullPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(backupFullPath))
                    return Response<RestoreAnalysisData>.Error("restore.err.pathEmpty");

                var header = _dal.RestoreHeaderOnly(backupFullPath);
                var filelist = _dal.RestoreFileListOnly(backupFullPath);

                if (filelist == default || string.IsNullOrWhiteSpace(filelist.LogicalDataName) || string.IsNullOrWhiteSpace(filelist.LogicalLogName))
                    return Response<RestoreAnalysisData>.Error("restore.err.noLogicalNames");

                var data = new RestoreAnalysisData
                {
                    BackupStartDate = header?.BackupStartDate,
                    SqlVersion = header?.SoftwareVersionMajor,
                    LogicalDataName = filelist.LogicalDataName,
                    LogicalLogName = filelist.LogicalLogName,
                    BackupFullPath = backupFullPath
                };

                return Response<RestoreAnalysisData>.Success(data, "restore.msgAnalyzeOk");
            }
            catch
            {
                return Response<RestoreAnalysisData>.Error("restore.msgAnalyzeFail");
            }
        }

        public Response<bool> RestoreDatabase(
            string backupFullPath,
            string targetDbName,
            string logicalData,
            string logicalLog,
            string targetDataPath,
            string targetLogPath)
        {
            try
            {
                var last = _dal.RestoreDatabaseFull(
                    backupFullPath,
                    targetDbName,      // "Artify"
                    logicalData,       // "Artify"
                    logicalLog,        // "Artify_log"
                    overrideDataPath: null,  // usa rutas por defecto del motor
                    overrideLogPath: null
                );

                if (last == null || (DateTime.UtcNow - last.Value.ToUniversalTime()).TotalMinutes > 10)
                    return Response<bool>.Error("restore.msgRestoreFail");

                return Response<bool>.Success(true, "restore.msgRestoreOk");
            }
            catch (SqlException ex)
            {
                // (opcional) logueá ex.Message para ver el motivo exacto si falla
                return Response<bool>.Error("restore.msgRestoreFail");
            }
        }
    }
}
