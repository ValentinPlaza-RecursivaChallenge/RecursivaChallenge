using RecursivaChallenge.Models.ResponseObject;

namespace RecursivaChallenge.Repository.Contract
{
    public interface ISociosRepository
    {
        public List<ClubInfoResponse> SocioXClubYInfoEdades();
        public List<string> NombresMasComunesXClub(string club);
        public List<SocioInfoParcialResponse> SocioXEstadoCivilYNivelDeEstudio(string estadoCivil, string nivelDeEstudios);
        public double PromedioEdadClub(string club);
        public int CantidadTotalSocio();
        public Task<bool> InsertBulkFileCsv(string filePath);
        public Task<bool> TruncateTabla(string nombreTabla);

    }
}