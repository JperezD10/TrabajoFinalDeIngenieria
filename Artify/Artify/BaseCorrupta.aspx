<%@ Page Title="Base de datos corrupta" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="BaseCorrupta.aspx.cs"
    Inherits="Artify.BaseCorrupta" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    :root{
      --bg:#0f172a; --card:#111827; --muted:#94a3b8; --ok:#16a34a; --bad:#ef4444; --warn:#f59e0b; --chip:#1f2937;
      --ring:rgba(239,68,68,.25);
    }
    .wrap{max-width:1000px;margin:24px auto;padding:0 16px}
    .banner{
      display:flex;align-items:center;gap:16px;border:1px solid rgba(239,68,68,.25);
      background:linear-gradient(135deg, rgba(239,68,68,.12), rgba(245,158,11,.12));
      padding:18px 20px;border-radius:16px;box-shadow:0 12px 28px rgba(0,0,0,.35);
      backdrop-filter:blur(6px)
    }
    .banner-icon{
      width:44px;height:44px;border-radius:12px;background:#1f2937;display:grid;place-items:center;
      box-shadow:0 0 0 6px var(--ring)
    }
    .banner h1{margin:0;font-size:22px;color:#fff}
    .banner p{margin:4px 0 0 0;color:var(--muted);font-size:14px}

    .stats{display:grid;grid-template-columns:repeat(3,1fr);gap:12px;margin:18px 0}
    .stat{background:var(--card);border:1px solid #1f2937;border-radius:14px;padding:14px}
    .stat h4{margin:0 0 6px 0;color:#cbd5e1;font-weight:600;font-size:13px;text-transform:uppercase;letter-spacing:.08em}
    .stat .v{font-size:22px;color:#fff}

    .cards{display:grid;grid-template-columns:1fr;gap:14px}
    .card{
      background:var(--card);border:1px solid #222;border-radius:18px;padding:16px 16px 12px 16px;
      box-shadow:0 8px 22px rgba(0,0,0,.35)
    }
    .card-head{display:flex;justify-content:space-between;align-items:center;margin-bottom:10px}
    .card-title{display:flex;align-items:center;gap:10px}
    .dot{width:10px;height:10px;border-radius:50%}
    .dot.bad{background:var(--bad);box-shadow:0 0 0 6px rgba(239,68,68,.15)}
    .dot.ok{background:var(--ok);box-shadow:0 0 0 6px rgba(34,197,94,.12)}
    .title{color:#fff;font-weight:600;font-size:18px;margin:0}
    .badge{padding:4px 8px;border-radius:999px;font-size:12px}
    .badge.ok{background:rgba(34,197,94,.1);color:#86efac;border:1px solid rgba(34,197,94,.25)}
    .badge.bad{background:rgba(239,68,68,.1);color:#fecaca;border:1px solid rgba(239,68,68,.25)}
    .badge.err{background:rgba(245,158,11,.12);color:#fde68a;border:1px solid rgba(245,158,11,.25)}

    .chips{display:flex;flex-wrap:wrap;gap:8px;margin:8px 0 0}
    .chip{background:var(--chip);border:1px solid #2b3443;color:#e5e7eb;padding:6px 10px;border-radius:999px;font-size:13px}

    .err{background:rgba(245,158,11,.08);border:1px dashed rgba(245,158,11,.35);color:#fde68a;
         padding:10px;border-radius:12px;margin-top:8px}

    .footer-actions{display:flex;justify-content:flex-end;margin-top:18px;gap:10px}
    .btn{appearance:none;border:none;border-radius:12px;padding:10px 14px;cursor:pointer;font-weight:600}
    .btn-secondary{background:#1f2937;color:#cbd5e1;border:1px solid #2b3443}
    .btn-primary{background:#ef4444;color:white;border:1px solid #ef4444}
    @media (max-width:720px){ .stats{grid-template-columns:1fr} }
  </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="wrap">
    <div class="banner">
      <div class="banner-icon">
        <span style="font-size:22px;color:#fecaca">⚠️</span>
      </div>
      <div>
        <h1><asp:Literal ID="litTitle" runat="server" /></h1>
        <p id="pResumen" runat="server"></p>
      </div>
    </div>

    <div class="stats">
      <div class="stat">
        <h4><asp:Label ID="lblStatsTables" runat="server" /></h4>
        <div class="v"><asp:Label ID="lblTablasAfectadas" runat="server" Text="0"></asp:Label></div>
      </div>
      <div class="stat">
        <h4><asp:Label ID="lblStatsRecords" runat="server" /></h4>
        <div class="v"><asp:Label ID="lblRegistrosCorruptos" runat="server" Text="0"></asp:Label></div>
      </div>
      <div class="stat">
        <h4><asp:Label ID="lblStatsDetected" runat="server" /></h4>
        <div class="v"><asp:Label ID="lblFecha" runat="server"></asp:Label></div>
      </div>
    </div>

    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="card">
      <div class="card-head">
        <div class="card-title">
          <div class="dot ok"></div>
          <h3 class="title"><asp:Literal ID="litEmptyTitle" runat="server" /></h3>
        </div>
      </div>
      <div style="color:#cbd5e1"><asp:Literal ID="litEmptyBody" runat="server" /></div>
    </asp:Panel>

    <div class="cards">
      <asp:Repeater ID="rptTablas" runat="server" OnItemDataBound="rptTablas_ItemDataBound">
        <ItemTemplate>
          <div class="card">
            <div class="card-head">
              <div class="card-title">
                <div runat="server" id="statusDot" class="dot"></div>
                <h3 class="title"><%# Eval("Tabla") %></h3>
              </div>
              <asp:Label ID="lblBadge" runat="server" CssClass="badge"></asp:Label>
            </div>

            <asp:Panel ID="pOk" runat="server" Visible="false">
              <span class="chip"><asp:Label ID="lblOkChip" runat="server" /></span>
            </asp:Panel>

            <asp:Panel ID="pErr" runat="server" Visible="false" CssClass="err">
              <asp:Label ID="lblErr" runat="server"></asp:Label>
            </asp:Panel>

            <asp:Panel ID="pCorrupt" runat="server" Visible="false">
              <div style="color:#cbd5e1;margin-bottom:6px">
                <asp:Literal ID="litCorruptTitle" runat="server" />
              </div>
              <asp:Repeater ID="rptIds" runat="server">
                <ItemTemplate>
                  <span class="chip"><asp:Literal ID="litChip" runat="server" /></span>
                </ItemTemplate>
              </asp:Repeater>
            </asp:Panel>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>

    <div class="footer-actions">
      <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="HomeWebMaster.aspx" CssClass="btn btn-secondary" />
    </div>
  </div>
</asp:Content>
