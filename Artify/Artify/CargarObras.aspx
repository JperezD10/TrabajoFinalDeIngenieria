<%@ Page Title="Cargar Obras" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="CargarObras.aspx.cs" Inherits="Artify.CargarObras" %>

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

        .form-card {
            background: var(--panel);
            border: 1px solid var(--border);
            border-radius: 16px;
            box-shadow: var(--shadow);
            padding: 18px
        }

        .grid {
            display: grid;
            grid-template-columns: 1fr 420px;
            gap: 18px
        }

        .field {
            display: flex;
            flex-direction: column;
            gap: 6px;
            margin-bottom: 12px
        }

        .label {
            color: #e5e7eb;
            font-weight: 800
        }

        .input, .select, textarea {
            width: 100%;
            padding: 10px 12px;
            border-radius: 10px;
            background: #0b1220;
            color: #e5e7eb;
            border: 1px solid #334155;
            outline: none
        }

        .hint {
            color: #9ca3af;
            font-size: .9rem
        }

        .btns {
            display: flex;
            gap: 10px;
            margin-top: 10px
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

        .btn-primary {
            background: linear-gradient(135deg,#6366f1 0%,#7c3aed 100%);
            color: #fff
        }

        .btn-plain {
            background: #0b1220;
            color: #e5e7eb;
            border-color: #334155
        }

        .preview {
            background: #0b1220;
            border: 1px solid #334155;
            border-radius: 14px;
            overflow: hidden
        }

            .preview img {
                width: 100%;
                height: 260px;
                object-fit: cover;
                display: block
            }

        .preview-body {
            padding: 12px
        }

        .preview-title {
            margin: 0 0 4px 0;
            color: #e5e7eb;
            font-weight: 900;
            font-size: 1.1rem
        }

        .preview-sub {
            color: #9ca3af;
            font-size: .9rem;
            margin: 0
        }

        .alert {
            border-radius: 12px;
            padding: 10px 12px;
            margin: 10px 0
        }

        .alert-ok {
            background: rgba(34,197,94,.12);
            border: 1px solid rgba(34,197,94,.35);
            color: #a7f3d0
        }

        .alert-err {
            background: rgba(239,68,68,.15);
            border: 1px solid rgba(239,68,68,.35);
            color: #fecaca
        }

        .val-summary {
            margin: 8px 0
        }

        .switch-row{ display:flex; align-items:center; gap:10px }

.switch{ display:inline-block; }

.switch input[type="checkbox"]{
  -webkit-appearance:none; appearance:none;
  width:56px; height:32px; border-radius:9999px;
  background:#0b1220; border:1px solid #334155;
  box-shadow:inset 0 2px 6px rgba(0,0,0,.35);
  position:relative; cursor:pointer; outline:none;
  transition:background .25s ease, border-color .25s ease, box-shadow .25s ease;
  vertical-align:middle;
}

.switch input[type="checkbox"]::before{
  content:"";
  position:absolute; top:4px; left:4px;
  width:24px; height:24px; border-radius:9999px;
  background:#e5e7eb; box-shadow:0 2px 8px rgba(0,0,0,.45);
  transition:transform .25s ease, background .25s ease;
}

.switch input[type="checkbox"]:checked{
  background:linear-gradient(135deg,#6366f1 0%,#7c3aed 100%);
  border-color:transparent;
}
.switch input[type="checkbox"]:checked::before{
  transform:translateX(24px); background:#fff;
}

.switch input[type="checkbox"]:focus{
  box-shadow:0 0 0 3px rgba(99,102,241,.35);
}
    </style>

    <script type="text/javascript">
        function hookPreview() {
            var url = document.getElementById('<%= txtUrlImagen.ClientID %>');
            var img = document.getElementById('<%= imgPreview.ClientID %>');
            var titulo = document.getElementById('<%= txtTitulo.ClientID %>');
            var anio = document.getElementById('<%= txtAnio.ClientID %>');
            var tecnica = document.getElementById('<%= txtTecnica.ClientID %>');
            var precioBase = document.getElementById('<%= txtPrecioBase.ClientID %>');
            var precioActual = document.getElementById('<%= txtPrecioActual.ClientID %>');

            function refresh() {
                if (url.value && /^https?:\/\//i.test(url.value)) img.src = url.value;
                document.getElementById('prevTitle').textContent = titulo.value || document.getElementById('prevTitleSeed').value;
                document.getElementById('prevSub').textContent =
                    (anio.value || document.getElementById('prevYearSeed').value) + ' • ' + (tecnica.value || document.getElementById('prevTechSeed').value);
            }
            url.addEventListener('input', refresh);
            titulo.addEventListener('input', refresh);
            anio.addEventListener('input', refresh);
            tecnica.addEventListener('input', refresh);

            function syncPrice() { precioActual.value = precioBase.value; }
            precioBase.addEventListener('input', syncPrice);

            refresh(); syncPrice();
        }
        document.addEventListener('DOMContentLoaded', hookPreview);
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-heading">
        <h1>
            <asp:Literal ID="title" runat="server" /></h1>
    </div>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="val-summary alert alert-err"
        HeaderText="" />

    <asp:PlaceHolder ID="phOk" runat="server" Visible="false">
        <div class="alert alert-ok">
            <asp:Literal ID="ok" runat="server" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phErr" runat="server" Visible="false">
        <div class="alert alert-err">
            <asp:Literal ID="litErr" runat="server" />
        </div>
    </asp:PlaceHolder>

    <div class="form-card grid">
        <div>
            <div class="field">
                <label class="label" for="txtTitulo">
                    <asp:Literal ID="lblTitulo" runat="server" /></label>
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="input" MaxLength="120" />
                <span class="hint">
                    <asp:Literal ID="hintTitulo" runat="server" /></span>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitulo" ErrorMessage="" ID="valTitulo" Display="None" />
            </div>

            <div class="field">
                <label class="label" for="ddlArtistas">
                    <asp:Literal ID="lblArtista" runat="server" /></label>
                <asp:DropDownList ID="ddlArtistas" runat="server" CssClass="select" DataTextField="Nombre" DataValueField="Id" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlArtistas" InitialValue=""
                    ErrorMessage="" ID="valArtista" Display="None" />
            </div>

            <div class="field">
                <label class="label" for="txtAnio">
                    <asp:Literal ID="lblAnio" runat="server" /></label>
                <asp:TextBox ID="txtAnio" runat="server" CssClass="input" MaxLength="4" />
                <span class="hint">
                    <asp:Literal ID="hintAnio" runat="server" /></span>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAnio" ErrorMessage="" ID="valAnioReq" Display="None" />
                <asp:RangeValidator runat="server" ControlToValidate="txtAnio" MinimumValue="1000" MaximumValue="2100"
                    Type="Integer" ErrorMessage="" ID="valAnioRng" Display="None" />
            </div>

            <div class="field">
                <label class="label" for="txtTecnica">
                    <asp:Literal ID="lblTecnica" runat="server" /></label>
                <asp:TextBox ID="txtTecnica" runat="server" CssClass="input" MaxLength="120" />
                <span class="hint">
                    <asp:Literal ID="hintTecnica" runat="server" /></span>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTecnica" ErrorMessage="" ID="valTec" Display="None" />
            </div>

            <div class="field">
                <label class="label" for="<%= chkOriginal.ClientID %>">
                    <asp:Literal ID="lblOriginal" runat="server" /></label>
                <div class="switch-row">
                    <asp:CheckBox ID="chkOriginal" runat="server" CssClass="switch" />
                    <span class="hint">
                        <asp:Literal ID="hintOriginal" runat="server" /></span>
                </div>
            </div>

            <div class="field">
                <label class="label" for="txtPrecioBase">
                    <asp:Literal ID="lblPrecioBase" runat="server" /></label>
                <asp:TextBox ID="txtPrecioBase" runat="server" CssClass="input" />
                <span class="hint">
                    <asp:Literal ID="hintPrecioBase" runat="server" /></span>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrecioBase" ErrorMessage="" ID="valPBReq" Display="None" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPrecioBase"
                    ValidationExpression="^\d+([.,]\d{1,2})?$" ErrorMessage="" ID="valPBRegex" Display="None" />
                <asp:CustomValidator ID="cvPrecio" runat="server" ControlToValidate="txtPrecioBase"
                    OnServerValidate="cvPrecio_ServerValidate" ErrorMessage="" Display="None" />
            </div>

            <div class="field">
                <label class="label" for="txtUrlImagen">
                    <asp:Literal ID="lblUrlImg" runat="server" /></label>
                <asp:TextBox ID="txtUrlImagen" runat="server" CssClass="input" />
                <span class="hint">
                    <asp:Literal ID="hintUrlImg" runat="server" /></span>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUrlImagen" ErrorMessage="" ID="valUrlReq" Display="None" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtUrlImagen"
                    ValidationExpression="^https?:\/\/.+" ErrorMessage="" ID="valUrlRegex" Display="None" />
            </div>

            <div class="field">
                <label class="label" for="txtPrecioActual">
                    <asp:Literal ID="lblPrecioActual" runat="server" /></label>
                <asp:TextBox ID="txtPrecioActual" runat="server" CssClass="input" ReadOnly="true" />
                <span class="hint">
                    <asp:Literal ID="hintPrecioActual" runat="server" /></span>
            </div>

            <div class="btns">
                <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Text="" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-plain" Text="" OnClick="btnLimpiar_Click" CausesValidation="false" />
                <asp:HyperLink ID="lnkVolver" runat="server" CssClass="btn btn-plain" NavigateUrl="~/HomeCurador.aspx">
                    <asp:Literal ID="btnVolver" runat="server" />
                </asp:HyperLink>
            </div>
        </div>

        <div class="preview">
            <asp:Image ID="imgPreview" runat="server" ImageUrl="~/assets/placeholder-art.jpg" AlternateText="Vista previa" />
            <div class="preview-body">
                <h3 id="prevTitle" class="preview-title">
                    <asp:Literal ID="prevTitle" runat="server" /></h3>
                <p id="prevSub" class="preview-sub">
                    <asp:Literal ID="prevSub" runat="server" />
                </p>
                <asp:HiddenField ID="prevTitleSeed" runat="server" />
                <asp:HiddenField ID="prevYearSeed" runat="server" />
                <asp:HiddenField ID="prevTechSeed" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
