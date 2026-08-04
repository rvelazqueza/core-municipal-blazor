using System;
using System.Collections.Generic;

namespace BlazorApp.Models;

public class PatenteSolicitudWizardModel
{
    public string ProcessKey { get; set; } = "patentes-nueva-solicitud";

    public string NumeroSolicitud { get; set; } = string.Empty;
    public string NumeroExpediente { get; set; } = string.Empty;
    public string TipoSolicitud { get; set; } = string.Empty;
    public string TipoLicencia { get; set; } = string.Empty;
    public string CanalIngreso { get; set; } = string.Empty;
    public string EstadoInicial { get; set; } = "Borrador";
    public string ModalidadDeclaracionJurada { get; set; } = string.Empty;
    public bool TieneDeclaracionJurada { get; set; } = false;
    public bool RequiereLocalFisico { get; set; } = true;

    public int? ContribuyenteId { get; set; }
    public bool PendienteRegistroContribuyente { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string ContribuyenteNombre { get; set; } = string.Empty;
    public string TipoPersona { get; set; } = "Fisica";
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string DireccionFiscal { get; set; } = string.Empty;
    public string MedioNotificacion { get; set; } = "Correo electrónico";
    public string EstadoContribuyente { get; set; } = "Activo";
    public string CalidadDatosRuc { get; set; } = "Validado";

    public string TipoUbicacion { get; set; } = "Local físico";
    public string Distrito { get; set; } = string.Empty;
    public string DireccionLocal { get; set; } = string.Empty;
    public string FincaOIdPredial { get; set; } = string.Empty;
    public string IdPredial { get; set; } = string.Empty;
    public string NumeroFinca { get; set; } = string.Empty;
    public string Dueno { get; set; } = string.Empty;
    public string CondicionOcupacion { get; set; } = string.Empty;
    public string AreaLocal { get; set; } = string.Empty;
    public string NombreComercialLocal { get; set; } = string.Empty;
    public string CuentaServiciosMunicipales { get; set; } = string.Empty;
    public string EstadoGis { get; set; } = "Pendiente";

    public string ActividadEconomica { get; set; } = string.Empty;
    public string CodigoCaecr { get; set; } = string.Empty;
    public string CodigoCiiuVisual { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public string CategoriaActividad { get; set; } = string.Empty;
    public string ActividadPrincipal { get; set; } = string.Empty;
    public string ActividadesSecundarias { get; set; } = string.Empty;
    public string RiesgoActividad { get; set; } = "Bajo";
    public string HorarioAtencion { get; set; } = string.Empty;
    public string Empleados { get; set; } = string.Empty;
    public bool RequiereLicores { get; set; }
    public bool RequierePermisoSanitario { get; set; }
    public bool RequiereInspeccion { get; set; }
    public bool ActividadTemporal { get; set; }

    public string EstadoUsoSuelo { get; set; } = "Pendiente";
    public string CertificadoUsoSuelo { get; set; } = string.Empty;
    public string NumeroCertificadoUsoSuelo { get; set; } = string.Empty;
    public string Zonificacion { get; set; } = string.Empty;
    public string ActividadesAutorizadas { get; set; } = string.Empty;
    public string CompatibilidadUsoSuelo { get; set; } = string.Empty;
    public DateTime? FechaValidacionUsoSuelo { get; set; }
    public DateTime? FechaVencimientoUsoSuelo { get; set; }
    public string ResultadoUsoSuelo { get; set; } = string.Empty;

    public string EstadoRequisitos { get; set; } = "Cumple";
    public string EstadoIdentificacion { get; set; } = "Cumple";
    public string EstadoPersoneriaJuridica { get; set; } = "Cumple";
    public string EstadoUsoSueloChecklist { get; set; } = "Cumple";
    public string EstadoPermisoSanitario { get; set; } = "Pendiente";
    public string EstadoArrendamientoPropiedad { get; set; } = "Cumple";
    public string EstadoCcssChecklist { get; set; } = "Cumple";
    public string EstadoFodesafChecklist { get; set; } = "Cumple";
    public string EstadoInsChecklist { get; set; } = "Cumple";
    public string EstadoDeclaracionJuradaChecklist { get; set; } = "Cumple";
    public string EstadoComprobantePago { get; set; } = "Pendiente";
    public string EstadoCroquis { get; set; } = "Cumple";
    public int DeclaracionJuradaDiasHabiles { get; set; } = 60;
    public List<RequisitoPatenteDto> Requisitos { get; set; } = new();

    public string EstadoDeclaracionJurada { get; set; } = "Sí";
    public string EstadoHacienda { get; set; } = "Conforme";
    public string EstadoCcss { get; set; } = "Conforme";
    public string EstadoFodesaf { get; set; } = "Conforme";
    public string EstadoIns { get; set; } = "Conforme";
    public string FechaInicioActividad { get; set; } = string.Empty;
    public string RegimenTributario { get; set; } = string.Empty;
    public string PeriodoFiscal { get; set; } = string.Empty;
    public string ActividadesEnOtrosCantones { get; set; } = string.Empty;
    public string EstadoIntegracionGeneral { get; set; } = "Completo";

    public bool SolicitanteAlDia { get; set; } = true;
    public bool DuenoPropiedadAlDia { get; set; } = true;
    public string ArreglosPago { get; set; } = string.Empty;
    public string ResultadoMorosidad { get; set; } = "Conforme";
    public string EstadoMorosidad { get; set; } = "Conforme";
    public bool SolicitarInspeccion { get; set; }
    public string EstadoInspeccion { get; set; } = "Solicitada";
    public string InspectorAsignado { get; set; } = string.Empty;
    public string EstadoRevision { get; set; } = "Conforme";
    public string ResultadoRevision { get; set; } = "Conforme";

    public string TipoTasacion { get; set; } = "Analogía";
    public string EstadoTasacion { get; set; } = "Pendiente";
    public decimal MontoAnualMock { get; set; }
    public decimal MontoTrimestralMock { get; set; }
    public decimal TimbreBiodiversidad { get; set; }
    public decimal PublicidadExterior { get; set; }
    public decimal Multa { get; set; }
    public decimal Intereses { get; set; }
    public string FechaInicioCobro { get; set; } = string.Empty;
    public string CobroProporcionalVisual { get; set; } = string.Empty;
    public string EstadoCobro { get; set; } = "Pendiente";
    public string EstadoCuentaTributariaMock { get; set; } = string.Empty;
    public string ReferenciaCuentaPorCobrarMock { get; set; } = string.Empty;
    public string EstadoResolucion { get; set; } = "Pendiente validación";
    public string ObservacionesResolucion { get; set; } = string.Empty;
    public bool NotificacionSimulada { get; set; }
    public string FirmaDigitalReferencial { get; set; } = string.Empty;
    public string EstadoFinalExpediente { get; set; } = "Borrador";

    public string Observaciones { get; set; } = string.Empty;
}
