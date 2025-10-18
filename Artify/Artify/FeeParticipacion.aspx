<%@ Page Title="Participación en Subasta" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="FeeParticipacion.aspx.cs" Inherits="Artify.FeeParticipacion" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .hero {
      background: var(--panel);
      border: 1px solid var(--border);
      border-radius: 16px;
      padding: 22px 20px;
      box-shadow: var(--shadow);
      margin-bottom: 18px;
    }
    .title {margin: 0 0 4px 0; font-size: 1.6rem; font-weight: 900; color: #f9fafb;}
    .muted {color: var(--muted); margin: 0 0 12px 0;}
    .card {background: var(--panel); border: 1px solid var(--border); border-radius: 16px; padding: 16px; display: flex; justify-content: space-between; align-items: center;}
    .price {font-weight: 900;}
    .btn {padding: 10px 12px; border-radius: 10px; border: 1px solid var(--border); font-weight: 800; color: #e5e7eb; background: #0b1220; text-decoration: none; cursor: pointer;}
    .btn:hover {filter: brightness(1.08);}
    .badge {display: inline-block; border: 1px solid var(--border); border-radius: 999px; padding: 3px 10px; margin-left: 8px; color: #e9d5ff;}
    .empty {background: var(--panel); border: 1px dashed var(--border); border-radius: 14px; padding: 12px; color: var(--muted); margin-top: 12px;}
    .row {display: flex; gap: 12px; align-items: center;}
  </style>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
  <div class="hero">
    <h2 class="title"><asp:Literal ID="litTitle" runat="server" /></h2>
    <p class="muted"><asp:Literal ID="litDesc" runat="server" /></p>

    <div class="card">
      <div>
        <div class="row">
          <div><strong><asp:Literal ID="litSubastaName" runat="server" /></strong></div>
          <div class="badge"><asp:Literal ID="litBadge" runat="server" /></div>
        </div>
        <div class="muted"><asp:Literal ID="litDetail" runat="server" /></div>
      </div>
      <div class="row">
        <div class="price">US$ 5</div>
        <asp:Button ID="btnConfirm" runat="server" CssClass="btn" OnClick="btnConfirm_Click" />
      </div>
    </div>

    <asp:PlaceHolder ID="phInfo" runat="server" Visible="false">
      <div class="empty"><asp:Literal ID="litInfo" runat="server" /></div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phError" runat="server" Visible="false">
      <div class="empty"><asp:Literal ID="litError" runat="server" /></div>
    </asp:PlaceHolder>
  </div>
</asp:Content>
