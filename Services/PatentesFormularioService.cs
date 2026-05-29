using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class PatentesFormularioService : IPatentesFormularioService
{
    private static readonly List<string> AdjuntosCatalogo = new()
    {
        "Documento de identidad",
        "Certificado de uso de suelo",
        "Plano o croquis",
        "Declaración jurada",
        "Permiso sanitario"
    };

    public async Task<SolicitudPatenteDto> CrearNuevaSolicitudAsync()
    {
        return new SolicitudPatenteDto
        {
            FechaSolicitud = DateTime.Today,
            Estado = "Borrador",
            TipoPatente = "Comercial",
            Responsable = "Analista Municipal",
            EstadoUsoSuelo = "Pendiente",
            Requisitos = await GetRequisitosBaseAsync()
        };
    }

    public Task<List<string>> GetAdjuntosCatalogoAsync()
        => Task.FromResult(AdjuntosCatalogo.ToList());

    public Task<List<RequisitoPatenteDto>> GetRequisitosBaseAsync()
        => Task.FromResult(new List<RequisitoPatenteDto>
        {
            new() { Nombre = "Documento de identidad", Obligatorio = true, Observacion = "Pendiente" },
            new() { Nombre = "Certificado de uso de suelo", Obligatorio = true, Observacion = "Pendiente" },
            new() { Nombre = "Plano o croquis", Obligatorio = true, Observacion = "Pendiente" },
            new() { Nombre = "Declaración jurada", Obligatorio = true, Observacion = "Pendiente" }
        });
}
