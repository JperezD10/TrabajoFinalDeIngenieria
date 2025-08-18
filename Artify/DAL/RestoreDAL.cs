using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RestoreDAL
    {
        private readonly Acceso _acceso;

        public RestoreDAL()
        {
            _acceso = new Acceso();
        }

        public (DateTime? BackupStartDate, string SoftwareVersionMajor)? RestoreHeaderOnly(string path)
        {
            const string sql = @"USE master; RESTORE HEADERONLY FROM DISK = @path;";
            var dt = _acceso.Leer(sql, new[]
            {
                new SqlParameter("@path", SqlDbType.NVarChar) { Value = path }
            }, CommandType.Text);

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            DateTime? start = row.Table.Columns.Contains("BackupStartDate") && row["BackupStartDate"] != DBNull.Value
                ? (DateTime?)Convert.ToDateTime(row["BackupStartDate"])
                : null;

            string version = row.Table.Columns.Contains("SoftwareVersionMajor") && row["SoftwareVersionMajor"] != DBNull.Value
                ? row["SoftwareVersionMajor"].ToString()
                : null;

            return (start, version);
        }

        public (string LogicalDataName, string LogicalLogName) RestoreFileListOnly(string path)
        {
            const string sql = @"USE master; RESTORE FILELISTONLY FROM DISK = @path;";
            var dt = _acceso.Leer(sql, new[]
            {
                new SqlParameter("@path", SqlDbType.NVarChar) { Value = path }
            }, CommandType.Text);

            string dataName = null, logName = null;
            foreach (DataRow r in dt.Rows)
            {
                var type = r["Type"]?.ToString(); // 'D' = data, 'L' = log
                var name = r["LogicalName"]?.ToString();
                if (type == "D" && dataName == null) dataName = name;
                if (type == "L" && logName == null) logName = name;
            }

            return (dataName, logName);
        }

        public DateTime? RestoreDatabaseFull(
    string backupPath, string dbName,
    string logicalData, string logicalLog,
    string overrideDataPath = null, string overrideLogPath = null)
        {
            const string sql = @"
SET NOCOUNT ON;
USE master;

DECLARE @db  sysname         = @dbName;
DECLARE @ld  sysname         = @logicalData;   -- logical data name (del FILELISTONLY)
DECLARE @ll  sysname         = @logicalLog;    -- logical log  name
DECLARE @bak nvarchar(4000)  = @backupPath;

-- Rutas por defecto del motor (si no pasás overrides)
DECLARE @defData nvarchar(4000) = CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS nvarchar(4000));
DECLARE @defLog  nvarchar(4000) = CAST(SERVERPROPERTY('InstanceDefaultLogPath')  AS nvarchar(4000));

IF @defData IS NULL SET @defData = N'';
IF @defLog  IS NULL SET @defLog  = N'';

DECLARE @dp nvarchar(4000) = COALESCE(@overrideDataPath,
    @defData + CASE WHEN RIGHT(@defData,1) IN (N':', N'\', N'/') THEN N'' ELSE N'\' END + @db + N'.mdf');

DECLARE @lp nvarchar(4000) = COALESCE(@overrideLogPath,
    @defLog  + CASE WHEN RIGHT(@defLog,1)  IN (N':', N'\', N'/') THEN N'' ELSE N'\' END + @db + N'_log.ldf');

BEGIN TRY
    -- Tomo control
    DECLARE @sql nvarchar(max) =
        N'ALTER DATABASE ' + QUOTENAME(@db) + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';
    EXEC (@sql);

    -- Verifico el .bak
    RESTORE VERIFYONLY FROM DISK = @bak;

    -- RESTORE con MOVE. OJO: los nombres lógicos deben ir como literales.
    SET @sql = N'RESTORE DATABASE ' + QUOTENAME(@db) + N'
      FROM DISK = @p
      WITH REPLACE, RECOVERY, STATS = 5,
           MOVE N''' + REPLACE(@ld, N'''', N'''''') + N''' TO @dp,
           MOVE N''' + REPLACE(@ll, N'''', N'''''') + N''' TO @lp;';

    EXEC sp_executesql
        @sql,
        N'@p nvarchar(4000), @dp nvarchar(4000), @lp nvarchar(4000)',
        @p=@bak, @dp=@dp, @lp=@lp;

END TRY
BEGIN CATCH
    -- Devuelvo MULTI_USER y propago error real
    BEGIN TRY
        DECLARE @sql2 nvarchar(max) =
            N'ALTER DATABASE ' + QUOTENAME(@db) + N' SET MULTI_USER;';
        EXEC (@sql2);
    END TRY BEGIN CATCH END CATCH;

    DECLARE @errmsg nvarchar(2048) = N'RESTORE FAILED: ' + ERROR_MESSAGE();
    RAISERROR(@errmsg, 16, 1);
END CATCH;

-- MULTI_USER al final
BEGIN TRY
    DECLARE @sql3 nvarchar(max) =
        N'ALTER DATABASE ' + QUOTENAME(@db) + N' SET MULTI_USER;';
    EXEC (@sql3);
END TRY BEGIN CATCH END CATCH;

-- Confirmación: fecha del último restore
SELECT MAX(rh.restore_date)
FROM msdb.dbo.restorehistory rh
WHERE rh.destination_database_name = @dbName;
";

            var dt = _acceso.Leer(sql, new[]
            {
        new SqlParameter("@backupPath",       SqlDbType.NVarChar) { Value = backupPath },
        new SqlParameter("@dbName",           SqlDbType.NVarChar) { Value = dbName },
        new SqlParameter("@logicalData",      SqlDbType.NVarChar) { Value = logicalData },
        new SqlParameter("@logicalLog",       SqlDbType.NVarChar) { Value = logicalLog },
        new SqlParameter("@overrideDataPath", SqlDbType.NVarChar) { Value = (object)overrideDataPath ?? DBNull.Value },
        new SqlParameter("@overrideLogPath",  SqlDbType.NVarChar) { Value = (object)overrideLogPath  ?? DBNull.Value },
    }, CommandType.Text);

            if (dt.Rows.Count == 0 || dt.Rows[0][0] == DBNull.Value) return null;
            return Convert.ToDateTime(dt.Rows[0][0]);
        }
    }
}