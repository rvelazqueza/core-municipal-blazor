using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class ContribuyentesMockService : IContribuyentesService
{
    private static readonly List<ContribuyenteDto> Data = new()
    {
        new ContribuyenteDto
        {
            Id = 1,
            TipoPersona = "Fisica",
            TipoIdentificacion = "Cedula",
            NumeroIdentificacion = "1-1234-5678",
            Nombre = "Ana",
            Apellidos = "Solano Perez",
            Nacionalidad = "Costarricense",
            EstadoCivil = "Casado",
            Telefono = "8888-1111",
            Correo = "ana.solano@correo.go.cr",
            DireccionFiscal = new DireccionFiscalDto { Provincia = "San Jose", Canton = "Central", Distrito = "Carmen", DireccionExacta = "Avenida central, edificio azul" },
            ActividadEconomica = "Servicios profesionales",
            EstadoContribuyente = "Activo",
            DatosIncompletos = false,
            PendienteCalidad = false,
            UltimaActualizacion = DateTime.Now.AddHours(-5),
            TributosVinculados = new List<TributoVinculadoDto>
            {
                new() { Codigo = "PAT-01", Nombre = "Patente comercial", Estado = "Activo" }
            },
            BienesInmueblesVinculados = new List<BienInmuebleVinculadoDto>
            {
                new() { FincaNumero = "SJ-002145", Distrito = "Carmen", Uso = "Residencial", Estado = "Activo" }
            }
        },
        new ContribuyenteDto
        {
            Id = 2,
            TipoPersona = "Juridica",
            TipoIdentificacion = "Cedula juridica",
            NumeroIdentificacion = "3-101-998877",
            RazonSocial = "Tecnologias Urbanas CR S.A.",
            Telefono = "2222-4444",
            Correo = "contacto@tecnologiasurbanas.cr",
            DireccionFiscal = new DireccionFiscalDto { Provincia = "San Jose", Canton = "Escazu", Distrito = "San Francisco", DireccionExacta = "Centro corporativo norte, piso 4" },
            RepresentanteLegal = new RepresentanteLegalDto { NombreCompleto = "Carlos Vega Ruiz", Identificacion = "1-0987-6543", Telefono = "8888-2222", Correo = "cvega@tecnologiasurbanas.cr" },
            ActividadEconomica = "Comercio",
            EstadoContribuyente = "Activo",
            DatosIncompletos = false,
            PendienteCalidad = true,
            UltimaActualizacion = DateTime.Now.AddHours(-2),
            TributosVinculados = new List<TributoVinculadoDto>
            {
                new() { Codigo = "PAT-02", Nombre = "Patente industrial", Estado = "Activo" },
                new() { Codigo = "COB-01", Nombre = "Cobro administrativo", Estado = "Seguimiento" }
            },
            BienesInmueblesVinculados = new List<BienInmuebleVinculadoDto>
            {
                new() { FincaNumero = "SJ-889900", Distrito = "San Francisco", Uso = "Comercial", Estado = "Activo" }
            }
        },
        new ContribuyenteDto
        {
            Id = 3,
            TipoPersona = "Fisica",
            TipoIdentificacion = "DIMEX",
            NumeroIdentificacion = "1555666777",
            Nombre = "Luis",
            Apellidos = "Mora Campos",
            Nacionalidad = "Nicaraguense",
            EstadoCivil = "Soltero",
            Telefono = "7010-1020",
            Correo = "",
            DireccionFiscal = new DireccionFiscalDto { Provincia = "Heredia", Canton = "Central", Distrito = "Hospital", DireccionExacta = "" },
            ActividadEconomica = "Construccion",
            EstadoContribuyente = "Pendiente revision",
            DatosIncompletos = true,
            PendienteCalidad = true,
            UltimaActualizacion = DateTime.Now.AddDays(-1)
        },
        new ContribuyenteDto
        {
            Id = 4,
            TipoPersona = "Fisica",
            TipoIdentificacion = "Cedula",
            NumeroIdentificacion = "2-3344-5566",
            Nombre = "Marta",
            Apellidos = "Cerdas Quesada",
            Nacionalidad = "Costarricense",
            EstadoCivil = "Viudo",
            Telefono = "6000-4545",
            Correo = "marta.cerdas@correo.go.cr",
            DireccionFiscal = new DireccionFiscalDto { Provincia = "Cartago", Canton = "Central", Distrito = "Merced", DireccionExacta = "Frente al parque municipal" },
            ActividadEconomica = "Alquileres",
            EstadoContribuyente = "Difunto",
            IndicadorDifunto = true,
            UltimaActualizacion = DateTime.Now.AddDays(-3)
        }
    };

    public Task<List<ContribuyenteDto>> GetAllAsync()
    {
        return Task.FromResult(Data.Select(Clone).ToList());
    }

    public Task<ContribuyenteDto> GetByIdAsync(int id)
    {
        var item = Data.FirstOrDefault(x => x.Id == id) ?? new ContribuyenteDto { Id = id };
        return Task.FromResult(Clone(item));
    }

    public Task<List<ContribuyenteDto>> SearchAsync(string? identificacion, string? nombre, string? tipoPersona, string? estado, string? distrito, string? actividadEconomica)
    {
        IEnumerable<ContribuyenteDto> query = Data;

        if (!string.IsNullOrWhiteSpace(identificacion))
            query = query.Where(x => x.NumeroIdentificacion.Contains(identificacion, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(x => x.NombreCompleto.Contains(nombre, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(tipoPersona))
            query = query.Where(x => x.TipoPersona == tipoPersona);
        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(x => x.EstadoContribuyente == estado);
        if (!string.IsNullOrWhiteSpace(distrito))
            query = query.Where(x => x.DireccionFiscal.Distrito == distrito);
        if (!string.IsNullOrWhiteSpace(actividadEconomica))
            query = query.Where(x => x.ActividadEconomica == actividadEconomica);

        return Task.FromResult(query.OrderByDescending(x => x.UltimaActualizacion).Select(Clone).ToList());
    }

    public Task SaveAsync(ContribuyenteDto contribuyente)
    {
        var existing = Data.FirstOrDefault(x => x.Id == contribuyente.Id);
        contribuyente.UltimaActualizacion = DateTime.Now;

        if (existing is null)
        {
            contribuyente.Id = Data.Max(x => x.Id) + 1;
            Data.Add(Clone(contribuyente));
        }
        else
        {
            var index = Data.IndexOf(existing);
            Data[index] = Clone(contribuyente);
        }

        return Task.CompletedTask;
    }

    public Task SimularActualizacionTseAsync(ContribuyenteDto contribuyente)
    {
        if (contribuyente.TipoPersona == "Fisica")
        {
            contribuyente.Nacionalidad = string.IsNullOrWhiteSpace(contribuyente.Nacionalidad) ? "Costarricense" : contribuyente.Nacionalidad;
            if (contribuyente.NumeroIdentificacion.EndsWith("66"))
            {
                contribuyente.IndicadorDifunto = true;
                contribuyente.EstadoContribuyente = "Difunto";
            }
        }

        contribuyente.UltimaActualizacion = DateTime.Now;
        return Task.CompletedTask;
    }

    public Task SimularActualizacionRegistroNacionalAsync(ContribuyenteDto contribuyente)
    {
        if (contribuyente.TipoPersona == "Juridica")
        {
            if (string.IsNullOrWhiteSpace(contribuyente.RepresentanteLegal.NombreCompleto))
            {
                contribuyente.RepresentanteLegal.NombreCompleto = "Representante verificado RN";
                contribuyente.RepresentanteLegal.Identificacion = "1-0000-0001";
            }
        }

        contribuyente.UltimaActualizacion = DateTime.Now;
        return Task.CompletedTask;
    }

    private static ContribuyenteDto Clone(ContribuyenteDto source)
    {
        return new ContribuyenteDto
        {
            Id = source.Id,
            TipoPersona = source.TipoPersona,
            TipoIdentificacion = source.TipoIdentificacion,
            NumeroIdentificacion = source.NumeroIdentificacion,
            Nombre = source.Nombre,
            Apellidos = source.Apellidos,
            RazonSocial = source.RazonSocial,
            Nacionalidad = source.Nacionalidad,
            EstadoCivil = source.EstadoCivil,
            Telefono = source.Telefono,
            Correo = source.Correo,
            DireccionFiscal = new DireccionFiscalDto
            {
                Provincia = source.DireccionFiscal.Provincia,
                Canton = source.DireccionFiscal.Canton,
                Distrito = source.DireccionFiscal.Distrito,
                DireccionExacta = source.DireccionFiscal.DireccionExacta
            },
            RepresentanteLegal = new RepresentanteLegalDto
            {
                NombreCompleto = source.RepresentanteLegal.NombreCompleto,
                Identificacion = source.RepresentanteLegal.Identificacion,
                Telefono = source.RepresentanteLegal.Telefono,
                Correo = source.RepresentanteLegal.Correo
            },
            ActividadEconomica = source.ActividadEconomica,
            EstadoContribuyente = source.EstadoContribuyente,
            DatosIncompletos = source.DatosIncompletos,
            PendienteCalidad = source.PendienteCalidad,
            IndicadorDifunto = source.IndicadorDifunto,
            UltimaActualizacion = source.UltimaActualizacion,
            TributosVinculados = source.TributosVinculados.Select(x => new TributoVinculadoDto { Codigo = x.Codigo, Nombre = x.Nombre, Estado = x.Estado }).ToList(),
            BienesInmueblesVinculados = source.BienesInmueblesVinculados.Select(x => new BienInmuebleVinculadoDto { FincaNumero = x.FincaNumero, Distrito = x.Distrito, Uso = x.Uso, Estado = x.Estado }).ToList(),
            HistorialCambios = source.HistorialCambios.Select(x => new AuditoriaCambioDto { Usuario = x.Usuario, FechaHora = x.FechaHora, CampoModificado = x.CampoModificado, ValorAnterior = x.ValorAnterior, ValorNuevo = x.ValorNuevo, Origen = x.Origen }).ToList()
        };
    }
}
