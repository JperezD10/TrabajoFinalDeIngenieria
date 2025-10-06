<%@ Page Title="Subastas pendientes" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="SubastasPendientes.aspx.cs"
    Inherits="Artify.SubastasPendientes" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    /* Heading band */
    .band { background:linear-gradient(90deg,#7c3aed 0%,#2563eb 100%); padding:18px 22px; border-radius:16px; box-shadow:0 12px 30px rgba(0,0,0,.35); margin-bottom:24px; color:#fff; }
    .band-head { display:flex; align-items:center; gap:12px; }
    .band-title { font-size:26px; font-weight:900; letter-spacing:.3px; }
    .back { display:inline-flex; align-items:center; gap:8px; padding:6px 10px; border-radius:10px; background:rgba(255,255,255,.12); color:#fff; text-decoration:none; font-weight:700; }
    .back:hover { background:rgba(255,255,255,.18); }
    .back svg { width:18px; height:18px; }
    /* Card container */
    .card {
      background: #0b1220; border: 1px solid rgba(255,255,255,.08);
      border-radius: 16px; padding: 18px; box-shadow: 0 8px 24px rgba(0,0,0,.25);
    }
    /* Messages */
    .msg { border-radius: 12px; padding: 12px 14px; margin-bottom: 12px; }
    .msg-ok  { background: rgba(16,185,129,.12); border:1px solid rgba(16,185,129,.35); color:#a7f3d0; }
    .msg-err { background: rgba(239,68,68,.12); border:1px solid rgba(252,165,165,.45); color:#fecaca; }

    /* Table styles */
    .table-modern {
      width:100%; border-collapse: separate; border-spacing:0;
      overflow:hidden; border-radius:12px; font-size:14px;
    }
    .table-modern thead th {
      background:#101a2f; color:#e5e7eb; text-align:left;
      padding:12px 14px; border-bottom:1px solid rgba(255,255,255,.08);
      font-weight:700;
    }
    .table-modern tbody td {
      padding:12px 14px; color:#d1d5db; border-bottom:1px solid rgba(255,255,255,.06);
    }
    .table-modern tbody tr:nth-child(even){ background:#0e162a; }
    .table-modern tbody tr:hover { background:#14203a; }
    .table-modern .cell-right { text-align:right; }
    .table-modern .cell-center { text-align:center; }

    /* Button */
    .btn { display:inline-block; padding:8px 12px; border-radius:10px;
           font-weight:700; text-decoration:none; border:0; cursor:pointer; }
    .btn-primary {
      background:linear-gradient(90deg,#7c3aed,#6d28d9 60%, #4f46e5);
      color:#fff; box-shadow:0 6px 18px rgba(124,58,237,.35);
    }
    .btn-primary:hover { filter:brightness(1.08); }

    /* Empty template */
    .empty {
      padding:22px; text-align:center; color:#9ca3af;
    }
    .empty a { margin-left:6px; }
  </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="band">
    <div class="band-head">
      <asp:HyperLink ID="lnkBackHome" runat="server" NavigateUrl="~/HomeCurador.aspx" CssClass="back">
        <svg viewBox="0 0 24 24" fill="none">
          <path d="M15 19l-7-7 7-7" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
        <asp:Literal ID="litBack" runat="server" />
      </asp:HyperLink>
      <span class="band-title"><asp:Literal ID="litTitle" runat="server" /></span>
    </div>
  </div>

  <asp:Panel ID="pnlMsgOk" runat="server" CssClass="msg msg-ok" Visible="false">
    <asp:Literal ID="litOk" runat="server" />
  </asp:Panel>
  <asp:Panel ID="pnlMsgErr" runat="server" CssClass="msg msg-err" Visible="false">
    <asp:Literal ID="litErr" runat="server" />
  </asp:Panel>

  <div class="card">
    <asp:GridView ID="gvPendientes" runat="server"
      AutoGenerateColumns="false" CssClass="table-modern" DataKeyNames="Id"
      GridLines="None"
      OnRowCommand="gvPendientes_RowCommand"
      OnRowDataBound="gvPendientes_RowDataBound">

      <Columns>
        <asp:BoundField DataField="Id" HeaderText="ID" />
        <asp:BoundField DataField="IdObra" HeaderText="Artwork" />
        <asp:BoundField DataField="PrecioInicial" HeaderText="Starting price"
                        DataFormatString="{0:0.00}" HtmlEncode="false" />
        <asp:BoundField DataField="IncrementoMinimo" HeaderText="Min increment"
                        DataFormatString="{0:0.00}" HtmlEncode="false" />
        <asp:BoundField DataField="DuracionMinutos" HeaderText="Duration (min)" />
        <asp:BoundField DataField="FechaCreacion" HeaderText="Created"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" HtmlEncode="false" />
        <asp:BoundField DataField="FechaProgramadaInicio" HeaderText="Scheduled"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" HtmlEncode="false" />
        <asp:TemplateField HeaderText="Actions">
          <ItemTemplate>
            <asp:LinkButton ID="btnStart" runat="server"
                CommandName="Start" CommandArgument='<%# Eval("Id") %>'
                CssClass="btn btn-primary" />
          </ItemTemplate>
          <ItemStyle CssClass="cell-center" />
          <HeaderStyle CssClass="cell-center" />
        </asp:TemplateField>
      </Columns>

      <EmptyDataTemplate>
        <div class="empty">
          <asp:Literal ID="litEmpty" runat="server" Text="No tenés subastas pendientes." /> 
          <asp:HyperLink runat="server" NavigateUrl="~/CrearSubasta.aspx" CssClass="btn btn-primary"
            Text="Crear subasta" />
        </div>
      </EmptyDataTemplate>
    </asp:GridView>
  </div>
</asp:Content>