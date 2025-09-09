using BE;
using BE.Observer;
using BLL;
using SEGURIDAD;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Artify
{
    public partial class HomeWebMaster : BasePage
    {
        private Usuario _usuario;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _usuario = SecurityManager.CheckAccess(this);
            RegisterLocalizablesById(this, "home");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_usuario == null) return;
            if (IsPostBack) return;

            var bitacoraBLL = new BitacoraBLL();
            var ahora = DateTime.Now;

            // --- DVH ---
            var integridadH = new IntegridadHorizontalBLL();
            var respH = integridadH.VerificarTodo();

            bool hayCorrupcionH = false;
            if (!respH.Exito)
            {
                // Error al intentar leer/verificar DVH
                bitacoraBLL.RegistrarAccion(new BE.Bitacora
                {
                    Fecha = ahora,
                    Accion = "DVH: error general de verificación -> " + (respH.Mensaje ?? "sin detalle"),
                    Criticidad = (int)Criticidad.Critica,
                    Modulo = "Integridad",
                    IdUsuario = _usuario.Id
                });
                hayCorrupcionH = true; // forzamos corte
            }
            else
            {
                foreach (var r in respH.Data)
                {
                    if (!string.IsNullOrEmpty(r.Error))
                    {
                        hayCorrupcionH = true;
                        bitacoraBLL.RegistrarAccion(new BE.Bitacora
                        {
                            Fecha = ahora,
                            Accion = "DVH: error al leer " + r.Tabla + " -> " + r.Error,
                            Criticidad = (int)Criticidad.Critica,
                            Modulo = "Integridad",
                            IdUsuario = _usuario.Id
                        });
                    }

                    if (r.IdsCorruptos != null && r.IdsCorruptos.Count > 0)
                    {
                        hayCorrupcionH = true;
                        foreach (var bloque in ChunkIds(r.IdsCorruptos, 10))
                        {
                            var lista = string.Join(",", bloque);
                            bitacoraBLL.RegistrarAccion(new BE.Bitacora
                            {
                                Fecha = ahora,
                                Accion = "DVH: " + r.Tabla + " registros corruptos -> [" + lista + "]",
                                Criticidad = (int)Criticidad.Critica,
                                Modulo = "Integridad",
                                IdUsuario = _usuario.Id
                            });
                        }
                    }
                }
            }

            // --- DVV ---
            var integridadV = new IntegridadVerticalBLL();
            var dvvCorruptas = integridadV.ObtenerVerticalesCorruptos(); // List<DVV> { Tabla, SumaDVH }
            bool hayCorrupcionV = dvvCorruptas != null && dvvCorruptas.Count > 0;

            if (hayCorrupcionV)
            {
                foreach (var t in dvvCorruptas)
                {
                    bitacoraBLL.RegistrarAccion(new BE.Bitacora
                    {
                        Fecha = ahora,
                        Accion = $"DVV: {t.Tabla} suma actual DVH={t.SumaDVH} no coincide con DVV registrado",
                        Criticidad = (int)Criticidad.Critica,
                        Modulo = "Integridad",
                        IdUsuario = _usuario.Id
                    });
                }
            }

            if (hayCorrupcionH || hayCorrupcionV)
            {
                Session["IntegridadResultados"] = respH.Data;              
                Session["IntegridadDVVResultados"] = dvvCorruptas;  

                Response.Redirect("~/BaseCorrupta.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

        }

        private static IEnumerable<List<int>> ChunkIds(List<int> ids, int size)
        {
            var actual = new List<int>(size);
            foreach (var id in ids)
            {
                actual.Add(id);
                if (actual.Count == size)
                {
                    yield return actual;
                    actual = new List<int>(size);
                }
            }
            if (actual.Count > 0) yield return actual;
        }
    }
}