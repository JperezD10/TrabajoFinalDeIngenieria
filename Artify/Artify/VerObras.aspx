<%@ Page Title="Ver Obras" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="VerObras.aspx.cs" Inherits="Artify.VerObras" %>

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
                font-size: 1.8rem;
                font-weight: 900;
                color: #fff
            }

        .topbar {
            display: flex;
            justify-content: flex-end;
            margin-bottom: 14px
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
            border: 1px solid var(--border)
        }

        .btn-plain {
            background: #0b1220;
            color: #e5e7eb;
            border-color: #334155
        }

        .grid {
            display: grid;
            grid-template-columns: repeat(3,minmax(260px,1fr));
            gap: 16px
        }

        @media (max-width:1100px) {
            .grid {
                grid-template-columns: repeat(2,minmax(260px,1fr))
            }
        }

        @media (max-width:720px) {
            .grid {
                grid-template-columns: 1fr
            }
        }

        .card {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            padding: 12px;
            box-shadow: var(--shadow)
        }

            .card.original {
                border: 2px solid #facc15;
                box-shadow: 0 0 15px rgba(250,204,21,.4);
                position: relative;
            }

                .card.original::before {
                    content: "★ Original";
                    position: absolute;
                    top: -10px;
                    right: 12px;
                    background: linear-gradient(135deg,#facc15 0%,#f59e0b 100%);
                    color: #111;
                    font-size: .75rem;
                    font-weight: 900;
                    padding: 2px 8px;
                    border-radius: 999px;
                    box-shadow: 0 2px 6px rgba(0,0,0,.3);
                }

        .thumb {
  width: 100%;
  height: auto;
  max-height: 400px;
  object-fit: contain;
  background: #0b1220;
  border: 1px solid #2b3344;
  border-radius: 12px;
}

        .title {
            margin: 0 0 4px 0;
            color: #fff;
            font-weight: 800
        }

        .meta {
            margin: 0;
            color: #9ca3af;
            font-size: .9rem
        }

        .price {
            margin-top: 8px;
            font-weight: 800;
            color: #c7d2fe
        }

        .empty {
            margin-top: 12px;
            color: #9ca3af
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

    <div class="topbar">
        <asp:HyperLink ID="btnBack" runat="server" NavigateUrl="~/HomeCurador.aspx" CssClass="btn btn-plain">
            <span>
                <asp:Literal ID="litBack" runat="server" /></span>
        </asp:HyperLink>
    </div>

    <asp:Repeater ID="rptObras" runat="server">
        <HeaderTemplate>
            <div class="grid">
        </HeaderTemplate>
        <ItemTemplate>
            <div class='card <%# (bool)Eval("EsOriginal") ? "original" : "" %>'>
                <img class="thumb" referrerpolicy="no-referrer" loading="lazy"
                    alt='<%# Eval("Titulo") %>'
                    src='<%# Eval("UrlImagen") %>'
                    onerror="this.src='/content/placeholder-art.png'" />
                <h3 class="title"><%# Eval("Titulo") %></h3>
                <p class="meta">
                    <%# Eval("Artista.Nombre") %> · <%# Eval("Anio") %> · <%# Eval("Tecnica") %>
                </p>
                <div class="price">
                    <%# (Context.Items["priceLabel"] as string) %><%# string.Format("${0:N2}", Eval("PrecioActual")) %>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty">
        <asp:Literal ID="litEmpty" runat="server" />
    </asp:Panel>
</asp:Content>
