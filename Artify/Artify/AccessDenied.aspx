<%@ Page Title="Acceso denegado" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="Artify.AccessDenied" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .wrap {
            max-width: 720px;
            margin: 50px auto;
            padding: 0 16px
        }

        .card {
            background: var(--panel,#1f2937);
            border: 1px solid var(--border,#374151);
            border-radius: 20px;
            box-shadow: 0 8px 30px rgba(0,0,0,.55);
            overflow: hidden;
            text-align: center
        }

        .hero {
            padding: 40px 24px;
            background: linear-gradient(135deg,#f87171 0%,#b91c1c 100%);
            color: #fff
        }

            .hero h1 {
                margin: 0;
                font-size: 2rem;
                font-weight: 900
            }

            .hero p {
                margin: 12px 0 0;
                opacity: .95;
                font-size: 1.1rem
            }

        .body {
            padding: 28px
        }

        .warning-icon {
            font-size: 4rem;
            display: block;
            margin-bottom: 18px
        }

        .detail {
            background: rgba(255,255,255,.05);
            border: 1px dashed var(--border,#334155);
            border-radius: 14px;
            padding: 20px;
            margin-top: 20px;
            text-align: left
        }

            .detail strong {
                display: block;
                margin-bottom: 8px
            }

        .badge {
            display: inline-block;
            margin-right: 10px;
            font-size: .9rem;
            padding: 6px 12px;
            border-radius: 999px;
            background: #111827;
            color: #fca5a5;
            border: 1px solid #b91c1c
        }

        .muted {
            color: var(--muted,#9ca3af);
            margin-top: 16px;
            font-size: .95rem
        }
    </style>
</asp:Content>

<asp:Content ID="MainBody" ContentPlaceHolderID="MainContent" runat="server">
    <div class="wrap">
        <div class="card">
            <div class="hero">
                <span class="warning-icon">⚠️</span>
                <h1>
                    <asp:Literal ID="litTitle" runat="server" /></h1>
                <p>
                    <asp:Literal ID="litSubtitle" runat="server" /></p>
            </div>
            <div class="body">
                <asp:Literal ID="litLead" runat="server" />

                <div class="detail">
                    <strong>
                        <asp:Literal ID="litReasonTitle" runat="server" /></strong>
                    <span class="badge">
                        <asp:Literal ID="litRequired" runat="server" /></span>
                    <span class="badge">
                        <asp:Literal ID="litCurrent" runat="server" /></span>
                </div>

                <p class="muted">
                    <asp:Literal ID="litExtra" runat="server" />
                </p>
            </div>
        </div>
    </div>
</asp:Content>
