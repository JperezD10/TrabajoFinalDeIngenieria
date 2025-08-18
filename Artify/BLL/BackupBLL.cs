using BE;
using DAL;
using System;
using System.Globalization;
using System.IO;

namespace BLL
{
    public class BackupBLL
    {
        private readonly BackupDAL _dal = new BackupDAL();

        
        public Response<BackupResult> GenerateBackup(string databaseName, string destinationFolder)
        {
            if (string.IsNullOrWhiteSpace(destinationFolder))
                return Response<BackupResult>.Error(messageKey: "backup.valDestination");

            try
            {
                // Validación básica de carpeta
                if (!Directory.Exists(destinationFolder))
                    return Response<BackupResult>.Error(messageKey: "backup.errFolderNotFound");

                // Construir nombre de archivo
                var stamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                var safeDb = string.Join("_", (databaseName ?? "Artify").Split(Path.GetInvalidFileNameChars()));
                var fileName = $"backup_{safeDb}_{stamp}.bak";
                var fullPath = Path.Combine(destinationFolder, fileName);

                // Ejecutar backup
                _dal.BackupDatabase(databaseName, fullPath);

                var verify = _dal.TryHeaderOnly(fullPath);
                if (!verify.Ok)
                    return Response<BackupResult>.Error(messageKey: "backup.msgFail"); // o una key más específica

                // Medir tamaño
                long sizeBytes = 0;
                try
                {
                    var fi = new FileInfo(fullPath);
                    if (fi.Exists) sizeBytes = fi.Length;
                }
                catch { /* si falla lectura de tamaño, seguimos igual */ }

                var result = new BackupResult
                {
                    FileName = fileName,
                    FullPath = fullPath,
                    SizeBytes = sizeBytes,
                    BackupDate = DateTime.Now
                };

                return Response<BackupResult>.Success(result, messageKey: "backup.msgOk");
            }
            catch (Exception)
            {
                return Response<BackupResult>.Error(messageKey: "backup.msgFail");
            }
        }

    }
}
