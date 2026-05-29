using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Web;
using BlazorApp;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, MockAuthenticationStateProvider>();

builder.Services.AddScoped<IContribuyentesService, ContribuyentesMockService>();
builder.Services.AddScoped<IMaestroDatosService, MaestroDatosMockService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaMockService>();
builder.Services.AddScoped<IBienesInmueblesService, BienesInmueblesMockService>();
builder.Services.AddScoped<IValoracionService, ValoracionMockService>();
builder.Services.AddScoped<IFiscalizacionService, FiscalizacionMockService>();
builder.Services.AddScoped<IPatentesService, PatentesMockService>();
builder.Services.AddScoped<IActividadEconomicaService, ActividadEconomicaMockService>();
builder.Services.AddScoped<IUsoSueloMockService, UsoSueloMockService>();
builder.Services.AddScoped<IPatentesFormularioService, PatentesFormularioService>();
builder.Services.AddScoped<IPatentesValidacionService, PatentesValidacionService>();
builder.Services.AddSingleton<ILoginSettingsService, LoginSettingsMockService>();
builder.Services.AddSingleton<IAuthService, AuthMockService>();

builder.Services.AddHttpClient("CoreMunicipalApi", client =>
{
    var baseUrl = builder.Configuration["ApiBaseUrl"];
    if (!string.IsNullOrWhiteSpace(baseUrl))
    {
        client.BaseAddress = new Uri(baseUrl);
    }
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
