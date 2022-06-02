using RecursivaChallenge.Models.ResponseObject;

namespace RecursivaChallenge.Repository.Contract
{
    public interface ISociosRepository
    {
        public Task<List<ClubInfoResponse>> SocioXClubYInfoEdades();
        public Task<List<string>> NombresMasComunesXClub(string club);
        public Task<List<SocioInfoParcialResponse>> SocioXEstadoCivilYNivelDeEstudio(string estadoCivil, string nivelDeEstudios);
        public Task<double?> PromedioEdadClub(string club);
        public Task<int> CantidadTotalSocio();
        public Task<bool> InsertBulkFileCsv(string filePath);
        public Task<bool> TruncateTabla(string nombreTabla);

    }
}