<%@ Page Title="Estadísticas de facturación" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="EstadisticasCurador.aspx.cs" Inherits="Artify.EstadisticasCurador" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .panel {
      background: #111827;
      border-radius: 16px;
      box-shadow: 0 12px 36px rgba(0,0,0,.55);
      padding: 32px 40px;
      color: #e5e7eb;
      max-width: 640px;
      margin: 60px auto;
    }

    .header {
      background: linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);
      color: #fff;
      padding: 16px 26px;
      border-radius: 14px;
      font-size: 1.5rem;
      font-weight: 600;
      text-align: center;
      margin-bottom: 28px;
    }

    .form-row {
      display: flex;
      justify-content: space-between;
      gap: 16px;
      margin-bottom: 20px;
    }

    .input-date {
      flex: 1;
      padding: 10px;
      border-radius: 8px;
      border: 1px solid #334155;
      background: #1f2937;
      color: #e5e7eb;
    }

    .btn {
      background: #2563eb;
      color: #fff;
      border: none;
      padding: 12px 24px;
      border-radius: 10px;
      font-weight: 600;
      cursor: pointer;
      transition: .2s;
    }

    .btn:hover {
      background: #1e40af;
    }

    .resultado {
      text-align: center;
      font-size: 1.2rem;
      margin-top: 24px;
      color: #facc15;
    }
  </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <div class="header">
    <asp:Literal ID="litHeader" runat="server" Text="Estadísticas de facturación"></asp:Literal>
  </div>

  <div class="panel">
    <div class="form-row">
      <asp:Label ID="lblDesde" runat="server" AssociatedControlID="txtDesde"
                 Text="Desde:"></asp:Label>
      <asp:TextBox ID="txtDesde" runat="server" CssClass="input-date" TextMode="Date"></asp:TextBox>

      <asp:Label ID="lblHasta" runat="server" AssociatedControlID="txtHasta"
                 Text="Hasta:"></asp:Label>
      <asp:TextBox ID="txtHasta" runat="server" CssClass="input-date" TextMode="Date"></asp:TextBox>
    </div>

    <div style="text-align:center;">
      <asp:Button ID="btnCalcular" runat="server" CssClass="btn"
                  Text="Calcular promedio" OnClick="btnCalcular_Click" />
    </div>

    <div class="resultado">
      <asp:Literal ID="litResultado" runat="server" Text=""></asp:Literal>
    </div>
  </div>
</asp:Content>
