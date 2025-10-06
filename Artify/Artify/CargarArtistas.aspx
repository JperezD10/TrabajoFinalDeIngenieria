<%@ Page Title="Cargar Artistas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="CargarArtistas.aspx.cs" Inherits="Artify.CargarArtistas" %>

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

        .form-card {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            padding: 24px;
            box-shadow: var(--shadow);
            max-width: 700px;
            margin: 0 auto
        }

        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 16px
        }

        @media (max-width:820px) {
            .form-grid {
                grid-template-columns: 1fr
            }
        }

        .form-group {
            margin-bottom: 16px
        }

            .form-group label {
                display: block;
                font-weight: 600;
                color: #e5e7eb;
                margin-bottom: 6px
            }

            .form-group input, .form-group textarea {
                width: 100%;
                padding: 10px 12px;
                border-radius: 10px;
                border: 1px solid var(--border);
                background: #0b1220;
                color: #f9fafb;
                font-size: .95rem
            }

            .form-group textarea {
                resize: vertical;
                min-height: 110px
            }

        .btn-primary {
            background: linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);
            color: #fff;
            font-weight: 700;
            padding: 10px 16px;
            border: none;
            border-radius: 10px;
            cursor: pointer;
            transition: opacity .2s
        }

            .btn-primary:hover {
                opacity: .9
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

        .val-summary {
            background: var(--danger-bg);
            border: 1px solid var(--danger-bd);
            color: var(--danger-ink);
            padding: 12px;
            border-radius: 10px;
            margin: 0 0 16px 0
        }

        .val-msg {
            display: block;
            color: #fca5a5;
            font-size: .9rem;
            margin-top: 6px
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
                    <asp:Literal ID="litHeroText" runat="server" />
                </p>
            </div>
            <asp:HyperLink ID="lnkVolver" runat="server"
                NavigateUrl="~/HomeCurador.aspx" CssClass="btn btn-plain">
                <asp:Literal ID="litBtnVolver" runat="server" />
            </asp:HyperLink>
        </div>
    </div>

    <div class="form-card">
        <p class="val-summary">
            <asp:Literal ID="litValSummaryHeader" runat="server" />
        </p>
        <asp:ValidationSummary ID="valSummary" runat="server"
            CssClass="val-summary" DisplayMode="BulletList"
            ShowSummary="true" ShowMessageBox="false" ValidationGroup="art" />

        <div class="form-grid">
            <div class="form-group">
                <label for="txtNombre">
                    <asp:Literal ID="litLblNombre" runat="server" /></label>
                <asp:TextBox ID="txtNombre" runat="server" MaxLength="120" />
                <asp:RequiredFieldValidator ID="reqNombre" runat="server"
                    ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio."
                    CssClass="val-msg" Display="Dynamic" ValidationGroup="art" />
            </div>

            <div class="form-group">
                <label for="txtNacionalidad">
                    <asp:Literal ID="litLblNac" runat="server" /></label>
                <asp:TextBox ID="txtNacionalidad" runat="server" MaxLength="80" />
                <asp:RequiredFieldValidator ID="reqNac" runat="server"
                    ControlToValidate="txtNacionalidad" ErrorMessage="La nacionalidad es obligatoria."
                    CssClass="val-msg" Display="Dynamic" ValidationGroup="art" />
            </div>

            <div class="form-group">
                <label for="txtFechaNacimiento">
                    <asp:Literal ID="litLblFechaNac" runat="server" /></label>

                <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" />

                <asp:CompareValidator ID="cmpFecha" runat="server"
                    ControlToValidate="txtFechaNacimiento" Operator="DataTypeCheck" Type="Date"
                    ErrorMessage="Formato de fecha inválido."
                    CssClass="val-msg" Display="Dynamic" ValidationGroup="art" />

                <asp:RangeValidator ID="rngFechaMax" runat="server"
                    ControlToValidate="txtFechaNacimiento" Type="Date"
                    MinimumValue="1400-01-01"
                    MaximumValue="2100-01-01"
                    ErrorMessage="La fecha no puede ser futura."
                    CssClass="val-msg" Display="Dynamic" ValidationGroup="art" />
            </div>

            <div class="form-group">
                <label for="txtUrlFoto">
                    <asp:Literal ID="litLblUrl" runat="server" /></label>
                <asp:TextBox ID="txtUrlFoto" runat="server" MaxLength="300" />
                <asp:RegularExpressionValidator ID="revUrl" runat="server"
                    ControlToValidate="txtUrlFoto"
                    ValidationExpression="^https?://[^\s]+$"
                    ErrorMessage="URL inválida. Ej: https://sitio.com/foto.jpg"
                    CssClass="val-msg" Display="Dynamic" ValidationGroup="art"
                    SetFocusOnError="true" />
            </div>
        </div>

        <div class="form-group">
            <label for="txtBio">
                <asp:Literal ID="litLblBio" runat="server" /></label>
            <asp:TextBox ID="txtBio" TextMode="MultiLine" runat="server" MaxLength="2000" />
        </div>

        <asp:Button ID="btnGuardar" runat="server" CssClass="btn-primary"
            OnClick="btnGuardar_Click" ValidationGroup="art" />
    </div>
</asp:Content>
