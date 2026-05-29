using BlazorApp.Models;

namespace BlazorApp.Services;

public class PatentesValidacionService : IPatentesValidacionService
{
    public ResultadoValidacionDto ValidarSolicitud(SolicitudPatenteDto solicitud, IEnumerable<string> estadosPermitidos, bool paraAprobacion)
    {
        var resultado = new ResultadoValidacionDto();

        if (solicitud.ContribuyenteId <= 0 || string.IsNullOrWhiteSpace(solicitud.ContribuyenteRuc))
            resultado.Errores.Add("Contribuyente RUC obligatorio.");
        if (string.IsNullOrWhiteSpace(solicitud.NombreComercial))
            resultado.Errores.Add("Nombre comercial obligatorio.");
        if (string.IsNullOrWhiteSpace(solicitud.ActividadEconomica) || string.IsNullOrWhiteSpace(solicitud.CodigoCaecr))
            resultado.Errores.Add("Actividad económica obligatoria.");
        if (string.IsNullOrWhiteSpace(solicitud.DireccionLocal))
            resultado.Errores.Add("Dirección del local obligatoria.");
        if (string.IsNullOrWhiteSpace(solicitud.FincaOIdPredial))
            resultado.Errores.Add("Finca o ID predial obligatorio.");
        if (string.IsNullOrWhiteSpace(solicitud.NumeroCertificadoUsoSuelo))
            resultado.Errores.Add("Certificado de uso de suelo obligatorio.");
        if (solicitud.Adjuntos.Count < 2)
            resultado.Errores.Add("Debe registrar los adjuntos mínimos obligatorios.");
        if (!estadosPermitidos.Contains(solicitud.Estado))
            resultado.Errores.Add("El estado del trámite no es permitido.");
        if (paraAprobacion && !solicitud.UsoSueloConforme)
            resultado.Errores.Add("No se puede aprobar la patente si el uso de suelo no es conforme.");

        return resultado;
    }

    public ResultadoValidacionDto ValidarMantenimiento(LicenciaComercialDto licencia, string motivoMovimiento, bool requiereMotivo)
    {
        var resultado = new ResultadoValidacionDto();

        if (string.IsNullOrWhiteSpace(licencia.NombreComercial))
            resultado.Errores.Add("Nombre comercial obligatorio para mantenimiento.");
        if (string.IsNullOrWhiteSpace(licencia.DireccionLocal))
            resultado.Errores.Add("Dirección del local obligatoria para mantenimiento.");
        if (string.IsNullOrWhiteSpace(licencia.Distrito))
            resultado.Errores.Add("Distrito obligatorio para mantenimiento.");
        if (requiereMotivo && string.IsNullOrWhiteSpace(motivoMovimiento))
            resultado.Errores.Add("Debe indicar motivo para suspensión o cancelación.");

        return resultado;
    }

    public void ActualizarRequisitos(SolicitudPatenteDto solicitud, ISet<string> adjuntosSeleccionados)
    {
        foreach (var item in solicitud.Requisitos)
        {
            item.Cumplido = item.Nombre switch
            {
                "Documento de identidad" => adjuntosSeleccionados.Contains("Documento de identidad"),
                "Certificado de uso de suelo" => !string.IsNullOrWhiteSpace(solicitud.NumeroCertificadoUsoSuelo) && solicitud.UsoSueloConforme,
                "Plano o croquis" => adjuntosSeleccionados.Contains("Plano o croquis"),
                "Declaración jurada" => adjuntosSeleccionados.Contains("Declaración jurada"),
                _ => false
            };

            item.Observacion = item.Cumplido ? "Validado" : "Pendiente";
        }
    }
}
