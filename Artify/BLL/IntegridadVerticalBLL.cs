using BE;
using DAL;
using System.Collections.Generic;

namespace BLL
{
    public class IntegridadVerticalBLL
    {
        DvvDAL Dal = new DvvDAL();
        public List<DVV> ObtenerVerticalesCorruptos()
        {
            return Dal.ObtenerVerticalesCorruptos();
        }
    }
}
