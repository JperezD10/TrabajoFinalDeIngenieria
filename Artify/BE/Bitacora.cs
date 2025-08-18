﻿using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace BE
{
    public class Bitacora : EntidadBase
    {
        public Bitacora()
        {
            
        }
        public Bitacora(DateTime fecha, string accion, int criticidad, string modulo, int idUsuario)
        {
            Fecha = fecha;
            Accion = accion;
            Criticidad = criticidad;
            Modulo = modulo;
            IdUsuario = idUsuario;
        }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public int Criticidad { get; set; }
        public string Modulo { get; set; }
        public int IdUsuario { get; set; }

        public override string FormatoDVH
        {
            get
            {
                var f = DvhTime.NormalizeForSqlDatetime(Fecha)
                               .ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                return string.Concat(Id, f, IdUsuario, Accion ?? "", Criticidad, Modulo ?? "");
            }
        }
    }

    public enum Criticidad
    {
        Leve = 1,
        Moderada = 2,
        Alta = 3,
        Critica = 4
    }
}
