<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsuariosBloqueados.aspx.cs" Inherits="Artify.UsuariosBloqueados" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Usuarios bloqueados</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        :root{
            --bg:#111827;          /* fondo oscuro */
            --panel:#1f2937;       /* cards/paneles */
            --border:#374151;
            --shadow:0 8px 24px rgba(0,0,0,.45);
            --ink:#e5e7eb;         /* texto principal */
            --muted:#9ca3af;       /* texto secundario */
            --accent:#7c3aed;      /* violeta de acento */
            --g1:linear-gradient(135deg,#a855f7 0%,#2563eb 100%);
        }
        *{box-sizing:border-box}
        body{
            margin:0; padding:24px;
            font-family:'Segoe UI', Arial, sans-serif;
            background:var(--bg); color:var(--ink);
        }

        .wrap{ max-width:900px; margin:0 auto; }

        /* Topbar */
        .topbar{
            display:flex; align-items:center; justify-content:space-between;
            margin-bottom:16px;
        }
        .title{
            margin:0;
            font-size:1.6rem; font-weight:800; color:#f9fafb;
        }
        .btn{
            border:1px solid var(--border); background:var(--panel); color:var(--ink);
            padding:10px 14px; border-radius:10px; cursor:pointer; font-weight:700;
            text-decoration:none; display:inline-flex; align-items:center; gap:8px;
            box-shadow:0 2px 6px rgba(0,0,0,.4);
        }
        .btn:hover{ background:#374151; border-color:#4b5563; }
        .btn svg{ width:18px; height:18px }

        /* Alerts (dark) */
        .alert{
            margin-bottom:16px; padding:12px 14px; border-radius:10px; border:1px solid;
        }
        .alert-success{
            background:rgba(16,185,129,.12); color:#86efac; border-color:rgba(134,239,172,.45);
        }
        .alert-danger{
            background:rgba(239,68,68,.15); color:#fca5a5; border-color:rgba(252,165,165,.5);
        }

        /* Listado */
        .list{ display:flex; flex-direction:column; gap:12px; }

        .card{
            background:var(--panel);
            border:1px solid var(--border);
            border-radius:16px;
            padding:16px;
            box-shadow:var(--shadow);
            display:flex; align-items:center; justify-content:space-between; gap:16px;
            transition:transform .1s ease, border-color .2s ease;
        }
        .card:hover{ transform:translateY(-2px); border-color:#4b5563; }

        .user{ display:flex; gap:14px; align-items:center; }
        .avatar{
            width:44px; height:44px; border-radius:50%;
            background:var(--g1); color:#fff; display:flex; align-items:center; justify-content:center;
            font-weight:700;
        }
        .meta{ display:flex; flex-direction:column; }
        .email{ font-weight:700; color:#f3f4f6; }
        .name{ color:var(--muted); font-size:.92rem; }

        .badge-lock{
            background:rgba(239,68,68,.15); /* rojo suave sobre oscuro */
            color:#fca5a5;
            border:1px solid rgba(252,165,165,.45);
            border-radius:999px; padding:2px 10px; font-weight:700; font-size:.85rem;
        }

        .btn-success{
            background:#22c55e; color:#fff; border:0; padding:8px 14px; border-radius:10px; cursor:pointer; font-weight:700;
        }
        .btn-success:hover{ background:#16a34a; }

        /* Empty state (dark) */
        .empty-wrap{ margin-top:64px; display:flex; justify-content:center; }
        .empty-card{
            width:100%; max-width:560px; text-align:center;
            background:var(--panel); color:var(--ink);
            border:1px solid var(--border); border-radius:16px; padding:28px;
            box-shadow:var(--shadow);
        }
        .empty-illustration{
            width:72px; height:72px; margin:0 auto 12px auto; display:block;
            filter:drop-shadow(0 2px 6px rgba(0,0,0,.25));
        }
        .empty-title{ margin:6px 0; font-size:1.15rem; color:#fff; font-weight:800; }
        .empty-text{ margin:0 0 18px 0; color:var(--muted); font-size:.98rem; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="wrap">

        <div class="topbar">
            <h2 class="title">Usuarios bloqueados</h2>
            <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/HomeWebMaster.aspx" CssClass="btn">
                <svg viewBox="0 0 24 24" fill="none" aria-hidden="true">
                    <path d="M15 19l-7-7 7-7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                Volver al Home
            </asp:HyperLink>
        </div>

        <asp:Panel ID="pnlMsg" runat="server" Visible="false" CssClass="alert"></asp:Panel>

        <div class="list">
            <asp:Repeater ID="rpUsuarios" runat="server" OnItemCommand="rpUsuarios_ItemCommand">
                <ItemTemplate>
                    <div class="card">
                        <div class="user">
                            <div class="avatar"><%# Eval("Email").ToString().Substring(0,1).ToUpper() %></div>
                            <div class="meta">
                                <div class="email"><%# Eval("Email") %></div>
                                <div class="name"><%# Eval("Nombre") %></div>
                            </div>
                        </div>
                        <div style="display:flex; align-items:center; gap:10px;">
                            <span class="badge-lock">Bloqueado</span>
                            <asp:LinkButton ID="btnDesbloquear" runat="server"
                                CommandName="Unblock" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn-success">Desbloquear</asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>

                <FooterTemplate>
                    <asp:Panel ID="pnlEmpty" runat="server" CssClass="empty-wrap" Visible="false">
                        <div class="empty-card">
                            <svg class="empty-illustration" viewBox="0 0 64 64" aria-hidden="true">
                                <defs>
                                    <linearGradient id="g1" x1="0" y1="0" x2="1" y2="1">
                                        <stop offset="0%" stop-color="#334155"/>
                                        <stop offset="100%" stop-color="#1f2937"/>
                                    </linearGradient>
                                </defs>
                                <circle cx="32" cy="32" r="30" fill="url(#g1)" stroke="#475569"/>
                                <path d="M32 14l14 5v9c0 9.5-6.8 17.9-14 20-7.2-2.1-14-10.5-14-20v-9l14-5z"
                                      fill="#0ea5e9" opacity=".18" stroke="#60a5fa" stroke-width="1.2"/>
                                <path d="M24 33l5 5 11-11" fill="none" stroke="#a78bfa" stroke-width="2.8" stroke-linecap="round" stroke-linejoin="round"/>
                            </svg>

                            <div class="empty-title">Sin usuarios bloqueados</div>
                            <div class="empty-text">Todo en orden. Cuando haya usuarios bloqueados, van a aparecer acá.</div>
                        </div>
                    </asp:Panel>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</form>
</body>
</html>