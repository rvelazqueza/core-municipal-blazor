using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorApp.Helpers;

public static class ValidationHelper
{
    // Teléfono: 8 dígitos numéricos exactos
    public static bool ValidarTelefono(string? telefono, out string? mensajeError)
    {
        mensajeError = null;
        if (string.IsNullOrWhiteSpace(telefono))
            return true; // Opcional, permitir vacío

        var cleaned = new string(telefono.Where(char.IsDigit).ToArray());
        if (cleaned.Length != 8)
        {
            mensajeError = "Ingrese un teléfono de 8 dígitos.";
            return false;
        }

        return true;
    }

    public static string FormatearTelefono(string? telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
            return string.Empty;

        var cleaned = new string(telefono.Where(char.IsDigit).ToArray());
        if (cleaned.Length == 8)
            return $"{cleaned.Substring(0, 4)}-{cleaned.Substring(4, 4)}";

        return telefono;
    }

    // Cédula física: 9 dígitos numéricos exactos
    public static bool ValidarCedulaFisica(string? cedula, out string? mensajeError)
    {
        mensajeError = null;
        if (string.IsNullOrWhiteSpace(cedula))
            return true; // Opcional, permitir vacío

        var cleaned = new string(cedula.Where(char.IsDigit).ToArray());
        if (cleaned.Length != 9)
        {
            mensajeError = "Ingrese una cédula física de 9 dígitos.";
            return false;
        }

        return true;
    }

    public static string FormatearCedulaFisica(string? cedula)
    {
        if (string.IsNullOrWhiteSpace(cedula))
            return string.Empty;

        var cleaned = new string(cedula.Where(char.IsDigit).ToArray());
        if (cleaned.Length == 9)
            return $"{cleaned.Substring(0, 1)}-{cleaned.Substring(1, 4)}-{cleaned.Substring(5, 4)}";

        return cedula;
    }

    // Correo electrónico
    public static bool ValidarCorreo(string? correo, out string? mensajeError)
    {
        mensajeError = null;
        if (string.IsNullOrWhiteSpace(correo))
            return true; // Opcional, permitir vacío

        if (!new EmailAddressAttribute().IsValid(correo))
        {
            mensajeError = "Ingrese un correo electrónico válido.";
            return false;
        }

        return true;
    }

    // Función genérica para validación de identificación (detecta tipo)
    public static bool ValidarIdentificacion(string? identificacion, string? tipoIdentificacion, out string? mensajeError)
    {
        mensajeError = null;
        if (string.IsNullOrWhiteSpace(identificacion))
        {
            mensajeError = "La identificación es obligatoria.";
            return false;
        }

        if (tipoIdentificacion == "Cedula" || tipoIdentificacion == "Cédula" || tipoIdentificacion == "Física")
        {
            return ValidarCedulaFisica(identificacion, out mensajeError);
        }

        // Para otros tipos (jurídica, DIMEX, pasaporte), solo validar no vacío
        return true;
    }
}
