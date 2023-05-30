using LogicLayer.Interfaces;
using LogicLayer.Model;

namespace SecurityLayer;

public class UserService
{
    private readonly IGebruikerRepository _gebruikerRepository;
    
    public UserService(IGebruikerRepository gebruikerRepository)
    {
        _gebruikerRepository = gebruikerRepository;
    }
    
    
    public async Task<Gebruiker?> AuthenticateAsync(Guid gebruikerId, string code)
    {
        var gebruiker = await _gebruikerRepository.GebruikerOphalenAsync(gebruikerId);
        if (gebruiker == null) return null;
        //return BCrypt.Net.BCrypt.Verify(code, gebruiker.HashedCode) ? gebruiker : null;
        return PeasieLib.Services.EncryptionService.ToSHA256(code) == gebruiker.HashedCode ? gebruiker : null;
    }
}