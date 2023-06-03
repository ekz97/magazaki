using LogicLayer.Interfaces;
using LogicLayer.Model;

namespace SecurityLayer;

public class UserService
{
    private readonly IGebruikerRepository _gebruikerRepository;
    private readonly ILogger<UserService> _logger;
    
    public UserService(IGebruikerRepository gebruikerRepository, ILogger<UserService> logger)
    {
        _gebruikerRepository = gebruikerRepository;
        _logger = logger;
    }
    
    
    public async Task<Gebruiker?> AuthenticateAsync(Guid gebruikerId, string code)
    {
        _logger?.LogDebug("-> AuthenticateAsync");
        var gebruiker = await _gebruikerRepository.GebruikerOphalenAsync(gebruikerId);
        if (gebruiker == null)
        {
            _logger?.LogDebug("<- AuthenticateAsync");
            return null;
        }
        //return BCrypt.Net.BCrypt.Verify(code, gebruiker.HashedCode) ? gebruiker : null;
        _logger?.LogDebug("<- AuthenticateAsync");
        return PeasieLib.Services.EncryptionService.ToSHA256(code) == gebruiker.HashedCode ? gebruiker : null;
    }
}