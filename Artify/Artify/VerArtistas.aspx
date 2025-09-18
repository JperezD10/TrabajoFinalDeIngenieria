<%@ Page Title="Ver Artistas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="VerArtistas.aspx.cs" Inherits="Artify.VerArtistas" %>

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
                font-size: 1.75rem;
                color: #fff
            }

            .hero p {
                margin: 0;
                color: var(--muted)
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
            box-shadow: var(--shadow);
            overflow: hidden;
            display: flex;
            flex-direction: column
        }

        .thumb {
            position: relative;
            width: 100%;
            aspect-ratio: 16/9;
            background: #0b1220;
            display: block;
            overflow: hidden
        }

            .thumb img {
                width: 100%;
                height: 100%;
                object-fit: cover;
                display: block
            }

        .body {
            padding: 14px 16px
        }

        .title {
            margin: 0 0 4px 0;
            color: #fff;
            font-weight: 800;
            font-size: 1.05rem
        }

        .meta {
            margin: 0;
            color: #cbd5e1;
            font-size: .9rem
        }

        .bio {
            margin: 8px 0 10px 0;
            color: var(--muted);
            font-size: .9rem;
            line-height: 1.35
        }

        .row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 8px
        }

        .btn-danger {
            background: linear-gradient(135deg,#ef4444 0%,#b91c1c 100%);
            color: #fff;
            font-weight: 700;
            padding: 8px 12px;
            border: none;
            border-radius: 10px;
            cursor: pointer
        }

        .empty {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            padding: 24px;
            text-align: center;
            color: #cbd5e1
        }

            .empty h3 {
                margin: 0 0 6px 0;
                color: #fff
            }

        .bio {
            margin: 8px 0 10px 0;
            color: var(--muted);
            font-size: .9rem;
            line-height: 1.35;
            display: -webkit-box;
            -webkit-line-clamp: 4;
            -webkit-box-orient: vertical;
            overflow: hidden;
            word-break: break-word;
        }

        .btn {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 8px 14px;
            border-radius: 10px;
            font-weight: 700;
            text-decoration: none;
            cursor: pointer;
            border: 1px solid var(--border);
        }

        .btn-plain {
            background: #0b1220;
            color: #e5e7eb;
            border-color: #334155;
            transition: background .2s;
        }

            .btn-plain:hover {
                background: #1e293b;
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
        <div style="display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 12px">
            <div>
                <h1>
                    <asp:Literal ID="litHeroTitle" runat="server" /></h1>
                <p>
                    <asp:Literal ID="litHeroText" runat="server" /></p>
            </div>
            <asp:HyperLink ID="lnkVolver" runat="server"
                NavigateUrl="~/HomeCurador.aspx" CssClass="btn btn-plain">
                <asp:Literal ID="litBtnVolver" runat="server" />
            </asp:HyperLink>
        </div>
    </div>

    <asp:Panel ID="pnlEmpty" runat="server" CssClass="empty" Visible="false">
        <h3>
            <asp:Literal ID="litEmptyTitle" runat="server" /></h3>
        <p>
            <asp:Literal ID="litEmptyText" runat="server" />
        </p>
    </asp:Panel>

    <asp:Repeater ID="rptArtistas" runat="server" OnItemDataBound="rptArtistas_ItemDataBound" OnItemCommand="rptArtistas_ItemCommand">
        <HeaderTemplate>
            <div class="grid">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="card">
                <a class="thumb" aria-label='<%# Eval("Nombre") %>'>
                    <asp:Image ID="imgFoto" runat="server" ImageUrl='<%# Eval("UrlFoto") %>' AlternateText='<%# Eval("Nombre") %>' />
                </a>
                <div class="body">
                    <h3 class="title"><%# Eval("Nombre") %></h3>
                    <p class="meta">
                        <%# Eval("Nacionalidad") %>
                        <%# Eval("FechaNacimiento", " • {0:dd/MM/yyyy}") %>
                    </p>
                    <p class="bio" title='<%# GetBioFull(Container.DataItem) %>'>
                        <%# GetBioSnippet(Container.DataItem) %>
                    </p>
                    <div class="row">
                        <span></span>
                        <asp:Button ID="btnEliminar" runat="server"
                            CommandName="Delete" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn-danger" />
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>
</asp:Content>
