using LogicLayer.Model;
using RESTLayer.Model.Output;

namespace RESTLayer.Mappers;

public class MapFromDomain
{
    public static AdresRESTOutputDTO MapFromDomainAdres(Adres adres)
    {
        return new AdresRESTOutputDTO
        {
            Id = (Guid)adres.Id,
            Straat = adres.Straat,
            Huisnummer = adres.Huisnummer,
            Postcode = adres.Postcode,
            Gemeente = adres.Gemeente,
            Land = adres.Land
        };
    }
    public static GebruikerRESTOutputDTO MapFromDomainGebruiker(Gebruiker gebruiker)
    {
        return new GebruikerRESTOutputDTO
        {
            Id = gebruiker.Id,
            Familienaam = gebruiker.Familienaam,
            Voornaam = gebruiker.Voornaam,
            Email = gebruiker.Email,
            Telefoonnummer = gebruiker.Telefoonnummer,
            Geboortedatum = gebruiker.Geboortedatum,
            Adres = MapFromDomainAdres(gebruiker.Adres)
        };
    }

    public static TransactieRESTOutputDTO MapFromDomainTransactie(Transactie transactie)
    {
        return new TransactieRESTOutputDTO
        {
            TransactieId = transactie.Id,
            Bedrag = transactie.Bedrag,
            Datum = transactie.Datum,
            TransactieType = transactie.TransactieType,
            Omschrijving = transactie.Mededeling
        };
    }

    public static RekeningRESTOutputDTO MapFromDomainRekening(Rekening rekening)
    {
        return new RekeningRESTOutputDTO
        {
            Rekeningnummer = rekening.Rekeningnummer,
            RekeningType = rekening.RekeningType.ToString(),
            KredietLimiet = rekening.KredietLimiet,
            Saldo = rekening.Saldo,
            Gebruiker = MapFromDomainGebruiker(rekening.Gebruiker),
            Transacties = rekening.Transacties.Select(MapFromDomainTransactie).ToList()
        };
    }
}