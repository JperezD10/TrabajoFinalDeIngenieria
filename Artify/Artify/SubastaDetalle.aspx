<%@ Page Title="Detalle de Subasta" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="SubastaDetalle.aspx.cs" Inherits="Artify.SubastaDetalle" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .panel-heading {
            background: linear-gradient(135deg,#7c3aed 0%,#2563eb 100%);
            padding: 18px 24px;
            border-radius: 14px;
            text-align: center;
            margin-bottom: 14px;
            box-shadow: 0 8px 24px rgba(0,0,0,.5)
        }

            .panel-heading h1 {
                margin: 0;
                font-size: 2rem;
                font-weight: 900;
                color: #fff;
                letter-spacing: .5px
            }

        /* Botón superior único */
        .top-actions {
            display: flex;
            justify-content: flex-end;
            margin: 0 0 16px 0
        }

        .btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            border: none;
            border-radius: 12px;
            padding: 12px 16px;
            font-weight: 800;
            cursor: pointer;
            text-decoration: none
        }

        .btn-primary {
            background: #2563eb;
            color: #fff
        }

            .btn-primary:hover {
                filter: brightness(1.1)
            }

        .btn-ghost {
            background: transparent;
            color: #e5e7eb;
            border: 1px solid #334155
        }

            .btn-ghost:hover {
                filter: brightness(1.08)
            }

        .auction-wrap {
            display: grid;
            grid-template-columns: 1.1fr .9fr;
            gap: 18px
        }

        @media(max-width:980px) {
            .auction-wrap {
                grid-template-columns: 1fr
            }
        }

        .card {
            background: #111827;
            border: 1px solid #334155;
            border-radius: 16px;
            padding: 18px;
            box-shadow: 0 12px 36px rgba(0,0,0,.55)
        }

            .card h2 {
                margin: 0 0 10px 0;
                color: #f3f4f6;
                font-size: 1.25rem;
                font-weight: 800
            }

        .meta {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 10px;
            margin-top: 10px
        }

            .meta div {
                background: #0b1220;
                border: 1px solid #334155;
                border-radius: 12px;
                padding: 12px
            }

            .meta .k {
                display: block;
                font-size: .8rem;
                color: #9ca3af;
                margin-bottom: 4px
            }

            .meta .v {
                color: #e5e7eb;
                font-weight: 700
            }

        .hero-img {
            width: 100%;
            aspect-ratio: 4/3;
            object-fit: cover;
            border-radius: 14px;
            border: 1px solid #334155
        }

        .price-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
            margin: 10px 0 6px
        }

        .price {
            font-size: 1.6rem;
            font-weight: 900;
            color: #e5e7eb
        }

        .muted {
            color: #9ca3af;
            font-size: .9rem
        }

        .countdown {
            font-family: ui-monospace,SFMono-Regular,Menlo,monospace;
            font-weight: 800
        }

        .badge {
            display: inline-block;
            padding: 6px 10px;
            border-radius: 999px;
            border: 1px solid #334155;
            color: #d1d5db;
            background: #0b1220
        }

        .input {
            width: 100%;
            background: #0b1220;
            color: #e5e7eb;
            border: 1px solid #334155;
            border-radius: 12px;
            padding: 12px 14px;
            font-size: 1rem
        }

        .actions {
            display: flex;
            gap: 10px;
            flex-wrap: wrap
        }

        .val-msg {
            color: #fca5a5;
            font-size: .9rem;
            margin-top: 6px
        }

        .ok-msg {
            color: #86efac;
            font-size: .9rem;
            margin-top: 6px
        }
    </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-heading">
        <h1>
            <asp:Literal ID="litPageTitle" runat="server" /></h1>
    </div>


    <div class="top-actions">
        <asp:HyperLink ID="lnkBack" runat="server" CssClass="btn btn-ghost" NavigateUrl="~/HomeCliente.aspx" />
    </div>

    <asp:HiddenField ID="hfSubastaId" runat="server" />
    <asp:HiddenField ID="hfEndsAtIso" runat="server" />

    <div class="auction-wrap">
        <div class="card">
            <h2>
                <asp:Literal ID="litInfoSection" runat="server" /></h2>
            <asp:Image ID="imgObra" runat="server" CssClass="hero-img" ImageUrl="~/img/placeholder.png" />
            <div class="meta">
                <div><span class="k">
                    <asp:Literal ID="litLblTitulo" runat="server" /></span><span class="v"><asp:Literal ID="litTitulo" runat="server" /></span></div>
                <div><span class="k">
                    <asp:Literal ID="litLblArtista" runat="server" /></span><span class="v"><asp:Literal ID="litArtista" runat="server" /></span></div>
                <div><span class="k">
                    <asp:Literal ID="litLblAnio" runat="server" /></span><span class="v"><asp:Literal ID="litAnio" runat="server" /></span></div>
                <div><span class="k">
                    <asp:Literal ID="litLblTecnica" runat="server" /></span><span class="v"><asp:Literal ID="litTecnica" runat="server" /></span></div>
                <div><span class="k">
                    <asp:Literal ID="litLblPrecioBase" runat="server" /></span><span class="v"><asp:Literal ID="litPrecioBase" runat="server" /></span></div>
                <div><span class="k">
                    <asp:Literal ID="litLblOfertas" runat="server" /></span><span class="v"><asp:Literal ID="litOfertasCount" runat="server" /></span></div>
                <div><span class="k">
                    <asp:Literal ID="litLblCierra" runat="server" /></span><span class="v"><asp:Literal ID="litCierraFecha" runat="server" /></span></div>
                <div><span class="k">
                    <asp:Literal ID="litLblEstado" runat="server" /></span><span class="v"><span class="badge"><asp:Literal ID="litEstado" runat="server" /></span></span></div>
            </div>
        </div>

        <div class="card">
            <h2>
                <asp:Literal ID="litBidSection" runat="server" /></h2>

            <div class="price-row">
                <div>
                    <div class="muted">
                        <asp:Literal ID="litLblPrecioActual" runat="server" /></div>
                    <div class="price">
                        <asp:Literal ID="litPrecioActual" runat="server" /></div>
                </div>
                <div>
                    <div class="muted">
                        <asp:Literal ID="litLblTiempoRestante" runat="server" /></div>
                    <div class="price countdown" id="countdown">--:--:--</div>
                </div>
            </div>

            <div class="muted" style="margin-bottom: 10px;">
                <asp:Literal ID="litHintIncremento" runat="server" />
            </div>

            <asp:Panel ID="pnlPuja" runat="server">
                <label class="muted" for="txtMonto">
                    <asp:Literal ID="litLblTuOferta" runat="server" /></label>
                <asp:TextBox ID="txtMonto" runat="server" CssClass="input" placeholder="0.00" />
                <asp:RequiredFieldValidator ID="reqMonto" runat="server" ControlToValidate="txtMonto" Display="Dynamic" CssClass="val-msg" ErrorMessage="*" />
                <asp:RegularExpressionValidator ID="valDec" runat="server" ControlToValidate="txtMonto" Display="Dynamic" CssClass="val-msg"
                    ValidationExpression="^\d+([\,\.]\d{1,2})?$" ErrorMessage="Formato inválido" />
                <asp:CustomValidator ID="valMin" runat="server" ControlToValidate="txtMonto" Display="Dynamic" CssClass="val-msg" OnServerValidate="valMin_ServerValidate" />

                <div class="actions" style="margin-top: 12px;">
                    <asp:Button ID="btnPujar" runat="server" CssClass="btn btn-primary" OnClick="btnPujar_Click" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlSinFee" runat="server" Visible="false">
                <div class="muted" style="margin-top: 8px; margin-bottom: 12px;">
                    <asp:Literal ID="litSinFeeMsg" runat="server" />
                </div>
                <div class="actions">
                    <asp:Button ID="btnPagarFee" runat="server" CssClass="btn btn-primary" OnClick="btnPagarFee_Click" />
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCerrada" runat="server" Visible="false">
                <div class="muted" style="margin-top: 8px;">
                    <asp:Literal ID="litCerradaMsg" runat="server" />
                </div>
            </asp:Panel>

            <asp:Label ID="lblMensaje" runat="server" EnableViewState="false" />
        </div>
    </div>

    <script type="text/javascript">
        (function () {
            var endsAt = document.getElementById('<%= hfEndsAtIso.ClientID %>').value;
            if (!endsAt) return;
            var el = document.getElementById('countdown');
            function pad(n) { return (n < 10 ? '0' : '') + n; }
            function tick() {
                var diff = new Date(endsAt) - new Date();
                if (diff <= 0) { el.textContent = "00:00:00"; return; }
                var h = Math.floor(diff / 3600000);
                var m = Math.floor((diff % 3600000) / 60000);
                var s = Math.floor((diff % 60000) / 1000);
                el.textContent = pad(h) + ":" + pad(m) + ":" + pad(s);
                requestAnimationFrame(function () { setTimeout(tick, 250); });
            }
            tick();
        })();
    </script>
</asp:Content>
