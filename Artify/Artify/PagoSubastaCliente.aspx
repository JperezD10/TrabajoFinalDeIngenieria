<%@ Page Title="Pago de Subasta" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PagoSubastaCliente.aspx.cs" Inherits="Artify.PagoSubastaCliente" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .panel {
      background: #111827;
      border-radius: 16px;
      box-shadow: 0 12px 36px rgba(0,0,0,.55);
      padding: 28px 36px;
      color: #e5e7eb;
      max-width: 680px;
      margin: 40px auto;
    }

    .header {
      background: linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);
      color: #fff;
      padding: 14px 26px;
      border-radius: 14px;
      font-size: 1.5rem;
      font-weight: 600;
      text-align: center;
      margin-bottom: 28px;
    }

    .obra-info {
      display: flex;
      align-items: center;
      justify-content: space-between;
      flex-wrap: wrap;
      border: 1px solid #334155;
      border-radius: 12px;
      padding: 16px 20px;
      margin-bottom: 18px;
    }

    .obra-datos {
      flex: 1;
      margin-right: 18px;
    }

    .obra-titulo {
      font-size: 1.25rem;
      font-weight: 600;
      margin-bottom: 4px;
      color: #fff;
    }

    .obra-artista {
      color: #9ca3af;
      font-size: .95rem;
      margin-bottom: 10px;
    }

    .obra-monto {
      font-weight: 700;
      font-size: 1.1rem;
      color: #facc15;
    }

    .btn-pagar {
      background: #2563eb;
      color: #fff;
      border: none;
      padding: 12px 24px;
      border-radius: 10px;
      font-weight: 600;
      transition: .2s;
    }

    .btn-pagar:hover {
      background: #1e40af;
    }

    .mensaje-info {
      margin-top: 12px;
      font-size: .95rem;
      color: #9ca3af;
    }
  </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  <div class="header">
    <asp:Literal ID="litHeader" runat="server" Text="Pago de subasta ganada"></asp:Literal>
  </div>

  <div class="panel">
    <h3><asp:Literal ID="litTitulo" runat="server" Text="Confirmá el pago de la obra adjudicada"></asp:Literal></h3>
    <p class="mensaje-info">
      <asp:Literal ID="litSubtitulo" runat="server"
        Text="Al confirmar el pago, la compra quedará registrada y el artista será notificado."></asp:Literal>
    </p>

    <div class="obra-info">
      <div class="obra-datos">
        <div class="obra-titulo"><asp:Literal ID="litObraTitulo" runat="server" Text="Nombre de la obra"></asp:Literal></div>
        <div class="obra-artista"><asp:Literal ID="litObraArtista" runat="server" Text="Artista"></asp:Literal></div>
        <div class="obra-monto"><asp:Literal ID="litObraMonto" runat="server" Text="US$ 0"></asp:Literal></div>
      </div>

      <asp:Button ID="btnPagar" runat="server" CssClass="btn-pagar"
                  Text="Confirmar pago"
                  OnClick="btnPagar_Click" />
    </div>

    <p class="mensaje-info">
      <asp:Literal ID="litInfoFinal" runat="server"
        Text="Una vez procesado el pago, recibirás la confirmación por correo electrónico."></asp:Literal>
    </p>
  </div>
</asp:Content>
