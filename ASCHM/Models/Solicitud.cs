namespace ASCHM.Models
{
    public class Solicitud
    {
        public string Id { get; set; }
        public string Fecha { get; set; }
        public string Solicitante { get; set; }
        public string Cod { get; set; }
        public string Sector { get; set; }
        public string Situacion { get; set; }
        public string Observacion { get; set; }
        public string UrlSolicitud { get; set; }
        public string UrlDatoAdj { get; set; }
        public string UsuarioAutorizador { get; set; }
        public string Nivel { get; set; }
    }
}
