<%@ Page Title="Pagos Pendientes" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PagosPendientes.aspx.cs" Inherits="Artify.PagosPendientes" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
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
            font-size: 1.6rem;
            color: #fff
        }

        .hero p {
            margin: 0;
            color: var(--muted)
        }

        .section-title {
            margin: 10px 4px 14px;
            color: #cbd5e1;
            font-weight: 800;
            font-size: .95rem;
            letter-spacing: .02em
        }

        .grid {
            display: grid;
            grid-template-columns: repeat(auto-fill,minmax(300px,1fr));
            gap: 18px
        }

        .card {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            overflow: hidden;
            box-shadow: var(--shadow);
            color: var(--ink);
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            transition: transform .1s, box-shadow .2s;
        }

        .card:hover {
            transform: translateY(-2px);
            box-shadow: 0 10px 28px rgba(0,0,0,.55);
        }

        .thumb {
            aspect-ratio: 4/3;
            background: #0f172a;
            position: relative;
            overflow: hidden;
        }

        .thumb img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            display: block;
            transition: transform .4s ease;
        }

        .card:hover .thumb img {
            transform: scale(1.06);
        }

        .body {
            padding: 14px 16px;
            display: flex;
            flex-direction: column;
            gap: 6px;
        }

        .card-title {
            margin: 0;
            font-size: 1.15rem;
            font-weight: 800;
            color: #fff
        }

        .meta {
            margin: 6px 0 10px 0;
            color: var(--muted);
            font-size: .9rem
        }

        .price {
            color: #fde68a;
            font-weight: 900;
            font-size: 1.05rem;
            margin-bottom: 10px
        }

        .btn {
            display: inline-block;
            text-align: center;
            padding: 10px 14px;
            border-radius: 10px;
            border: 1px solid var(--border);
            font-weight: 800;
            color: #e5e7eb;
            background: #0b1220;
            text-decoration: none;
            cursor: pointer;
            transition: filter .2s ease;
        }

        .btn:hover {
            filter: brightness(1.1)
        }

        .empty {
            background: var(--panel);
            border: 1px dashed var(--border);
            border-radius: 14px;
            padding: 20px;
            color: var(--muted);
            text-align: center;
            margin-top: 16px
        }

        .back-link {
            display: inline-block;
            color: #e5e7eb;
            font-weight: 700;
            font-size: .9rem;
            text-decoration: none;
            margin: 6px 0 18px 4px;
            border: 1px solid var(--border);
            border-radius: 10px;
            padding: 8px 14px;
            background: #0b1220;
            transition: all .2s ease;
        }

        .back-link:hover {
            background: #1e293b;
            color: var(--accent2);
            border-color: #475569;
        }
    </style>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-heading">
        <h1><asp:Literal ID="litHeaderTitle" runat="server" Text="paypend.headerTitle" /></h1>
    </div>

    <div class="hero">
        <h1><asp:Literal ID="litHeroTitle" runat="server" Text="paypend.heroTitle" /></h1>
        <p><asp:Literal ID="litHeroText" runat="server" Text="paypend.heroText" /></p>
    </div>

    <a href="HomeCliente.aspx" class="back-link">← <asp:Literal ID="litBackHome" runat="server" Text="paypend.backHome" /></a>

    <div class="section-title">
        <asp:Literal ID="litSectionTitle" runat="server" Text="paypend.sectionTitle" />
    </div>

    <asp:PlaceHolder ID="phEmpty" runat="server" Visible="false">
        <div class="empty">
            <asp:Literal ID="litEmpty" runat="server" Text="paypend.empty" />
        </div>
    </asp:PlaceHolder>

    <asp:Repeater ID="rptPagos" runat="server" OnItemDataBound="rptPagos_ItemDataBound">
        <HeaderTemplate>
            <div class="grid">
        </HeaderTemplate>

        <ItemTemplate>
            <div class="card">
                <div class="thumb">
                    <img alt='<%# Eval("TituloObra") %>' loading="lazy"
                         src='<%# Eval("UrlImagen") %>'
                         onerror="this.onerror=null;this.src='https://picsum.photos/seed/fallbackimg/800/600';" />
                </div>

                <div class="body">
                    <h3 class="card-title"><%# Eval("TituloObra") %></h3>
                    <p class="meta"><%# Eval("ArtistaNombre") %></p>
                    <div class="price">
                        <%# "US$ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:N0}", Eval("Monto")) %>
                    </div>
                    <asp:HyperLink ID="lnkPagar" runat="server" CssClass="btn" NavigateUrl='<%# "~/PagoSubastaCliente.aspx?id=" + Eval("IdPago") %>'>
                        <asp:Literal ID="litBtnPagar" runat="server" />
                    </asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>

        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
