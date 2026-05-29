using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class BienesInmueblesMockService : IBienesInmueblesService
{
    private static readonly List<BienInmuebleDto> Data = new()
    {
        new BienInmuebleDto
        {
            Id = 1,
            NumeroFinca = "SJ-002145",
            IdPredial = "P-SJ-CAR-0001",
            TipoFinca = "Individual",
            EstadoRegistral = "Inscrita",
            Distrito = "Carmen",
            DireccionExacta = "Avenida central, contiguo al edificio azul",
            AreaTerreno = 250.45m,
            PropietarioPrincipalId = 1,
            PropietarioPrincipal = "Ana Solano Perez",
            PropietarioPrincipalIdentificacion = "1-1234-5678",
            PorcentajeDerecho = 100m,
            ValorTerreno = 48000000m,
            ValorConstruccion = 27500000m,
            ValorFiscalTotal = 75500000m,
            ZonaHomogenea = "Zona A-1",
            TipologiaConstructiva = "Residencial media",
            UsoInmueble = "Residencial",
            Observaciones = "Finca urbana con acceso a todos los servicios.",
            Estado = "Activo",
            Condicion = "Al dia",
            FincaValorActualizado = true,
            RegistroPublicoSincronizado = true,
            RemitidoHacienda = true,
            UbicacionGis = "Polígono validado por GIS municipal",
            CuentaTributaria = "CT-0002145",
            UltimaActualizacion = DateTime.Now.AddHours(-4),
            DerechosPropiedad = new List<DerechoPropiedadDto>
            {
                new() { ContribuyenteId = 1, Titular = "Ana Solano Perez", Identificacion = "1-1234-5678", TipoDerecho = "Pleno dominio", Porcentaje = 100m, Estado = "Activo", FechaInicio = DateTime.Today.AddYears(-8) }
            },
            Valoraciones = new List<ValoracionDto>
            {
                new() { Periodo = "2025", ZonaHomogenea = "Zona A-1", TipologiaConstructiva = "Residencial media", ValorTerreno = 48000000m, ValorConstruccion = 27500000m, ValorFiscalTotal = 75500000m, FechaValoracion = DateTime.Today.AddDays(-20), Actualizado = true }
            },
            Declaraciones = new List<DeclaracionBienDto>
            {
                new() { Periodo = "2025", Estado = "Presentada", FechaPresentacion = DateTime.Today.AddDays(-12), MedioRecepcion = "Portal municipal", Observacion = "Declaración anual recibida sin ajustes." }
            },
            Exoneraciones = new List<ExoneracionDto>(),
            Fiscalizaciones = new List<FiscalizacionBienDto>
            {
                new() { NumeroCaso = "FIS-2024-018", Inspector = "Laura Rojas", Estado = "Finalizada", Resultado = "Sin diferencias relevantes", FechaProgramada = DateTime.Today.AddMonths(-2), FechaCierre = DateTime.Today.AddMonths(-2).AddDays(4), Hallazgos = "Uso residencial conforme." }
            },
            Imagenes = new List<ImagenPredialDto>
            {
                new() { Titulo = "Fachada principal", Tipo = "Imagen", Url = "https://placehold.co/600x400?text=Finca+SJ-002145", Fuente = "Inspección municipal", FechaRegistro = DateTime.Today.AddMonths(-1) }
            },
            HistorialCambios = new List<AuditoriaCambioDto>
            {
                new() { Usuario = "valoracion.sistema", FechaHora = DateTime.Now.AddHours(-4), CampoModificado = "Valor fiscal", ValorAnterior = "73,000,000", ValorNuevo = "75,500,000", Origen = "Valoración" },
                new() { Usuario = "registro.publico", FechaHora = DateTime.Now.AddDays(-3), CampoModificado = "Estado registral", ValorAnterior = "En estudio", ValorNuevo = "Inscrita", Origen = "Registro Público" }
            }
        },
        new BienInmuebleDto
        {
            Id = 2,
            NumeroFinca = "SJ-889900",
            IdPredial = "P-SJ-SF-0145",
            TipoFinca = "Horizontal",
            EstadoRegistral = "Inscrita",
            Distrito = "San Francisco",
            DireccionExacta = "Centro corporativo norte, módulo B",
            AreaTerreno = 540.80m,
            PropietarioPrincipalId = 2,
            PropietarioPrincipal = "Tecnologias Urbanas CR S.A.",
            PropietarioPrincipalIdentificacion = "3-101-998877",
            PorcentajeDerecho = 100m,
            ValorTerreno = 125000000m,
            ValorConstruccion = 260000000m,
            ValorFiscalTotal = 385000000m,
            ZonaHomogenea = "Zona C-2",
            TipologiaConstructiva = "Comercial alta",
            UsoInmueble = "Comercial",
            Observaciones = "Condominio comercial sujeto a verificación de ampliaciones.",
            Estado = "En fiscalizacion",
            Condicion = "Con revision",
            FincaValorActualizado = true,
            PendienteFiscalizacion = true,
            RegistroPublicoSincronizado = true,
            UbicacionGis = "Ubicación pendiente de validación de capas internas",
            CuentaTributaria = "CT-0889900",
            UltimaActualizacion = DateTime.Now.AddHours(-1),
            DerechosPropiedad = new List<DerechoPropiedadDto>
            {
                new() { ContribuyenteId = 2, Titular = "Tecnologias Urbanas CR S.A.", Identificacion = "3-101-998877", TipoDerecho = "Pleno dominio", Porcentaje = 100m, Estado = "Activo", FechaInicio = DateTime.Today.AddYears(-5) }
            },
            Valoraciones = new List<ValoracionDto>
            {
                new() { Periodo = "2025", ZonaHomogenea = "Zona C-2", TipologiaConstructiva = "Comercial alta", ValorTerreno = 125000000m, ValorConstruccion = 260000000m, ValorFiscalTotal = 385000000m, FechaValoracion = DateTime.Today.AddDays(-5), Actualizado = true }
            },
            Declaraciones = new List<DeclaracionBienDto>
            {
                new() { Periodo = "2025", Estado = "Pendiente", MedioRecepcion = "Pendiente", Observacion = "Declaración no recibida al cierre de corte." }
            },
            Exoneraciones = new List<ExoneracionDto>
            {
                new() { TipoExoneracion = "Interés social", Porcentaje = 25m, FechaInicio = DateTime.Today.AddMonths(-6), FechaFin = DateTime.Today.AddMonths(6), Estado = "Activa", Institucion = "Concejo Municipal" }
            },
            Fiscalizaciones = new List<FiscalizacionBienDto>
            {
                new() { NumeroCaso = "FIS-2025-004", Inspector = "Marvin Soto", Estado = "Programada", Resultado = "", FechaProgramada = DateTime.Today.AddDays(6), Hallazgos = "Revisar ampliación reportada por catastro." }
            },
            Imagenes = new List<ImagenPredialDto>
            {
                new() { Titulo = "Plano catastrado", Tipo = "Plano", Url = "https://placehold.co/600x400?text=Plano+SJ-889900", Fuente = "Catastro", FechaRegistro = DateTime.Today.AddMonths(-2) }
            },
            HistorialCambios = new List<AuditoriaCambioDto>
            {
                new() { Usuario = "fiscalizacion.bienes", FechaHora = DateTime.Now.AddHours(-1), CampoModificado = "Estado", ValorAnterior = "Activo", ValorNuevo = "En fiscalizacion", Origen = "Fiscalización" },
                new() { Usuario = "control.omisos", FechaHora = DateTime.Now.AddDays(-7), CampoModificado = "Declaración", ValorAnterior = "Esperada", ValorNuevo = "Pendiente", Origen = "Declaraciones" }
            }
        },
        new BienInmuebleDto
        {
            Id = 3,
            NumeroFinca = "CT-450210",
            IdPredial = "P-CT-MER-0099",
            TipoFinca = "Individual",
            EstadoRegistral = "Pendiente revision",
            Distrito = "Merced",
            DireccionExacta = "Costado norte del mercado municipal",
            AreaTerreno = 185.20m,
            PropietarioPrincipalId = 4,
            PropietarioPrincipal = "Marta Cerdas Quesada",
            PropietarioPrincipalIdentificacion = "2-3344-5566",
            PorcentajeDerecho = 50m,
            ValorTerreno = 35500000m,
            ValorConstruccion = 14800000m,
            ValorFiscalTotal = 50300000m,
            ZonaHomogenea = "Zona B-3",
            TipologiaConstructiva = "Mixta básica",
            UsoInmueble = "Mixto",
            Observaciones = "Expediente con omisión detectada en 2025.",
            Estado = "Pendiente revision",
            Condicion = "Omiso",
            PendienteFiscalizacion = true,
            OmisoDetectado = true,
            UltimaActualizacion = DateTime.Now.AddDays(-2),
            DerechosPropiedad = new List<DerechoPropiedadDto>
            {
                new() { ContribuyenteId = 4, Titular = "Marta Cerdas Quesada", Identificacion = "2-3344-5566", TipoDerecho = "Nuda propiedad", Porcentaje = 50m, Estado = "Activo", FechaInicio = DateTime.Today.AddYears(-12) },
                new() { ContribuyenteId = 1, Titular = "Ana Solano Perez", Identificacion = "1-1234-5678", TipoDerecho = "Usufructo", Porcentaje = 50m, Estado = "Activo", FechaInicio = DateTime.Today.AddYears(-6) }
            },
            Valoraciones = new List<ValoracionDto>
            {
                new() { Periodo = "2024", ZonaHomogenea = "Zona B-3", TipologiaConstructiva = "Mixta básica", ValorTerreno = 34000000m, ValorConstruccion = 14500000m, ValorFiscalTotal = 48500000m, FechaValoracion = DateTime.Today.AddYears(-1), Actualizado = false }
            },
            Declaraciones = new List<DeclaracionBienDto>
            {
                new() { Periodo = "2025", Estado = "Pendiente", MedioRecepcion = "No presentada", Observacion = "Se incluyó en corte de omisos." }
            },
            Exoneraciones = new List<ExoneracionDto>(),
            Fiscalizaciones = new List<FiscalizacionBienDto>(),
            Imagenes = new List<ImagenPredialDto>(),
            HistorialCambios = new List<AuditoriaCambioDto>
            {
                new() { Usuario = "control.omisos", FechaHora = DateTime.Now.AddDays(-2), CampoModificado = "Condición", ValorAnterior = "Al dia", ValorNuevo = "Omiso", Origen = "Control tributario" }
            }
        },
        new BienInmuebleDto
        {
            Id = 4,
            NumeroFinca = "HE-778812",
            IdPredial = "P-HE-HOS-2201",
            TipoFinca = "Agrícola",
            EstadoRegistral = "Inscrita",
            Distrito = "Hospital",
            DireccionExacta = "Ruta cantonal 12, lote esquinero",
            AreaTerreno = 1200m,
            PropietarioPrincipalId = 3,
            PropietarioPrincipal = "Luis Mora Campos",
            PropietarioPrincipalIdentificacion = "1555666777",
            PorcentajeDerecho = 100m,
            ValorTerreno = 69000000m,
            ValorConstruccion = 12000000m,
            ValorFiscalTotal = 81000000m,
            ZonaHomogenea = "Zona R-1",
            TipologiaConstructiva = "Rural ligera",
            UsoInmueble = "Agropecuario",
            Observaciones = "Finca con actualización GIS requerida.",
            Estado = "Activo",
            Condicion = "Pendiente GIS",
            FincaValorActualizado = false,
            RegistroPublicoSincronizado = true,
            RemitidoHacienda = false,
            UbicacionGis = "Sin georreferenciación consolidada",
            UltimaActualizacion = DateTime.Now.AddDays(-5),
            DerechosPropiedad = new List<DerechoPropiedadDto>
            {
                new() { ContribuyenteId = 3, Titular = "Luis Mora Campos", Identificacion = "1555666777", TipoDerecho = "Pleno dominio", Porcentaje = 100m, Estado = "Activo", FechaInicio = DateTime.Today.AddYears(-3) }
            },
            Valoraciones = new List<ValoracionDto>(),
            Declaraciones = new List<DeclaracionBienDto>
            {
                new() { Periodo = "2025", Estado = "Presentada", FechaPresentacion = DateTime.Today.AddDays(-3), MedioRecepcion = "Ventanilla", Observacion = "Pendiente remisión al Ministerio de Hacienda." }
            },
            Exoneraciones = new List<ExoneracionDto>(),
            Fiscalizaciones = new List<FiscalizacionBienDto>
            {
                new() { NumeroCaso = "FIS-2025-015", Inspector = "Daniel Ureña", Estado = "Programada", Resultado = "", FechaProgramada = DateTime.Today.AddDays(12), Hallazgos = "Validar área declarada y uso del suelo." }
            },
            Imagenes = new List<ImagenPredialDto>
            {
                new() { Titulo = "Vista satelital", Tipo = "Imagen", Url = "https://placehold.co/600x400?text=GIS+HE-778812", Fuente = "GIS", FechaRegistro = DateTime.Today.AddDays(-30) }
            },
            HistorialCambios = new List<AuditoriaCambioDto>
            {
                new() { Usuario = "gis.municipal", FechaHora = DateTime.Now.AddDays(-5), CampoModificado = "Ubicación GIS", ValorAnterior = "No ubicada", ValorNuevo = "Pendiente de consolidación", Origen = "GIS" }
            }
        }
    };

    public Task<List<BienInmuebleDto>> GetAllAsync()
    {
        return Task.FromResult(Data.Select(Clone).OrderByDescending(x => x.UltimaActualizacion).ToList());
    }

    public Task<BienInmuebleDto> GetByIdAsync(int id)
    {
        var item = Data.FirstOrDefault(x => x.Id == id) ?? new BienInmuebleDto { Id = id };
        return Task.FromResult(Clone(item));
    }

    public Task<List<BienInmuebleDto>> SearchAsync(string? numeroFinca, string? idPredial, string? propietario, string? distrito, string? estado, decimal? valorFiscalMinimo, string? condicion)
    {
        IEnumerable<BienInmuebleDto> query = Data;

        if (!string.IsNullOrWhiteSpace(numeroFinca))
            query = query.Where(x => x.NumeroFinca.Contains(numeroFinca, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(idPredial))
            query = query.Where(x => x.IdPredial.Contains(idPredial, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(propietario))
            query = query.Where(x => x.PropietarioPrincipal.Contains(propietario, StringComparison.OrdinalIgnoreCase) || x.PropietarioPrincipalIdentificacion.Contains(propietario, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(distrito))
            query = query.Where(x => x.Distrito == distrito);
        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(x => x.Estado == estado);
        if (valorFiscalMinimo.HasValue)
            query = query.Where(x => x.ValorFiscalTotal >= valorFiscalMinimo.Value);
        if (!string.IsNullOrWhiteSpace(condicion))
            query = query.Where(x => x.Condicion == condicion);

        return Task.FromResult(query.Select(Clone).OrderByDescending(x => x.UltimaActualizacion).ToList());
    }

    public Task SaveAsync(BienInmuebleDto bien)
    {
        bien.ValorFiscalTotal = bien.ValorTerreno + bien.ValorConstruccion;
        bien.UltimaActualizacion = DateTime.Now;

        var existing = Data.FirstOrDefault(x => x.Id == bien.Id);
        if (existing is null)
        {
            bien.Id = Data.Any() ? Data.Max(x => x.Id) + 1 : 1;
            Data.Add(Clone(bien));
        }
        else
        {
            var index = Data.IndexOf(existing);
            Data[index] = Clone(bien);
        }

        return Task.CompletedTask;
    }

    public Task SimularRegistroPublicoAsync(BienInmuebleDto bien)
    {
        bien.EstadoRegistral = "Inscrita";
        bien.RegistroPublicoSincronizado = true;
        bien.UltimaActualizacion = DateTime.Now;
        return Task.CompletedTask;
    }

    public Task SimularRemisionHaciendaAsync(BienInmuebleDto bien)
    {
        bien.RemitidoHacienda = true;
        var declaracionPendiente = bien.Declaraciones.FirstOrDefault(x => x.Estado == "Pendiente");
        if (declaracionPendiente is not null)
        {
            declaracionPendiente.Estado = "Presentada";
            declaracionPendiente.FechaPresentacion = DateTime.Today;
            declaracionPendiente.MedioRecepcion = "Remisión periódica";
        }
        bien.UltimaActualizacion = DateTime.Now;
        return Task.CompletedTask;
    }

    public Task SimularGisAsync(BienInmuebleDto bien)
    {
        bien.UbicacionGis = $"Georreferenciada el {DateTime.Now:dd/MM/yyyy HH:mm}";
        bien.UltimaActualizacion = DateTime.Now;
        return Task.CompletedTask;
    }

    public Task SimularCobroAsync(BienInmuebleDto bien)
    {
        if (string.IsNullOrWhiteSpace(bien.CuentaTributaria))
        {
            bien.CuentaTributaria = $"CT-{(bien.Id == 0 ? 999999 : bien.Id.ToString("000000"))}";
        }

        bien.UltimaActualizacion = DateTime.Now;
        return Task.CompletedTask;
    }

    private static BienInmuebleDto Clone(BienInmuebleDto source)
    {
        return new BienInmuebleDto
        {
            Id = source.Id,
            NumeroFinca = source.NumeroFinca,
            IdPredial = source.IdPredial,
            TipoFinca = source.TipoFinca,
            EstadoRegistral = source.EstadoRegistral,
            Distrito = source.Distrito,
            DireccionExacta = source.DireccionExacta,
            AreaTerreno = source.AreaTerreno,
            PropietarioPrincipalId = source.PropietarioPrincipalId,
            PropietarioPrincipal = source.PropietarioPrincipal,
            PropietarioPrincipalIdentificacion = source.PropietarioPrincipalIdentificacion,
            PorcentajeDerecho = source.PorcentajeDerecho,
            ValorTerreno = source.ValorTerreno,
            ValorConstruccion = source.ValorConstruccion,
            ValorFiscalTotal = source.ValorFiscalTotal,
            ZonaHomogenea = source.ZonaHomogenea,
            TipologiaConstructiva = source.TipologiaConstructiva,
            UsoInmueble = source.UsoInmueble,
            Observaciones = source.Observaciones,
            Estado = source.Estado,
            Condicion = source.Condicion,
            FincaValorActualizado = source.FincaValorActualizado,
            PendienteFiscalizacion = source.PendienteFiscalizacion,
            OmisoDetectado = source.OmisoDetectado,
            RegistroPublicoSincronizado = source.RegistroPublicoSincronizado,
            RemitidoHacienda = source.RemitidoHacienda,
            UbicacionGis = source.UbicacionGis,
            CuentaTributaria = source.CuentaTributaria,
            UltimaActualizacion = source.UltimaActualizacion,
            DerechosPropiedad = source.DerechosPropiedad.Select(x => new DerechoPropiedadDto
            {
                ContribuyenteId = x.ContribuyenteId,
                Titular = x.Titular,
                Identificacion = x.Identificacion,
                TipoDerecho = x.TipoDerecho,
                Porcentaje = x.Porcentaje,
                Estado = x.Estado,
                FechaInicio = x.FechaInicio
            }).ToList(),
            Valoraciones = source.Valoraciones.Select(x => new ValoracionDto
            {
                Periodo = x.Periodo,
                ZonaHomogenea = x.ZonaHomogenea,
                TipologiaConstructiva = x.TipologiaConstructiva,
                ValorTerreno = x.ValorTerreno,
                ValorConstruccion = x.ValorConstruccion,
                ValorFiscalTotal = x.ValorFiscalTotal,
                FechaValoracion = x.FechaValoracion,
                Actualizado = x.Actualizado
            }).ToList(),
            Declaraciones = source.Declaraciones.Select(x => new DeclaracionBienDto
            {
                Periodo = x.Periodo,
                Estado = x.Estado,
                FechaPresentacion = x.FechaPresentacion,
                MedioRecepcion = x.MedioRecepcion,
                Observacion = x.Observacion
            }).ToList(),
            Exoneraciones = source.Exoneraciones.Select(x => new ExoneracionDto
            {
                TipoExoneracion = x.TipoExoneracion,
                Porcentaje = x.Porcentaje,
                FechaInicio = x.FechaInicio,
                FechaFin = x.FechaFin,
                Estado = x.Estado,
                Institucion = x.Institucion
            }).ToList(),
            Fiscalizaciones = source.Fiscalizaciones.Select(x => new FiscalizacionBienDto
            {
                NumeroCaso = x.NumeroCaso,
                Inspector = x.Inspector,
                Estado = x.Estado,
                Resultado = x.Resultado,
                FechaProgramada = x.FechaProgramada,
                FechaCierre = x.FechaCierre,
                Hallazgos = x.Hallazgos
            }).ToList(),
            Imagenes = source.Imagenes.Select(x => new ImagenPredialDto
            {
                Titulo = x.Titulo,
                Tipo = x.Tipo,
                Url = x.Url,
                Fuente = x.Fuente,
                FechaRegistro = x.FechaRegistro
            }).ToList(),
            HistorialCambios = source.HistorialCambios.Select(x => new AuditoriaCambioDto
            {
                Usuario = x.Usuario,
                FechaHora = x.FechaHora,
                CampoModificado = x.CampoModificado,
                ValorAnterior = x.ValorAnterior,
                ValorNuevo = x.ValorNuevo,
                Origen = x.Origen
            }).ToList()
        };
    }
}
