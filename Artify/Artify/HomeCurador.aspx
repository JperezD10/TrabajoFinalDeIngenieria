<%@ Page Title="Panel Curador" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HomeCurador.aspx.cs" Inherits="Artify.HomeCurador" %>

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

        .badge--blue {
            background: linear-gradient(135deg,#2563eb 0%,#06b6d4 100%);
            box-shadow: 0 8px 20px rgba(37,99,235,.32)
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
        <asp:HyperLink ID="lnkCrearSubasta" runat="server" NavigateUrl="~/CrearSubasta.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--blue" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M6 7h12M6 12h12M6 17h8" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litSubTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litSubText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litSubChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litSubChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litSubCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>
        <asp:HyperLink ID="lnkSubPendientes" runat="server" NavigateUrl="~/SubastasPendientes.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--blue" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <circle cx="12" cy="12" r="8" stroke="currentColor" stroke-width="1.5" />
                            <path d="M12 8v4l3 2" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litPendTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litPendText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litPendChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litPendChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litPendCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>


        <asp:HyperLink ID="lnkList" runat="server" NavigateUrl="~/VerArtistas.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--blue" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <circle cx="8" cy="8" r="3" stroke="currentColor" stroke-width="1.5" />
                            <circle cx="16" cy="10" r="3" stroke="currentColor" stroke-width="1.5" />
                            <path d="M3.5 18a4.5 4.5 0 0 1 9 0" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                            <path d="M12 18a4 4 0 0 1 8 0" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litListTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litListText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litListChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litListChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litListCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>

        <asp:HyperLink ID="lnkArtistas" runat="server" NavigateUrl="~/CargarArtistas.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M8 14a5 5 0 1 1 8 0" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                            <circle cx="12" cy="7" r="3" stroke="currentColor" stroke-width="1.5" />
                            <path d="M19 7v4M21 9h-4" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litArtTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litArtText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litArtChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litArtChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litArtCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>

        <asp:HyperLink ID="lnkVerObras" runat="server" NavigateUrl="~/VerObras.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--blue" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <rect x="3" y="4" width="18" height="14" rx="2" stroke="currentColor" stroke-width="1.5" />
                            <path d="M7 14l3-3 3 3 3-2 4 4" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litViewObrTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litViewObrText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litViewObrChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litViewObrChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litViewObrCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>

        <asp:HyperLink ID="lnkObras" runat="server" NavigateUrl="~/CargarObras.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--green" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <rect x="3" y="5" width="18" height="14" rx="2" stroke="currentColor" stroke-width="1.5" />
                            <path d="M7 14l3-3 3 3 3-2 4 4" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                            <circle cx="9" cy="9" r="1.5" stroke="currentColor" stroke-width="1.5" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litObrTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litObrText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litObrChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litObrChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litObrCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>
        <asp:HyperLink ID="lnkStats" runat="server" NavigateUrl="~/EstadisticasCurador.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--blue" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M4 19V5" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                            <rect x="6.5" y="11" width="3" height="8" rx="1" stroke="currentColor" stroke-width="1.5" />
                            <rect x="11.5" y="8" width="3" height="11" rx="1" stroke="currentColor" stroke-width="1.5" />
                            <rect x="16.5" y="6" width="3" height="13" rx="1" stroke="currentColor" stroke-width="1.5" />
                        </svg>
                    </div>
                    <h3 class="card-title">
                        <asp:Literal ID="litStaTitle" runat="server" /></h3>
                </div>
                <p class="card-text">
                    <asp:Literal ID="litStaText" runat="server" />
                </p>
                <div class="chips">
                    <span class="chip">
                        <asp:Literal ID="litStaChip1" runat="server" /></span>
                    <span class="chip">
                        <asp:Literal ID="litStaChip2" runat="server" /></span>
                </div>
                <div class="cta">
                    <span>
                        <asp:Literal ID="litStaCta" runat="server" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>

    </div>
</asp:Content>
