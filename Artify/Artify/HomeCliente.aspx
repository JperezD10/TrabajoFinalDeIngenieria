<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HomeCliente.aspx.cs" Inherits="Artify.HomeCliente" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        :root {
            --bg: #0b1220;
            --panel: #111827;
            --border: #334155;
            --ink: #e5e7eb;
            --muted: #9ca3af;
            --accent: #7c3aed;
            --accent2: #2563eb;
            --shadow: 0 12px 36px rgba(0,0,0,.55)
        }

        .topbar {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 18px
        }

        .title {
            margin: 0;
            font-size: 1.8rem;
            font-weight: 900;
            color: #f9fafb
        }

        .hint {
            margin: 0;
            color: var(--muted)
        }

        .gallery {
            display: grid;
            grid-template-columns: repeat(auto-fill,minmax(260px,1fr));
            gap: 16px;
            align-items: stretch
        }

        .card {
            background: #111827;
            border: 1px solid #334155;
            border-radius: 16px;
            position: relative;
            overflow: hidden;
            box-shadow: 0 12px 36px rgba(0,0,0,.55);
            display: flex;
            flex-direction: column;
        }


            .card.original {
                outline: 2px solid #facc15;
                outline-offset: 0;
                box-shadow: 0 12px 36px rgba(0,0,0,.55), 0 0 18px rgba(250,204,21,.45);
            }



                .card.original::before {
                    content: "★ Original";
                    position: absolute;
                    top: 8px;
                    right: 12px;
                    background: linear-gradient(135deg,#facc15 0%,#f59e0b 100%);
                    color: #111;
                    font-size: .75rem;
                    font-weight: 900;
                    padding: 2px 8px;
                    border-radius: 999px;
                    box-shadow: 0 2px 6px rgba(0,0,0,.3);
                    z-index: 1;
                }

        .thumb {
            aspect-ratio: 4/3;
            background: #0f172a;
            position: relative;
            overflow: hidden;
            border-top-left-radius: 16px;
            border-top-right-radius: 16px;
        }

            .thumb img {
                width: 100%;
                height: 100%;
                object-fit: cover;
                display: block;
                transition: transform .4s ease
            }

        .card:hover .thumb img {
            transform: scale(1.06)
        }

        .body {
            padding: 12px 14px;
            display: flex;
            flex-direction: column;
            gap: 6px;
            flex: 1
        }

        .title2 {
            margin: 0;
            color: var(--ink);
            font-size: 1.05rem;
            font-weight: 800;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            overflow: hidden;
            min-height: 2.6em
        }

        .meta {
            margin: 0;
            color: var(--muted);
            font-size: .9rem;
            display: -webkit-box;
            -webkit-line-clamp: 1;
            -webkit-box-orient: vertical;
            overflow: hidden;
            min-height: 1.2em
        }

        .prices {
            display: flex;
            gap: 8px;
            align-items: baseline;
            margin-top: 4px
        }

        .price-base {
            color: #93c5fd;
            text-decoration: line-through;
            opacity: .8;
            font-size: .9rem
        }

        .price-actual {
            color: #fde68a;
            font-weight: 900
        }

        .price-label {
            color: var(--muted);
            font-size: .85rem;
            margin-left: 4px;
        }

        .state {
            margin-top: 6px;
            font-size: .9rem;
            color: #c7d2fe
        }

            .state .badge {
                padding: 2px 8px;
                border: 1px solid var(--border);
                border-radius: 999px;
                margin-right: 8px;
                color: #e9d5ff
            }

        .actions {
            display: flex;
            gap: 8px;
            margin-top: auto
        }

        .btn {
            flex: 1;
            text-align: center;
            padding: 10px 12px;
            border-radius: 10px;
            border: 1px solid var(--border);
            font-weight: 800;
            color: #e5e7eb;
            background: #0b1220;
            text-decoration: none;
            cursor: pointer
        }

            .btn:hover {
                filter: brightness(1.08)
            }

            .btn[disabled] {
                opacity: .5;
                cursor: not-allowed
            }

        .empty {
            background: var(--panel);
            border: 1px dashed var(--border);
            border-radius: 14px;
            padding: 18px;
            color: var(--muted);
            text-align: center
        }
    </style>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <div class="topbar">
        <h1 class="title">
            <asp:Literal ID="litTitle" runat="server" /></h1>
        <p class="hint">
            <asp:Literal ID="litHint" runat="server" />
        </p>
    </div>

    <asp:PlaceHolder ID="phEmpty" runat="server" Visible="false">
        <div class="empty">
            <asp:Literal ID="litEmpty" runat="server" />
        </div>
    </asp:PlaceHolder>

    <asp:Repeater ID="rptSubastas" runat="server" OnItemDataBound="rptSubastas_ItemDataBound">
        <HeaderTemplate>
            <div class="gallery">
        </HeaderTemplate>
        <ItemTemplate>
            <div class='card <%# (bool)Eval("EsOriginal") ? "original" : "" %>'>
                <div class="thumb">
                    <img alt='<%# Eval("TituloObra") %>' loading="lazy" src='<%# Eval("UrlImagen") %>'
                        onerror="this.onerror=null;this.src='https://picsum.photos/seed/fallbackimg/800/600';" />
                </div>

                <div class="body">
                    <h3 class="title2"><%# Eval("TituloObra") %></h3>
                    <p class="meta"><%# Eval("ArtistaNombre") %> • <%# Eval("Anio") %> • <%# Eval("Tecnica") %></p>

                    <div class="prices">
                        <span class="price-actual">
                            <%# "US$ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:N0}", Eval("PrecioActual")) %>
                        </span>
                        <span class="price-label">
                            <%# Eval("HuboPujas") != null && (bool)Eval("HuboPujas") ? "Current bid" : "Starting bid" %>
                        </span>
                    </div>

                    <div class="state">
                        <span class="badge">
                            <asp:Literal ID="litEstado" runat="server" /></span>
                        <span>
                            <asp:Literal ID="litTiempo" runat="server" /></span>
                    </div>

                    <div class="actions">
                        <asp:HyperLink ID="lnkVer" runat="server" CssClass="btn" />
                        <asp:HyperLink ID="lnkPujar" runat="server" CssClass="btn" />
                        <asp:HyperLink ID="lnkSuscribir" runat="server" CssClass="btn" Visible="false" />
                        <asp:Button ID="btnDisabled" runat="server" CssClass="btn" Enabled="false" Visible="false" />
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>
</asp:Content>
