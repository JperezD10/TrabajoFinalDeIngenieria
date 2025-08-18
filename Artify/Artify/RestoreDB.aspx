<%@ Page Title="Restore de Base de Datos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="RestoreDB.aspx.cs" Inherits="Artify.RestoreDB" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .topbar{display:flex;align-items:center;justify-content:space-between;margin-bottom:16px}
    .title{margin:0;font-size:1.6rem;font-weight:800;color:#f9fafb}
    .btn{display:inline-flex;align-items:center;gap:8px;padding:10px 14px;border-radius:10px;cursor:pointer;text-decoration:none;font-weight:700;border:1px solid var(--border)}
    .btn-plain{background:#0b1220;color:#e5e7eb;border-color:#334155}
    .btn-danger{background:linear-gradient(135deg,#ef4444 0%,#b91c1c 100%);color:#fff;border-color:#7f1d1d}
    .panel{background:rgba(12,19,33,.75);border:1px solid #1f2a44;border-radius:14px;padding:16px}
    .panel-heading{background:linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);padding:18px 24px;border-radius:14px;text-align:center;margin-bottom:22px;box-shadow:0 8px 24px rgba(0,0,0,.5)}
    .panel-title{margin:0;font-size:1.2rem;font-weight:800;color:#fff}
    .panel-sub{margin:6px 0 0 0;color:#e5e7eb;opacity:.9}
    .grid{display:grid;grid-template-columns:1fr;gap:14px}
    @media(min-width:900px){.grid{grid-template-columns:2fr 1fr}}
    .form-row{display:flex;flex-direction:column;gap:8px}
    .form-row-inline{display:flex;gap:10px;flex-wrap:wrap;align-items:center}
    .filebox{display:flex;align-items:center;gap:10px;background:#0b1220;border:1px dashed #334155;padding:14px;border-radius:12px}
    .hint{font-size:.9rem;color:#9ca3af}
    .badge{display:inline-block;padding:4px 10px;border-radius:999px;background:#0b1220;border:1px solid #334155;color:#e5e7eb}
    .summary{display:grid;grid-template-columns:1fr 1fr;gap:10px}
    .summary .item{background:#0b1220;border:1px solid #334155;border-radius:12px;padding:10px}
    .label{color:#9ca3af;font-size:.85rem;margin-bottom:4px}
    .value{color:#e5e7eb;font-weight:700}
    .alert{border-radius:12px;padding:12px;border:1px solid #334155;background:#0b1220;color:#e5e7eb}
  </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">

  <div class="topbar">
    <h1 class="title"><asp:Literal ID="title" runat="server" Text="Restore de Base de Datos" /></h1>
    <asp:HyperLink ID="back" runat="server" NavigateUrl="~/HomeWebMaster.aspx" CssClass="btn btn-plain" Text="Volver" />
  </div>

  <div class="panel">
    <div class="panel-heading">
      <h2 class="panel-title"><asp:Literal ID="panelTitle" runat="server" Text="Restaurar desde archivo .BAK" /></h2>
      <p class="panel-sub"><asp:Literal ID="panelSub" runat="server" Text="Subí un .bak válido para restaurar la base." /></p>
    </div>

    <div class="alert"><asp:Literal ID="hint" runat="server" Text="Tip: realizá un backup actual antes de continuar." /></div>

    <div class="grid">
      <div class="form-row">
        <span class="label"><asp:Literal ID="selectFile" runat="server" Text="Seleccionar archivo .bak" /></span>
        <div class="filebox">
          <asp:FileUpload ID="file" runat="server" />
          <span class="badge"><asp:Literal ID="accept" runat="server" Text=".bak" /></span>
        </div>
        <span class="hint"><asp:Literal ID="dropHint" runat="server" Text="Elegí el archivo desde tu equipo." /></span>

        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert" ShowMessageBox="false" ShowSummary="true" />
        <asp:CustomValidator ID="valHasFile" runat="server" Display="None" ErrorMessage="Debe seleccionar un archivo." />
        <asp:CustomValidator ID="valExt"     runat="server" Display="None" ErrorMessage="La extensión debe ser .bak." />
        <asp:CustomValidator ID="valSize"    runat="server" Display="None" ErrorMessage="El archivo es demasiado grande." />
        <asp:CustomValidator ID="valConfirm" runat="server" Display="None" ErrorMessage="Debe confirmar antes de continuar." />

        <div class="form-row-inline">
          <asp:CheckBox ID="confirm" runat="server" />
          <asp:Label ID="confirmText" runat="server" AssociatedControlID="confirm" Text="Entiendo que se sobrescribirán datos." />
        </div>

        <div class="form-row-inline">
          <asp:Button ID="analyze" runat="server" CssClass="btn btn-plain" Text="Validar respaldo" Visible="false" />
          <asp:Button ID="restore" runat="server" CssClass="btn btn-danger" Text="Restaurar ahora" />
          <asp:Button ID="clear"   runat="server" CssClass="btn btn-plain" Text="Limpiar" CausesValidation="false" />
        </div>
      </div>

      <div class="form-row">
        <span class="label"><asp:Literal ID="summary" runat="server" Text="Resumen del respaldo" /></span>
        <div class="summary">
          <div class="item">
            <div class="label"><asp:Literal ID="dbTarget" runat="server" Text="Base destino" /></div>
            <div class="value">Artify</div>
          </div>
          <div class="item">
            <div class="label"><asp:Literal ID="backupDate" runat="server" Text="Fecha del backup" /></div>
            <div class="value"><asp:Label ID="lblBackupDate" runat="server" Text="—" /></div>
          </div>
          <div class="item">
            <div class="label"><asp:Literal ID="size" runat="server" Text="Tamaño" /></div>
            <div class="value"><asp:Label ID="lblSize" runat="server" Text="—" /></div>
          </div>
          <div class="item">
            <div class="label"><asp:Literal ID="sqlVersion" runat="server" Text="Versión SQL" /></div>
            <div class="value"><asp:Label ID="lblSqlVersion" runat="server" Text="—" /></div>
          </div>
        </div>

        <div class="alert"><asp:Literal ID="message" runat="server" Text="" /></div>
      </div>
    </div>
  </div>

</asp:Content>
