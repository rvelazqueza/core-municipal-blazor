namespace BlazorApp.Models;

public class ResumenDistritoBienViewModel
{
    public string Distrito { get; set; } = string.Empty;
    public int Propiedades { get; set; }
    public int PendientesFiscalizacion { get; set; }
    public int DeclaracionesPendientes { get; set; }
    public decimal ValorFiscalTotal { get; set; }
}
