<%@ Page Title="Permisos por Usuario" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="PermisosUsuario.aspx.cs" Inherits="Artify.PermisosUsuario" %>

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
            padding: 20px;
            box-shadow: var(--shadow);
            margin-bottom: 18px
        }

        .row {
            display: flex;
            gap: 16px;
            align-items: flex-end;
            flex-wrap: wrap
        }

        .grow {
            flex: 1
        }

        .list {
            margin-top: 12px;
            max-height: 420px;
            overflow: auto;
            border: 1px solid var(--border);
            border-radius: 12px;
            padding: 10px
        }

        .actions {
            display: flex;
            gap: 10px;
            justify-content: flex-end;
            margin-top: 14px
        }

        .hint {
            font-size: .9rem;
            opacity: .85;
            margin-top: 8px
        }

        .badge {
            display: inline-block;
            padding: 4px 8px;
            border-radius: 999px;
            border: 1px solid var(--border);
            margin-left: 8px
        }

        .form-control,
        input.form-control,
        select.form-control,
        textarea.form-control {
            width: 100%;
            min-height: 40px;
            padding: 10px 12px;
            border-radius: 12px;
            border: 1px solid var(--border);
            background: var(--panel);
            color: var(--text, #e5e7eb);
            outline: none;
            transition: border-color .15s ease, box-shadow .15s ease, background .15s ease;
        }

            .form-control:focus,
            input.form-control:focus,
            select.form-control:focus,
            textarea.form-control:focus {
                border-color: #7c3aed; /* violeta del header */
                box-shadow: 0 0 0 3px rgba(124,58,237,.25);
                background: rgba(255,255,255,.02);
            }

            .form-control::placeholder {
                color: rgba(229,231,235,.55);
            }

        /* select flechita (sin librerías) */
        select.form-control {
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            background-image: linear-gradient(45deg, transparent 50%, #9aa0a6 50%), linear-gradient(135deg, #9aa0a6 50%, transparent 50%), linear-gradient(to right, transparent, transparent);
            background-position: calc(100% - 18px) calc(50% - 3px), calc(100% - 12px) calc(50% - 3px), calc(100% - 2.2rem) 0;
            background-size: 6px 6px, 6px 6px, 1px 100%;
            background-repeat: no-repeat;
            padding-right: 2.4rem;
        }

        .btn,
        input[type="button"].btn,
        input[type="submit"].btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            height: 40px;
            padding: 0 14px;
            border-radius: 12px;
            border: 1px solid var(--border);
            background: rgba(255,255,255,.06);
            color: var(--text, #e5e7eb);
            font-weight: 600;
            cursor: pointer;
            transition: transform .08s ease, filter .15s ease, box-shadow .15s ease;
            box-shadow: 0 2px 8px rgba(0,0,0,.25);
        }

            .btn:hover {
                transform: translateY(-1px);
                filter: brightness(1.05);
            }

            .btn:active {
                transform: translateY(0);
                filter: brightness(.98);
            }

            .btn:disabled {
                opacity: .6;
                cursor: default;
            }

            .btn-primary,
            input[type="button"].btn.btn-primary,
            input[type="submit"].btn.btn-primary {
                border: 0;
                color: #fff;
                background: linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);
            }

        /* ==== CheckBoxList look & feel ==== */
        .list {
            background: rgba(255,255,255,.02);
        }

            .list input[type="checkbox"] {
                margin-right: 8px;
                transform: translateY(1px);
            }

                .list input[type="checkbox"][disabled] {
                    opacity: .55;
                }

            .list label {
                display: inline-block;
                margin: 2px 0 6px 0;
            }

        .flash {
            margin-top: 10px;
        }

        .flash {
            padding: 10px 12px;
            border-radius: 12px;
            border: 1px solid;
            display: block;
            opacity: 1;
            transition: opacity .3s ease;
        }

        .flash--ok {
            background: rgba(16,185,129,.12);
            border-color: #10b981;
            color: #d1fae5;
        }

        .flash--err {
            background: rgba(239,68,68,.12);
            border-color: #ef4444;
            color: #fee2e2;
        }
    </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-heading">
        <h1>
            <asp:Literal ID="litPageTitle" runat="server" Text="Permisos adicionales por usuario"></asp:Literal></h1>
    </div>

    <div class="hero">
        <div class="row">
            <div class="grow">
                <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="ddlUsuarios" Text="Usuario"></asp:Label>
                <asp:DropDownList ID="ddlUsuarios" runat="server" CssClass="form-control" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged" />
                <div class="hint">
                    <asp:Literal ID="litUsuarioHint" runat="server"
                        Text="Elegí el usuario para ver y asignar permisos extra a los que ya tiene por su rol."></asp:Literal>
                </div>
            </div>
            <div class="grow">
                <asp:Label ID="lblFiltro" runat="server" AssociatedControlID="txtBuscar" Text="Filtrar por ruta .aspx"></asp:Label>
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" />
                <div class="actions" style="justify-content: flex-start">
                    <asp:Button ID="btnFiltrar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
                    <asp:Button ID="btnLimpiarFiltro" runat="server" Text="Limpiar" CssClass="btn" OnClick="btnLimpiarFiltro_Click" />
                </div>
            </div>
            <div class="grow" style="align-self: center">
                <span class="badge">
                    <asp:Literal ID="litRolBaseLabel" runat="server" Text="Rol base:"></asp:Literal>
                    <strong>
                        <asp:Literal ID="litRolActual" runat="server" Text="-"></asp:Literal></strong>
                </span>
            </div>
        </div>
    </div>

    <div class="hero">
        <asp:Label ID="lblDisponibles" runat="server" Text="Permisos disponibles (.aspx)"></asp:Label>
        <div class="list">
            <asp:CheckBoxList ID="cblPermisos" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical" />
        </div>

        <div class="actions">
            <asp:Button ID="btnSelectAll" runat="server" Text="Seleccionar todo" CssClass="btn" OnClick="btnSelectAll_Click" />
            <asp:Button ID="btnUnselectAll" runat="server" Text="Quitar selección" CssClass="btn" OnClick="btnUnselectAll_Click" />
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar cambios" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
        </div>
        <div class="flash">
            <asp:Panel ID="pnlFlash" runat="server" Visible="false" CssClass="flash">
                <asp:Literal ID="litFlash" runat="server" />
            </asp:Panel>
        </div>

        <div class="hint">
            <asp:Literal ID="litNota" runat="server"
                Text="Lo marcado acá se suma a las páginas permitidas por el rol. No quita permisos del rol."></asp:Literal>
        </div>
    </div>
</asp:Content>
