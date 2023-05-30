using System.Globalization;
using LogicLayer.Exceptions;

namespace LogicLayer.Model;

public class Gebruiker
{
    public Guid Id { get; set; }
    public string Familienaam { get; set; }
    public string Voornaam { get; set; }
    public string Email { get; set; }
    public string Telefoonnummer { get; set; }
    public string Code { get; set; }
    public string HashedCode { get; set; }
    public DateTime Geboortedatum { get; set; }
    public Adres Adres { get; set; }

    // Nieuw gebruikerobject aanmaken
    public Gebruiker(string familienaam, string voornaam, string email, string telefoonnummer, string code,
        string geboortedatum, Adres adres)
    {
        Id = Guid.NewGuid();
        ZetFamilienaam(familienaam);
        ZetVoornaam(voornaam);
        ZetEmail(email);
        ZetTelefoonnummer(telefoonnummer);
        ZetCode(code);
        ZetGeboortedatum(geboortedatum);
        ZetAdres(adres);
    }

    // Gebruiker wijzigen
    public Gebruiker(Guid id, string familienaam, string voornaam, string email, string telefoonnummer, string code,
        string geboortedatum, Adres adres)
    {
        Id = id;
        ZetFamilienaam(familienaam);
        ZetVoornaam(voornaam);
        ZetEmail(email);
        ZetTelefoonnummer(telefoonnummer);
        ZetCode(code);
        ZetGeboortedatum(geboortedatum);
        ZetAdres(adres);
    }

    // Gebruiker ophalen uit database// Controles zijn niet nodig
    public Gebruiker(string familienaam, string voornaam, string email, string telefoonnummer,
        DateTime geboortedatum, Adres adres, Guid id)
    {
        Id = id;
        Familienaam = familienaam;
        Voornaam = voornaam;
        Email = email;
        Telefoonnummer = telefoonnummer;
        Geboortedatum = geboortedatum;
        Adres = adres;
    }

    // Test constructor
    public Gebruiker()
    {
    }

    public void ZetFamilienaam(string familienaam)
    {
        if (string.IsNullOrWhiteSpace(familienaam))
        {
            throw new GebruikerException("Familienaam mag niet leeg zijn.");
        }

        Familienaam = familienaam;
    }

    public void ZetVoornaam(string voornaam)
    {
        if (string.IsNullOrWhiteSpace(voornaam))
        {
            throw new GebruikerException("Voornaam mag niet leeg zijn.");
        }

        Voornaam = voornaam;
    }

    public void ZetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new GebruikerException("Email mag niet leeg zijn.");
        }

        Email = email;
    }

    public void ZetTelefoonnummer(string telefoonnummer)
    {
        if (string.IsNullOrWhiteSpace(telefoonnummer))
        {
            throw new GebruikerException("Telefoonnummer mag niet leeg zijn.");
        }

        Telefoonnummer = telefoonnummer;
    }

    public void ZetCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new GebruikerException("Code mag niet leeg zijn.");
        }

        Code = code;
    }

    public void ZetGeboortedatum(string geboortedatum)
    {
        if (geboortedatum == null)
        {
            throw new GebruikerException("Geboortedatum mag niet leeg zijn.");
        }

        Geboortedatum = DateTime.ParseExact(geboortedatum, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    public void ZetAdres(Adres adres)
    {
        if (adres == null)
        {
            throw new GebruikerException("Adres mag niet leeg zijn.");
        }

        Adres = adres;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Gebruiker)obj;

        return Familienaam == other.Familienaam && Voornaam == other.Voornaam && Email == other.Email &&
               Telefoonnummer == other.Telefoonnummer && Geboortedatum == other.Geboortedatum &&
               Adres.Equals(other.Adres);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Familienaam, Voornaam, Email, Telefoonnummer, Geboortedatum, Adres);
    }
}