<%@ Page Title="Bitácora" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="Artify.Bitacora" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Toolbar */
        .toolbar {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 16px;
        }

        .title {
            margin: 0;
            font-size: 1.6rem;
            font-weight: 800;
            color: #f9fafb;
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
                height: 18px;
            }

        /* Filtros */
        .filters {
            display: flex;
            align-items: flex-end;
            gap: 10px;
        }

        .field {
            display: flex;
            flex-direction: column;
        }

            .field .label {
                color: #cbd5e1;
                font-weight: 800;
                font-size: .95rem;
                margin: 0 0 6px;
            }

        .input {
            padding: 10px 12px;
            border-radius: 10px;
            border: 1px solid var(--border);
            background: #0f172a;
            color: #e5e7eb;
        }

        .btn-secondary {
            border: 1px solid var(--border);
            background: var(--panel);
            color: var(--ink);
            padding: 10px 14px;
            border-radius: 10px;
            font-weight: 700;
            cursor: pointer;
        }

            .btn-secondary:hover {
                background: #374151;
                border-color: #4b5563;
            }

        /* Panel */
        .panel {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            box-shadow: var(--shadow);
            padding: 18px;
        }

        .panel-head {
            margin-bottom: 10px;
        }

        .panel-title {
            margin: 0;
            font-size: 1.25rem;
            font-weight: 800;
            color: #fff;
        }

        .panel-sub {
            margin: 0;
            color: var(--muted);
            font-size: .95rem;
        }

        /* GridView */
        .gridview {
            width: 100%;
            border-collapse: collapse;
            border-radius: 12px;
            overflow: hidden;
            margin-top: 8px;
        }

            .gridview th {
                background: #6d28d9;
                color: #f9fafb;
                padding: 12px;
                text-align: left;
                font-weight: 600;
                border-bottom: 1px solid #4b5563;
            }

            .gridview td {
                padding: 10px 12px;
                border-bottom: 1px solid var(--border);
                color: var(--ink);
                background: var(--panel);
            }

            .gridview tr:hover td {
                background: #374151;
            }

            /* Pager */
            .gridview .pager {
                background: var(--panel);
                text-align: center;
                padding: 10px 0;
                border-top: 1px solid var(--border);
            }

                .gridview .pager a, .gridview .pager span {
                    display: inline-block;
                    margin: 0 3px;
                    padding: 6px 12px;
                    border-radius: 8px;
                    text-decoration: none;
                    font-weight: 700;
                }

                .gridview .pager a {
                    background: var(--bg);
                    color: var(--accent);
                    border: 1px solid var(--border);
                }

                    .gridview .pager a:hover {
                        background: var(--accent);
                        color: #fff;
                    }

                .gridview .pager span {
                    background: var(--accent);
                    color: #fff;
                    border: 1px solid var(--accent);
                }

        /* Badges Criticidad */
        .badge {
            display: inline-block;
            padding: 4px 10px;
            border-radius: 999px;
            font-size: .85em;
            font-weight: 700;
        }

        .badge-leve {
            background: rgba(16,185,129,.15);
            color: #34d399;
        }

        .badge-moderada {
            background: rgba(245,158,11,.15);
            color: #fcd34d;
        }

        .badge-alta {
            background: rgba(249,115,22,.15);
            color: #fb923c;
        }

        .badge-critica {
            background: rgba(239,68,68,.2);
            color: #fca5a5;
        }
    </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="toolbar">
        <h1 class="title">Bitácora</h1>

        <div class="filters">
            <div class="field">
                <asp:Label ID="lblDesde" runat="server" CssClass="label" AssociatedControlID="txtDesde" />
                <asp:TextBox ID="txtDesde" runat="server" TextMode="Date" CssClass="input" />
            </div>
            <div class="field">
                <asp:Label ID="lblHasta" runat="server" CssClass="label" AssociatedControlID="txtHasta" />
                <asp:TextBox ID="txtHasta" runat="server" TextMode="Date" CssClass="input" />
            </div>
            <asp:Button ID="btnFiltrar" runat="server" CssClass="btn-secondary" OnClick="btnFiltrar_Click" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="btn-secondary" OnClick="btnLimpiar_Click" UseSubmitBehavior="false" />
            <asp:Button ID="btnDescargarXml" runat="server" CssClass="btn" Text="📥" OnClick="btnDescargarXml_Click" UseSubmitBehavior="false" />
            <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/HomeWebMaster.aspx" CssClass="btn">
                <svg viewBox="0 0 24 24" fill="none" aria-hidden="true">
                    <path d="M15 19l-7-7 7-7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
                </svg>
                <span>
                    <asp:Literal ID="litBack" runat="server" /></span>
            </asp:HyperLink>
        </div>
    </div>

    <div class="panel">
        <div class="panel-head">
            <h3 class="panel-title">
                <asp:Literal ID="litPanelTitle" runat="server" />
            </h3>
            <p class="panel-sub">
                <asp:Literal ID="litPanelSub" runat="server" />
            </p>
        </div>

        <asp:GridView ID="gvBitacora" runat="server"
            AutoGenerateColumns="false"
            CssClass="gridview"
            AllowPaging="true"
            PageSize="15"
            OnPageIndexChanging="gvBitacora_PageIndexChanging"
            OnRowDataBound="gvBitacora_RowDataBound"
            PagerStyle-CssClass="pager">
            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                <asp:BoundField DataField="Accion" HeaderText="Acción" />
                <asp:TemplateField HeaderText="Criticidad">
                    <ItemTemplate>
                        <asp:Label ID="lblCriticidad" runat="server" Text='<%# Eval("Criticidad") %>' CssClass="badge" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
