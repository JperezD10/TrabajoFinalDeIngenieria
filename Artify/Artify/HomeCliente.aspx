<%@ Page Title="Panel Cliente" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HomeCliente.aspx.cs" Inherits="Artify.HomeCliente" %>

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
            .grid { grid-template-columns: 1fr }
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
    <asp:Literal ID="litPageTitle" runat="server" Text="homecli.pageTitle" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-heading">
        <h1><asp:Literal ID="litHeaderTitle" runat="server" Text="homecli.headerTitle" /></h1>
    </div>

    <div class="hero">
        <h1><asp:Literal ID="litHeroTitle" runat="server" Text="homecli.heroTitle" /></h1>
        <p class="card-text"><asp:Literal ID="litHeroText" runat="server" Text="homecli.heroText" /></p>
    </div>

    <div class="section-title">
        <asp:Literal ID="litModules" runat="server" Text="homecli.modules" />
    </div>

    <div class="grid">
        <asp:HyperLink ID="lnkSubastas" runat="server" NavigateUrl="~/SubastasCliente.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M6 7h12M6 12h12M6 17h8" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                        </svg>
                    </div>
                    <h3 class="card-title"><asp:Literal ID="litSubTitle" runat="server" Text="homecli.sub.title" /></h3>
                </div>
                <p class="card-text"><asp:Literal ID="litSubText" runat="server" Text="homecli.sub.text" /></p>
                <div class="chips">
                    <span class="chip"><asp:Literal ID="litSubChip1" runat="server" Text="homecli.sub.chip1" /></span>
                    <span class="chip"><asp:Literal ID="litSubChip2" runat="server" Text="homecli.sub.chip2" /></span>
                </div>
                <div class="cta">
                    <span><asp:Literal ID="litSubCta" runat="server" Text="homecli.sub.cta" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>

        <asp:HyperLink ID="lnkPagos" runat="server" NavigateUrl="~/PagosPendientes.aspx" CssClass="card-link">
            <div class="card">
                <div class="card-head">
                    <div class="badge badge--green" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M3 7h18M3 12h18M3 17h18" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
                        </svg>
                    </div>
                    <h3 class="card-title"><asp:Literal ID="litPayTitle" runat="server" Text="homecli.pay.title" /></h3>
                </div>
                <p class="card-text"><asp:Literal ID="litPayText" runat="server" Text="homecli.pay.text" /></p>
                <div class="chips">
                    <span class="chip"><asp:Literal ID="litPayChip1" runat="server" Text="homecli.pay.chip1" /></span>
                    <span class="chip"><asp:Literal ID="litPayChip2" runat="server" Text="homecli.pay.chip2" /></span>
                </div>
                <div class="cta">
                    <span><asp:Literal ID="litPayCta" runat="server" Text="homecli.pay.cta" /></span>
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M9 5l7 7-7 7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>
            </div>
        </asp:HyperLink>
    </div>
</asp:Content>
