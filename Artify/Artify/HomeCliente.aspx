<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="HomeCliente.aspx.cs" Inherits="Artify.HomeCliente" %>

<asp:Content ID="HeadCss" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    :root{
      --bg:#0b1220; --panel:#111827; --border:#334155; --ink:#e5e7eb; --muted:#9ca3af;
      --accent:#7c3aed; --accent2:#2563eb; --shadow:0 12px 36px rgba(0,0,0,.55)
    }
    .topbar{display:flex;align-items:center;justify-content:space-between;margin-bottom:18px}
    .title{margin:0;font-size:1.8rem;font-weight:900;color:#f9fafb}
    .hint{margin:0;color:var(--muted)}

    .gallery{display:grid;grid-template-columns:repeat(auto-fill,minmax(240px,1fr));gap:16px;align-items:stretch}
    .card{background:var(--panel);border:1px solid var(--border);border-radius:16px;overflow:hidden;box-shadow:var(--shadow);display:flex;flex-direction:column}
    .thumb{aspect-ratio:4/3;background:#0f172a;position:relative;overflow:hidden}
    .thumb img{width:100%;height:100%;object-fit:cover;display:block;transition:transform .4s ease}
    .card:hover .thumb img{transform:scale(1.06)}

    .body{padding:12px 14px;display:flex;flex-direction:column;gap:6px;flex:1}
    .title2{
      margin:0;color:var(--ink);font-size:1.05rem;font-weight:800;
      display:-webkit-box;-webkit-line-clamp:2;-webkit-box-orient:vertical;overflow:hidden;min-height:2.6em;
    }
    .meta{
      margin:0;color:var(--muted);font-size:.9rem;
      display:-webkit-box;-webkit-line-clamp:1;-webkit-box-orient:vertical;overflow:hidden;min-height:1.2em;
    }

    .prices{display:flex;gap:8px;align-items:baseline;margin-top:4px}
    .price-base{color:#93c5fd;text-decoration:line-through;opacity:.8;font-size:.9rem}
    .price-actual{color:#fde68a;font-weight:900}

    .actions{display:flex;gap:8px;margin-top:auto}
    .btn{flex:1;text-align:center;padding:10px 12px;border-radius:10px;border:1px solid var(--border);font-weight:800;color:#e5e7eb;background:#0b1220;text-decoration:none;cursor:pointer}
    .btn:hover{filter:brightness(1.08)}
  </style>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
  <div class="topbar">
    <h1 class="title">Obras</h1>
    <p class="hint">Vista previa de diseño (datos de prueba)</p>
  </div>

  <asp:Repeater ID="rptObras" runat="server">
    <HeaderTemplate><div class="gallery"></HeaderTemplate>
    <ItemTemplate>
      <div class="card">
        <div class="thumb">
          <img alt='<%# Eval("Titulo") %>'
               loading="lazy"
               src='<%# Eval("UrlImagen") %>'
               onerror="this.onerror=null;this.src='https://picsum.photos/seed/fallbackimg/800/600';" />
        </div>
        <div class="body">
          <h3 class="title2"><%# Eval("Titulo") %></h3>
          <p class="meta">
            <%# Eval("Artista.Nombre") %> • <%# Eval("Anio") %> • <%# Eval("Tecnica") %>
          </p>
          <div class="prices">
            <span class="price-base">
              <%# "US$ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:N0}", Eval("PrecioBase")) %>
            </span>
            <span class="price-actual">
              <%# "US$ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:N0}", Eval("PrecioActual")) %>
            </span>
          </div>
          <div class="actions">
            <a class="btn" href="#">Ver detalle</a>
            <a class="btn" href="#">Ofertar</a>
          </div>
        </div>
      </div>
    </ItemTemplate>
    <FooterTemplate></div></FooterTemplate>
  </asp:Repeater>
</asp:Content>
