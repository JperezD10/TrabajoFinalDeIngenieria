using BE;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class DvvDAL
    {
        Acceso acceso = Acceso.GetInstance;

        public List<DVV> ObtenerVerticalesCorruptos()
        {
            var dvvs = new List<DVV>();
            var table = acceso.Leer("sp_VerificarDVV", null, CommandType.StoredProcedure);
            foreach (System.Data.DataRow row in table.Rows)
            {
                dvvs.Add(new DVV
                {
                    Tabla = row["Tabla"].ToString(),
                    SumaDVH = Convert.ToInt32(row["SumaDVH"])
                });
            }
            return dvvs;
        }
    }
}
