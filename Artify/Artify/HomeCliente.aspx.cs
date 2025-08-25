using BE;
using BE.Observer;
using BLL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Artify
{
    public partial class HomeCliente : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "homecli");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var integridad = new IntegridadHorizontalBLL();
            var resp = integridad.VerificarTodo();
            if (!resp.Exito) return;

            // ¿hay corrupción?
            bool hayCorrupcion = false;
            foreach (var r in resp.Data)
            {
                if (!string.IsNullOrEmpty(r.Error) || (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0))
                { hayCorrupcion = true; break; }
            }
            if (hayCorrupcion)
            {
                Response.Redirect("~/Mantenimiento.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            var picasso = new Artista { Id = 1, Nombre = "Pablo Picasso", Nacionalidad = "España" };
            var frida = new Artista { Id = 2, Nombre = "Frida Kahlo", Nacionalidad = "México" };
            var banksy = new Artista { Id = 3, Nombre = "Banksy", Nacionalidad = "Reino Unido" };
            var dali = new Artista { Id = 4, Nombre = "Salvador Dalí", Nacionalidad = "España" };
            var monet = new Artista { Id = 5, Nombre = "Claude Monet", Nacionalidad = "Francia" };
            var vanGogh = new Artista { Id = 6, Nombre = "Vincent van Gogh", Nacionalidad = "Países Bajos" };
            var klimt = new Artista { Id = 7, Nombre = "Gustav Klimt", Nacionalidad = "Austria" };
            var matisse = new Artista { Id = 8, Nombre = "Henri Matisse", Nacionalidad = "Francia" };

            var obras = new List<Obra>
            {
                new Obra {
                    Id = 101, Titulo = "Les Demoiselles d'Avignon",
                    Anio = 1907, Tecnica = "Óleo sobre lienzo",
                    ArtistaId = picasso.Id, Artista = picasso,
                    PrecioBase = 150_000_000m, PrecioActual = 158_200_000m,
                    UrlImagen = "https://picsum.photos/seed/obra101/800/600"
                },
                new Obra {
                    Id = 102, Titulo = "Autorretrato con collar de espinas",
                    Anio = 1940, Tecnica = "Óleo sobre lienzo",
                    ArtistaId = frida.Id, Artista = frida,
                    PrecioBase = 12_000_000m, PrecioActual = 12_450_000m,
                    UrlImagen = "https://picsum.photos/seed/obra102/800/600"
                },
                new Obra {
                    Id = 103, Titulo = "Girl with Balloon",
                    Anio = 2002, Tecnica = "Aerosol y acrílico",
                    ArtistaId = banksy.Id, Artista = banksy,
                    PrecioBase = 2_000_000m, PrecioActual = 2_880_000m,
                    UrlImagen = "https://picsum.photos/seed/obra103/800/600"
                },
                new Obra {
                    Id = 104, Titulo = "La persistencia de la memoria",
                    Anio = 1931, Tecnica = "Óleo sobre lienzo",
                    ArtistaId = dali.Id, Artista = dali,
                    PrecioBase = 40_000_000m, PrecioActual = 42_300_000m,
                    UrlImagen = "https://picsum.photos/seed/obra104/800/600"
                },
                new Obra {
                    Id = 105, Titulo = "Impresión, sol naciente",
                    Anio = 1872, Tecnica = "Óleo sobre lienzo",
                    ArtistaId = monet.Id, Artista = monet,
                    PrecioBase = 25_000_000m, PrecioActual = 27_100_000m,
                    UrlImagen = "https://picsum.photos/seed/obra105/800/600"
                },
                new Obra {
                    Id = 106, Titulo = "La noche estrellada",
                    Anio = 1889, Tecnica = "Óleo sobre lienzo",
                    ArtistaId = vanGogh.Id, Artista = vanGogh,
                    PrecioBase = 100_000_000m, PrecioActual = 112_000_000m,
                    UrlImagen = "https://picsum.photos/seed/obra106/800/600"
                },
                new Obra {
                    Id = 107, Titulo = "Los girasoles",
                    Anio = 1888, Tecnica = "Óleo sobre lienzo",
                    ArtistaId = vanGogh.Id, Artista = vanGogh,
                    PrecioBase = 90_000_000m, PrecioActual = 96_500_000m,
                    UrlImagen = "https://picsum.photos/seed/obra107/800/600"
                },
                new Obra {
                    Id = 108, Titulo = "El beso",
                    Anio = 1908, Tecnica = "Óleo y pan de oro",
                    ArtistaId = klimt.Id, Artista = klimt,
                    PrecioBase = 70_000_000m, PrecioActual = 74_300_000m,
                    UrlImagen = "https://picsum.photos/seed/obra108/800/600"
                },
                new Obra {
                    Id = 109, Titulo = "La danza",
                    Anio = 1910, Tecnica = "Óleo sobre lienzo",
                    ArtistaId = matisse.Id, Artista = matisse,
                    PrecioBase = 45_000_000m, PrecioActual = 49_250_000m,
                    UrlImagen = "https://picsum.photos/seed/obra109/800/600"
                }
            };

            // DVH de prueba
            foreach (var o in obras)
            {
                o.DVH = o.CalcularDVH();
                o.Artista.DVH = o.Artista.CalcularDVH();
            }

            rptObras.DataSource = obras;
            rptObras.DataBind();
        }
    }
}