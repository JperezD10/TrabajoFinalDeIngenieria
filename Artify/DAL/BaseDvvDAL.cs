using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public abstract class BaseDvvDAL
    {
        public Acceso Acceso = new Acceso();
        public abstract string TableName { get;}

        protected void ActualizarDVV()
        {
            Acceso.Escribir("sp_ActualizarDVV", new SqlParameter[]
                {
                    new SqlParameter("@Tabla", TableName)
                }, CommandType.StoredProcedure);
        }
    }
}
