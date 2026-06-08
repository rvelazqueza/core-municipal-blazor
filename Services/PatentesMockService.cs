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

    static PatentesMockService()
    {
        EnsureMinimumLicencias();
    }

    public Task<List<LicenciaComercialDto>> GetAllAsync()
        => Task.FromResult(Data.OrderByDescending(x => x.FechaSolicitud).Select(CloneLicencia).ToList());

    public Task<List<SolicitudPatenteDto>> GetSolicitudesAsync()
        => Task.FromResult(Data.Select(x => CloneSolicitud(x.Solicitud)).OrderByDescending(x => x.FechaSolicitud).ToList());

    public Task<PatentesModuleSnapshotDto> GetModuleSnapshotAsync()
        => Task.FromResult(PatentesMockDatasetFactory.BuildSnapshot(Data.Select(CloneLicencia).ToList()));

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

    private static void EnsureMinimumLicencias()
    {
        if (Data.Count >= 10)
        {
            return;
        }

        var additionalLicencias = new List<LicenciaComercialDto>
        {
            CreateSupplementalLicencia(6, "PAT-2025-031", 4, "2-3456-7890", "Distribuidora El Roble S.R.L.", "Licorera El Roble", "Frente a la estación, local 4", "SJ-009901", "Expendio de bebidas alcohólicas", "5630", "Licores", "Merced", "Aprobado", DateTime.Today.AddMonths(-4), DateTime.Today.AddMonths(-4).AddDays(6), DateTime.Today.AddYears(5), "Carolina Muñoz", "Expediente con licencia especial vinculada.", new UsoSueloVinculadoDto { NumeroCertificado = "US-2025-311", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddMonths(-4), Observaciones = "Compatible con actividad especial.", Fuente = "Uso de Suelo" }, true),
            CreateSupplementalLicencia(7, "PAT-2025-041", 5, "1-9876-5432", "María Fernanda Quesada", "Consultoría Integral", "Cobertura cantonal sin establecimiento permanente", "Sin local físico", "Servicios profesionales sin local físico", "4789", "Servicios sin local", "Zapote", "Pendiente validación", DateTime.Today.AddDays(-18), null, null, "Sofía Rojas", "Actividad declarada sin local físico; pendiente revisión documental.", new UsoSueloVinculadoDto { NumeroCertificado = "SLF-2025-041", Estado = "Pendiente", EsConforme = false, FechaValidacion = DateTime.Today.AddDays(-10), Observaciones = "Actividad exceptuada de local fijo en validación mock.", Fuente = "Patentes" }, false),
            CreateSupplementalLicencia(8, "PAT-2024-088", 6, "3-102-456789", "Eventos Metropolitanos S.A.", "Eventos Metro", "Centro comercial sur, local 19", "SJ-870011", "Organización de eventos temporales", "8230", "Temporal", "Mata Redonda", "En revision", DateTime.Today.AddMonths(-2), null, DateTime.Today.AddMonths(4), "Daniel Zúñiga", "Renovación con inspección pendiente.", new UsoSueloVinculadoDto { NumeroCertificado = "US-2024-771", Estado = "Pendiente", EsConforme = false, FechaValidacion = DateTime.Today.AddDays(-12), Observaciones = "Pendiente criterio técnico para evento recurrente.", Fuente = "Uso de Suelo" }, false),
            CreateSupplementalLicencia(9, "PAT-2025-052", 8, "3-101-223344", "Logística del Valle S.A.", "Centro Logístico del Valle", "Zona industrial oeste, bodega 12", "SJ-440022", "Transporte privado referencial", "4922", "Transporte", "Uruca", "Aprobado", DateTime.Today.AddMonths(-5), DateTime.Today.AddMonths(-5).AddDays(9), DateTime.Today.AddMonths(7), "Mauricio Porras", "Licencia de logística con cobro mock sincronizado.", new UsoSueloVinculadoDto { NumeroCertificado = "US-2025-402", Estado = "Conforme", EsConforme = true, FechaValidacion = DateTime.Today.AddMonths(-5), Observaciones = "Zonificación compatible para bodega y despacho.", Fuente = "Uso de Suelo" }, true),
            CreateSupplementalLicencia(10, "PAT-2025-061", 9, "1-2233-4455", "Karen Brenes Alpízar", "Punto Navideño Catedral", "Boulevard peatonal, módulo temporal", "SIN-LOCAL-02", "Comercio ambulante autorizado", "4789", "Ambulante", "Catedral", "Borrador", DateTime.Today.AddDays(-3), null, null, "Analista Demo", "Solicitud reciente ingresada desde ventanilla digital.", new UsoSueloVinculadoDto { NumeroCertificado = "US-2025-611", Estado = "Pendiente", EsConforme = false, FechaValidacion = DateTime.Today.AddDays(-2), Observaciones = "Pendiente validación simplificada para ubicación temporal.", Fuente = "Uso de Suelo" }, false)
        };

        foreach (var licencia in additionalLicencias.Where(x => Data.All(y => y.NumeroLicencia != x.NumeroLicencia)))
        {
            Data.Add(licencia);
        }
    }

    private static LicenciaComercialDto CreateSupplementalLicencia(int id, string numeroLicencia, int contribuyenteId, string identificacion, string contribuyenteNombre, string nombreComercial, string direccionLocal, string fincaOIdPredial, string actividadEconomica, string codigoCaecr, string tipoPatente, string distrito, string estado, DateTime fechaSolicitud, DateTime? fechaAprobacion, DateTime? fechaVencimiento, string responsable, string observaciones, UsoSueloVinculadoDto usoSuelo, bool requisitosCompletos)
    {
        var solicitud = new SolicitudPatenteDto
        {
            Id = id,
            LicenciaId = id,
            NumeroSolicitud = $"SOL-{numeroLicencia}",
            NumeroExpediente = $"EXP-{numeroLicencia}",
            CanalIngreso = ResolveCanalIngreso(tipoPatente, estado, direccionLocal, fincaOIdPredial, fechaSolicitud),
            ContribuyenteId = contribuyenteId,
            ContribuyenteRuc = identificacion,
            Identificacion = identificacion,
            ContribuyenteNombre = contribuyenteNombre,
            NombreComercial = nombreComercial,
            DireccionLocal = direccionLocal,
            FincaOIdPredial = fincaOIdPredial,
            ActividadEconomica = actividadEconomica,
            CodigoCaecr = codigoCaecr,
            TipoPatente = tipoPatente,
            NumeroCertificadoUsoSuelo = usoSuelo.NumeroCertificado,
            EstadoUsoSuelo = usoSuelo.Estado,
            UsoSueloConforme = usoSuelo.EsConforme,
            FechaSolicitud = fechaSolicitud,
            Responsable = responsable,
            Estado = estado,
            Observaciones = observaciones,
            Adjuntos = new List<string> { "Documento de identidad", "Declaración jurada", "Croquis referencial" },
            Requisitos = DefaultRequirements(requisitosCompletos),
            UsoSuelo = CloneUsoSuelo(usoSuelo),
            Distrito = distrito
        };

        return new LicenciaComercialDto
        {
            Id = id,
            NumeroLicencia = numeroLicencia,
            Expediente = $"EXP-{numeroLicencia}",
            CanalIngreso = ResolveCanalIngreso(tipoPatente, estado, direccionLocal, fincaOIdPredial, fechaSolicitud),
            ContribuyenteId = contribuyenteId,
            ContribuyenteRuc = identificacion,
            Identificacion = identificacion,
            ContribuyenteNombre = contribuyenteNombre,
            NombreComercial = nombreComercial,
            DireccionLocal = direccionLocal,
            FincaOIdPredial = fincaOIdPredial,
            ActividadEconomica = actividadEconomica,
            CodigoCaecr = codigoCaecr,
            TipoPatente = tipoPatente,
            Distrito = distrito,
            Estado = estado,
            FechaSolicitud = fechaSolicitud,
            FechaAprobacion = fechaAprobacion,
            FechaVencimiento = fechaVencimiento,
            Responsable = responsable,
            Observaciones = observaciones,
            PendienteUsoSuelo = !usoSuelo.EsConforme,
            EmisionSemestralPendiente = fechaAprobacion.HasValue && estado != "Aprobado",
            NotificacionPendiente = estado != "Aprobado",
            CobroSincronizado = fechaAprobacion.HasValue,
            GisValidado = usoSuelo.Estado != "Pendiente",
            UsoSuelo = CloneUsoSuelo(usoSuelo),
            Solicitud = solicitud,
            Requisitos = DefaultRequirements(requisitosCompletos),
            Movimientos = new List<MovimientoPatenteDto>
            {
                new() { FechaHora = fechaSolicitud.AddHours(8), TipoMovimiento = "Solicitud", Motivo = "Ingreso mock complementario de patente", Usuario = "Plataforma", EstadoResultante = "Borrador", Origen = "Patentes" },
                new() { FechaHora = fechaSolicitud.AddDays(1).AddHours(10), TipoMovimiento = "Revisión", Motivo = "Expediente técnico en modo demo", Usuario = responsable, EstadoResultante = estado, Origen = "Patentes" }
            }
        };
    }

    private static void MapSolicitud(LicenciaComercialDto target, SolicitudPatenteDto solicitud)
    {
        target.ContribuyenteId = solicitud.ContribuyenteId;
        target.ContribuyenteRuc = solicitud.ContribuyenteRuc;
        target.Identificacion = solicitud.Identificacion;
        target.Expediente = string.IsNullOrWhiteSpace(solicitud.NumeroExpediente) ? target.Expediente : solicitud.NumeroExpediente;
        target.CanalIngreso = string.IsNullOrWhiteSpace(solicitud.CanalIngreso) ? target.CanalIngreso : solicitud.CanalIngreso;
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
        target.Expediente = string.IsNullOrWhiteSpace(target.Expediente) ? $"EXP-{target.NumeroLicencia}" : target.Expediente;
        target.CanalIngreso = string.IsNullOrWhiteSpace(target.CanalIngreso)
            ? ResolveCanalIngreso(target.TipoPatente, target.Estado, target.DireccionLocal, target.FincaOIdPredial, target.FechaSolicitud)
            : target.CanalIngreso;
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
        Expediente = string.IsNullOrWhiteSpace(item.Expediente) ? $"EXP-{item.NumeroLicencia}" : item.Expediente,
        CanalIngreso = string.IsNullOrWhiteSpace(item.CanalIngreso) ? ResolveCanalIngreso(item.TipoPatente, item.Estado, item.DireccionLocal, item.FincaOIdPredial, item.FechaSolicitud) : item.CanalIngreso,
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
        NumeroExpediente = string.IsNullOrWhiteSpace(item.NumeroExpediente) ? $"EXP-{item.NumeroSolicitud}" : item.NumeroExpediente,
        CanalIngreso = string.IsNullOrWhiteSpace(item.CanalIngreso) ? "Ventanilla digital" : item.CanalIngreso,
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
        TipoTasacion = item.TipoTasacion,
        EstadoTasacion = item.EstadoTasacion,
        MontoAnualMock = item.MontoAnualMock,
        MontoTrimestralMock = item.MontoTrimestralMock,
        TimbreBiodiversidad = item.TimbreBiodiversidad,
        PublicidadExterior = item.PublicidadExterior,
        Multa = item.Multa,
        Intereses = item.Intereses,
        FechaInicioCobro = item.FechaInicioCobro,
        CobroProporcionalVisual = item.CobroProporcionalVisual,
        EstadoCobro = item.EstadoCobro,
        EstadoCuentaTributariaMock = item.EstadoCuentaTributariaMock,
        ReferenciaCuentaPorCobrarMock = item.ReferenciaCuentaPorCobrarMock,
        EstadoResolucion = item.EstadoResolucion,
        ObservacionesResolucion = item.ObservacionesResolucion,
        NotificacionSimulada = item.NotificacionSimulada,
        FirmaDigitalReferencial = item.FirmaDigitalReferencial,
        EstadoFinalExpediente = item.EstadoFinalExpediente,
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
        Clave = item.Clave,
        Nombre = item.Nombre,
        Obligatorio = item.Obligatorio,
        Cumplido = item.Cumplido,
        Estado = item.Estado,
        Observacion = item.Observacion,
        DocumentoPlaceholder = item.DocumentoPlaceholder,
        Origen = item.Origen
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
        new() { Clave = "identificacion", Nombre = "Documento de identidad", Cumplido = completed, Estado = completed ? "Cumple" : "Pendiente", Observacion = completed ? "Adjunto" : "Pendiente", DocumentoPlaceholder = "Documento de identidad mock", Origen = "Mock" },
        new() { Clave = "uso-suelo", Nombre = "Certificado de uso de suelo", Cumplido = completed, Estado = completed ? "Cumple" : "Pendiente", Observacion = completed ? "Validado" : "Pendiente validación", DocumentoPlaceholder = "Certificado de uso de suelo mock", Origen = "Mock" },
        new() { Clave = "croquis", Nombre = "Plano o croquis", Cumplido = completed, Estado = completed ? "Cumple" : "Pendiente", Observacion = completed ? "Adjunto" : "Pendiente", DocumentoPlaceholder = "Croquis mock", Origen = "Manual" },
        new() { Clave = "declaracion-jurada", Nombre = "Declaración jurada", Cumplido = completed, Estado = completed ? "Cumple" : "Pendiente", Observacion = completed ? "Adjunto" : "Pendiente", DocumentoPlaceholder = "Declaración jurada mock", Origen = "Manual" }
    };

    private static string ResolveCanalIngreso(string tipoPatente, string estado, string direccionLocal, string fincaOIdPredial, DateTime fechaSolicitud)
    {
        if (!string.IsNullOrWhiteSpace(direccionLocal) && direccionLocal.Contains("sin local físico", StringComparison.OrdinalIgnoreCase))
            return "Digital";

        if (!string.IsNullOrWhiteSpace(fincaOIdPredial) && fincaOIdPredial.Contains("SIN-LOCAL", StringComparison.OrdinalIgnoreCase))
            return "Digital";

        if (tipoPatente.Equals("Licores", StringComparison.OrdinalIgnoreCase))
            return "Ventanilla especializada";

        if (tipoPatente.Equals("Temporal", StringComparison.OrdinalIgnoreCase))
            return "Ventanilla presencial";

        if (estado.Equals("Borrador", StringComparison.OrdinalIgnoreCase))
            return "Canal digital";

        return fechaSolicitud.Day % 2 == 0 ? "Canal digital" : "Ventanilla presencial";
    }
}
