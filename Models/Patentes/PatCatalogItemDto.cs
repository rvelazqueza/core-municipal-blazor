namespace BlazorApp.Models;

public class PatCatalogItemDto
{
    public string Categoria { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public bool EsParametro { get; set; }
    public string ValorReferencia { get; set; } = string.Empty;
}
