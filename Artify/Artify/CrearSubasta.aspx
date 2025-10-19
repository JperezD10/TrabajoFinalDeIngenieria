<%@ Page Title="Crear Subasta" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="CrearSubasta.aspx.cs" Inherits="Artify.CrearSubasta" %>

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

        .row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px
        }

        .field {
            display: flex;
            flex-direction: column;
            gap: 6px
        }

            .field label {
                color: var(--ink);
                font-weight: 600
            }

        .input, select {
            background: #0f172a;
            border: 1px solid var(--border);
            color: var(--ink);
            border-radius: 10px;
            padding: 10px 12px;
            outline: 0
        }

        .actions {
            display: flex;
            gap: 10px;
            justify-content: flex-end;
            margin-top: 12px
        }

        .btn {
            border: 0;
            border-radius: 12px;
            padding: 10px 14px;
            font-weight: 700;
            cursor: pointer;
            box-shadow: var(--shadow)
        }

        .btn-primary {
            background: #7c3aed;
            color: #fff
        }

        .btn-secondary {
            background: #1e293b;
            color: #e2e8f0;
        }

            .btn-secondary:hover {
                background: #334155;
            }

        .msg {
            border-radius: 12px;
            padding: 12px 14px;
            margin-bottom: 12px
        }

        .msg-ok {
            background: rgba(16,185,129,.15);
            border: 1px solid rgba(16,185,129,.35);
            color: #a7f3d0
        }

        .msg-err {
            background: rgba(239,68,68,.15);
            border: 1px solid rgba(252,165,165,.5);
            color: #fecaca
        }

        .hint {
            color: var(--muted);
            font-size: .9rem
        }

        @media(max-width:900px) {
            .row {
                grid-template-columns: 1fr
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-heading">
        <h1>
            <asp:Literal ID="litPageTitle" runat="server" Text="Crear subasta"></asp:Literal></h1>
    </div>

    <asp:Panel ID="pnlMsgOk" runat="server" CssClass="msg msg-ok" Visible="false">
        <asp:Literal ID="litOk" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlMsgErr" runat="server" CssClass="msg msg-err" Visible="false">
        <asp:Literal ID="litErr" runat="server" />
    </asp:Panel>

    <div class="hero">
        <div class="row">
            <div class="field">
                <label for="ddlObra">
                    <asp:Literal ID="litObra" runat="server" Text="Obra"></asp:Literal></label>
                <asp:DropDownList ID="ddlObra" runat="server" CssClass="input"
                    DataTextField="Titulo" DataValueField="Id"
                    AppendDataBoundItems="true" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlObra_SelectedIndexChanged">
                    <asp:ListItem Text="-- seleccionar --" Value="" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvObra" runat="server" ControlToValidate="ddlObra"
                    InitialValue="" Display="Dynamic" CssClass="hint" ErrorMessage="Seleccione una obra." />
            </div>

            <div class="field">
                <label for="txtPrecioInicial">
                    <asp:Literal ID="litPrecioInicial" runat="server" Text="Precio inicial"></asp:Literal></label>
                <asp:TextBox ID="txtPrecioInicial" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvPI" runat="server" ControlToValidate="txtPrecioInicial"
                    Display="Dynamic" CssClass="hint" ErrorMessage="Requerido." />
                <asp:RegularExpressionValidator ID="revPI" runat="server" ControlToValidate="txtPrecioInicial"
                    ValidationExpression="^\d+(\.\d{1,2})?$" Display="Dynamic" CssClass="hint"
                    ErrorMessage="Formato numérico (dos decimales)." />
            </div>

            <div class="field">
                <label for="txtIncremento">
                    <asp:Literal ID="litInc" runat="server" Text="Incremento mínimo"></asp:Literal></label>
                <asp:TextBox ID="txtIncremento" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvInc" runat="server" ControlToValidate="txtIncremento"
                    Display="Dynamic" CssClass="hint" ErrorMessage="Requerido." />
                <asp:RegularExpressionValidator ID="revInc" runat="server" ControlToValidate="txtIncremento"
                    ValidationExpression="^\d+(\.\d{1,2})?$" Display="Dynamic" CssClass="hint"
                    ErrorMessage="Formato numérico (dos decimales)." />
            </div>

            <div class="field">
                <label for="txtDuracion">
                    <asp:Literal ID="litDuracion" runat="server" Text="Duración (min)"></asp:Literal></label>
                <asp:TextBox ID="txtDuracion" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvDur" runat="server" ControlToValidate="txtDuracion"
                    Display="Dynamic" CssClass="hint" ErrorMessage="Requerido." />
                <asp:RegularExpressionValidator ID="revDur" runat="server" ControlToValidate="txtDuracion"
                    ValidationExpression="^\d+$" Display="Dynamic" CssClass="hint"
                    ErrorMessage="Ingrese minutos enteros." />
            </div>

            <div class="field">
                <label for="txtFechaProg">
                    <asp:Literal ID="litFechaProg" runat="server" Text="Fecha programada (opcional)"></asp:Literal></label>
                <asp:TextBox ID="txtFechaProg" runat="server" CssClass="input" Placeholder="dd/MM/yyyy HH:mm" />
                <span class="hint">Si la dejás vacía, queda pendiente sin fecha programada.</span>
            </div>

            <div class="field">
                <label for="txtCurador">
                    <asp:Literal ID="litCurador" runat="server" Text="Curador"></asp:Literal></label>
                <asp:TextBox ID="txtCurador" runat="server" CssClass="input" ReadOnly="true" />
            </div>
        </div>

        <div class="actions">
            <asp:Button ID="btnVolver" runat="server" CssClass="btn btn-secondary" Text="Volver al inicio" OnClick="btnVolver_Click" CausesValidation="false"/>
            <asp:Button ID="btnCrear" runat="server" CssClass="btn btn-primary" Text="Crear subasta" OnClick="btnCrear_Click"  />
        </div>
    </div>
</asp:Content>
