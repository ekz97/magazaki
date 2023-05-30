using LogicLayer.Managers;
using LogicLayer.Model;
using RESTLayer.Exceptions;
using RESTLayer.Model.Input;

namespace RESTLayer.Mappers;

public static class MapToDomain
{
    public static Adres MapToDomainAdres(AdresRESTinputDTO adresRESTinputDTO)
    {
        try
        {
            return new Adres(adresRESTinputDTO.Straat, adresRESTinputDTO.Huisnummer, adresRESTinputDTO.Postcode,
                adresRESTinputDTO.Gemeente, adresRESTinputDTO.Land);
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van het adres.", e);
        }
    }

    public static Adres MapToDomainAdres(Guid adresId, AdresRESTinputDTO adresRESTinputDTO)
    {
        try
        {
            return new Adres(adresId, adresRESTinputDTO.Straat, adresRESTinputDTO.Huisnummer,
                adresRESTinputDTO.Postcode,
                adresRESTinputDTO.Gemeente, adresRESTinputDTO.Land);
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van het adres.", e);
        }
    }
    
    // Nieuwe rekening
    public static Rekening MapToDomainRekening(RekeningRESTinputDTO rekeningRESTinputDTO, Gebruiker gebruiker)
    {
        try
        {
            return new Rekening(rekeningRESTinputDTO.RekeningType, rekeningRESTinputDTO.KredietLimiet, gebruiker);
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van de rekening.", e);
        }
    }

    // Wijzigen
    public static Rekening MapToDomainRekening(Guid id, RekeningRESTinputDTO rekeningRESTinputDTO)
    {
        try
        {
            return new Rekening(id, rekeningRESTinputDTO.KredietLimiet);
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van de rekening.", e);
        }
    }

    public static Bank MapToDomainBank(BankRESTinputDTO bankRESTinputDTO, Guid adresId)
    {
        try
        {
            return new Bank(bankRESTinputDTO.Naam, MapToDomainAdres(adresId, bankRESTinputDTO.Adres),
                bankRESTinputDTO.Telefoonnummer);
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van de bank.", e);
        }
    }

    public static Bank MapToDomainBank(Guid bankId, BankRESTinputDTO bankRESTinputDTO, Guid adresId)
    {
        try
        {
            return new Bank(bankId, bankRESTinputDTO.Naam, MapToDomainAdres(adresId, bankRESTinputDTO.Adres),
                bankRESTinputDTO.Telefoonnummer);
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van de bank.", e);
        }
    }

    public static Gebruiker MapToDomainGebruiker(GebruikerRESTinputDTO gebruikerRestInputDto, Guid adresId)
    {
        try
        {
            return new Gebruiker(gebruikerRestInputDto.Familienaam, gebruikerRestInputDto.Voornaam,
                gebruikerRestInputDto.Email,
                gebruikerRestInputDto.Telefoonnummer, gebruikerRestInputDto.Code, gebruikerRestInputDto.Geboortedatum,
                MapToDomainAdres(adresId, gebruikerRestInputDto.Adres));
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van de gebruiker.", e);
        }
    }

    public static Gebruiker MapToDomainGebruiker(Guid id, GebruikerRESTinputDTO gebruikerRestInputDto, Guid adresId)
    {
        try
        {
            return new Gebruiker(id, gebruikerRestInputDto.Familienaam, gebruikerRestInputDto.Voornaam,
                gebruikerRestInputDto.Email,
                gebruikerRestInputDto.Telefoonnummer, gebruikerRestInputDto.Code, gebruikerRestInputDto.Geboortedatum,
                MapToDomainAdres(adresId, gebruikerRestInputDto.Adres));
        }
        catch (Exception e)
        {
            throw new MapException("Er ging iets fout bij het mappen van de gebruiker.", e);
        }
    }
}