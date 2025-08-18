using BE.Observer;
using BLL;
using System;
using System.Configuration;
using System.Globalization;
using System.Web.UI;

namespace Artify
{
    public partial class BackupDB : BasePage
    {
        private readonly BackupBLL _bll = new BackupBLL();

        private string TargetDbName =>
            ConfigurationManager.AppSettings["BackupTargetDbName"] ?? "Artify";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            RegisterLocalizablesById(this, "backup");

            generate.Click += Generate_Click;
            clear.Click += Clear_Click;

            valDestination.ServerValidate += (s, a) =>
            {
                a.IsValid = !string.IsNullOrWhiteSpace(txtDestination.Text);
            };
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
            valSummary.HeaderText = IdiomaManager.Instance.T("backup.valSummaryHeader");
            valDestination.ErrorMessage = IdiomaManager.Instance.T("backup.valDestination");
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                message.Text = IdiomaManager.Instance.T("backup.msgFail");
                return;
            }

            try
            {
                var resp = _bll.GenerateBackup(TargetDbName, txtDestination.Text);

                if (!resp.Exito || resp.Data == null)
                {
                    message.Text = IdiomaManager.Instance.T(resp.MessageKey ?? "backup.msgFail");
                    return;
                }

                // actualizar UI
                lblBackupDate.Text = resp.Data.BackupDate.ToString("yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
                lblFileName.Text = resp.Data.FileName;
                lblSize.Text = resp.Data.SizeBytes > 0
                    ? $"{(resp.Data.SizeBytes / 1024d / 1024d):F2} MB"
                    : "—";

                message.Text = IdiomaManager.Instance.T(resp.MessageKey ?? "backup.msgOk");

                // bitácora
                try
                {
                    var usr = Session["Usuario"] as BE.Usuario;
                    var bitacoraBLL = new BitacoraBLL();
                    var entrada = new BE.Bitacora
                    {
                        Fecha = DateTime.Now,
                        Accion = resp.Exito
                            ? $"BACKUP generado en '{resp.Data?.FullPath ?? txtDestination.Text}' para base '{TargetDbName}'."
                            : $"ERROR al generar BACKUP en '{txtDestination.Text}' para base '{TargetDbName}'.",
                        Criticidad = resp.Exito ? (int)BE.Criticidad.Moderada : (int)BE.Criticidad.Alta,
                        Modulo = "BackupDB",
                        IdUsuario = usr?.Id ?? 0
                    };
                    bitacoraBLL.RegistrarAccion(entrada);
                }
                catch { /* no romper flujo si bitácora falla */ }
            }
            catch
            {
                message.Text = IdiomaManager.Instance.T("backup.msgFail");
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ResetSummary();
            txtDestination.Text = string.Empty;
            message.Text = IdiomaManager.Instance.T("backup.msgCleared");
        }

        private void ResetSummary()
        {
            lblBackupDate.Text = "—";
            lblFileName.Text = "—";
            lblSize.Text = "—";
            message.Text = string.Empty;
        }
    }
}
