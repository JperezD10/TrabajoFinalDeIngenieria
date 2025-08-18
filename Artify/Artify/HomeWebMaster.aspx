﻿<%@ Page Title="Panel Webmaster" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HomeWebMaster.aspx.cs" Inherits="Artify.HomeWebMaster" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .topbar {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 16px
        }

        .panel-heading {
            background: linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);
            padding: 18px 24px;
            border-radius: 14px;
            text-align: center;
            margin-bottom: 22px;
            box-shadow: 0 8px 24px rgba(0,0,0,.5)
        }

            .panel-heading h1 {
                margin: 0;
                font-size: 2rem;
                font-weight: 900;
                color: #fff;
                letter-spacing: .5px
            }

        .hero {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            padding: 22px 20px;
            box-shadow: var(--shadow);
            margin-bottom: 18px
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
            transition: transform .1s,box-shadow .2s,border-color .2s;
            color: var(--ink)
        }

            .card:hover {
                transform: translateY(-2px);
                border-color: #4b5563;
                box-shadow: 0 10px 28px rgba(0,0,0,.55)
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
            box-shadow: 0 8px 20px rgba(124,58,237,.35)
        }

        .badge--green {
            background: var(--g2);
            box-shadow: 0 8px 20px rgba(5,150,105,.32)
        }

        .badge svg {
            width: 26px;
            height: 26px;
            color: #fff
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
            background: rgba(99,102,241,.15)
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
</asp:Content>

<asp:Content ID="TitleCph" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal ID="litPageTitle" runat="server" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-heading">
        <h1>
            <asp:Literal ID="litHeaderTitle" runat="server" /></h1>
    </div>

    <div class="hero">
        <h1>
            <asp:Literal ID="litHeroTitle" runat="server" /></h1>
        <p class="card-text">
            <asp:Literal ID="litHeroText" runat="server" />
        </p>
    </div>

    <div class="section-title">
        <asp:Literal ID="litModules" runat="server" />
    </div>

    <div class="grid">
        <asp:HyperLink ID="lnkBitacora" runat="server" NavigateUrl="~/Bitacora.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M9 3h6a2 2 0 0 1 2 2v1h1a2 2 0 0 1 2 2v10a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h1V5a2 2 0 0 1 2-2z" stroke="currentColor" stroke-width="1.5" />
                            <path d="M8 11h2M8 15h2M12 11h6M12 15h6" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litBitTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litBitText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litBitChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litBitChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litBitCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>

        <asp:HyperLink ID="lnkBloqueados" runat="server" NavigateUrl="~/UsuariosBloqueados.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--green" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <rect x="4" y="11" width="16" height="9" rx="2" stroke="currentColor" stroke-width="1.5" />
                            <path d="M16 11V8a4 4 0 0 0-8 0v3" stroke="currentColor" stroke-width="1.5" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litBlkTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litBlkText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litBlkChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litBlkChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litBlkCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>
        <asp:HyperLink ID="lnkBackup" runat="server" NavigateUrl="~/BackupDB.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--green" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M7 17a4 4 0 0 1 0-8c.3-2.8 2.7-5 5.5-5 3 0 5.5 2.5 5.5 5.5v.5A4.5 4.5 0 0 1 18 17H7z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                            <path d="M12 12v6M9.5 14.5 12 12l2.5 2.5" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litBkpTitle" runat="server" Text="" />
                    </h3>
                </div>

                <p class="card-text">
                    <asp:Literal ID="litBkpText" runat="server" Text="" />
                </p>

                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litBkpChip1" runat="server" Text="" /></span>
                    <span class="chip">
                        <asp:Literal ID="litBkpChip2" runat="server" Text="" /></span>
                </div>

                <div class="cta">
                    <span>
                        <asp:Literal ID="litBkpCta" runat="server" Text="" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>
        <asp:HyperLink ID="lnkRestore" runat="server" NavigateUrl="~/RestoreDB.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M12 6v6l4 2" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                            <circle cx="12" cy="12" r="9" stroke="currentColor" stroke-width="1.5" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litRstTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litRstText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litRstChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litRstChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litRstCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>
    </div>
</asp:Content>
