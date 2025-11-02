<%@ Page Title="Pago exitoso" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PagoSubastaSuccess.aspx.cs" Inherits="Artify.PagoSubastaSuccess" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .panel {
      background: #111827;
      border-radius: 16px;
      box-shadow: 0 12px 36px rgba(0,0,0,.55);
      padding: 32px 40px;
      color: #e5e7eb;
      max-width: 620px;
      margin: 60px auto;
      text-align: center;
    }

    .header {
      background: linear-gradient(135deg,#22c55e 0%,#16a34a 100%);
      color: #fff;
      padding: 16px 26px;
      border-radius: 14px;
      font-size: 1.6rem;
      font-weight: 600;
      text-align: center;
      margin-bottom: 28px;
    }

    .icono-ok {
      font-size: 64px;
      color: #22c55e;
      margin-bottom: 18px;
    }

    .mensaje {
      font-size: 1.1rem;
      color: #e5e7eb;
      margin-bottom: 18px;
    }

    .obra {
      font-weight: 600;
      font-size: 1.05rem;
      color: #facc15;
      margin-bottom: 20px;
    }

    .btn-volver {
      background: #2563eb;
      color: #fff;
      border: none;
      padding: 12px 26px;
      border-radius: 10px;
      font-weight: 600;
      transition: .2s;
    }

    .btn-volver:hover {
      background: #1e40af;
    }
  </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <div class="header">
    <asp:Literal ID="litHeader" runat="server" Text="¡Pago exitoso!"></asp:Literal>
  </div>

  <div class="panel">
    <div class="icono-ok">✔</div>
    <p class="mensaje">
      <asp:Literal ID="litMensaje" runat="server"
        Text="Tu pago fue procesado correctamente."></asp:Literal>
    </p>
    <p class="obra">
      <asp:Literal ID="litObra" runat="server"
        Text="Obra: —"></asp:Literal>
    </p>

    <p class="mensaje">
      <asp:Literal ID="litDetalle" runat="server"
        Text="En breve recibirás la confirmación y el artista será notificado."></asp:Literal>
    </p>

    <asp:Button ID="btnVolver" runat="server" CssClass="btn-volver"
                Text="Volver al inicio"
                OnClick="btnVolver_Click" />
  </div>
</asp:Content>
