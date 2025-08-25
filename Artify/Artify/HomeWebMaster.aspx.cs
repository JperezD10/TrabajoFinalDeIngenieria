﻿using BE;
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

            if (!IsPostBack)
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
                    var bitacoraBLL = new BitacoraBLL();
                    var ahora = DateTime.Now; // tu DAL normaliza y calcula DVH después

                    foreach (var r in resp.Data)
                    {
                        if (!string.IsNullOrEmpty(r.Error))
                        {
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

                    Session["IntegridadResultados"] = resp.Data;
                    Response.Redirect("~/BaseCorrupta.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
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