using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class PatentesMockService : IPatentesService
{
    private static readonly List<LicenciaComercialDto> Data = new()
    {
        new()
        {
            Id = 1,
            NumeroLicencia = "PAT-2024-001",
            ContribuyenteId = 2,
            ContribuyenteRuc = "3-101-998877",
            Identificacion = "3-101-998877",
            ContribuyenteNombre = "Tecnologias Urbanas CR S.A.",
            NombreComercial = "TecnoCentro",
            DireccionLocal = "Centro corporativo norte, local 2",
            FincaOIdPredial = "SJ-889900",
            ActividadEconomica = "Venta de equipos y accesorios tecnológicos",
            CodigoCaecr = "4741",
            TipoPatente = "Comercial",
            Distrito = "San Francisco",
            Estado = "Aprobado",
            FechaSolicitud = DateTime.Today.AddMonths(-7),
            FechaAprobacion = DateTime.Today.AddMonths(-6),
            FechaVencimiento = DateTime.Today.AddDays(40),
            Responsable = "Analista Municipal",
            Observaciones = "Licencia vigente con emisión al día.",
            PendienteUsoSuelo = false,
            EmisionSemestralPendiente = false,
            NotificacionPendiente = false,
            CobroSincronizado = true,
            GisValidado = true,
            UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2024-340", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddMonths(-6), Observaciones = "Conformidad vigente.", Fuente = "Uso de Suelo" },
            Solicitud = new SolicitudPatenteDto
            {
                Id = 1,
                LicenciaId = 1,
                NumeroSolicitud = "SOL-PAT-2024-001",
                ContribuyenteId = 2,
                ContribuyenteRuc = "3-101-998877",
                Identificacion = "3-101-998877",
                ContribuyenteNombre = "Tecnologias Urbanas CR S.A.",
                NombreComercial = "TecnoCentro",
                DireccionLocal = "Centro corporativo norte, local 2",
                FincaOIdPredial = "SJ-889900",
                ActividadEconomica = "Venta de equipos y accesorios tecnológicos",
                CodigoCaecr = "4741",
                TipoPatente = "Comercial",
                NumeroCertificadoUsoSuelo = "US-2024-340",
                EstadoUsoSuelo = "Conforme",
                UsoSueloConforme = true,
                FechaSolicitud = DateTime.Today.AddMonths(-7),
                Responsable = "Analista Municipal",
                Estado = "Aprobado",
                Observaciones = "Expediente completo.",
                Adjuntos = new List<string> { "Documento de identidad", "Certificado de uso de suelo", "Plano o croquis" },
                Distrito = "San Francisco",
                Requisitos = DefaultRequirements(true),
                UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2024-340", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddMonths(-6), Observaciones = "Conformidad vigente.", Fuente = "Uso de Suelo" }
            },
            Requisitos = DefaultRequirements(true),
            Movimientos = new List<MovimientoPatenteDto>
            {
                new() { FechaHora = DateTime.Today.AddMonths(-7).AddHours(9), TipoMovimiento = "Solicitud", Motivo = "Ingreso de solicitud comercial", Usuario = "Plataforma", EstadoResultante = "En revision", Origen = "Patentes" },
                new() { FechaHora = DateTime.Today.AddMonths(-6).AddHours(11), TipoMovimiento = "Aprobación", Motivo = "Uso de suelo conforme y requisitos completos", Usuario = "Analista Municipal", EstadoResultante = "Aprobado", Origen = "Patentes" }
            }
        },
        new()
        {
            Id = 2,
            NumeroLicencia = "PAT-2025-018",
            ContribuyenteId = 1,
            ContribuyenteRuc = "1-1234-5678",
            Identificacion = "1-1234-5678",
            ContribuyenteNombre = "Ana Solano Perez",
            NombreComercial = "Cafe Central",
            DireccionLocal = "Boulevard central, local esquinero",
            FincaOIdPredial = "SJ-002145",
            ActividadEconomica = "Restaurantes y sodas",
            CodigoCaecr = "5611",
            TipoPatente = "Comercial",
            Distrito = "Carmen",
            Estado = "En revision",
            FechaSolicitud = DateTime.Today.AddDays(-5),
            Responsable = "Mariela Vargas",
            Observaciones = "Pendiente criterio definitivo de uso de suelo.",
            PendienteUsoSuelo = true,
            EmisionSemestralPendiente = true,
            NotificacionPendiente = true,
            CobroSincronizado = false,
            GisValidado = true,
            UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2025-090", Estado = "Pendiente", EsConforme = false, FechaValidacion = DateTime.Today.AddDays(-3), Observaciones = "Revisión técnica en curso.", Fuente = "Uso de Suelo" },
            Solicitud = new SolicitudPatenteDto
            {
                Id = 2,
                LicenciaId = 2,
                NumeroSolicitud = "SOL-PAT-2025-018",
                ContribuyenteId = 1,
                ContribuyenteRuc = "1-1234-5678",
                Identificacion = "1-1234-5678",
                ContribuyenteNombre = "Ana Solano Perez",
                NombreComercial = "Cafe Central",
                DireccionLocal = "Boulevard central, local esquinero",
                FincaOIdPredial = "SJ-002145",
                ActividadEconomica = "Restaurantes y sodas",
                CodigoCaecr = "5611",
                TipoPatente = "Comercial",
                NumeroCertificadoUsoSuelo = "US-2025-090",
                EstadoUsoSuelo = "Pendiente",
                UsoSueloConforme = false,
                FechaSolicitud = DateTime.Today.AddDays(-5),
                Responsable = "Mariela Vargas",
                Estado = "En revision",
                Observaciones = "Pendiente resolución de uso de suelo.",
                Adjuntos = new List<string> { "Documento de identidad", "Certificado de uso de suelo" },
                Distrito = "Carmen",
                Requisitos = DefaultRequirements(false),
                UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2025-090", Estado = "Pendiente", EsConforme = false, FechaValidacion = DateTime.Today.AddDays(-3), Observaciones = "Revisión técnica en curso.", Fuente = "Uso de Suelo" }
            },
            Requisitos = DefaultRequirements(false),
            Movimientos = new List<MovimientoPatenteDto>
            {
                new() { FechaHora = DateTime.Today.AddDays(-5).AddHours(8), TipoMovimiento = "Solicitud", Motivo = "Ingreso de nueva licencia comercial", Usuario = "Plataforma", EstadoResultante = "Borrador", Origen = "Patentes" },
                new() { FechaHora = DateTime.Today.AddDays(-4).AddHours(14), TipoMovimiento = "Revisión", Motivo = "Se remite a criterio de uso de suelo", Usuario = "Mariela Vargas", EstadoResultante = "En revision", Origen = "Patentes" }
            }
        },
        new()
        {
            Id = 3,
            NumeroLicencia = "PAT-2023-077",
            ContribuyenteId = 3,
            ContribuyenteRuc = "1555666777",
            Identificacion = "1555666777",
            ContribuyenteNombre = "Luis Mora Campos",
            NombreComercial = "Ferreteria Hospital",
            DireccionLocal = "Calle 8, frente al parque del distrito",
            FincaOIdPredial = "SJ-550011",
            ActividadEconomica = "Ferreterías y materiales de construcción",
            CodigoCaecr = "4752",
            TipoPatente = "Industrial",
            Distrito = "Hospital",
            Estado = "Aprobado",
            FechaSolicitud = DateTime.Today.AddYears(-2),
            FechaAprobacion = DateTime.Today.AddYears(-2).AddDays(8),
            FechaVencimiento = DateTime.Today.AddDays(-30),
            Responsable = "Luis Arce",
            Observaciones = "Pendiente renovación y emisión semestral.",
            PendienteUsoSuelo = false,
            EmisionSemestralPendiente = true,
            NotificacionPendiente = true,
            CobroSincronizado = false,
            GisValidado = false,
            UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2023-510", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddYears(-2), Observaciones = "Uso de suelo emitido en 2023.", Fuente = "Uso de Suelo" },
            Solicitud = new SolicitudPatenteDto
            {
                Id = 3,
                LicenciaId = 3,
                NumeroSolicitud = "SOL-PAT-2023-077",
                ContribuyenteId = 3,
                ContribuyenteRuc = "1555666777",
                Identificacion = "1555666777",
                ContribuyenteNombre = "Luis Mora Campos",
                NombreComercial = "Ferreteria Hospital",
                DireccionLocal = "Calle 8, frente al parque del distrito",
                FincaOIdPredial = "SJ-550011",
                ActividadEconomica = "Ferreterías y materiales de construcción",
                CodigoCaecr = "4752",
                TipoPatente = "Industrial",
                NumeroCertificadoUsoSuelo = "US-2023-510",
                EstadoUsoSuelo = "Conforme",
                UsoSueloConforme = true,
                FechaSolicitud = DateTime.Today.AddYears(-2),
                Responsable = "Luis Arce",
                Estado = "Aprobado",
                Observaciones = "Expediente histórico.",
                Adjuntos = new List<string> { "Documento de identidad", "Certificado de uso de suelo", "Plano o croquis", "Declaración jurada" },
                Distrito = "Hospital",
                Requisitos = DefaultRequirements(true),
                UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2023-510", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddYears(-2), Observaciones = "Uso de suelo emitido en 2023.", Fuente = "Uso de Suelo" }
            },
            Requisitos = DefaultRequirements(true),
            Movimientos = new List<MovimientoPatenteDto>
            {
                new() { FechaHora = DateTime.Today.AddYears(-2), TipoMovimiento = "Solicitud", Motivo = "Solicitud tramitada", Usuario = "Plataforma", EstadoResultante = "En revision", Origen = "Patentes" },
                new() { FechaHora = DateTime.Today.AddYears(-2).AddDays(8), TipoMovimiento = "Aprobación", Motivo = "Patente industrial aprobada", Usuario = "Luis Arce", EstadoResultante = "Aprobado", Origen = "Patentes" },
                new() { FechaHora = DateTime.Today.AddDays(-15), TipoMovimiento = "Notificación", Motivo = "Aviso de vencimiento semestral", Usuario = "Sistema", EstadoResultante = "Aprobado", Origen = "Notificaciones" }
            }
        },
        new()
        {
            Id = 4,
            NumeroLicencia = "PAT-2025-009",
            ContribuyenteId = 1,
            ContribuyenteRuc = "1-1234-5678",
            Identificacion = "1-1234-5678",
            ContribuyenteNombre = "Ana Solano Perez",
            NombreComercial = "Bar Mirador",
            DireccionLocal = "Costado oeste del mercado municipal",
            FincaOIdPredial = "SJ-002145",
            ActividadEconomica = "Restaurantes y sodas",
            CodigoCaecr = "5611",
            TipoPatente = "Temporal",
            Distrito = "Carmen",
            Estado = "Rechazado",
            FechaSolicitud = DateTime.Today.AddDays(-22),
            Responsable = "Mauricio Porras",
            Observaciones = "Uso de suelo no conforme para la actividad solicitada.",
            PendienteUsoSuelo = false,
            EmisionSemestralPendiente = false,
            NotificacionPendiente = false,
            CobroSincronizado = false,
            GisValidado = true,
            UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "NC-2025-021", Estado = "No conforme", EsConforme = false, FechaValidacion = DateTime.Today.AddDays(-20), Observaciones = "Actividad no permitida en la ubicación indicada.", Fuente = "Uso de Suelo" },
            Solicitud = new SolicitudPatenteDto
            {
                Id = 4,
                LicenciaId = 4,
                NumeroSolicitud = "SOL-PAT-2025-009",
                ContribuyenteId = 1,
                ContribuyenteRuc = "1-1234-5678",
                Identificacion = "1-1234-5678",
                ContribuyenteNombre = "Ana Solano Perez",
                NombreComercial = "Bar Mirador",
                DireccionLocal = "Costado oeste del mercado municipal",
                FincaOIdPredial = "SJ-002145",
                ActividadEconomica = "Restaurantes y sodas",
                CodigoCaecr = "5611",
                TipoPatente = "Temporal",
                NumeroCertificadoUsoSuelo = "NC-2025-021",
                EstadoUsoSuelo = "No conforme",
                UsoSueloConforme = false,
                FechaSolicitud = DateTime.Today.AddDays(-22),
                Responsable = "Mauricio Porras",
                Estado = "Rechazado",
                Observaciones = "Solicitud rechazada por incompatibilidad urbanística.",
                Adjuntos = new List<string> { "Documento de identidad", "Certificado de uso de suelo", "Plano o croquis" },
                Distrito = "Carmen",
                Requisitos = DefaultRequirements(true),
                UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "NC-2025-021", Estado = "No conforme", EsConforme = false, FechaValidacion = DateTime.Today.AddDays(-20), Observaciones = "Actividad no permitida en la ubicación indicada.", Fuente = "Uso de Suelo" }
            },
            Requisitos = DefaultRequirements(true),
            Movimientos = new List<MovimientoPatenteDto>
            {
                new() { FechaHora = DateTime.Today.AddDays(-22), TipoMovimiento = "Solicitud", Motivo = "Ingreso de solicitud temporal", Usuario = "Plataforma", EstadoResultante = "En revision", Origen = "Patentes" },
                new() { FechaHora = DateTime.Today.AddDays(-20), TipoMovimiento = "Rechazo", Motivo = "Uso de suelo no conforme", Usuario = "Mauricio Porras", EstadoResultante = "Rechazado", Origen = "Patentes" }
            }
        },
        new()
        {
            Id = 5,
            NumeroLicencia = "PAT-2024-055",
            ContribuyenteId = 2,
            ContribuyenteRuc = "3-101-998877",
            Identificacion = "3-101-998877",
            ContribuyenteNombre = "Tecnologias Urbanas CR S.A.",
            NombreComercial = "Bodega Urbana",
            DireccionLocal = "Parque empresarial este, bodega 5",
            FincaOIdPredial = "SJ-778811",
            ActividadEconomica = "Ferreterías y materiales de construcción",
            CodigoCaecr = "4752",
            TipoPatente = "Industrial",
            Distrito = "Pavas",
            Estado = "Suspendido",
            FechaSolicitud = DateTime.Today.AddMonths(-10),
            FechaAprobacion = DateTime.Today.AddMonths(-9),
            FechaVencimiento = DateTime.Today.AddMonths(2),
            Responsable = "Analista Municipal",
            Observaciones = "Suspendida por remodelación del local.",
            PendienteUsoSuelo = false,
            EmisionSemestralPendiente = true,
            NotificacionPendiente = true,
            CobroSincronizado = true,
            GisValidado = true,
            UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2024-881", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddMonths(-9), Observaciones = "Conforme.", Fuente = "Uso de Suelo" },
            Solicitud = new SolicitudPatenteDto
            {
                Id = 5,
                LicenciaId = 5,
                NumeroSolicitud = "SOL-PAT-2024-055",
                ContribuyenteId = 2,
                ContribuyenteRuc = "3-101-998877",
                Identificacion = "3-101-998877",
                ContribuyenteNombre = "Tecnologias Urbanas CR S.A.",
                NombreComercial = "Bodega Urbana",
                DireccionLocal = "Parque empresarial este, bodega 5",
                FincaOIdPredial = "SJ-778811",
                ActividadEconomica = "Ferreterías y materiales de construcción",
                CodigoCaecr = "4752",
                TipoPatente = "Industrial",
                NumeroCertificadoUsoSuelo = "US-2024-881",
                EstadoUsoSuelo = "Conforme",
                UsoSueloConforme = true,
                FechaSolicitud = DateTime.Today.AddMonths(-10),
                Responsable = "Analista Municipal",
                Estado = "Suspendido",
                Observaciones = "Licencia en suspensión temporal.",
                Adjuntos = new List<string> { "Documento de identidad", "Certificado de uso de suelo", "Plano o croquis" },
                Distrito = "Pavas",
                Requisitos = DefaultRequirements(true),
                UsoSuelo = new UsoSueloVinculadoDto { NumeroCertificado = "US-2024-881", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddMonths(-9), Observaciones = "Conforme.", Fuente = "Uso de Suelo" }
            },
            Requisitos = DefaultRequirements(true),
            Movimientos = new List<MovimientoPatenteDto>
            {
                new() { FechaHora = DateTime.Today.AddMonths(-10), TipoMovimiento = "Solicitud", Motivo = "Licencia industrial", Usuario = "Plataforma", EstadoResultante = "En revision", Origen = "Patentes" },
                new() { FechaHora = DateTime.Today.AddMonths(-9), TipoMovimiento = "Aprobación", Motivo = "Expediente aprobado", Usuario = "Analista Municipal", EstadoResultante = "Aprobado", Origen = "Patentes" },
                new() { FechaHora = DateTime.Today.AddMonths(-1), TipoMovimiento = "Suspensión", Motivo = "Suspensión temporal a solicitud del contribuyente", Usuario = "Analista Municipal", EstadoResultante = "Suspendido", Origen = "Patentes" }
            }
        }
    };

    public Task<List<LicenciaComercialDto>> GetAllAsync()
        => Task.FromResult(Data.OrderByDescending(x => x.FechaSolicitud).Select(CloneLicencia).ToList());

    public Task<LicenciaComercialDto> GetByIdAsync(int id)
        => Task.FromResult(CloneLicencia(Data.FirstOrDefault(x => x.Id == id) ?? new LicenciaComercialDto()));

    public Task<List<LicenciaComercialDto>> SearchAsync(string? numeroLicencia, string? contribuyente, string? actividadEconomica, string? distrito, string? estado, DateTime? fechaVencimientoHasta, string? tipo)
    {
        var query = Data.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(numeroLicencia))
            query = query.Where(x => x.NumeroLicencia.Contains(numeroLicencia, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(contribuyente))
            query = query.Where(x => x.ContribuyenteNombre.Contains(contribuyente, StringComparison.OrdinalIgnoreCase)
                || x.ContribuyenteRuc.Contains(contribuyente, StringComparison.OrdinalIgnoreCase)
                || x.NombreComercial.Contains(contribuyente, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(actividadEconomica))
            query = query.Where(x => x.ActividadEconomica.Contains(actividadEconomica, StringComparison.OrdinalIgnoreCase)
                || x.CodigoCaecr.Contains(actividadEconomica, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(distrito))
            query = query.Where(x => x.Distrito.Equals(distrito, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(x => x.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));

        if (fechaVencimientoHasta.HasValue)
            query = query.Where(x => x.FechaVencimiento.HasValue && x.FechaVencimiento.Value.Date <= fechaVencimientoHasta.Value.Date);

        if (!string.IsNullOrWhiteSpace(tipo))
            query = query.Where(x => x.TipoPatente.Equals(tipo, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(query.OrderByDescending(x => x.FechaSolicitud).Select(CloneLicencia).ToList());
    }

    public Task<LicenciaComercialDto> SaveSolicitudAsync(SolicitudPatenteDto solicitud)
    {
        var target = Data.FirstOrDefault(x => x.Id == solicitud.LicenciaId.GetValueOrDefault());
        if (target is null)
        {
            target = new LicenciaComercialDto
            {
                Id = Data.Any() ? Data.Max(x => x.Id) + 1 : 1,
                NumeroLicencia = GenerateNumeroLicencia(),
                FechaSolicitud = solicitud.FechaSolicitud == default ? DateTime.Today : solicitud.FechaSolicitud
            };
            Data.Add(target);
        }

        solicitud.Id = solicitud.Id == 0 ? Data.Select(x => x.Solicitud.Id).DefaultIfEmpty().Max() + 1 : solicitud.Id;
        solicitud.NumeroSolicitud = string.IsNullOrWhiteSpace(solicitud.NumeroSolicitud) ? GenerateNumeroSolicitud() : solicitud.NumeroSolicitud;
        solicitud.LicenciaId = target.Id;
        MapSolicitud(target, solicitud);
        AppendMovement(target, "Solicitud", "Actualización o ingreso de solicitud", solicitud.Responsable, target.Estado, "Patentes");
        return Task.FromResult(CloneLicencia(target));
    }

    public Task<LicenciaComercialDto> SaveLicenciaAsync(LicenciaComercialDto licencia, string usuario)
    {
        var target = Data.FirstOrDefault(x => x.Id == licencia.Id);
        if (target is null)
        {
            target = CloneLicencia(licencia);
            target.Id = Data.Any() ? Data.Max(x => x.Id) + 1 : 1;
            target.NumeroLicencia = string.IsNullOrWhiteSpace(target.NumeroLicencia) ? GenerateNumeroLicencia() : target.NumeroLicencia;
            Data.Add(target);
        }
        else
        {
            target.NombreComercial = licencia.NombreComercial;
            target.DireccionLocal = licencia.DireccionLocal;
            target.Distrito = licencia.Distrito;
            target.TipoPatente = licencia.TipoPatente;
            target.Responsable = licencia.Responsable;
            target.Observaciones = licencia.Observaciones;
            target.NotificacionPendiente = licencia.NotificacionPendiente;
            target.EmisionSemestralPendiente = licencia.EmisionSemestralPendiente;
        }

        AppendMovement(target, "Mantenimiento", "Actualización de datos básicos de la licencia", usuario, target.Estado, "Patentes");
        return Task.FromResult(CloneLicencia(target));
    }

    public Task<LicenciaComercialDto> AprobarSolicitudAsync(int licenciaId, string usuario)
    {
        var target = Data.First(x => x.Id == licenciaId);
        if (!target.UsoSuelo.EsConforme)
            throw new InvalidOperationException("No se puede aprobar la patente si el uso de suelo no es conforme.");

        target.Estado = "Aprobado";
        target.FechaAprobacion = DateTime.Today;
        target.FechaVencimiento = DateTime.Today.AddMonths(6);
        target.PendienteUsoSuelo = false;
        target.EmisionSemestralPendiente = false;
        target.CobroSincronizado = true;
        target.NotificacionPendiente = true;
        target.Solicitud.Estado = "Aprobado";
        AppendMovement(target, "Aprobación", "Patente aprobada y remitida a cobro", usuario, target.Estado, "Patentes");
        return Task.FromResult(CloneLicencia(target));
    }

    public Task<LicenciaComercialDto> SuspenderAsync(int licenciaId, string motivo, DateTime fecha, string usuario)
    {
        var target = Data.First(x => x.Id == licenciaId);
        target.Estado = "Suspendido";
        target.NotificacionPendiente = true;
        target.Solicitud.Estado = "Suspendido";
        AppendMovement(target, "Suspensión", $"{motivo} · {fecha:yyyy-MM-dd}", usuario, target.Estado, "Patentes");
        return Task.FromResult(CloneLicencia(target));
    }

    public Task<LicenciaComercialDto> CancelarAsync(int licenciaId, string motivo, DateTime fecha, string usuario)
    {
        var target = Data.First(x => x.Id == licenciaId);
        target.Estado = "Cancelado";
        target.NotificacionPendiente = true;
        target.FechaVencimiento = fecha;
        target.Solicitud.Estado = "Cancelado";
        AppendMovement(target, "Cancelación", $"{motivo} · {fecha:yyyy-MM-dd}", usuario, target.Estado, "Patentes");
        return Task.FromResult(CloneLicencia(target));
    }

    private static void MapSolicitud(LicenciaComercialDto target, SolicitudPatenteDto solicitud)
    {
        target.ContribuyenteId = solicitud.ContribuyenteId;
        target.ContribuyenteRuc = solicitud.ContribuyenteRuc;
        target.Identificacion = solicitud.Identificacion;
        target.ContribuyenteNombre = solicitud.ContribuyenteNombre;
        target.NombreComercial = solicitud.NombreComercial;
        target.DireccionLocal = solicitud.DireccionLocal;
        target.FincaOIdPredial = solicitud.FincaOIdPredial;
        target.ActividadEconomica = solicitud.ActividadEconomica;
        target.CodigoCaecr = solicitud.CodigoCaecr;
        target.TipoPatente = solicitud.TipoPatente;
        target.Distrito = solicitud.Distrito;
        target.FechaSolicitud = solicitud.FechaSolicitud == default ? DateTime.Today : solicitud.FechaSolicitud;
        target.Responsable = solicitud.Responsable;
        target.Observaciones = solicitud.Observaciones;
        target.Estado = string.IsNullOrWhiteSpace(solicitud.Estado) ? "Borrador" : solicitud.Estado;
        target.PendienteUsoSuelo = !solicitud.UsoSueloConforme;

        var usoSuelo = CloneUsoSuelo(solicitud.UsoSuelo);
        if (string.IsNullOrWhiteSpace(usoSuelo.NumeroCertificado))
        {
            usoSuelo.NumeroCertificado = solicitud.NumeroCertificadoUsoSuelo;
            usoSuelo.Estado = string.IsNullOrWhiteSpace(solicitud.EstadoUsoSuelo) ? "Pendiente" : solicitud.EstadoUsoSuelo;
            usoSuelo.EsConforme = solicitud.UsoSueloConforme;
            usoSuelo.FechaValidacion = DateTime.Today;
            usoSuelo.Observaciones = string.IsNullOrWhiteSpace(usoSuelo.Observaciones) ? "Pendiente validación de uso de suelo." : usoSuelo.Observaciones;
            usoSuelo.Fuente = "Uso de Suelo";
        }

        target.UsoSuelo = usoSuelo;
        target.Requisitos = solicitud.Requisitos.Select(CloneRequisito).ToList();
        target.Solicitud = CloneSolicitud(solicitud);
    }

    private static string GenerateNumeroLicencia()
        => $"PAT-{DateTime.Today.Year}-{(Data.Any() ? Data.Max(x => x.Id) + 1 : 1):000}";

    private static string GenerateNumeroSolicitud()
        => $"SOL-PAT-{DateTime.Today.Year}-{(Data.Select(x => x.Solicitud.Id).DefaultIfEmpty().Max() + 1):000}";

    private static void AppendMovement(LicenciaComercialDto licencia, string tipo, string motivo, string usuario, string estado, string origen)
        => licencia.Movimientos.Insert(0, new MovimientoPatenteDto
        {
            FechaHora = DateTime.Now,
            TipoMovimiento = tipo,
            Motivo = motivo,
            Usuario = string.IsNullOrWhiteSpace(usuario) ? "Sistema" : usuario,
            EstadoResultante = estado,
            Origen = origen
        });

    private static LicenciaComercialDto CloneLicencia(LicenciaComercialDto item) => new()
    {
        Id = item.Id,
        NumeroLicencia = item.NumeroLicencia,
        ContribuyenteId = item.ContribuyenteId,
        ContribuyenteRuc = item.ContribuyenteRuc,
        Identificacion = item.Identificacion,
        ContribuyenteNombre = item.ContribuyenteNombre,
        NombreComercial = item.NombreComercial,
        DireccionLocal = item.DireccionLocal,
        FincaOIdPredial = item.FincaOIdPredial,
        ActividadEconomica = item.ActividadEconomica,
        CodigoCaecr = item.CodigoCaecr,
        TipoPatente = item.TipoPatente,
        Distrito = item.Distrito,
        Estado = item.Estado,
        FechaSolicitud = item.FechaSolicitud,
        FechaAprobacion = item.FechaAprobacion,
        FechaVencimiento = item.FechaVencimiento,
        Responsable = item.Responsable,
        Observaciones = item.Observaciones,
        PendienteUsoSuelo = item.PendienteUsoSuelo,
        EmisionSemestralPendiente = item.EmisionSemestralPendiente,
        NotificacionPendiente = item.NotificacionPendiente,
        CobroSincronizado = item.CobroSincronizado,
        GisValidado = item.GisValidado,
        UsoSuelo = CloneUsoSuelo(item.UsoSuelo),
        Solicitud = CloneSolicitud(item.Solicitud),
        Requisitos = item.Requisitos.Select(CloneRequisito).ToList(),
        Movimientos = item.Movimientos.Select(CloneMovimiento).ToList()
    };

    private static SolicitudPatenteDto CloneSolicitud(SolicitudPatenteDto item) => new()
    {
        Id = item.Id,
        LicenciaId = item.LicenciaId,
        NumeroSolicitud = item.NumeroSolicitud,
        ContribuyenteId = item.ContribuyenteId,
        ContribuyenteRuc = item.ContribuyenteRuc,
        Identificacion = item.Identificacion,
        ContribuyenteNombre = item.ContribuyenteNombre,
        NombreComercial = item.NombreComercial,
        DireccionLocal = item.DireccionLocal,
        FincaOIdPredial = item.FincaOIdPredial,
        ActividadEconomica = item.ActividadEconomica,
        CodigoCaecr = item.CodigoCaecr,
        TipoPatente = item.TipoPatente,
        NumeroCertificadoUsoSuelo = item.NumeroCertificadoUsoSuelo,
        EstadoUsoSuelo = item.EstadoUsoSuelo,
        UsoSueloConforme = item.UsoSueloConforme,
        FechaSolicitud = item.FechaSolicitud,
        Responsable = item.Responsable,
        Estado = item.Estado,
        Observaciones = item.Observaciones,
        Adjuntos = item.Adjuntos.ToList(),
        Requisitos = item.Requisitos.Select(CloneRequisito).ToList(),
        UsoSuelo = CloneUsoSuelo(item.UsoSuelo),
        Distrito = item.Distrito
    };

    private static UsoSueloVinculadoDto CloneUsoSuelo(UsoSueloVinculadoDto item) => new()
    {
        NumeroCertificado = item.NumeroCertificado,
        Estado = item.Estado,
        EsConforme = item.EsConforme,
        FechaValidacion = item.FechaValidacion,
        Observaciones = item.Observaciones,
        Fuente = item.Fuente
    };

    private static RequisitoPatenteDto CloneRequisito(RequisitoPatenteDto item) => new()
    {
        Nombre = item.Nombre,
        Obligatorio = item.Obligatorio,
        Cumplido = item.Cumplido,
        Observacion = item.Observacion
    };

    private static MovimientoPatenteDto CloneMovimiento(MovimientoPatenteDto item) => new()
    {
        FechaHora = item.FechaHora,
        TipoMovimiento = item.TipoMovimiento,
        Motivo = item.Motivo,
        Usuario = item.Usuario,
        EstadoResultante = item.EstadoResultante,
        Origen = item.Origen
    };

    private static List<RequisitoPatenteDto> DefaultRequirements(bool completed) => new()
    {
        new() { Nombre = "Documento de identidad", Cumplido = completed, Observacion = completed ? "Adjunto" : "Pendiente" },
        new() { Nombre = "Certificado de uso de suelo", Cumplido = completed, Observacion = completed ? "Validado" : "Pendiente validación" },
        new() { Nombre = "Plano o croquis", Cumplido = completed, Observacion = completed ? "Adjunto" : "Pendiente" },
        new() { Nombre = "Declaración jurada", Cumplido = completed, Observacion = completed ? "Adjunto" : "Pendiente" }
    };
}
