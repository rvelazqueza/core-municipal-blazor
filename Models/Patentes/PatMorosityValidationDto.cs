namespace BlazorApp.Models;

public class PatMorosityValidationDto
{
    public int LicenciaId { get; set; }
    public string NumeroLicencia { get; set; } = string.Empty;
    public bool SolicitanteAlDia { get; set; }
    public bool PropietarioAlDia { get; set; }
    public bool TieneArregloPago { get; set; }
    public bool ObligacionesFormalesAlDia { get; set; }
    public string Resultado { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
}
