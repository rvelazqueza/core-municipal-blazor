namespace BlazorApp.Models;

public class PatInfractionDto
{
    public string NumeroGestion { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public string NumeroInspeccion { get; set; } = string.Empty;
    public string TipoInfraccion { get; set; } = string.Empty;
    public bool AplicaMulta { get; set; }
    public bool RequiereProcesoAdministrativo { get; set; }
    public decimal MontoMulta { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
}
