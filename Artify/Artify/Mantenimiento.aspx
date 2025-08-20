<%@ Page Title="Mantenimiento" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Mantenimiento.aspx.cs" Inherits="Artify.Mantenimiento" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .maint-wrap{display:flex;align-items:center;justify-content:center;min-height:60vh}
    .maint-card{
      max-width:720px;width:100%;
      background:linear-gradient(180deg, rgba(17,24,39,0.85), rgba(15,23,42,0.85));
      border:1px solid #334155;border-radius:16px;padding:28px;
      box-shadow:0 18px 60px rgba(0,0,0,.45)
    }
    .maint-icon{
      width:56px;height:56px;border-radius:14px;
      display:flex;align-items:center;justify-content:center;
      background:linear-gradient(135deg,#ef4444 0%,#b91c1c 100%);
      color:#fff;font-weight:900;font-size:28px;margin-bottom:14px
    }
    .maint-title{margin:0;color:#f3f4f6;font-size:1.8rem;font-weight:900;letter-spacing:.2px}
    .maint-sub{margin:6px 0 18px 0;color:#cbd5e1;font-size:1rem}
    .maint-box{
      background:rgba(2,6,23,.5);border:1px dashed #475569;border-radius:12px;padding:14px;margin-top:6px
    }
    .maint-kv{display:flex;gap:10px;flex-wrap:wrap}
    .maint-kv div{flex:1 1 220px;color:#e5e7eb}
    .maint-kv small{display:block;color:#94a3b8}
    .maint-actions{display:flex;gap:10px;margin-top:18px;flex-wrap:wrap}
    .btn{display:inline-flex;align-items:center;gap:8px;padding:10px 14px;border-radius:10px;
         cursor:pointer;text-decoration:none;font-weight:800;border:1px solid #334155}
    .btn-primary{background:linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);color:#fff;border-color:transparent}
    .btn-plain{background:#0b1220;color:#e5e7eb}
  </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="maint-wrap">
    <div class="maint-card">

      <div class="maint-icon" aria-hidden="true">!</div>

      <h1 class="maint-title">
        <asp:Literal ID="litTitle" runat="server" />
      </h1>

      <p class="maint-sub">
        <asp:Literal ID="litSubtitle" runat="server" />
      </p>

      <div class="maint-box">
        <div class="maint-kv">
          <div>
            <small><asp:Label ID="lblItem1Label" runat="server" /></small>
            <span><asp:Literal ID="lblItem1Value" runat="server" /></span>
          </div>
          <div>
            <small><asp:Label ID="lblItem2Label" runat="server" /></small>
            <span><asp:Literal ID="lblItem2Value" runat="server" /></span>
          </div>
        </div>
      </div>

      <div class="maint-actions">
        <asp:HyperLink ID="btnHome"
                 runat="server"
                 NavigateUrl="~/HomeCliente.aspx"
                 CssClass="btn btn-plain" />

      <p class="maint-sub" style="margin-top:14px">
        <asp:Literal ID="litNote" runat="server" />
      </p>

    </div>
  </div>
</asp:Content>