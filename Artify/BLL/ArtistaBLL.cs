using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ArtistaBLL
    {
        private readonly ArtistaDAL _artistaDal = new ArtistaDAL();
        public Response<Artista> Agregar(Artista artista)
        {
            try
            {
                var result = _artistaDal.Agregar(artista);
                return Response<Artista>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<Artista>.Error($"Error al agregar el artista: {ex.Message}");
            }
        }

        public Response<IEnumerable<Artista>> GetAllForDVH()
        {
            try
            {
                var artistas = _artistaDal.GetAllForDVH();
                return Response<IEnumerable<Artista>>.Success(artistas);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<Artista>>.Error($"Error al obtener artistas para DVH: {ex.Message}");
            }
        }
    }
}
