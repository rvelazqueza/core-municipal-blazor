using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IPatentesValidacionService
{
    ResultadoValidacionDto ValidarSolicitud(SolicitudPatenteDto solicitud, IEnumerable<string> estadosPermitidos, bool paraAprobacion);
    ResultadoValidacionDto ValidarMantenimiento(LicenciaComercialDto licencia, string motivoMovimiento, bool requiereMotivo);
    void ActualizarRequisitos(SolicitudPatenteDto solicitud, ISet<string> adjuntosSeleccionados);
}
