namespace BlazorApp.Models;

public class ActividadEconomicaDto
{
    public string CodigoCaecr { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public bool RequiereUsoSuelo { get; set; } = true;
}
