using LogicLayer.Model;

namespace LogicLayer.Interfaces;

public interface IGebruikerRepository
{
    Task GebruikerToevoegenAsync(Gebruiker gebruiker);
    Task GebruikerVerwijderenAsync(Guid id);
    Task GebruikerWijzigenAsync(Guid id, Gebruiker gebruiker);
    Task<Gebruiker?> GebruikerOphalenAsync(Guid id);
    Task<Gebruiker?> GebruikerOphalenAsync(string email);
    Task<Gebruiker?> GebruikerOphalenAsync(string voornaam, string familienaam, string email);
}