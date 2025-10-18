<%@ Page Title="Pago no completado" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="FeeParticipacionError.aspx.cs" Inherits="Artify.FeeParticipacionError" %>

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
      color: #f87171;
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
    .icon {
      font-size: 42px;
      color: #ef4444;
      margin-bottom: 12px;
    }
  </style>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
  <div class="hero">
    <div class="icon">✕</div>
    <h2 class="title"><asp:Literal ID="litTitle" runat="server" /></h2>
    <p class="muted"><asp:Literal ID="litDesc" runat="server" /></p>
    <asp:HyperLink ID="lnkVolver" runat="server" CssClass="btn" />
  </div>
</asp:Content>
