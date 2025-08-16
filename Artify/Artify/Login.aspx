<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Artify.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        :root{
            --bg:#111827; --panel:#1f2937; --border:#374151;
            --shadow:0 12px 36px rgba(0,0,0,.55);
            --ink:#e5e7eb; --muted:#9ca3af;
            --accent:#7c3aed; --accent-2:#6366f1;
            --danger-bg:rgba(239,68,68,.15); --danger-bd:rgba(252,165,165,.5); --danger-ink:#fca5a5;
        }
        *{box-sizing:border-box}
        html,body{height:100%}
        body{
            margin:0; font-family:'Segoe UI',Arial,sans-serif;
            background:var(--bg); color:var(--ink);
            display:flex; align-items:center; justify-content:center; padding:24px;
        }

        /* ANCHO FIJO GRANDE */
        .login{
            width:760px;                    /* <<— ACA EL ANCHO */
            background:var(--panel);
            border:1px solid var(--border);
            border-radius:20px;
            box-shadow:var(--shadow);
            padding:40px 36px;
        }

        .login h2{
            margin:0 0 6px 0; color:#fff; font-size:2.4rem; font-weight:900; text-align:center;
        }
        .subtitle{ margin:0 0 24px 0; color:var(--muted); text-align:center; font-size:1.05rem; }

        .alert{
            margin:10px 0 22px 0; padding:12px 14px; border-radius:12px; border:1px solid var(--danger-bd);
            background:var(--danger-bg); color:var(--danger-ink); font-weight:600;
        }

        .field{ margin-bottom:22px; }
        .label{ display:block; margin-bottom:10px; color:#cbd5e1; font-weight:900; font-size:1.2rem; }

        .pwd-wrap{ position:relative; }

        .input{
            width:100%;
            padding:16px 56px 16px 16px;   /* espacio para el ojo */
            border-radius:14px;
            border:1px solid var(--border);
            background:#0f172a; color:#e5e7eb;
            font-size:1.1rem; line-height:1.4;
            outline:none;
            transition:border-color .15s ease, box-shadow .15s ease;
        }
        .input:focus{ border-color:#4b5563; box-shadow:0 0 0 3px rgba(99,102,241,.25); }

        .text-danger{ color:#fca5a5; font-size:1rem; display:block; margin-top:8px; }

        .toggle-eye{
            position:absolute; right:12px; top:50%; transform:translateY(-50%);
            background:transparent; border:0; color:#cbd5e1; cursor:pointer;
            display:grid; place-items:center; padding:10px; border-radius:12px;
        }
        .toggle-eye:hover{ background:#0b1220; }
        .toggle-eye svg{ width:24px; height:24px }

        .btn{
            width:100%; padding:16px 18px;
            border-radius:16px; cursor:pointer; font-weight:900; border:0; color:#fff;
            background:linear-gradient(90deg,var(--accent),var(--accent-2));
            box-shadow:0 10px 30px rgba(124,58,237,.35);
            font-size:1.15rem; letter-spacing:.3px;
        }
        .btn:hover{ filter:brightness(1.06); }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="login">
        <h2>Iniciar Sesión</h2>
        <p class="subtitle">Acceso al panel del WebMaster</p>

        <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </asp:Panel>

        <div class="field">
            <label for="txtEmail" class="label">Correo electrónico</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                ControlToValidate="txtEmail" ErrorMessage="Correo requerido"
                CssClass="text-danger" Display="Dynamic" ValidationGroup="Login" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="txtEmail"
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                ErrorMessage="Formato de correo inválido"
                CssClass="text-danger" Display="Dynamic" ValidationGroup="Login" />
        </div>

        <div class="field">
            <label for="txtPassword" class="label">Contraseña</label>
            <div class="pwd-wrap">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input" />
                <button type="button" class="toggle-eye" onclick="togglePwd()" aria-label="Mostrar u ocultar contraseña">
                    <svg id="eyeIcon" viewBox="0 0 24 24" fill="none" aria-hidden="true">
                        <path d="M2 12s4-7 10-7 10 7 10 7-4 7-10 7S2 12 2 12Z" stroke="currentColor" stroke-width="1.8"/>
                        <circle cx="12" cy="12" r="3.2" stroke="currentColor" stroke-width="1.8"/>
                    </svg>
                </button>
            </div>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                ControlToValidate="txtPassword" ErrorMessage="Contraseña requerida"
                CssClass="text-danger" Display="Dynamic" ValidationGroup="Login" />
            <asp:RegularExpressionValidator ID="revPassword" runat="server"
                ControlToValidate="txtPassword"
                ValidationExpression="^(?=.*[A-Z])(?=.*\d)(?=.*[_])[A-Za-z\d_]{8,}$"
                ErrorMessage="Mínimo 8 caracteres, 1 mayúscula, 1 número y '_'"
                CssClass="text-danger" Display="Dynamic" ValidationGroup="Login" />
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Ingresar" CssClass="btn"
            OnClick="btnLogin_Click" ValidationGroup="Login" />
    </div>
</form>

<script>
    function togglePwd() {
        var input = document.getElementById('<%= txtPassword.ClientID %>');
        var icon = document.getElementById('eyeIcon');
        if (!input) return;
        var showing = input.type === 'text';
        input.type = showing ? 'password' : 'text';
        icon.innerHTML = showing
            ? '<path d="M2 12s4-7 10-7 10 7 10 7-4 7-10 7S2 12 2 12Z" stroke="currentColor" stroke-width="1.8"/><circle cx="12" cy="12" r="3.2" stroke="currentColor" stroke-width="1.8"/>'
            : '<path d="M2 12s4-7 10-7 10 7 10 7-4 7-10 7S2 12 2 12Z" stroke="currentColor" stroke-width="1.8"/><path d="M4 4l16 16" stroke="currentColor" stroke-width="1.8" stroke-linecap="round"/>';
    }
</script>
</body>
</html>