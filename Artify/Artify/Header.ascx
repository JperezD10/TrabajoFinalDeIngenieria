<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Artify.WebUserControl1" %>
<style>
  .header-bar {
    position: sticky; top: 0; z-index: 1000;
    display: flex; align-items: center; justify-content: space-between;
    padding: 14px 24px;
    margin: 16px 16px 24px 16px;
    background: linear-gradient(90deg,#4f46e5,#9333ea);
    color: #fff;
    font-family: 'Segoe UI', Arial, sans-serif;
    font-weight: 600;
    border-radius: 16px;
    box-shadow: 0 4px 18px rgba(0,0,0,.4);
  }
  .header-bar .hello { font-size: 1.1rem; }
  .logout-btn {
    display: inline-flex; align-items: center; gap: 6px;
    padding: 8px 16px; border-radius: 999px; border: none; cursor: pointer;
    font-weight: 700; font-size: .95rem; background: #ef4444; color: #fff;
    box-shadow: 0 2px 6px rgba(0,0,0,.35); transition: background .2s, transform .05s;
  }
  .logout-btn:hover { background: #dc2626; transform: translateY(-1px); }
  .logout-btn:active { transform: translateY(0); }
</style>

<div class="header-bar">
  <div class="hello">
    <asp:Literal ID="litGreeting" runat="server" />
  </div>
  <asp:Button ID="btnLogout" runat="server" CssClass="logout-btn" OnClick="btnLogout_Click" CausesValidation="false" />
</div>