namespace BlazorApp.Models;

public class ResultadoValidacionDto
{
    public List<string> Errores { get; set; } = new();
    public bool EsValido => !Errores.Any();
}
