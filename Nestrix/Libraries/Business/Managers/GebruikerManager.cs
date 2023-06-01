using LogicLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;

namespace LogicLayer.Managers;

public class GebruikerManager
{
    private readonly IGebruikerRepository _gebruikerRepository;

    public GebruikerManager(IGebruikerRepository gebruikerRepository)
    {
        _gebruikerRepository = gebruikerRepository;
    }

    public async Task<Gebruiker?> GebruikerOphalenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new GebruikerManagerException("Id is leeg");
            }

            var gebruiker = await _gebruikerRepository.GebruikerOphalenAsync(id);
            if (gebruiker == null)
            {
                throw new GebruikerManagerException("Gebruiker niet gevonden");
            }

            return gebruiker;
        }
        catch (Exception e)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het ophalen van de gebruiker.", e);
        }
    }

    public async Task<Gebruiker?> GebruikerOphalenAsync(string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new GebruikerManagerException("Id is leeg");
            }

            var gebruiker = await _gebruikerRepository.GebruikerOphalenViaEmailAsync(email);
            if (gebruiker == null)
            {
                throw new GebruikerManagerException($"Gebruiker met email {email} niet gevonden");
            }

            return gebruiker;
        }
        catch (Exception e)
        {
            throw new GebruikerManagerException($"Er is een fout opgetreden bij het ophalen van de gebruiker met email {email}", e);
        }
    }

    public async Task GebruikerToevoegenAsync(Gebruiker gebruiker)
    {
        try
        {
            if (gebruiker == null)
            {
                throw new GebruikerManagerException("Gebruiker is leeg");
            }

            var gebruikerDB =
                await _gebruikerRepository.GebruikerOphalenAsync(gebruiker.Voornaam, gebruiker.Familienaam, gebruiker.Email);
            if (gebruikerDB != null)
            {
                throw new GebruikerManagerException("Gebruiker bestaat al");
            }

            await _gebruikerRepository.GebruikerToevoegenAsync(gebruiker);
        }
        catch (Exception e)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het toevoegen van de gebruiker.", e);
        }
    }

    public async Task GebruikerWijzigenAsync(Guid id, Gebruiker gebruiker)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new GebruikerManagerException("Id is leeg");
            }

            if (gebruiker == null)
            {
                throw new GebruikerManagerException("Gebruiker is leeg");
            }

            var gebruikerDb = await _gebruikerRepository.GebruikerOphalenAsync(id);
            if (gebruikerDb == null)
            {
                throw new GebruikerManagerException("Gebruiker niet gevonden");
            }

            if (gebruikerDb.Equals(gebruiker))
            {
                throw new GebruikerManagerException("Gebruiker is niet gewijzigd");
            }

            await _gebruikerRepository.GebruikerWijzigenAsync(id, gebruiker);
        }
        catch (Exception e)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het wijzigen van de gebruiker.", e);
        }
    }

    public async Task GebruikerVerwijderenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new GebruikerManagerException("Id is leeg");
            }

            var gebruikerDb = await _gebruikerRepository.GebruikerOphalenAsync(id);
            if (gebruikerDb == null)
            {
                throw new GebruikerManagerException("Gebruiker niet gevonden");
            }

            await _gebruikerRepository.GebruikerVerwijderenAsync(id);
        }
        catch (Exception e)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het verwijderen van de gebruiker.", e);
        }
    }
}