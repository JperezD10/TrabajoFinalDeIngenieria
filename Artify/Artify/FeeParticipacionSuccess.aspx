<%@ Page Title="Pago confirmado" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="FeeParticipacionSuccess.aspx.cs" Inherits="Artify.FeeParticipacionSuccess" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .hero {
      background: var(--panel);
      border: 1px solid var(--border);
      border-radius: 16px;
      padding: 24px 20px;
      box-shadow: var(--shadow);
      margin-bottom: 18px;
      text-align: center;
    }
    .title {
      margin: 0 0 10px 0;
      font-size: 1.8rem;
      font-weight: 900;
      color: #f9fafb;
    }
    .muted {
      color: var(--muted);
      margin-bottom: 12px;
    }
    .btn {
      display: inline-block;
      padding: 10px 18px;
      border-radius: 10px;
      border: 1px solid var(--border);
      font-weight: 800;
      color: #e5e7eb;
      background: #0b1220;
      text-decoration: none;
      cursor: pointer;
    }
    .btn:hover {
      filter: brightness(1.08);
    }
    .ok {
      color: #86efac;
      font-size: 1rem;
      font-weight: 600;
    }
    .err {
      color: #fca5a5;
      font-size: 1rem;
      font-weight: 600;
    }
  </style>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
  <div class="hero">
    <h2 class="title"><asp:Literal ID="litTitle" runat="server" /></h2>
    <p class="muted"><asp:Literal ID="litDesc" runat="server" /></p>
    <p id="msg" runat="server" />
    <asp:HyperLink ID="lnkVolver" runat="server" CssClass="btn" />
  </div>
</asp:Content>
