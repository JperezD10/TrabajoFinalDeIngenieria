using BE;
using DAL;

namespace BLL
{
    public class ParticipacionSubastaBLL
    {
        private readonly ParticipacionSubastaDAL _dal = new ParticipacionSubastaDAL();

        public Response<bool> PuedeOfertar(int idCliente, int idSubasta)
        {
            var puede = _dal.PuedeOfertar(idCliente, idSubasta);

            if (!puede)
                return Response<bool>.Error("err.subasta.noFee");

            return Response<bool>.Success(true);
        }

        public Response<ParticipacionSubasta> GuardarParticipacion(ParticipacionSubasta participacion)
        {
            var ok = _dal.GuardarParticipacion(participacion);

            if (!ok)
                return Response<ParticipacionSubasta>.Error( "err.participacion.guardar");

            return Response<ParticipacionSubasta>.Success(participacion,"ok.participacion.guardada");
        }
    }
}
