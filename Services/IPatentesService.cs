using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IPatentesService
{
    Task<List<LicenciaComercialDto>> GetAllAsync();
    Task<LicenciaComercialDto> GetByIdAsync(int id);
    Task<List<LicenciaComercialDto>> SearchAsync(string? numeroLicencia, string? contribuyente, string? actividadEconomica, string? distrito, string? estado, DateTime? fechaVencimientoHasta, string? tipo);
    Task<LicenciaComercialDto> SaveSolicitudAsync(SolicitudPatenteDto solicitud);
    Task<LicenciaComercialDto> SaveLicenciaAsync(LicenciaComercialDto licencia, string usuario);
    Task<LicenciaComercialDto> AprobarSolicitudAsync(int licenciaId, string usuario);
    Task<LicenciaComercialDto> SuspenderAsync(int licenciaId, string motivo, DateTime fecha, string usuario);
    Task<LicenciaComercialDto> CancelarAsync(int licenciaId, string motivo, DateTime fecha, string usuario);
}
