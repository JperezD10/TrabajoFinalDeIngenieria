using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BackupDAL
    {
        // Si querés, dejá Acceso para otras cosas, pero el BACKUP va directo.
        private static string ConnStr =>
            ConfigurationManager.ConnectionStrings["ArtifyConnection"].ConnectionString;

        public void BackupDatabase(string databaseName, string fullPath, int commandTimeoutSeconds = 600)
        {
            if (string.IsNullOrWhiteSpace(databaseName)) throw new ArgumentNullException(nameof(databaseName));
            if (string.IsNullOrWhiteSpace(fullPath)) throw new ArgumentNullException(nameof(fullPath));

            var safeDb = databaseName.Replace("]", "]]");

            var sql = $@"
USE master;
BACKUP DATABASE [{safeDb}]
TO DISK = @path
WITH INIT, COMPRESSION, STATS = 5;";

            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = commandTimeoutSeconds; // 10 minutos
                cmd.Parameters.Add(new SqlParameter("@path", SqlDbType.NVarChar) { Value = fullPath });

                con.Open();

                // IMPORTANTE: NO ABRIR SqlTransaction ACÁ
                cmd.ExecuteNonQuery(); // si falla, lanza SqlException (no la atrapes acá)
            }
        }

        public (bool Ok, DateTime? BackupStartDate) TryHeaderOnly(string fullPath, int commandTimeoutSeconds = 120)
        {
            const string sql = @"USE master; RESTORE HEADERONLY FROM DISK = @path;";
            using (var con = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.CommandTimeout = commandTimeoutSeconds;
                cmd.Parameters.Add(new SqlParameter("@path", SqlDbType.NVarChar) { Value = fullPath });

                con.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new System.Data.DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) return (false, null);

                    var row = dt.Rows[0];
                    DateTime? start =
                        row.Table.Columns.Contains("BackupStartDate") && row["BackupStartDate"] != DBNull.Value
                        ? (DateTime?)Convert.ToDateTime(row["BackupStartDate"])
                        : null;

                    return (true, start);
                }
            }
        }
    }
}
