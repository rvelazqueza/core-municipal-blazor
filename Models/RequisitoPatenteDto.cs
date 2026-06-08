namespace BlazorApp.Models;

public class RequisitoPatenteDto
{
    public string Clave { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public bool Obligatorio { get; set; } = true;
    public bool Cumplido { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public string Observacion { get; set; } = string.Empty;
    public string DocumentoPlaceholder { get; set; } = string.Empty;
    public string Origen { get; set; } = "Mock";
}
