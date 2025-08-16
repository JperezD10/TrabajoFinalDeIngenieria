using System.Data.SqlClient;
using System.Data;
using System;
using System.Configuration;

namespace DAL
{
    public class Acceso
    {
        public SqlConnection con;
        SqlTransaction _transaction;
        public Acceso()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ArtifyConnection"].ConnectionString);
        }

        private static Acceso _instance = null;

        public static Acceso GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Acceso();
                }

                return _instance;
            }
        }
        public SqlConnection AbrirConexion()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        public void CerrarConexion()
        {
            try
            {
                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable Leer(string st, SqlParameter[] parameters, CommandType commandType = CommandType.Text)
        {
            CerrarConexion();
            AbrirConexion();
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = new SqlCommand();
            sqlDataAdapter.SelectCommand.CommandType = commandType;
            sqlDataAdapter.SelectCommand.CommandText = st;
            if (parameters != null)
            {
                sqlDataAdapter.SelectCommand.Parameters.AddRange(parameters);
            }
            sqlDataAdapter.SelectCommand.Connection = con;
            sqlDataAdapter.Fill(dataTable);
            CerrarConexion();
            return dataTable;
        }

        public int Escribir(string st, SqlParameter[] parameters, CommandType commandType = CommandType.Text)
        {
            CerrarConexion();
            AbrirConexion();
            _transaction = con.BeginTransaction();
            SqlCommand sqlCommand = new SqlCommand()
            {
                CommandType = commandType,
                CommandText = st,
                Connection = con,
            };
            sqlCommand.Parameters.AddRange(parameters);
            try
            {
                sqlCommand.Transaction = _transaction;
                sqlCommand.ExecuteNonQuery();
                _transaction.Commit();
            }
            catch (Exception)
            {
                _transaction.Rollback();
                return 0;
            }
            CerrarConexion();
            return 1;
        }
    }
}
