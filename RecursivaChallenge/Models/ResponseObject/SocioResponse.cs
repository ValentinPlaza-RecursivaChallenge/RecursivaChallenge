namespace RecursivaChallenge.Models.ResponseObject
{
    public class SocioResponse
    {
        public int? CantidadTotal { get; set; }
        public List<ClubInfoResponse>? ClubsInfo { get; set; }
        public List<string>? NombresComunes { get; set; }
        public List<SocioInfoParcialResponse>? SocioInfoParcial { get; set; }
        public double? PromedioEdadClub { get; set; }
    }
}