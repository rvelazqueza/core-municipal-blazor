using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IPatentesFormularioService
{
    Task<SolicitudPatenteDto> CrearNuevaSolicitudAsync();
    Task<List<string>> GetAdjuntosCatalogoAsync();
    Task<List<RequisitoPatenteDto>> GetRequisitosBaseAsync();
}
