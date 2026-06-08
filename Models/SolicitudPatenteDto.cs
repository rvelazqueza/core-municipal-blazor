using System;
using System.Collections.Generic;

namespace BlazorApp.Models;

public class SolicitudPatenteDto
{
    public int Id { get; set; }
    public int? LicenciaId { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string NumeroExpediente { get; set; } = string.Empty;
    public string TipoSolicitud { get; set; } = string.Empty;
    public string TipoLicencia { get; set; } = string.Empty;
    public string CanalIngreso { get; set; } = string.Empty;
    public string EstadoInicial { get; set; } = "Borrador";
    public string ModalidadDeclaracionJurada { get; set; } = string.Empty;
    public bool TieneDeclaracionJurada { get; set; }

    public int ContribuyenteId { get; set; }
    public string ContribuyenteRuc { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string ContribuyenteNombre { get; set; } = string.Empty;
    public string TipoPersona { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string DireccionFiscal { get; set; } = string.Empty;
    public string MedioNotificacion { get; set; } = string.Empty;
    public string EstadoContribuyente { get; set; } = string.Empty;
    public string CalidadDatosRuc { get; set; } = string.Empty;

    public bool RequiereLocalFisico { get; set; }
    public string TipoUbicacion { get; set; } = string.Empty;
    public string IdPredial { get; set; } = string.Empty;
    public string NumeroFinca { get; set; } = string.Empty;
    public string FincaOIdPredial { get; set; } = string.Empty;
    public string DireccionLocal { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string Dueno { get; set; } = string.Empty;
    public string CondicionOcupacion { get; set; } = string.Empty;
    public string AreaLocal { get; set; } = string.Empty;
    public string NombreComercialLocal { get; set; } = string.Empty;
    public string CuentaServiciosMunicipales { get; set; } = string.Empty;
    public string EstadoGis { get; set; } = string.Empty;

    public string NombreComercial { get; set; } = string.Empty;
    public string ActividadEconomica { get; set; } = string.Empty;
    public string CodigoCaecr { get; set; } = string.Empty;
    public string TipoPatente { get; set; } = "Comercial";
    public string CodigoCiiuVisual { get; set; } = string.Empty;
    public string CategoriaActividad { get; set; } = string.Empty;
    public string ActividadPrincipal { get; set; } = string.Empty;
    public string ActividadesSecundarias { get; set; } = string.Empty;
    public string RiesgoActividad { get; set; } = string.Empty;
    public string HorarioAtencion { get; set; } = string.Empty;
    public string Empleados { get; set; } = string.Empty;
    public bool RequiereLicores { get; set; }
    public bool RequierePermisoSanitario { get; set; }
    public bool RequiereInspeccion { get; set; }
    public bool ActividadTemporal { get; set; }

    public string CertificadoUsoSuelo { get; set; } = string.Empty;
    public string NumeroCertificadoUsoSuelo { get; set; } = string.Empty;
    public string EstadoUsoSuelo { get; set; } = "Pendiente";
    public bool UsoSueloConforme { get; set; }
    public string Zonificacion { get; set; } = string.Empty;
    public string ActividadesAutorizadas { get; set; } = string.Empty;
    public string CompatibilidadUsoSuelo { get; set; } = string.Empty;
    public DateTime? FechaValidacionUsoSuelo { get; set; }
    public DateTime? FechaVencimientoUsoSuelo { get; set; }
    public string ResultadoUsoSuelo { get; set; } = string.Empty;

    public string EstadoRequisitos { get; set; } = string.Empty;
    public string EstadoIdentificacion { get; set; } = string.Empty;
    public string EstadoPersoneriaJuridica { get; set; } = string.Empty;
    public string EstadoUsoSueloChecklist { get; set; } = string.Empty;
    public string EstadoPermisoSanitario { get; set; } = string.Empty;
    public string EstadoArrendamientoPropiedad { get; set; } = string.Empty;
    public string EstadoCcssChecklist { get; set; } = string.Empty;
    public string EstadoFodesafChecklist { get; set; } = string.Empty;
    public string EstadoInsChecklist { get; set; } = string.Empty;
    public string EstadoDeclaracionJuradaChecklist { get; set; } = string.Empty;
    public string EstadoComprobantePago { get; set; } = string.Empty;
    public string EstadoCroquis { get; set; } = string.Empty;
    public int DeclaracionJuradaDiasHabiles { get; set; }

    public string EstadoDeclaracionJurada { get; set; } = string.Empty;
    public string EstadoHacienda { get; set; } = string.Empty;
    public string EstadoCcss { get; set; } = string.Empty;
    public string EstadoFodesaf { get; set; } = string.Empty;
    public string EstadoIns { get; set; } = string.Empty;
    public string FechaInicioActividad { get; set; } = string.Empty;
    public string RegimenTributario { get; set; } = string.Empty;
    public string PeriodoFiscal { get; set; } = string.Empty;
    public string ActividadesEnOtrosCantones { get; set; } = string.Empty;
    public string EstadoIntegracionGeneral { get; set; } = string.Empty;

    public bool SolicitanteAlDia { get; set; }
    public bool DuenoPropiedadAlDia { get; set; }
    public string ArreglosPago { get; set; } = string.Empty;
    public string ResultadoMorosidad { get; set; } = string.Empty;
    public string EstadoMorosidad { get; set; } = string.Empty;
    public bool SolicitarInspeccion { get; set; }
    public string EstadoInspeccion { get; set; } = string.Empty;
    public string InspectorAsignado { get; set; } = string.Empty;
    public string EstadoRevision { get; set; } = string.Empty;
    public string ResultadoRevision { get; set; } = string.Empty;

    public string TipoTasacion { get; set; } = string.Empty;
    public string EstadoTasacion { get; set; } = string.Empty;
    public decimal MontoAnualMock { get; set; }
    public decimal MontoTrimestralMock { get; set; }
    public decimal TimbreBiodiversidad { get; set; }
    public decimal PublicidadExterior { get; set; }
    public decimal Multa { get; set; }
    public decimal Intereses { get; set; }
    public string FechaInicioCobro { get; set; } = string.Empty;
    public string CobroProporcionalVisual { get; set; } = string.Empty;
    public string EstadoCobro { get; set; } = string.Empty;
    public string EstadoCuentaTributariaMock { get; set; } = string.Empty;
    public string ReferenciaCuentaPorCobrarMock { get; set; } = string.Empty;
    public string EstadoResolucion { get; set; } = string.Empty;
    public string ObservacionesResolucion { get; set; } = string.Empty;
    public bool NotificacionSimulada { get; set; }
    public string FirmaDigitalReferencial { get; set; } = string.Empty;
    public string EstadoFinalExpediente { get; set; } = "Borrador";

    public DateTime FechaSolicitud { get; set; } = DateTime.Today;
    public string Responsable { get; set; } = string.Empty;
    public string Estado { get; set; } = "Borrador";
    public string Observaciones { get; set; } = string.Empty;
    public List<string> Adjuntos { get; set; } = new();
    public List<RequisitoPatenteDto> Requisitos { get; set; } = new();
    public UsoSueloVinculadoDto UsoSuelo { get; set; } = new();
}
