using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface ILoginSettingsService
{
    Task<LoginSettingsDto> GetAsync();
    Task SaveAsync(LoginSettingsDto settings);
}
