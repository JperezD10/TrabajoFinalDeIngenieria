<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeWebMaster.aspx.cs" Inherits="Artify.HomeWebMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Panel del WebMaster</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        :root {
            --bg: #111827; /* fondo oscuro (igual Bitácora) */
            --panel: #1f2937; /* cards/paneles */
            --border: #374151;
            --shadow: 0 8px 24px rgba(0,0,0,.45);
            --ink: #e5e7eb; /* texto principal */
            --muted: #9ca3af; /* texto secundario */
            --accent: #7c3aed; /* violeta de acento */
            --g1: linear-gradient(135deg,#a855f7 0%,#2563eb 100%);
            --g2: linear-gradient(135deg,#22c55e 0%,#06b6d4 100%);
        }

        * {
            box-sizing: border-box
        }

        body {
            margin: 0;
            font-family: 'Segoe UI',Arial,sans-serif;
            background: var(--bg);
            color: var(--ink);
        }

        .wrap {
            max-width: 1100px;
            margin: 0 auto;
            padding: 24px
        }

        /* TOPBAR */
        .topbar {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 16px;
        }

        .panel-heading {
            background: linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);
            padding: 18px 24px;
            border-radius: 14px;
            text-align: center;
            margin-bottom: 22px;
            box-shadow: 0 8px 24px rgba(0,0,0,.5);
        }

            .panel-heading h1 {
                margin: 0;
                font-size: 2rem;
                font-weight: 900;
                color: #fff;
                letter-spacing: .5px;
            }

        .title {
            margin: 0;
            font-size: 1.6rem;
            font-weight: 800;
            color: #f9fafb
        }

        .btn {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 10px 14px;
            border-radius: 10px;
            cursor: pointer;
            text-decoration: none;
            font-weight: 700;
            border: 1px solid var(--border);
            background: var(--panel);
            color: var(--ink);
            box-shadow: 0 2px 6px rgba(0,0,0,.4);
        }

            .btn:hover {
                background: #374151;
                border-color: #4b5563;
            }

            .btn svg {
                width: 18px;
                height: 18px
            }

        /* HERO (panel, no full gradient para mantener coherencia oscura) */
        .hero {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            padding: 22px 20px;
            box-shadow: var(--shadow);
            margin-bottom: 18px;
        }

            .hero h1 {
                margin: 0 0 6px 0;
                font-size: 1.75rem;
                color: #fff
            }

            .hero p {
                margin: 0;
                color: var(--muted)
            }

        .section-title {
            margin: 10px 4px 10px;
            color: #cbd5e1;
            font-weight: 800;
            font-size: .95rem;
            letter-spacing: .02em
        }

        /* GRID */
        .grid {
            display: grid;
            grid-template-columns: repeat(2,minmax(280px,1fr));
            gap: 16px
        }

        @media (max-width:820px) {
            .grid {
                grid-template-columns: 1fr
            }
        }

        .card-link {
            display: block;
            text-decoration: none;
            color: inherit
        }

        .card {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            padding: 18px;
            box-shadow: var(--shadow);
            transition: transform .1s ease, box-shadow .2s ease, border-color .2s ease;
            color: var(--ink);
        }

            .card:hover {
                transform: translateY(-2px);
                border-color: #4b5563;
                box-shadow: 0 10px 28px rgba(0,0,0,.55);
            }

        .card-head {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 8px
        }

        .badge {
            width: 48px;
            height: 48px;
            border-radius: 14px;
            display: grid;
            place-items: center;
            background: var(--g1);
            box-shadow: 0 8px 20px rgba(124,58,237,.35);
        }

        .badge--green {
            background: var(--g2);
            box-shadow: 0 8px 20px rgba(5,150,105,.32)
        }

        .badge svg {
            width: 26px;
            height: 26px;
            color: white
        }

        .card-title {
            margin: 0;
            font-size: 1.15rem;
            font-weight: 800;
            color: #fff
        }

        .card-text {
            margin: 4px 0 12px 0;
            color: var(--muted)
        }

        .chips {
            display: flex;
            gap: 8px;
            flex-wrap: wrap
        }

        .chip {
            font-size: .78rem;
            padding: 6px 10px;
            border-radius: 999px;
            border: 1px solid rgba(255,255,255,.18);
            color: #dbeafe;
            background: rgba(99,102,241,.15);
        }

        .cta {
            margin-top: 12px;
            display: flex;
            align-items: center;
            gap: 8px;
            font-weight: 700;
            color: #c7d2fe
        }

            .cta svg {
                width: 18px;
                height: 18px
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrap">

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

                <!-- Usuarios bloqueados -->
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

        </div>
    </form>
</body>
</html>
