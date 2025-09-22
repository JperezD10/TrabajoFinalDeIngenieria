using BE;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ObraBLL
    {
        private readonly DAL.ObraDAL _obraDal = new DAL.ObraDAL();
        public IEnumerable<Obra> GetAllForDVH()
        {
            return _obraDal.GetAllForDVH();
        }
        public Response<Obra> CargarObra(Obra obra)
        {
            try
            {
                return Response<Obra>.Success(_obraDal.Agregar(obra));
            }
            catch (Exception ex)
            {
                return Response<Obra>.Error("Error al agregar obra: " + ex.Message);
            }
        }
    }
}
