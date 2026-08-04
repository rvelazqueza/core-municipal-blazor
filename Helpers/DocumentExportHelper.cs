using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorApp.Helpers;

public class DocumentExportHelper
{
    private readonly IJSRuntime _jsRuntime;

    public DocumentExportHelper(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ExportAsHtmlAsync(string content, string fileName)
    {
        var sanitizedFileName = SanitizeFileName(fileName);
        await _jsRuntime.InvokeVoidAsync("downloadHtml", content, sanitizedFileName);
    }

    public async Task PrintHtmlAsync(string content)
    {
        await _jsRuntime.InvokeVoidAsync("printHtml", content);
    }

    public async Task ExportAsPdfAsync(string content, string fileName)
    {
        var sanitizedFileName = SanitizeFileName(fileName);
        await _jsRuntime.InvokeVoidAsync("printToPdf", content, sanitizedFileName);
    }

    public static string HtmlEncode(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        return HtmlEncoder.Default.Encode(text);
    }

    public static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "documento";
        }

        var invalidChars = System.IO.Path.GetInvalidFileNameChars();
        var sanitized = new StringBuilder();

        foreach (var c in fileName)
        {
            if (Array.IndexOf(invalidChars, c) == -1)
            {
                sanitized.Append(c);
            }
            else
            {
                sanitized.Append('_');
            }
        }

        return sanitized.ToString();
    }

    public static string BuildMunicipalDocument(string title, string expediente, string content)
    {
        var html = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{HtmlEncode(title)}</title>
    <style>
        @page {{
            size: A4;
            margin: 2cm;
        }}
        body {{
            font-family: 'Segoe UI', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.5;
            color: #333;
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
            border-bottom: 2px solid #2196F3;
            padding-bottom: 15px;
        }}
        .header h1 {{
            font-size: 18pt;
            margin: 0 0 5px 0;
            color: #1976D2;
        }}
        .header h2 {{
            font-size: 14pt;
            margin: 0 0 5px 0;
            color: #555;
        }}
        .header .expediente {{
            font-size: 12pt;
            font-weight: bold;
            color: #2196F3;
            margin: 10px 0;
        }}
        .header .date {{
            font-size: 10pt;
            color: #777;
        }}
        .section {{
            margin-bottom: 25px;
        }}
        .section-title {{
            font-size: 13pt;
            font-weight: bold;
            color: #1976D2;
            border-bottom: 1px solid #ddd;
            padding-bottom: 5px;
            margin-bottom: 10px;
        }}
        .field {{
            margin-bottom: 8px;
        }}
        .field-label {{
            font-weight: bold;
            color: #555;
            display: inline-block;
            width: 180px;
        }}
        .field-value {{
            display: inline-block;
            color: #333;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin: 10px 0;
        }}
        th, td {{
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }}
        th {{
            background-color: #f5f5f5;
            font-weight: bold;
            color: #1976D2;
        }}
        .footer {{
            margin-top: 40px;
            padding-top: 15px;
            border-top: 1px solid #ddd;
            font-size: 9pt;
            color: #777;
            text-align: center;
        }}
        .badge {{
            display: inline-block;
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 9pt;
            font-weight: bold;
        }}
        .badge-success {{ background-color: #4CAF50; color: white; }}
        .badge-warning {{ background-color: #FF9800; color: white; }}
        .badge-error {{ background-color: #F44336; color: white; }}
        .badge-info {{ background-color: #2196F3; color: white; }}
        @media print {{
            body {{
                padding: 0;
            }}
            .no-print {{
                display: none;
            }}
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Municipalidad</h1>
        <h2>Core Municipal - Sistema Integrado</h2>
        <div class='expediente'>Expediente: {HtmlEncode(expediente)}</div>
        <div class='date'>Generado: {DateTime.Now:dd/MM/yyyy HH:mm}</div>
    </div>
    <div class='content'>
        {content}
    </div>
    <div class='footer'>
        <p>Documento generado electrónicamente por el Core Municipal</p>
        <p>Usuario: usuario.municipal | Fecha: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
    </div>
</body>
</html>";
        return html;
    }
}
