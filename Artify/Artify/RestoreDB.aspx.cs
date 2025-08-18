using BE.Observer;
using BLL;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace Artify
{
    public partial class RestoreDB : BasePage
    {
        private readonly RestoreBLL _bll = new RestoreBLL();

        private string DropFolder =>
            ConfigurationManager.AppSettings["RestoreDropFolder"]
            ?? Server.MapPath("~/App_Data/Restores");

        private string TargetDbName =>
            ConfigurationManager.AppSettings["RestoreTargetDbName"] ?? "Artify";

        private string SqlDataDir =>
            ConfigurationManager.AppSettings["RestoreSqlDataDir"] ?? @"C:\SqlData";

        private string SqlLogDir =>
            ConfigurationManager.AppSettings["RestoreSqlLogDir"] ?? @"C:\SqlLogs";

        private int MaxSizeMb =>
            int.TryParse(ConfigurationManager.AppSettings["RestoreMaxSizeMB"], out var mb) ? mb : 2048;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            RegisterLocalizablesById(this, "restore");

            // solo estos dos botones ahora
            restore.Click += Restore_Click;
            clear.Click += Clear_Click;

            // Validaciones mínimas (archivo, ext, tamaño, confirm)
            valHasFile.ServerValidate += (s, a) => a.IsValid = file != null && file.HasFile;
            valExt.ServerValidate += (s, a) =>
            {
                if (file == null || !file.HasFile) { a.IsValid = true; return; }
                a.IsValid = string.Equals(Path.GetExtension(file.FileName), ".bak", StringComparison.OrdinalIgnoreCase);
            };
            valSize.ServerValidate += (s, a) =>
            {
                if (file == null || !file.HasFile) { a.IsValid = true; return; }
                a.IsValid = file.PostedFile.ContentLength <= (long)MaxSizeMb * 1024L * 1024L;
            };
            valConfirm.ServerValidate += (s, a) => a.IsValid = confirm.Checked;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ApplyNonObserverTexts();
                ResetSummary();
            }
        }

        private void ApplyNonObserverTexts()
        {
            valSummary.HeaderText = IdiomaManager.Instance.T("restore.valSummaryHeader");
            valHasFile.ErrorMessage = IdiomaManager.Instance.T("restore.valHasFile");
            valExt.ErrorMessage = IdiomaManager.Instance.T("restore.valExt");
            valSize.ErrorMessage = string.Format(CultureInfo.CurrentCulture,
                                       IdiomaManager.Instance.T("restore.valSize"), MaxSizeMb);
            valConfirm.ErrorMessage = IdiomaManager.Instance.T("restore.valConfirm");
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            // 1) validaciones básicas
            Page.Validate();
            if (!Page.IsValid)
            {
                message.Text = IdiomaManager.Instance.T("restore.message");
                return;
            }

            try
            {
                // 2) Guardar el .bak en carpeta accesible para SQL Server
                Directory.CreateDirectory(DropFolder);
                var safeName = Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(DropFolder, $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{safeName}");
                file.SaveAs(fullPath);

                // 3) Analizar lo mínimo (HEADERONLY/FILELISTONLY) y restaurar en el mismo flujo
                var analyzeResp = _bll.AnalyzeBackup(fullPath);
                if (!analyzeResp.Exito)
                {
                    ResetSummary();
                    message.Text = IdiomaManager.Instance.T(analyzeResp.MessageKey ?? "restore.msgAnalyzeFail");
                    return;
                }

                var data = analyzeResp.Data;

                // 3.1. Update UI con info útil (no obligatorio)
                lblBackupDate.Text = data.BackupStartDate?.ToString("yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture) ?? "—";
                lblSqlVersion.Text = data.SqlVersion ?? "—";
                var sizeMb = new FileInfo(fullPath).Length / 1024d / 1024d;
                lblSize.Text = $"{sizeMb:F2} MB";

                // 4) Ejecutar RESTORE directamente
                var restoreResp = _bll.RestoreDatabase(
                    backupFullPath: fullPath,
                    targetDbName: TargetDbName,
                    logicalData: data.LogicalDataName,
                    logicalLog: data.LogicalLogName,
                    targetDataPath: Path.Combine(SqlDataDir, $"{TargetDbName}.mdf"),
                    targetLogPath: Path.Combine(SqlLogDir, $"{TargetDbName}_log.ldf")
                );
                var usr = Session["Usuario"] as BE.Usuario; 
                var bitacoraBLL = new BitacoraBLL();

                var entrada = new BE.Bitacora
                {
                    Fecha = DateTime.Now,
                    Accion = restoreResp.Exito
                        ? $"RESTORE ejecutado correctamente desde '{Path.GetFileName(fullPath)}' sobre la base '{TargetDbName}'."
                        : $"FALLO de RESTORE desde '{Path.GetFileName(fullPath)}' sobre la base '{TargetDbName}'.",
                    Criticidad = (int)BE.Criticidad.Alta,         
                    Modulo = "RestoreDB",                     
                    IdUsuario = usr?.Id ?? 0                  
                };

                bitacoraBLL.RegistrarAccion(entrada);
                message.Text = IdiomaManager.Instance.T(restoreResp.MessageKey ?? (restoreResp.Exito ? "restore.msgRestoreOk" : "restore.msgRestoreFail"));
            }
            catch
            {
                message.Text = IdiomaManager.Instance.T("restore.msgRestoreFail");
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ResetSummary();
            confirm.Checked = false;
            message.Text = IdiomaManager.Instance.T("restore.msgCleared");
        }

        private void ResetSummary()
        {
            message.Text = string.Empty;
            lblBackupDate.Text = "—";
            lblSize.Text = "—";
            lblSqlVersion.Text = "—";
        }
    }
}
