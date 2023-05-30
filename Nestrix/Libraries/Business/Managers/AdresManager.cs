using LogicLayer.Exceptions;
using LogicLayer.Interfaces;
using LogicLayer.Model;

namespace LogicLayer.Managers;

public class AdresManager
{
    private readonly IAdresRepository _adresRepository;

    public AdresManager(IAdresRepository adresRepository)
    {
        _adresRepository = adresRepository;
    }

    public async Task<Adres?> AdresOphalenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new AdresManagerException("Id is leeg");
            }

            var adres = await _adresRepository.AdresOphalenAsync(id);

            return adres ?? null;
        }
        catch (Exception e)
        {
            throw new AdresManagerException("Er is een fout opgetreden bij het ophalen van het adres.", e);
        }
    }

    public async Task AdresToevoegenAsync(Adres adres)
    {
        try
        {
            if (adres == null)
            {
                throw new AdresManagerException("Adres is leeg");
            }

            var adresDb = await _adresRepository.AdresOphalenAsync(adres.Straat, adres.Huisnummer, adres.Postcode, adres.Gemeente, adres.Land);
            if (adresDb != null)
            {
                throw new AdresManagerException("Adres bestaat al");
            }

            await _adresRepository.AdresToevoegenAsync(adres);
        }
        catch (Exception e)
        {
            throw new AdresManagerException("Er is een fout opgetreden bij het toevoegen van het adres.", e);
        }
    }

    public async Task AdresWijzigenAsync(Guid id, Adres adres)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new AdresManagerException("Id is leeg");
            }
            if (adres == null)
            {
                throw new AdresManagerException("Adres is leeg");
            }

            //var adresDB = _adresRepository.AdresOphalen(adres.Straat, adres.Huisnummer, adres.Postcode, adres.Gemeente, adres.Land);
            var adresDb = await _adresRepository.AdresOphalenAsync(id);
            if (adresDb == null)
            {
                throw new AdresManagerException("Adres niet gevonden");
            }

            if (adresDb.Equals(adres))
            {
                throw new AdresManagerException("Adres niks gewijzigd.");
            }

            await _adresRepository.AdresWijzigenAsync(id, adres);
        }
        catch (Exception e)
        {
            throw new AdresManagerException("Er is een fout opgetreden bij het wijzigen van het adres.", e);
        }
    }

    public async Task AdresVerwijderenAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new AdresManagerException("Id is leeg");
            }

            var adresDb = await _adresRepository.AdresOphalenAsync(id);
            if (adresDb == null)
            {
                throw new AdresManagerException("Adres niet gevonden");
            }

            await _adresRepository.AdresVerwijderenAsync(id);
        }
        catch (Exception e)
        {
            throw new AdresManagerException("Er is een fout opgetreden bij het verwijderen van het adres.", e);
        }
    }
    
    public async Task<Adres?> AdresOphalenAsync(string straat, string huisnummer, string postcode, string gemeente, string land)
    {
        try
        {
            if (string.IsNullOrEmpty(straat))
            {
                throw new AdresManagerException("Straat is leeg");
            }

            if (string.IsNullOrEmpty(huisnummer))
            {
                throw new AdresManagerException("Huisnummer is leeg");
            }

            if (string.IsNullOrEmpty(postcode))
            {
                throw new AdresManagerException("Postcode is leeg");
            }

            if (string.IsNullOrEmpty(gemeente))
            {
                throw new AdresManagerException("Gemeente is leeg");
            }
            
            if (string.IsNullOrEmpty(land))
            {
                throw new AdresManagerException("Land is leeg");
            }

            var adres = await _adresRepository.AdresOphalenAsync(straat, huisnummer, postcode, gemeente, land);

            return adres ?? null;
        }
        catch (Exception e)
        {
            throw new AdresManagerException("Er is een fout opgetreden bij het ophalen van het adres.", e);
        }
    }
}