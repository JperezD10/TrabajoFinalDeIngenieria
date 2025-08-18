<%@ Page Title="Backup de Base de Datos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BackupDB.aspx.cs" Inherits="Artify.BackupDB" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .topbar{display:flex;align-items:center;justify-content:space-between;margin-bottom:16px}
    .title{margin:0;font-size:1.6rem;font-weight:800;color:#f9fafb}

    .btn{display:inline-flex;align-items:center;gap:8px;padding:10px 14px;border-radius:10px;cursor:pointer;text-decoration:none;font-weight:700;border:1px solid var(--border)}
    .btn-plain{background:#0b1220;color:#e5e7eb;border-color:#334155}
    .btn-primary{background:linear-gradient(135deg,#22c55e 0%,#16a34a 100%);color:#fff;border-color:#166534}

    .panel{background:rgba(12,19,33,.75);border:1px solid #1f2a44;border-radius:14px;padding:16px}
    .panel-heading{background:linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);padding:18px 24px;border-radius:14px;text-align:center;margin-bottom:22px;box-shadow:0 8px 24px rgba(0,0,0,.5)}
    .panel-title{margin:0;font-size:1.2rem;font-weight:800;color:#fff}
    .panel-sub{margin:6px 0 0 0;color:#e5e7eb;opacity:.9}

    .form-row{display:flex;flex-direction:column;gap:10px}
    .form-row-inline{display:flex;gap:10px;flex-wrap:wrap;align-items:center}

    .label{color:#9ca3af;font-size:.85rem;margin-bottom:4px}
    .value{color:#e5e7eb;font-weight:700}
    .hint{font-size:.9rem;color:#9ca3af}
    .alert{border-radius:12px;padding:12px;border:1px solid #334155;background:#0b1220;color:#e5e7eb;margin-bottom:14px}

    .summary{display:grid;grid-template-columns:1fr 1fr;gap:10px}
    .summary .item{background:#0b1220;border:1px solid #334155;border-radius:12px;padding:10px}

    .textbox{background:#0b1220;border:1px solid #334155;border-radius:10px;padding:10px;color:#e5e7eb;outline:none}
    .textbox:focus{border-color:#64748b;box-shadow:0 0 0 3px rgba(100,116,139,.25)}
    .textbox::placeholder{color:#6b7280}
  </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">

  <div class="topbar">
    <h1 class="title"><asp:Literal ID="title" runat="server" Text="Backup de Base de Datos" /></h1>
    <asp:HyperLink ID="back" runat="server" NavigateUrl="~/HomeWebMaster.aspx" CssClass="btn btn-plain" Text="Volver" />
  </div>

  <div class="panel">
    <div class="panel-heading">
      <h2 class="panel-title"><asp:Literal ID="panelTitle" runat="server" Text="Generar backup" /></h2>
      <p class="panel-sub"><asp:Literal ID="panelSub" runat="server" Text="Creá un archivo .bak con la copia de seguridad." /></p>
    </div>

    <div class="alert"><asp:Literal ID="hint" runat="server" Text="Tip: realizá backups periódicos para mayor seguridad." /></div>

    <div class="form-row">
      <span class="label"><asp:Literal ID="destination" runat="server" Text="Carpeta de destino" /></span>

      <asp:TextBox ID="txtDestination" runat="server" CssClass="textbox"
                   placeholder="Ej: C:\SqlBackups\Artify" />

      <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert" ShowMessageBox="false" ShowSummary="true" />
      <asp:CustomValidator ID="valDestination" runat="server" Display="None" ErrorMessage="Debe ingresar una carpeta de destino." />

      <span class="hint">
        <asp:Literal ID="destHint" runat="server"
          Text="Elegí una carpeta del SERVIDOR donde guardar el .bak (no la PC del cliente)." />
      </span>

      <div class="form-row-inline">
        <asp:Button ID="generate" runat="server" CssClass="btn btn-primary" Text="Generar backup" />
        <asp:Button ID="clear" runat="server" CssClass="btn btn-plain" Text="Limpiar" CausesValidation="false" />
      </div>
    </div>

    <div class="form-row" style="margin-top:18px">
      <span class="label"><asp:Literal ID="summary" runat="server" Text="Resumen del último backup" /></span>
      <div class="summary">
        <div class="item">
          <div class="label"><asp:Literal ID="dbTarget" runat="server" Text="Base respaldada" /></div>
          <div class="value">Artify</div>
        </div>
        <div class="item">
          <div class="label"><asp:Literal ID="backupDate" runat="server" Text="Fecha" /></div>
          <div class="value"><asp:Label ID="lblBackupDate" runat="server" Text="—" /></div>
        </div>
        <div class="item">
          <div class="label"><asp:Literal ID="fileName" runat="server" Text="Archivo" /></div>
          <div class="value"><asp:Label ID="lblFileName" runat="server" Text="—" /></div>
        </div>
        <div class="item">
          <div class="label"><asp:Literal ID="size" runat="server" Text="Tamaño" /></div>
          <div class="value"><asp:Label ID="lblSize" runat="server" Text="—" /></div>
        </div>
      </div>

      <div class="alert"><asp:Literal ID="message" runat="server" Text="" /></div>
    </div>
  </div>

</asp:Content>
