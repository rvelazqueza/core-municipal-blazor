namespace BlazorApp.Models;

public class RequisitoPatenteDto
{
    public string Nombre { get; set; } = string.Empty;
    public bool Obligatorio { get; set; } = true;
    public bool Cumplido { get; set; }
    public string Observacion { get; set; } = string.Empty;
}
