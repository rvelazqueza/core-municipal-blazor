using System;
using System.Globalization;

namespace BlazorApp.Helpers;

public static class CurrencyHelper
{
    private static readonly CultureInfo CostaRicaCulture = new CultureInfo("es-CR");

    public static string FormatColones(decimal amount)
    {
        return amount.ToString("C0", CostaRicaCulture);
    }

    public static string FormatColonesWithDecimals(decimal amount)
    {
        return amount.ToString("C2", CostaRicaCulture);
    }
}
