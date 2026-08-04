using System;
using System.Linq;
using System.Text;

namespace BlazorApp.Helpers;

public static class IdentificationHelper
{
    public static string NormalizeIdentification(string? identification)
    {
        if (string.IsNullOrWhiteSpace(identification))
        {
            return string.Empty;
        }

        var normalized = new StringBuilder();
        foreach (var c in identification)
        {
            if (char.IsLetterOrDigit(c))
            {
                normalized.Append(char.ToUpperInvariant(c));
            }
        }

        return normalized.ToString();
    }

    public static string NormalizeAccountNumber(string? accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return string.Empty;
        }

        return accountNumber.Trim().ToUpperInvariant();
    }

    public static bool IdentificationMatches(string? id1, string? id2)
    {
        var norm1 = NormalizeIdentification(id1);
        var norm2 = NormalizeIdentification(id2);
        return !string.IsNullOrEmpty(norm1) && string.Equals(norm1, norm2, StringComparison.Ordinal);
    }

    public static bool AccountNumberMatches(string? acc1, string? acc2)
    {
        var norm1 = NormalizeAccountNumber(acc1);
        var norm2 = NormalizeAccountNumber(acc2);
        return !string.IsNullOrEmpty(norm1) && string.Equals(norm1, norm2, StringComparison.Ordinal);
    }
}
