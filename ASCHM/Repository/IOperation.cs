using ASCHM.Models;

namespace ASCHM.Repository
{
    public interface IOperation
    {
        List<Solicitud> GetSolicitudesPendientes { get; }
        List<NivelAuth> GetNivelAuth { get; }
        List<Motivo> GetMotivo { get; }
        List<Historial> GetHistorial { get; }
        void Autorizar(string usuario, int id, int nivel);
        void Cancelar(string usuario, int id, int nivel, int motivo, string observacion);

        List<Autorizador> GetAutorizador { get; }
    }
}
