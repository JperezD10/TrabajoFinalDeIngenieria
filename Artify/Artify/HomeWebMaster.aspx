<%@ Page Title="Panel Webmaster" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HomeWebMaster.aspx.cs" Inherits="Artify.HomeWebMaster" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .topbar{display:flex;align-items:center;justify-content:space-between;margin-bottom:16px}
    .panel-heading{background:linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);padding:18px 24px;border-radius:14px;text-align:center;margin-bottom:22px;box-shadow:0 8px 24px rgba(0,0,0,.5)}
    .panel-heading h1{margin:0;font-size:2rem;font-weight:900;color:#fff;letter-spacing:.5px}
    .hero{background:var(--panel);border:1px solid var(--border);border-radius:16px;padding:22px 20px;box-shadow:var(--shadow);margin-bottom:18px}
    .hero h1{margin:0 0 6px 0;font-size:1.75rem;color:#fff}
    .hero p{margin:0;color:var(--muted)}
    .section-title{margin:10px 4px 10px;color:#cbd5e1;font-weight:800;font-size:.95rem;letter-spacing:.02em}
    .grid{display:grid;grid-template-columns:repeat(2,minmax(280px,1fr));gap:16px}
    @media (max-width:820px){.grid{grid-template-columns:1fr}}
    .card-link{display:block;text-decoration:none;color:inherit}
    .card{background:var(--panel);border:1px solid var(--border);border-radius:16px;padding:18px;box-shadow:var(--shadow);transition:transform .1s,box-shadow .2s,border-color .2s;color:var(--ink)}
    .card:hover{transform:translateY(-2px);border-color:#4b5563;box-shadow:0 10px 28px rgba(0,0,0,.55)}
    .card-head{display:flex;align-items:center;gap:12px;margin-bottom:8px}
    .badge{width:48px;height:48px;border-radius:14px;display:grid;place-items:center;background:var(--g1);box-shadow:0 8px 20px rgba(124,58,237,.35)}
    .badge--green{background:var(--g2);box-shadow:0 8px 20px rgba(5,150,105,.32)}
    .badge svg{width:26px;height:26px;color:#fff}
    .card-title{margin:0;font-size:1.15rem;font-weight:800;color:#fff}
    .card-text{margin:4px 0 12px 0;color:var(--muted)}
    .chips{display:flex;gap:8px;flex-wrap:wrap}
    .chip{font-size:.78rem;padding:6px 10px;border-radius:999px;border:1px solid rgba(255,255,255,.18);color:#dbeafe;background:rgba(99,102,241,.15)}
    .cta{margin-top:12px;display:flex;align-items:center;gap:8px;font-weight:700;color:#c7d2fe}
    .cta svg{width:18px;height:18px}
  </style>
</asp:Content>

<asp:Content ID="TitleCph" ContentPlaceHolderID="TitleContent" runat="server">
  Panel del WebMaster
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="panel-heading">
    <h1>Panel Webmaster</h1>
  </div>

  <div class="hero">
    <h1>Bienvenido</h1>
    <p>Accesos rápidos a administración y monitoreo del sistema.</p>
  </div>

  <div class="section-title">Módulos</div>

  <div class="grid">
    <asp:HyperLink ID="lnkBitacora" runat="server" NavigateUrl="~/Bitacora.aspx" CssClass="card-link">
      <div class="card">
        <div class="card-head">
          <div class="badge" aria-hidden="true">
            <svg viewBox="0 0 24 24" fill="none">
              <path d="M9 3h6a2 2 0 0 1 2 2v1h1a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h1V5a2 2 0 0 1 2-2z" stroke="currentColor" stroke-width="1.5" />
              <path d="M8 11h2M8 15h2M12 11h6M12 15h6" stroke="currentColor" stroke-width="1.5" stroke-linecap="round"/>
            </svg>
          </div>
          <h3 class="card-title">Bitácora</h3>
        </div>
        <p class="card-text">Explorá eventos del sistema, auditoría y seguimiento.</p>
        <div class="chips">
          <span class="chip">Sólo lectura</span>
          <span class="chip">Histórico</span>
        </div>
        <div class="cta">
          <span>Entrar</span>
          <svg viewBox="0 0 24 24" fill="none"><path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/></svg>
        </div>
      </div>
    </asp:HyperLink>

    <asp:HyperLink ID="lnkBloqueados" runat="server" NavigateUrl="~/UsuariosBloqueados.aspx" CssClass="card-link">
      <div class="card">
        <div class="card-head">
          <div class="badge badge--green" aria-hidden="true">
            <svg viewBox="0 0 24 24" fill="none">
              <rect x="4" y="11" width="16" height="9" rx="2" stroke="currentColor" stroke-width="1.5"/>
              <path d="M16 11V8a4 4 0 0 0-8 0v3" stroke="currentColor" stroke-width="1.5"/>
            </svg>
          </div>
          <h3 class="card-title">Usuarios bloqueados</h3>
        </div>
        <p class="card-text">Desbloqueá cuentas de forma rápida y segura.</p>
        <div class="chips">
          <span class="chip">Acción directa</span>
          <span class="chip">Estado crítico</span>
        </div>
        <div class="cta">
          <span>Gestionar</span>
          <svg viewBox="0 0 24 24" fill="none"><path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/></svg>
        </div>
      </div>
    </asp:HyperLink>
  </div>
</asp:Content>
